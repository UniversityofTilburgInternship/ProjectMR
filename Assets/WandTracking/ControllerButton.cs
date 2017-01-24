using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Controller button. This is a temporary fix to getting the correct button IDs to work 
/// This will check whether a controller button is being pressed down or not, this should
/// be attached to the wand for each button you want used
/// </summary>
public class ControllerButton : MonoBehaviour 
{
	public string TrackerName = "PPT_WAND3";
	public string port = "8945";
	public int buttonID = 0; //trigger is buttonID 6

	public Camera StandardPlayer;

	[HideInInspector] public bool isDebug = false;
	[HideInInspector] public bool isHoldingObject = false;

	private string VRPNAddress;

	private bool canChangeLevel = true;

	#region Unity Callbacks
	protected void Awake ()
	{
		UpdateAddress();
	}

	/// <summary>
	/// Checks if the game is running in editor mode or not to decide whether to enable debug mode (mouse controls)
	/// </summary>
	protected void Start () 
	{
		UpdateAddress();

		if (Application.isEditor) 
		{
			isDebug = true;
		}
		else
		{
			isDebug = false;
		}
	}

    //public void OnGUI()
    //{
    //    for (int i = 0; i < 64; i++)
    //    {
    //        if(VRPN.vrpnButton(VRPNAddress, i))
    //            GUI.Label(new Rect(10, 10, 150, 100), "you have pressed button " + i);
    //    }
    //}
	
	protected void Update ()
	{
		if(ClusterNetwork.isMasterOfCluster && !ClusterNetwork.isDisconnected)
		{
			ClusterInput.SetButton("WandButton2",VRPN.vrpnButton(VRPNAddress,5));
		}
		
		if(ClusterInput.GetButton("WandButton2"))
		{
			StandardPlayer.stereoSeparation += .01f;
			Debug.LogError ("Separation: " + StandardPlayer.stereoSeparation);
		}
	}
	#endregion

	/// <summary>
	/// This function returns whether the trigger buttons is being pressed.
	/// Returns true if it is pressed and returns false if it is not pressed.
	/// If the game is run from the editor it will react to a Left mouse click.
	/// </summary>
	public int isPressed()
	{
//	    Debug.Log("isPressed");
//		if(isDebug)
//		{
//		    Debug.Log("isDebug");
//			if(Input.GetMouseButton(0) == false)
//			{
//				isHoldingObject = false;
//			}
//
//			return Input.GetMouseButton(0);
//		}
//		else
//		{
//		    Debug.Log("!isDebug");
//			if(ClusterNetwork.isMasterOfCluster && !ClusterNetwork.isDisconnected)
//			{
//			    Debug.Log("button set");
//				ClusterInput.SetButton("WandButton",VRPN.vrpnButton(VRPNAddress,6));
//			}
//
//			if(ClusterInput.GetButton("WandButton"))//if(isButtonPressed == false)
//			{
//				isHoldingObject = false;
//			}
//
//			//return isButtonPressed;
//			return ClusterInput.GetButton("WandButton");
//		}
	    for (int i = 0; i < 64; i++)
	    {
	        if (VRPN.vrpnButton(VRPNAddress, i))
	            return i;
	    }
	    return 0;
	}

	/// <summary>
	/// Updates the VRPN address.
	/// </summary>
	public void UpdateAddress()
	{	
		VRPNAddress = TrackerName + "@" + "FC-NUC" + ":" + port;
	}
}                         