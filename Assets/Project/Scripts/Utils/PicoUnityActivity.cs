using UnityEngine;
using System.Collections;

/// <summary>
/// Pico unity activity.
/// </summary>
public class PicoUnityActivity
{
	private static AndroidJavaObject picoUnityAcvity;

	private static AndroidJavaObject CurrentActivity ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			string packageName = "com.picovr.launcherlib.UnityActivity";
			if (picoUnityAcvity == null) {
				picoUnityAcvity = new AndroidJavaClass (packageName).GetStatic<AndroidJavaObject> ("mUnityActivity");
			}
		}

		return picoUnityAcvity;
	}

	public static bool CallObjectMethod (string name, params object[] args)
	{
		if (CurrentActivity () == null) {
			Debug.LogError ("Object is null when calling method " + name);
			return false;
		}
		try {
			CurrentActivity ().Call (name, args);
			return true;
		} catch (AndroidJavaException e) {
			Debug.LogError ("Exception calling method " + name + ": " + e);
			return false;
		}
	}


	public static bool CallObjectMethod<T> (ref T result, string name)
	{
		if (CurrentActivity () == null) {
			Debug.LogError ("Object is null when calling method " + name);
			return false;
		}
		try {
			result = CurrentActivity ().Call<T> (name);
			return true;
		} catch (AndroidJavaException e) {
			Debug.LogError ("Exception calling method " + name + ": " + e);
			return false;
		}
	}

	public static bool CallObjectMethod<T> (ref T result, string name,
	                                        params object[] args)
	{
		if (CurrentActivity () == null) {
			Debug.LogError ("Object is null when calling method " + name);
			return false;
		}
		try {
			result = CurrentActivity ().Call<T> (name, args);
			return true;
		} catch (AndroidJavaException e) {
			Debug.LogError ("Exception calling method " + name + ": " + e);
			return false;
		}
	}


	public static bool CallObjectMethod1<T> (ref T result, string name,
	                                         object[] args)
	{
		if (CurrentActivity () == null) {
			Debug.LogError ("Object is null when calling method " + name);
			return false;
		}
		try {
			result = CurrentActivity ().Call<T> (name, args);
			return true;
		} catch (AndroidJavaException e) {
			Debug.LogError ("Exception calling method " + name + ": " + e + args.ToString ());
			return false;
		}
	}

	
	public static AndroidJavaObject GetActivity (string package_name, string activity_name)
	{
		return new AndroidJavaClass (package_name).GetStatic<AndroidJavaObject> (activity_name);
	}

}
