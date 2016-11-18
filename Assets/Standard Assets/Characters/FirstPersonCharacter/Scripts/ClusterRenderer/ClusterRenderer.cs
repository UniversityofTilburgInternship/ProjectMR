using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO; 
using System;

public class ClusterRenderer : MonoBehaviour {
	[SerializeField]
	private CameraRig m_Rig;
	[SerializeField]
	private bool m_OverrideNodeIndex = false;
	[SerializeField]
	private int m_ForcedIndex;

	public CameraRig rig
	{
		get { return m_Rig; }
	}

	[System.NonSerialized]
	public string rigAttributes = null;

	private string GetCmdArguments(string arg) {
		string[] arguments = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < arguments.Length; i++) {
			if (arguments[i] == arg) {
				if (i+1 < arguments.Length)
					return arguments[i+1];
			}
		}
		// default to null
		return null;
	}

	public int GetCameraIndex ()
	{
		// load from command line
		string cmdIndex = GetCmdArguments ("-client");
		// if there is an index given from the command line (slave node)
		if (cmdIndex != null)
			return int.Parse (cmdIndex);
		// if node index is overriden for testing
		else if (m_OverrideNodeIndex)
			return m_ForcedIndex;
		// if node is master node and option has been chosen to integrate it
		else if (m_Rig.masterNodeIntegrated)
			return m_Rig.masterNodeIndex;
		else
			return -1;
	}

	void Awake() {
		LoadAndCreateRig();
		UpdateCamera();	
	}

	void UpdateCamera() {
		
		// get the camera component from this object
		Camera camera = this.gameObject.GetComponent<Camera>();

		int cameraIndex = GetCameraIndex ();
		if (cameraIndex == -1)
		{
			// if not found, we don't change the camera settings
			camera.ResetProjectionMatrix ();
			return;
		}

		m_Rig.SetupCamera (camera, cameraIndex);

	}

	void LateUpdate() {
		if(Application.isEditor) {
			UpdateCamera();
		}
	}

	private void LoadAndCreateRig() {
		// Return if no rig path
		string rigPath = GetCmdArguments("-rigPath");
		if (rigPath == null)
			return;
		
		// Handle any problems that might arise when reading the text
		// Create a new StreamReader, tell it which file to read
		try
		{
			// Create full path of rig file from the application path and the command line input
			string applicationParentPath;
			if (Application.platform == RuntimePlatform.OSXPlayer)
				// The parent to OXS data folder is the .app file. We need the parent of the .app.
				applicationParentPath = Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
			else
				applicationParentPath = Directory.GetParent(Application.dataPath).FullName;

			// Obtain the final rig path from relative to the application path.
			string rigFinalPath = Path.Combine(applicationParentPath, rigPath);

			// Read in data
			StreamReader theReader = new StreamReader(rigFinalPath, Encoding.Default);
			rigAttributes = theReader.ReadToEnd();

			// Done reading, close the reader 
			theReader.Close ();

			// Create rig using C#'s XmlSerializer to deserialize
			CameraRig newCR = null;
			XmlSerializer ser = new XmlSerializer(typeof(CameraRig));
			TextReader reader = new StringReader (rigAttributes);
			newCR = (CameraRig)ser.Deserialize(reader);

			// Check new CameraRig to see if attributes are ok
			CheckCameraRig(ref newCR);

			// Replace the current rig
			m_Rig = newCR;

		}
		catch(Exception e)
		{
			Debug.Log ("[Date: " + System.DateTime.Now.ToString("MM/dd/yyyy")
			           + " Time: " + System.DateTime.Now.ToString("hh:mm:ss") + "]"
			           + Environment.NewLine);
			Debug.Log ("Exception caught: " + e
			           + Environment.NewLine + Environment.NewLine);
		}
	}

	private void CheckCameraRig(ref CameraRig cr) {
		// Check for negative or 0 values
		ThrowNegativeException((cr.cameraIndexOffset < 0), "cameraIndexOffset", true);
		ThrowNegativeException((cr.horizontalSeam < 0), "horizontalSeam", true);
		ThrowNegativeException((cr.verticalSeam < 0), "verticalSeam", true);
		ThrowNegativeException((cr.nearClip < 0), "nearClip", true);
		ThrowNegativeException((cr.farClip <= 0), "farClip", false);
		ThrowNegativeException((cr.screenAspectWidth <= 0), "screenAspectWidth", false);
		ThrowNegativeException((cr.screenAspectHeight <= 0), "screenAspectHeight", false);
		ThrowNegativeException((cr.masterNodeIndex < 0), "masterNodeIndex", true);

		if (cr.clusterType == CameraRig.ClusterType.Wall) {
			ThrowNegativeException((cr.wallParams.rows <= 0), "wallParams.rows", false);
			ThrowNegativeException((cr.wallParams.cols <= 0), "wallParams.cols", false);
			ThrowNegativeException((cr.fov <= 0), "fov", false);
		}
		else if (cr.clusterType == CameraRig.ClusterType.Cave) {
			ThrowNegativeException((cr.caveParams.sides <= 0), "caveParams.sides", false);

			// Cave without 4 sides shouldn't have sky or floor
			if (cr.caveParams.sides != 4) {
				if (cr.caveParams.hasSky)
					throw new Exception ("Error: Sky only available for Cave with 4 sides.");
				if (cr.caveParams.hasFloor)
					throw new Exception ("Error: Floor only available for Cave with 4 sides.");
			}
			// Cave has 4 walls
			else
			{
				if (cr.caveParams.hasSky)
					ThrowNegativeException((cr.caveParams.indexSky < 0), "caveParams.indexSky", true);
				if (cr.caveParams.hasFloor)
					ThrowNegativeException((cr.caveParams.indexFloor < 0), "caveParams.indexFloor", true);
				// If has both sky and floor, indexes for both cannot be the same
				if (cr.caveParams.hasSky && cr.caveParams.hasFloor && cr.caveParams.indexSky == cr.caveParams.indexFloor)
					throw new Exception ("Error: Index for Sky canot be similar to index for Floor.");
			}
		}

		// Check if far clip is lesser than near clip
		if (cr.farClip <= cr.nearClip)
			throw new Exception ("Error: Far clip must be more than near clip.");
	}

	private void ThrowNegativeException(bool error, string name, bool zeroAllowed) {
		if (!error)
			return;

		if (zeroAllowed)
			throw new Exception ("Error: " + name + " must be 0 or more.");
		else
			throw new Exception ("Error: " + name + " must be more than 0.");
	}
}


