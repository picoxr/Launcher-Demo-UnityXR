using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipDialog : BaseDialog {

	private static Transform uiRoot;
	private static TipDialog instance;
	public Button button;
	public Image image;
	public RectTransform textTransform;
	public RectTransform bgTransform;
	private bool toggle = false;
	private int flag;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (flag < 7) {
			flag++;
			bgTransform.sizeDelta = new Vector2(bgTransform.sizeDelta.x, textTransform.sizeDelta.y + 410f);
		}
	}

	public static void Show ()
	{
		DialogManager.Instance.ClosePrevDialog ();
		if (uiRoot == null) {
			uiRoot = GameObject.Find ("UI Root").transform;
		}
		if (instance == null) {
			GameObject go = Resources.Load ("Prefab/TipDialog") as GameObject;
			if (go != null) {
				GameObject obj = Instantiate (go);
				obj.transform.parent = uiRoot;
				obj.transform.localScale = 1.0f * Vector3.one;
				obj.transform.localPosition = new Vector3 (0f, 0f, -5f);
				instance = obj.GetComponent<TipDialog> ();
			}
		}
		instance.gameObject.SetActive(true);
		DialogManager.Instance.Show (instance);
	}

	public void ToggleClick()
	{
		if (!toggle) {
			toggle = true;
			image.sprite = Resources.Load("Image/check_pressed", typeof(Sprite)) as Sprite;
		} else {
			toggle = false;
			image.sprite = Resources.Load("Image/check_normal", typeof(Sprite)) as Sprite;
		}
	}

	public void ButtonClick ()
	{
		if (toggle) {
			LogUtils.Log ("不再提示");
			OtherUtils.Instance.PutIntDataToSp (Constant.SP_SUN_TIP, Constant.SP_SUN_TIP_VALUE);
		} else {
			LogUtils.Log ("还会提示");
		}
		Close ();
	}

	public static void Close()
	{
		instance.toggle = false;
		instance.flag = 0;
		instance.gameObject.SetActive(false);
		DialogManager.Instance.Close ();
	}

}
