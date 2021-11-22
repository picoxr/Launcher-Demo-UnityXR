using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherUtils : MonoBehaviour {

	private static OtherUtils instance;
	public static OtherUtils Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<OtherUtils> ();
			}
			if (instance == null) {
				GameObject obj = new GameObject ("OtherUtils");
				instance = obj.AddComponent<OtherUtils> ();
				DontDestroyOnLoad (obj);
			}
			return instance;
		}
	}
	void Awake(){
		if (Application.platform == RuntimePlatform.Android) {
			Call_SetUnityActivity ();
		}
	}

	private void Call_SetUnityActivity(){
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		GetJavaObject().Call("setUnityActivity", jo);
	}

	private AndroidJavaObject javaObj = null;
	private AndroidJavaObject GetJavaObject(){
		if (javaObj == null){
			javaObj = new AndroidJavaObject("com.picovr.manager.OtherManager");
		}
		return javaObj;
	}

	
	public string GetCurrentTime() {
		if (Application.platform == RuntimePlatform.Android) {
			string time = GetJavaObject().Call<string>("getCurrentTime");
			return time;
		} else {
			return "00:00";
		}
	}

	
	public bool GetNetStatus() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<bool> ("getNetStatus");
		} else {
			return true;
		}
	}

	
	public int GetWifiLevel() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getWifiLevel");
		} else {
			return 0;
		}
	}

	
	public int GetWifiSwitchState(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getWifiSwitchState");
		} else {
			return 0;
		}
	}

	
	public int GetNetworkConnectionState(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getWifiConnectionState");
		} else {
			return 1;
		}
	}

	
	public int GetBluetoothStatus() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getBluetoothStatus");
		} else {
			return 10;
		}
	}

	
	public int GetVolume() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getVolume");
		} else {
			return 1;
		}
	}

	
	public string GetLanguage() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getLanguage");
		} else {
			return "zh";
		}
	}

	
	public string GetCountry(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getCountry");
		} else {
			return "zh";
		}
	}

	
	public string GetCountryCode(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getCountryCode");
		} else {
			return "CN";
		}
	}

	
	public string GetVersionName(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("GetVersionName");
		} else {
			return "1.0";
		}
	}

	
	public int GetVersionCode(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getVersionCode");
		} else {
			return 1;
		}
	}

	
	public string GetBaseUrl(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getBaseUrl");
		} else {
			return Constant.BASE_URL;
		}
	}

	
	public bool PutIntDataToSp(string key, int value){
		return GetJavaObject ().Call<bool> ("putIntDataToSp", key, value);
	}

	
	public int GetIntDataFromSp(string key, int defValue) {
		return GetJavaObject ().Call<int> ("getIntDataFromSp", key, defValue);
	}

	
	public string GetUserInfo(){
		return GetJavaObject ().Call<string> ("getUserInfo");
	}

	
	public byte[] GetUserAvatar(){
		return GetJavaObject ().Call<byte[]> ("getUserAvatar");
	}

	
	public string GetSystemProperties(string key, string defaultValue) {
		return GetJavaObject ().Call<string> ("getSystemProperties" ,key, defaultValue);
	}

	
	public bool HasExternalSdcard(){
		return GetJavaObject ().Call<bool> ("hasExternalSdcard");
	}

	
	public string GetExternalSdcardPath() {
		return GetJavaObject ().Call<string> ("getExternalSdcardPath");
	}

	
	public void SendLaunchBroadcast(){
		LogUtils.Log ("发送launcher启动可见的广播");
		GetJavaObject ().Call ("sendLaunchBroadcast");
	}

	
	public int GetBattery(){
		int battery = GetJavaObject().Call<int>("getBattery");
		return battery;
	}

	
	public int HasSystemUpdate(){
		return GetJavaObject ().Call<int> ("hasSystemUpdate");
	}

	
	public int HasAppUpdate(){
		return GetJavaObject ().Call<int> ("hasAppUpdate");
	}

	public void OpenVersionManager(){
		LogUtils.Log ("延迟2s，检查是否可以打开版本管理");
		GetJavaObject ().Call ("openVersionManager");
	}

	
	public void StartVersionManager(){
		GetJavaObject ().Call ("startVersionManager");
	}

}
