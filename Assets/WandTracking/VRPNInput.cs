using UnityEngine;
using System.Collections;

/// <summary>
/// VRPN input. Used to add Cluster VRPN input once the network is started
/// </summary>
public class VRPNInput : MonoBehaviour 
{
	private bool wandTrackerAdded = false;
	private bool eyeTrackerAdded = false;
	private bool wandButtonAdded = false;
	private bool wandButtonAdded2 = false;

	#region Unity Callbacks
	/// <summary>
	/// Update function, This will add the cluster input fields into the cluster input manager.
	/// This is done in the update because it must first wait and check if Cluster network is connected.
	/// This ensures that there is a connection to the clients and input server. If there is no connection
	/// to the input server, for example running in the editor mode, the game will freeze. 
	/// 
	/// A WandTracker and EyeTracker field is added which will be used to access the positon and orientation
	/// of the wand and/or tracked glasses.
	/// 
	/// TODO: BUTTON STUFF
	/// </summary>
	protected void Update () 
	{
		// There is no isConnected
		if(!ClusterNetwork.isDisconnected)
		{
			if(!wandTrackerAdded)
			{
				ClusterInput.AddInput("WandTracker", "PPT0", "fc-nuc", 2, ClusterInputType.Tracker);
				wandTrackerAdded = true;
				Debug.LogError("WandTracker Added");
			}

			if(!eyeTrackerAdded)
			{
				ClusterInput.AddInput("EyeTracker", "PPT0", "FC-NUC", 0, ClusterInputType.Tracker);	
				eyeTrackerAdded = true;
				Debug.LogError("EyeTracker Added");
			}

			if(!wandButtonAdded)
			{
				//ClusterInput.AddInput("WandButton", "PPT_WAND3", "FC-NUC", 64, ClusterInputType.Button);
				ClusterInput.AddInput("WandButton", "PPT0", "FC-NUC", 2, ClusterInputType.CustomProvidedInput);
				//ClusterInput.AddInput("WandButton2", "PPT0", "FC-NUC", 2, ClusterInputType.CustomProvidedInput);



				//ClusterInput.AddInput("test", "PPT_WAND3", "FC-NUC", 16, ClusterInputType.Button);

				wandButtonAdded = true;
				Debug.LogError("WandButton Added");
			}


			if(!wandButtonAdded2)
			{
				//ClusterInput.AddInput("WandButton", "PPT_WAND3", "FC-NUC", 64, ClusterInputType.Button);
				ClusterInput.AddInput("WandButton2", "PPT0", "FC-NUC", 3, ClusterInputType.CustomProvidedInput);
				//ClusterInput.AddInput("WandButton2", "PPT0", "FC-NUC", 2, ClusterInputType.CustomProvidedInput);



				//ClusterInput.AddInput("test", "PPT_WAND3", "FC-NUC", 16, ClusterInputType.Button);

				wandButtonAdded2 = true;
				Debug.LogError("WandButton2 Added");
			}

		
		}
			
		if (ClusterNetwork.isDisconnected) 
		{
			Application.Quit();
		}
	}
	#endregion
}
                              