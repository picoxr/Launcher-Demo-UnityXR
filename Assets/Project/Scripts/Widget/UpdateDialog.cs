using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UpdateDialog : BaseDialog {

	private static Transform uiRoot;
	private static UpdateDialog instance;
	public Text versionText;
	public Text sizeText;
	public Text timeText;
	public Text logText;
	public Button button;

	// Use this for initialization
	void Start () {
		button.onClick.AddListener (delegate() {
			buttonClick();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void buttonClick(){
		int battery = OtherUtils.Instance.GetBattery();
		if (battery < 40) {
			Toast.Show (Localization.Get("poorBattery"));
			return;
		}
		AppUtils.Instance.SendBroadcastReboot();
		Close ();
	}

	public static void Close ()
	{
		instance.gameObject.SetActive (false);
		DialogManager.Instance.Close ();
	}

	public static void Show (string info)
	{
		DialogManager.Instance.ClosePrevDialog ();
		if (uiRoot == null) {
			uiRoot = GameObject.Find ("UI Root").transform;
		}
		if (instance == null) {
			GameObject go = Resources.Load ("Prefab/UpdateDialog") as GameObject;
			if (go != null) {
				GameObject obj = Instantiate (go);
				obj.transform.parent = uiRoot;
				obj.transform.localScale = 1.0f * Vector3.one;
				obj.transform.localPosition = new Vector3 (0f, 0f, -5f);
				instance = obj.GetComponent<UpdateDialog> ();
			}
		}
		instance.gameObject.SetActive (true);
		DialogManager.Instance.Show (instance);
		SetUI (info);
	}

	private static void SetUI(string info){
		if (string.IsNullOrEmpty (info))
			return;
		if (!info.Contains("error")){
			LitJson.JsonData jd = LitJson.JsonMapper.ToObject(info);
			LitJson.JsonData item = jd["version"];
			instance.versionText.text = Localization.Get("newVersion") + item ["versionname"].ToString () + Localization.Get("majorUpdate");//版本号
			LitJson.JsonData timeItem = item["publishAt"];
			instance.sizeText.text = Localization.Get ("vesrionSize") + item ["size"].ToString () + "M";
			instance.timeText.text = Localization.Get ("pulishTime") + timeItem ["date"].ToString ();
			instance.logText.text = item ["releasenote"].ToString ();
		}
	}

}
