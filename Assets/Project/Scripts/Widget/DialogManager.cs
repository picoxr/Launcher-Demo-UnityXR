using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogManager : MonoBehaviour {

	private static DialogManager instance;
	public static DialogManager Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<DialogManager> ();
			}
			return instance;
		}
	}

	private CanvasGroup screneCanvas;
	public GameObject maskObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	/*
	* 当前是否有dialog显示
	*/
	private bool dialogIsShow;
	public bool DialogIsShow
	{
		get { return dialogIsShow; }
		set { dialogIsShow = value; }
	}

	/*
	* 当前正在显示的dialog
	*/
	private BaseDialog baseDialog;
	public BaseDialog BaseDialog
	{
		get { return baseDialog; }
		set { baseDialog = value; }
	}

	public void Show(BaseDialog dialog)
	{
		this.baseDialog = dialog;
		LogUtils.Log ("显示Dialog--->"+this.baseDialog.name);
		this.dialogIsShow = true;

        screneCanvas = GameObject.Find("HomeScreen").GetComponent<CanvasGroup>();

        if (screneCanvas != null) {
			screneCanvas.alpha = 0.2f;
//			screneCanvas.transform.localPosition = new Vector3 (0f, 0f, 200f);
			screneCanvas.transform.DOLocalMoveZ (200f, 0.2f);
			maskObj.SetActive (true);
		}
	}

	public void Close()
	{
		this.baseDialog = null;
		this.dialogIsShow = false;
		if (screneCanvas != null) {
			screneCanvas.alpha = 1.0f;
//			screneCanvas.transform.localPosition = Vector3.zero;
			screneCanvas.transform.DOLocalMoveZ (0f, 0.2f);
			maskObj.SetActive (false);
		}
		screneCanvas = null;
	}

	public void ClosePrevDialog()
	{
		if (this.baseDialog != null) {
			string dialogName = this.baseDialog.name;
			LogUtils.Log ("关闭Dialog--->"+dialogName);
			if (dialogName == "Dialog(Clone)") {
				Dialog.Close ();
			}else if (dialogName == "TipDialog(Clone)") {
				TipDialog.Close ();
			}else if (dialogName == "UpdateDialog(Clone)") {
				UpdateDialog.Close ();
			}else if (dialogName == "VoiceDialog(Clone)") {
				VoiceDialog.Close ();
			}
		}
	}

}
