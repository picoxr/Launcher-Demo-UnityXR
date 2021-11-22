using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VoiceManager : MonoBehaviour
{

	public CanvasGroup startPanelCanvas;

	public CanvasGroup voicePanelCanvas;
	
	public GameObject startEffectObj;
	
	public Image voiceImage;
	public Button voiceBtn;
	public PositionAnimation voiceAnim;
	
	public SpeakAnimation speakEffect;
	
	public GameObject ttsEffectObj;
	
	public GameObject audioEffectObj;
	
	public Text resultText;
	
	public GameObject stopBtnObj;
	
	public AudioSource audioSource;
	
	private VoiceType currentVoiceType = VoiceType.UNKNOWN;
	private enum VoiceType
	{
		UNKNOWN,
		RECORD,
		TTS,
		AUDIO,
	}
	
	private bool isFocus = true;
	
	private Tweener startEffectTweener;
	private Tweener speakEffectTweener;

	private static VoiceManager instance;
	public static VoiceManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<VoiceManager> ();
			}
			return instance;
		}
	}

	// Use this for initialization
	void Start ()
	{
		resultText.text = GetTipContent();
	}
	
	// Update is called once per frame
//	void Update ()
//	{
//		
//	}

	void OnApplicationFocus(bool focus)
	{
		isFocus = focus;
	}

	public void WakeUp ()
	{
		int sp_tmall_protocol_value = OtherUtils.Instance.GetIntDataFromSp (Constant.SP_TMALL_PROTOCOL_KEY, Constant.SP_TMALL_PROTOCOL_DEF_VALUE);
		LogUtils.Log ("sp_tmall_protocol_value = "+sp_tmall_protocol_value);
		if (sp_tmall_protocol_value != Constant.SP_TMALL_PROTOCOL_VALUE) {
			VoiceDialog.Show ();
			return;
		}
		if (!OtherUtils.Instance.GetNetStatus ()) {
			
			Toast.Show (Localization.Get ("Net_TimeOut"));
			
			PicoUnityActivity.CallObjectMethod ("wakeUp");
		} else {
			PicoUnityActivity.CallObjectMethod ("wakeUp");
			SwitchPanel (true);
		}
	}

	
	public void WakeUp2 ()
	{
		if (!OtherUtils.Instance.GetNetStatus ()) {
			Toast.Show (Localization.Get ("Net_TimeOut"));
			PicoUnityActivity.CallObjectMethod ("wakeUp");
		} else {
			if (currentVoiceType == VoiceType.TTS) {
				PicoUnityActivity.CallObjectMethod ("stopTTS");
			}
			PicoUnityActivity.CallObjectMethod ("wakeUp");
		}
	}

	
	public void StopAudio ()
	{
		PicoUnityActivity.CallObjectMethod ("stopAudio");
	}

	
	public void CloseVoicePanel ()
	{
		LogUtils.Log ("CloseVoicePanel");
		if (currentVoiceType == VoiceType.TTS) {
			PicoUnityActivity.CallObjectMethod ("stopTTS");
		}else if(currentVoiceType == VoiceType.RECORD) {
			PicoUnityActivity.CallObjectMethod ("vadEnd");
		}
		SwitchPanel (false);
	}

	
	private void SwitchPanel (bool isShowVoicePanel)
	{
		if (isShowVoicePanel) {
			if (!voicePanelCanvas.gameObject.activeSelf) {
				voicePanelCanvas.gameObject.SetActive (true);
				SwitchPanelAnim (startPanelCanvas, 0.0f).OnComplete (() => startPanelCanvas.gameObject.SetActive (false));
				SwitchPanelAnim (voicePanelCanvas, 1.0f);
				voicePanelCanvas.transform.DOLocalRotate (new Vector3 (30.0f, 0.0f, 0.0f), 0.3f);
				audioSource.Play ();
			}
		} else {
			if (!startPanelCanvas.gameObject.activeSelf) {
				startPanelCanvas.gameObject.SetActive (true);
				voicePanelCanvas.transform.DOLocalRotate (new Vector3 (0.0f, 0.0f, 0.0f), 0.3f);
				SwitchPanelAnim (voicePanelCanvas, 0.0f).OnComplete (() => voicePanelCanvas.gameObject.SetActive (false));
				SwitchPanelAnim (startPanelCanvas, 1.0f);
				audioSource.Play ();
			}
		}

	}

	private Tween SwitchPanelAnim (CanvasGroup canvasGroup, float value, float timer = 0.5f)
	{
		return DOTween.To (() => canvasGroup.alpha, x => canvasGroup.alpha = x, value, timer);
	}

	public void OnRecordStart ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnRecordStart");
		currentVoiceType = VoiceType.RECORD;
		resultText.gameObject.SetActive (true);
		stopBtnObj.SetActive (false);
		audioEffectObj.SetActive (false);
		startEffectObj.SetActive (true);
		speakEffect.gameObject.SetActive (true);
		voiceImage.color = new Color (114f/255f, 72f/255f, 189f/255f);
		voiceBtn.enabled = false;
		voiceAnim.enabled = false;
		ttsEffectObj.SetActive (false);
	}

	public void OnRecordStop ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnRecordStop");
		currentVoiceType = VoiceType.UNKNOWN;
		resultText.gameObject.SetActive (true);
		stopBtnObj.SetActive (false);
		audioEffectObj.SetActive (false);
		startEffectTweener = startEffectObj.transform.DOScale(0.1f, 1.0f).OnComplete (() => ResetStartEffect());
		speakEffectTweener = speakEffect.transform.DOScale(0.1f, 1.0f).OnComplete (() => ResetSpeakEffect());
		voiceImage.color = Color.white;
		voiceBtn.enabled = true;
		voiceAnim.enabled = true;
		ttsEffectObj.SetActive (false);
	}

	
	private void ResetStartEffect()
	{
		startEffectObj.SetActive (false);
		startEffectObj.transform.localScale = Vector3.one;
	}
	
	private void ResetSpeakEffect()
	{
		speakEffect.gameObject.SetActive (false);
		speakEffect.transform.localScale = Vector3.one;
	}

	public void OnVolume (string volume)
	{
		if (!isFocus) {
			return;
		}
		if (speakEffect.gameObject.activeSelf) {
			speakEffect.VolumeAnim (int.Parse (volume));
		}
	}

	public void OnStreaming (string msg)
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnStreaming, msg = " + msg);
		if (msg.Contains ("|")) {
			string[] ss = msg.Split ('|');
			if (ss [1].Equals ("true")) {
				
			} else {
				resultText.text = ss[0];
			}
		} else {
			resultText.text = msg;
		}
	}

	public void OnRecognizeResult (string msg)
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnRecognizeResult, msg = " + msg);
	}

	public void OnAudioPlayStart ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnAudioPlayStart");
		currentVoiceType = VoiceType.AUDIO;
		resultText.gameObject.SetActive (true);
		stopBtnObj.SetActive (true);
		audioEffectObj.SetActive (true);
		startEffectObj.SetActive (false);
		speakEffect.gameObject.SetActive (false);
		voiceImage.color = Color.white;
		voiceBtn.enabled = true;
		voiceAnim.enabled = true;
		ttsEffectObj.SetActive (false);
	}

	public void OnAudioPlayStop ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnAudioPlayStop");
		ResetUIEffect ();
	}

	public void OnAudioPlayComplete ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnAudioPlayComplete");
		ResetUIEffect ();
	}

	public void OnTtsStart ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnTtsStart");
		currentVoiceType = VoiceType.TTS;
		resultText.gameObject.SetActive (false);
		stopBtnObj.SetActive (false);
		audioEffectObj.SetActive (false);
		startEffectObj.SetActive (false);
		speakEffect.gameObject.SetActive (false);
		voiceImage.color = Color.white;
		voiceBtn.enabled = true;
		voiceAnim.enabled = true;
		ttsEffectObj.SetActive (true);
	}

	public void OnTtsStop ()
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnTtsStop");
		ResetUIEffect ();
	}

	public void OnError (string errorCode)
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnError, errorCode = " + errorCode);
		ResetUIEffect ();
		SwitchPanel (false);
	}

	public void OnOpenApp (string msg)
	{
		if (!isFocus) {
			return;
		}
		LogUtils.Log ("OnOpenApp, msg = " + msg);
//		ResetUIEffect ();
//		SwitchPanel (false);
		StartCoroutine(OpenApp(msg));
	}

	IEnumerator OpenApp(string msg)
	{
		yield return new WaitForSeconds(0.5f);
		if (msg.Contains ("|")) {
			string[] ss = msg.Split ('|');
			string packageName = ss [0];
			string activityName = ss [1];
			AppUtils.Instance.OpenAppByComponentName (packageName, activityName);
		}
	}


	private void ResetUIEffect()
	{
		currentVoiceType = VoiceType.UNKNOWN;
		resultText.text = GetTipContent();
		resultText.gameObject.SetActive (true);
		stopBtnObj.SetActive (false);
		audioEffectObj.SetActive (false);
		startEffectObj.SetActive (false);
		startEffectObj.transform.localScale = Vector3.one;
		if (startEffectTweener != null) {
			startEffectTweener.Kill ();
		}
		speakEffect.gameObject.SetActive (false);
		speakEffect.transform.localScale = Vector3.one;
		if (speakEffectTweener != null) {
			speakEffectTweener.Kill ();
		}
		voiceImage.color = Color.white;
		voiceBtn.enabled = true;
		voiceAnim.enabled = true;
		ttsEffectObj.SetActive (false);
	}

	private string GetTipContent()
	{
		string[] tips = {"我想听抖音神曲", "讲个笑话",
			"大自然的声音", "历史上的今天", "打开在线影院",
			"马云语录", "明天北京天气怎么样"};
		int random1 = Random.Range (0, tips.Length);
		int random2 = 0;
		while (true) {
			random2 = Random.Range (0, tips.Length);
			if (random1 != random2) {
				break;
			}
		}
		return "<color=#333333>你可以这样问我：</color>\n" + "\"" + tips[random1] + "\"" +"\n" + "\"" + tips[random2] + "\"";
	}

}
