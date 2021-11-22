using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VoiceDialog : BaseDialog {

	private static Transform uiRoot;
	private static VoiceDialog instance;
	public Text contentText;
	public Text pageText;
	public Button downBtn;
	public PositionAnimation downAnim;
	public Button upBtn;
	public PositionAnimation upAnim;
	private float currentPosY = -10832.0f;
	private bool canScroll = true;
	private int currentPage = 1;

	// Use this for initialization
	void Start () {
		pageText.text = currentPage+"<color=#707070><size=22>/74</size></color>";
		downBtn.interactable = false;
		downAnim.enabled = false;
		upBtn.interactable = true;
		upAnim.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void Show ()
	{
		DialogManager.Instance.ClosePrevDialog ();
		if (uiRoot == null) {
			uiRoot = GameObject.Find ("UI Root").transform;
		}
		if (instance == null) {
			GameObject go = Resources.Load ("Prefab/VoiceDialog") as GameObject;
			if (go != null) {
				GameObject obj = Instantiate (go);
				obj.transform.parent = uiRoot;
				obj.transform.localScale = 1.0f * Vector3.one;
				obj.transform.localPosition = new Vector3 (0f, 0f, -5f);
				instance = obj.GetComponent<VoiceDialog> ();
			}
		}
		instance.gameObject.SetActive(true);
		DialogManager.Instance.Show (instance);
	}

	public static void Close()
	{
		instance.gameObject.SetActive (false);
		DialogManager.Instance.Close ();
	}

	public void Cancel()
	{
		Close ();
	}

	public void Ok()
	{
		Close ();
		OtherUtils.Instance.PutIntDataToSp (Constant.SP_TMALL_PROTOCOL_KEY, Constant.SP_TMALL_PROTOCOL_VALUE);
		VoiceManager.Instance.WakeUp ();
	}

	public void Up()
	{
		if (!canScroll) {
			return;
		}
		currentPage += 1;
		currentPosY += 300.0f;
		downBtn.interactable = true;
		downAnim.enabled = true;
		upBtn.interactable = true;
		upAnim.enabled = true;
		if (currentPosY >= 10828.0f) {
			currentPosY = 10828.0f;
			currentPage = 74;
			downBtn.interactable = true;
			downAnim.enabled = true;
			upBtn.interactable = false;
			upAnim.enabled = false;
		}
		canScroll = false;
		pageText.text = currentPage+"<color=#707070><size=22>/74</size></color>";
		contentText.transform.DOLocalMoveY(currentPosY, 0.3f).OnComplete (() => canScroll = true);
	}

	public void Down()
	{
		if (!canScroll) {
			return;
		}
		currentPage -= 1;
		currentPosY -= 300.0f;
		downBtn.interactable = true;
		downAnim.enabled = true;
		upBtn.interactable = true;
		upAnim.enabled = true;
		if (currentPosY <= -10832.0f) {
			currentPosY = -10832.0f;
			currentPage = 1;
			downBtn.interactable = false;
			downAnim.enabled = false;
			upBtn.interactable = true;
			upAnim.enabled = true;
		}
		canScroll = false;
		pageText.text = currentPage+"<color=#707070><size=22>/74</size></color>";
		contentText.transform.DOLocalMoveY(currentPosY, 0.3f).OnComplete (() => canScroll = true);
	}
}
