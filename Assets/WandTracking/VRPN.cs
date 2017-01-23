using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// VRPN dll. This class communicates with the VRPN server using the "unityVrpn.dll"
/// </summary>
public static class VRPN{
	[DllImport ("unityVrpn")]
	private static extern double vrpnAnalogExtern(string address, int channel, int frameCount);

	[DllImport ("unityVrpn")]
	private static extern bool vrpnButtonExtern(string address, int channel, int frameCount);
	
	[DllImport ("unityVrpn")]
	private static extern double vrpnTrackerExtern(string address, int channel, int component, int frameCount);

	/// <summary>
	/// Gets analog data from the VRPN server through the dll
	/// </summary>
	/// <returns>The analog value as a double</returns>
	/// <param name="address">Address.</param>
	/// <param name="channel">Channel.</param>
	public static double vrpnAnalog(string address, int channel){
		return vrpnAnalogExtern (address, channel, Time.frameCount);
	}

	/// <summary>
	/// Gets button data from the VRPN server through the dll
	/// </summary>
	/// <returns><c>true</c>, if button was vrpned, <c>false</c> otherwise.</returns>
	/// <param name="address">Address.</param>
	/// <param name="channel">Channel.</param>
	public static bool vrpnButton(string address, int channel){
		return vrpnButtonExtern (address, channel, Time.frameCount);	
	}

	//sensorID -1 is the default
	/// <summary>
	/// Gets position data from the VRPN server through the dll
	/// </summary>
	/// <returns>The tracker position as a Vector3.</returns>
	/// <param name="address">Address.</param>
	/// <param name="channel">Channel.</param>
	public static Vector3 vrpnTrackerPos(string address, int channel){
		return new Vector3(
			(float) vrpnTrackerExtern(address, channel, 0, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 1, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 2, Time.frameCount));
	}

	//sensorID -1 is the default
	/// <summary>
	/// Gets rotation data from the VRPN server through the dll
	/// </summary>
	/// <returns>The Tracker orientation as a Quaternion</returns>
	/// <param name="address">Address.</param>
	/// <param name="channel">Channel.</param>
	public static Quaternion vrpnTrackerQuat(string address, int channel){
		return new Quaternion(
			(float) vrpnTrackerExtern(address, channel, 3, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 4, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 5, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 6, Time.frameCount));
	}
}
                              