using UnityEngine;
using System.Collections;
using System.IO;

public class LauncherUtils
{
	
	public static string GetLanguage ()
	{
		string result = Localization.language;
		if (Localization.language == "zh") {
			result = "cn";
		}
		return result;
	}

	
	public static string GetDisplayId ()
	{
		return GetSystemProperties("ro.pui.build.version", "2.4.0");
	}

	
	public static string GetInternalVersion ()
	{
		return GetSystemProperties("ro.picovr.internal.version", "C086_RF01_BV2.0_SV0.88_20180120_B113");
	}

	
	public static string GetBuildProduct ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass jc = new AndroidJavaClass ("android.os.Build");
			string device = jc.GetStatic<string> ("PRODUCT");
			if (!string.IsNullOrEmpty (device) && device == "Pico Neo DK") {
				device = "Falcon";
			}
			return device;
		}
		return "A7210";
	}

	
	public static string GetSnCode ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass jc = new AndroidJavaClass ("android.os.Build");
			string sn = jc.GetStatic<string> ("SERIAL");
			return sn;
		}
		return "dev000000000wayne";
	}


	public static string GetImageCachePath ()
	{
		return imageCachePath;
	}
	private static string imageCachePath = "";
	public static void SetImageCachePath(string path)
	{
		imageCachePath = path;
	}

	
	
	public static string GetHomeAppField ()
	{
		string appField = "HomeApp";
		string countryCode = OtherUtils.Instance.GetCountryCode ();
		string productName = GetBuildProduct ();
		if (productName == "A7510_KT") {
			appField = "HomeApp_KT";
		} else {
			if (countryCode == "CN") {
				appField = "HomeApp";
			} else {
				appField = "HomeApp_US";
			}
		}
		LogUtils.Log ("GetHomeAppField--->countryCode = " + countryCode + ", productName = " + productName + ", appField = " + appField);
		return appField;
	}

	
	public static string GetHomeRecField ()
	{
		string homeField = "HomeRec";
		string countryCode = OtherUtils.Instance.GetCountryCode ();
		string productName = GetBuildProduct();
		if (productName == "A7510_KT") {
			homeField = "HomeRec_KT";
		} else {
			if (countryCode == "CN") {
				homeField = "HomeRec";
			} else {
				homeField = "HomeRec_US";
			}
		}
		LogUtils.Log ("GetHomeRecField--->countryCode = " + countryCode + ", productName = " + productName + ", homeField = " + homeField);
		return homeField;
	}

	
	public static string GetRecentKey()
	{
		string key = Constant.PREFS_KEY + "_" + OtherUtils.Instance.GetCountryCode();
		LogUtils.Log ("RecentKey = " + key);
		return key;
	}

	
	public static bool IsControllerConnected ()
	{
		return true;
		//Annotation
		//Pvr_UnitySDKAPI.Controller.UPvr_GetControllerState (0) == Pvr_UnitySDKAPI.ControllerState.Connected ||
		//Pvr_UnitySDKAPI.Controller.UPvr_GetControllerState (1) == Pvr_UnitySDKAPI.ControllerState.Connected;
	}


	public static string GetSystemProperties(string key,string def)
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass jc = new AndroidJavaClass ("android.os.SystemProperties");
			string value = jc.CallStatic<string> ("get",key,def);
			return value;
		}
		return def;
	}


	public static void DoSomething(BaseModel model)
	{
		if (model == null) {
			LogUtils.LogError ("BaseModel is null !!!");
			return;
		}
		Debug.Log (model.ToString());
		switch (model.dataType) {
		case DataType.VIDEO:
			OpenVideo(model);
			break;
		case DataType.GAME:
			OpenStoreGame(model);
			break;
		case DataType.URL:
			if (model.packageName.Equals (Constant.X5_PACKAGE_NAME) && AppUtils.Instance.IsInstall (Constant.X5_PACKAGE_NAME)) {
				AppUtils.Instance.OpenBrowserByUrlByX5 (model.url);
			} else {
				AppUtils.Instance.OpenBrowserByUrl (model.url);
			}
			break;
		case DataType.GALLERY_CATEGORY:
			AppUtils.Instance.StartGalleryCategory(model.mid.ToString());
			break;
		case DataType.GALLERY_IMAGE:
			AppUtils.Instance.StartGalleryPalyer(model.mid.ToString(), model.title, model.imageUrl, model.url);
			break;
		case DataType.GALLERY_THEME:
			AppUtils.Instance.StartGalleryTheme(model.mid.ToString());
			break;
		case DataType.WING_CATEGORY:
			AppUtils.Instance.StartVRWingCategory(model.pid.ToString(), model.mid.ToString());
			break;
		case DataType.WING_THEME:
			AppUtils.Instance.StartVRWingTheme(model.pid.ToString());
			break;
		case DataType.APP:
			OpenApp(model);
			gotoPUIApp(model.packageName,model.className);
			break;
		case DataType.RECENT:
			break;
		case DataType.SYSTEM:
			
			OpenSystemUpdate();
			break;
		case DataType.STORE_CATEGORY:
			
			AppUtils.Instance.StartStoreCategory(model.mid, model.pid);
			break;
		}
	}

	
	public static void OpenVideo(BaseModel model)
	{
		string path = model.url;
		if (File.Exists (path)) {
			AppUtils.Instance.StartVideo (path, model.mid.ToString (), "", model.videoType.ToString (), model.title);
		} else {
			if (!OtherUtils.Instance.GetNetStatus ()) {
				Toast.Show (Localization.Get ("Net_TimeOut"));
			} else {
				AppUtils.Instance.StartVideo (path, model.mid.ToString (), "", model.videoType.ToString (), model.title);
			}
		}
	}

	
	public static void OpenStoreGame(BaseModel model)
	{
		string packageName = model.packageName;
		bool isInstalled = AppUtils.Instance.IsInstall(packageName);
		if(isInstalled){
			AppUtils.Instance.OpenAppByPackageName (packageName);
		}else{
			if (!OtherUtils.Instance.GetNetStatus ()) {
				Toast.Show (Localization.Get ("Net_TimeOut"));
			} else {
				AppUtils.Instance.StartStoreAppDetail (model.mid.ToString(), packageName);
			}
		}
	}

	/*
	 * Open default app of system
	 */
	public static void OpenApp(BaseModel model)
	{
		string packageName = model.packageName;
		if(packageName.Contains("/")){
			string[] splits = packageName.Split ('/');
			string s1 = splits [0];
			string s2 = splits [1];
			bool isInstalled = AppUtils.Instance.IsInstall (s1);
			if (isInstalled) {
				AppUtils.Instance.OpenAppByComponentName (s1, s2);
			}
		}else{
			bool isInstalled = AppUtils.Instance.IsInstall (packageName);
			if (isInstalled) {
				AppUtils.Instance.OpenAppByPackageName (packageName);
			}
		}
	}
	
	public static void gotoPUIApp(string pkgName, string clsName)
	{
        AndroidJavaClass jcPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject joActivity = jcPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject joIntent = new AndroidJavaObject("android.content.Intent", "pvr.intent.action.ADAPTER");
        joIntent.Call<AndroidJavaObject>("setPackage", "com.pvr.adapter");
        joIntent.Call<AndroidJavaObject>("putExtra", "way", 0);
        joIntent.Call<AndroidJavaObject>("putExtra", "args", new string[] { pkgName, clsName });
        joActivity.Call<AndroidJavaObject>("startService", joIntent);

     
    }
	
	public static void OpenSystemUpdate()
	{
		if (OtherUtils.Instance.HasAppUpdate() == Constant.HAS_APP_UPDATE) {
			OtherUtils.Instance.StartVersionManager ();
		} else {
			AppUtils.Instance.OpenAppByComponentName (Constant.SYSTEM_UPDATE_PACKAGE_NAME, Constant.SYSTEM_UPDATE_ACTIVITY_NAME);
		}
	}

	public static string WrapText(UnityEngine.UI.Text text, string content, int lines = 1, string endValue = "...")
	{
		string outstr = "";
		if (string.IsNullOrEmpty(content))
			return "";

		Font font = text.font;
		int size = text.fontSize;
		CharacterInfo characterInfo = new CharacterInfo();

		char[] endValueChar = endValue.ToCharArray();
		int endValueWidth = 0;

		if(endValueChar.Length > 0) 
			foreach(char c in endValueChar)
			{
				font.GetCharacterInfo(c, out characterInfo, size, FontStyle.Normal);
				endValueWidth += characterInfo.advance;
			}

		int textWidth = (int)text.rectTransform.sizeDelta.x;
		char[] contentChar = content.ToCharArray();

		int tempWidth = 0;
		foreach(char c in contentChar)
		{
			font.GetCharacterInfo(c, out characterInfo, size, FontStyle.Normal);
			tempWidth += characterInfo.advance;
			if(lines == 1) 
			{
				if (tempWidth <= textWidth - endValueWidth)  
					outstr += c;
				else   
				{
					outstr += endValue;
					break;
				}
			}
			else  
			{
				if(tempWidth <= textWidth)  
				{
					outstr += c;
					if (tempWidth == textWidth) 
					{
						tempWidth = 0;
						lines -= 1;
					}
				}
				else  
				{
					outstr += c;
					tempWidth = characterInfo.advance;
					lines -= 1;
				}
			}
		}
		return outstr;
	}

	
	public static bool getBindVoiceStatus()
	{
		bool status = false;
		#if UNITY_EDITOR
		status = true;
		#elif UNITY_ANDROID
		PicoUnityActivity.CallObjectMethod<bool> (ref status, "getBindVoiceStatus");
		#endif
		LogUtils.Log ("Bind VoiceAssistant Status : " + status);
		return status;
	}

}
