using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Dialog : BaseDialog{

	private static Transform uiRoot;
	private static Dialog instance;
	public RectTransform bgTransform;
	public RectTransform titleTransform;
	public Text titleText;
	public Text button1Text;
	public Text button2Text;
	public Text button3Text;
	public GameObject button1Obj;
	public GameObject button2Obj;
	public GameObject button3Obj;
	private Action button1Action;
	private Action button2Action;
	private Action button3Action;
	private int flag;

	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	void Update (){
		if (flag < 7) {
			flag++;
			bgTransform.sizeDelta = new Vector2(bgTransform.sizeDelta.x, titleTransform.sizeDelta.y + 90f);
		}
	}

	public void Button1Click ()
	{
		if (button1Action != null)
			button1Action ();
		Close ();
	}

	public void Button2Click ()
	{
		if (button2Action != null)
			button2Action ();
		Close ();
	}

	public void Button3Click ()
	{
		if (button3Action != null)
			button3Action ();
		Close ();
	}

	public static void Close ()
	{
		instance.button1Action = null;
		instance.button2Action = null;
		instance.button3Action = null;
		instance.titleText.text = string.Empty;
		instance.bgTransform.sizeDelta = new Vector2(540f, 93f);
		instance.flag = 0;
		instance.button1Text.text = string.Empty;
		instance.button2Text.text = string.Empty;
		instance.button3Text.text = string.Empty;
		instance.gameObject.SetActive(false);
		DialogManager.Instance.Close ();
	}

	public static void Show (string title, string tip1, string tip2,
	                        Action action1, Action action2)
	{
		DialogManager.Instance.ClosePrevDialog ();
		if (uiRoot == null) {
			uiRoot = GameObject.Find ("UI Root").transform;
		}
		if (instance == null) {
			GameObject go = Resources.Load ("Prefab/Dialog") as GameObject;
			if (go != null) {
				GameObject obj = Instantiate (go);
				obj.transform.parent = uiRoot;
				obj.transform.localScale = 1.0f * Vector3.one;
				obj.transform.localPosition = new Vector3 (0f, 0f, -5f);
				instance = obj.GetComponent<Dialog> ();
			}
		} 
		instance.gameObject.SetActive(true);
		DialogManager.Instance.Show (instance);
		instance.button1Obj.SetActive (true);
		instance.button1Action = action1;
		instance.button2Obj.SetActive (true);
		instance.button2Action = action2;
		instance.button3Obj.SetActive (false);
		instance.button3Action = null;
		instance.titleText.text = title;
		instance.button1Text.text = tip1;
		instance.button2Text.text = tip2;
		instance.button3Text.text = string.Empty;
	}

	public static void Show (string title, string tip, Action action)
	{
		DialogManager.Instance.ClosePrevDialog ();
		if (uiRoot == null) {
			uiRoot = GameObject.Find ("UI Root").transform;
		}
		if (instance == null) {
			GameObject go = Resources.Load ("Prefab/Dialog") as GameObject;
			if (go != null) {
				GameObject obj = Instantiate (go);
				obj.transform.parent = uiRoot;
				obj.transform.localScale = 1.0f * Vector3.one;
				obj.transform.localPosition = new Vector3 (0f, 0f, -200f);
				instance = obj.GetComponent<Dialog> ();
			}
		}
		instance.gameObject.SetActive(true);
		DialogManager.Instance.Show (instance);
		instance.button1Obj.SetActive (false);
		instance.button1Action = null;
		instance.button2Obj.SetActive (false);
		instance.button2Action = null;
		instance.button3Obj.SetActive (true);
		instance.button3Action = action;
		instance.titleText.text = title;
		instance.button1Text.text = string.Empty;
		instance.button2Text.text = string.Empty;
		instance.button3Text.text = tip;
	}

}
