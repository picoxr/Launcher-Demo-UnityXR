using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtils : MonoBehaviour {

	private static AppUtils instance;
	public static AppUtils Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<AppUtils> ();
			}
			if (instance == null) {
				GameObject obj = new GameObject ("AppUtils");
				instance = obj.AddComponent<AppUtils> ();
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
			javaObj = new AndroidJavaObject("com.picovr.manager.AppManager");
		}
		return javaObj;
	}


	
	public void OpenAppByPackageName(string packageName){
		if (Application.platform == RuntimePlatform.Android) {
			LogUtils.Log ("OpenAppByPackageName : " + packageName);
			GetJavaObject ().Call ("openAppByPackageName", packageName);
		}
	}

	
	public void OpenAppByComponentName(string packageName, string activityName) {
		if (Application.platform == RuntimePlatform.Android) {
			LogUtils.Log ("OpenAppByComponentName : " + packageName + ", " + activityName);
			GetJavaObject ().Call ("openAppByComponentName", packageName, activityName);
		}
	}

	
	public void OpenAppByAction(string action){
		if (Application.platform == RuntimePlatform.Android) {
			LogUtils.Log ("OpenAppByAction : " + action);
			GetJavaObject ().Call ("openAppByAction", action);
		}
	}

	
	public bool IsInstall(string packageName) {
		if (Application.platform == RuntimePlatform.Android) {
			bool result = GetJavaObject ().Call<bool> ("isInstall", packageName);
			if (result) {
				return IsEnabled (packageName);
			} else {
				return false; 
			}
		} else {
			return false;
		}
	}

	
	public bool IsEnabled(string packageName) {
		return GetJavaObject ().Call<bool> ("isEnabled", packageName);
	}

	
	public void OpenBrowserByUrl(string url) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("openBrowserByUrl", url);
		}
	}

	
	public void OpenBrowserByUrlByX5(string url) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("openBrowserByUrlByX5", url);
		}
	}

	
	public void StartStoreAppDetail(string mid,string packageName) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startStoreAppDetail", mid, packageName);
		}
	}

	
	public void StartStoreCategory(int firstId,int secondId) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startStoreCategory", firstId, secondId);
		}
	}

	
	public void StartGalleryPalyer(string mid,string title,string small,string large){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startGalleryPalyer", mid, title, small, large);
		}
	}

	
	public void StartGalleryCategory(string mid){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startGalleryCategory", mid);
		}
	}

	
	public void StartGalleryTheme(string mid){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startGalleryTheme", mid);
		}
	}

	
	public void StartVRWingCategory(string firstHierarchy,string secondHierarchy){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startVRWingCategory", firstHierarchy, secondHierarchy);
		}
	}

	
	public void StartVRWingTheme(string mid){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startVRWingTheme", mid);
		}
	}

	
	public void StartVideo(string path, string mid, string itemId,
		string videoType, string title) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startVideo", path, mid, itemId, videoType, title);
		}
	}

    
    public void PlayVideo(string path, string mid, string itemId,
        string videoType, string title,int key)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GetJavaObject().Call("playVideo", path, mid, itemId, videoType, title, key);
        }
    }

    
    public void StartSubSettings(string category) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startSubSettings", category);
		}
	}

	
	public void SendBroadcastReboot() {
		GetJavaObject ().Call ("sendBroadcastReboot");
	}
		
}
