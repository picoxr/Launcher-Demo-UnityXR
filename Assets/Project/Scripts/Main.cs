using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LogUtils.Log ("===================================== VRLauncher Unity Start =====================================");
		
		string language = OtherUtils.Instance.GetLanguage();
		string country = OtherUtils.Instance.GetCountry ();
		LogUtils.Log (language+"<===>"+country);
		if (country == "TW") {
			Localization.language = language + "-" + country;
		} else {
			Localization.language = language;
		}
		
		LauncherUtils.SetImageCachePath(Application.persistentDataPath + "/ImageCache/");
		
		//ScreenManager.Instance.InitScreen();
		//bind sdk service
		if (Application.platform == RuntimePlatform.Android) {
			StartCoroutine (StartBind ());
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	
	public void RemoveTasks()
	{
		LogUtils.Log ("RemoveTasks");
		PicoUnityActivity.CallObjectMethod ("removeAppTasks");
	}

	IEnumerator StartBind(){
		yield return new WaitForSeconds(1.0f);
		LogUtils.Log ("-----------------BindService");
		//Annotation
		//Pvr_ControllerManager.Instance.BindService ();
	}

}
