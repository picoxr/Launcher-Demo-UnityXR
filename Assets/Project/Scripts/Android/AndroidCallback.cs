using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AndroidCallback : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		InputController.GetInstance ().AddListener (ListenerEventType.APP, OnAllScreenBack);
		InputController.GetInstance ().AddListener (ListenerEventType.CANCEL, OnAllScreenBack);
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.X)) {
			OnAllScreenBack ();
		}
		#endif
	}

	void OnDestory()
	{
		InputController.GetInstance ().RemoveListener (ListenerEventType.APP, OnAllScreenBack);
		InputController.GetInstance ().RemoveListener (ListenerEventType.CANCEL, OnAllScreenBack);
	}

	/*
	* onNewIntent()
	*/
	public void OnNewIntent (string where)
	{
		LogUtils.Log ("OnNewIntent where = " + where);
		if (DialogManager.Instance.DialogIsShow) {
			//如果有dialog显示
			if (DialogManager.Instance.BaseDialog.name == "UpdateDialog(Clone)") {
				//如果显示的是强制升级dialog，不做处理
				return;
			} else {
				//如果有dialog显示，先关闭dialog，再做后续处理
				DialogManager.Instance.ClosePrevDialog ();
				DialogManager.Instance.Close ();
			}
		}
	}

	/*
	* onKeyDown()
	* 3:home 4:back
	*/
	public void OnKeyDown (string keyCode)
	{
		LogUtils.Log ("OnKeyDown keyCode = " + keyCode);
		if (keyCode.Equals ("4")) {
			OnAllScreenBack ();
		}
	}

	/*
	 * 设备返回键、手柄返回键均触发该方法。
	*/
	private void OnAllScreenBack()
	{
		if (DialogManager.Instance.DialogIsShow) {
			//如果有dialog显示
			if (DialogManager.Instance.BaseDialog.name == "UpdateDialog(Clone)") {
				//如果显示的是强制升级dialog，不做处理
				return;
			} else {
				//如果显示的是其他dialog，则关闭dialog
				DialogManager.Instance.ClosePrevDialog ();
				DialogManager.Instance.Close ();
				return;
			}
		}
	}

	/*
	 * 网络连接状态
	*/
	public void OnNetworkStateChanged (string state)
	{
		StatusManager.Instance.UpdateNetworkState (state);
	}

	/*
	 * wifi开关状态
	*/
	public void OnWifiStateChanged (string state)
	{
		StatusManager.Instance.UpdateWifiState (state);
	}

	/*
	* wifi信号强弱变化
	*/
	public void OnRSSIChanged (string level)
	{
		StatusManager.Instance.UpdateWifiLevel (level);
	}

	/*
	* 时间变化
	*/
	public void OnTimeChanged (string time)
	{
		StatusManager.Instance.UpdateTime (time);
	}

	/*
	 * 蓝牙开关状态
	*/
	public void  OnBluetoothStateChanged (string state)
	{
		StatusManager.Instance.UpdateBluetoothState (state);
	}

	/*
	 * 蓝牙连接状态
	*/
	public void  OnBluetoothConnectChanged (string profile)
	{
		StatusManager.Instance.UpdateBluetoothConnectState (profile);
	}

	/*
	 * 电池状态改变
	*/
	public void OnBatteryStatusChanged (string status)
	{
		StatusManager.Instance.UpdateBatteryState (status);
	}

	/*
	 * 电量发生变化
	*/
	public void OnBatteryLevelChanged (string level)
	{
		StatusManager.Instance.UpdateBatteryLevel (level);
	}

	/*
	* 音量发生变化
	*/
	public void OnVolumeChanged (string volume)
	{
		StatusManager.Instance.UpdateVolume (volume);
	}

	/*
	 * 系统更新发来的强制升级广播
	 */
	public void OnForceUpdate (string info)
	{
		UpdateDialog.Show(info);
	}

	/*
	 * 未读信息
	*/
	public void UpdateBadge (string info)
	{
		Debug.Log ("UpdateBadge------->" + info);
		if (!string.IsNullOrEmpty (info) && info.Contains ("/")) {
			string[] infos = info.Split ('/');
			if (infos != null && infos.Length != 3)
				return;
			string packageName = infos [0];
			if (packageName.Equals (Constant.SYSTEM_UPDATE_PACKAGE_NAME)) {
				//显示系统升级Dialog
				Dialog.Show(Localization.Get("Update_Title"),Localization.Get("Dialog_Ok"),Localization.Get("Dialog_Cancel"),()=> {
					LauncherUtils.OpenSystemUpdate();
				},()=> {});
			}
		}
	}

	/*
	 * htc发送的错误广播
	*/
	public void OnHtcErrorCode (string errorCode)
	{
		Debug.Log ("OnHtcErrorCode------->" + errorCode);
		if (errorCode == "-1") {
			Dialog.Show (Localization.Get ("DENY_NOT_LICENSED"), Localization.Get ("Dialog_Yes"), Localization.Get ("Dialog_No"), () => {
				AppUtils.Instance.OpenAppByPackageName(Constant.VIVEPORT_PACKAGE_NAME);
			}, () => {
			});
		} else if (errorCode == "-2") {
			Dialog.Show (Localization.Get ("DENY_RETRY"), Localization.Get ("Dialog_Ok"), () => {
			});
		} else if (errorCode == "-3") {
			Dialog.Show (Localization.Get ("DENY_NOT_SIGN_IN"), Localization.Get ("Dialog_Yes"), Localization.Get ("Dialog_No"), () => {
				AppUtils.Instance.OpenAppByPackageName(Constant.VIVEPORT_PACKAGE_NAME);
			}, () => {
			});
		} else if (errorCode == "-4") {
			Dialog.Show (Localization.Get ("DENY_NO_SERVICE"), Localization.Get ("Dialog_Ok"), () => {
			});
		} else if (errorCode == "-5") {
			Dialog.Show (Localization.Get ("DENY_LICENSE_EXPIRED"), Localization.Get ("Dialog_Yes"), Localization.Get ("Dialog_No"), () => {
				AppUtils.Instance.OpenAppByPackageName(Constant.VIVEPORT_PACKAGE_NAME);
			}, () => {
			});
		}
	}
		
	/*
	 * app安装、卸载的回调
	 * action = 0，安装
	 * action = 1，卸载
	*/
	public void OnPackageChanged (string action)
	{
		//如果进入过app界面，刷新列表；
		//if (AppScreen.goIntoAppScreen) {
		//	//此判断以防未进入过app界面，刷新列表报空；
		//	AppManager.Instance.GetAllAppData ();
		//}
		//LogUtils.Log ("action--->" + action);
		////刷新最近使用
		//string[] strings = action.Split (',');
		//if (strings.Length == 3) {
		//	if (strings [0].Equals ("0")) {
		//		RecentManager.Instance.AddHistory (strings [1], strings [2]);
		//	} else if (strings [0].Equals ("1")) {
		//		RecentManager.Instance.RemoveHistory (strings [1]);
		//	}
		//}
	}

	/*
	* 系统更新，存储空间不足
	*/
	public void OnEnospcReceiver(string content)
	{
		Dialog.Show (content, Localization.Get ("NOT_Clean"), Localization.Get ("Clean"), () => {
		}, () => {
			if(AppUtils.Instance.IsInstall(Constant.FILE_MANAGER_PACKAGE_NAME)){
				AppUtils.Instance.OpenAppByPackageName(Constant.FILE_MANAGER_PACKAGE_NAME);
			}else{
				AppUtils.Instance.OpenAppByPackageName(Constant.FILE_MANAGER2_PACKAGE_NAME);
			}
		});
	}

	/*
	 * 开始语音识别
	 */
	public void OnRecordStart(){
		VoiceManager.Instance.OnRecordStart ();
	}

	/*
	 * 结束语音识别
	 */
	public void OnRecordStop(){
		VoiceManager.Instance.OnRecordStop ();
	}

	/*
	 * 实时音量变化
	 */
	public void OnVolume(string volume){
		VoiceManager.Instance.OnVolume (volume);
	}

	/*
	 * 语音识别结果
	 */
	public void OnStreaming(string msg){
		VoiceManager.Instance.OnStreaming (msg);
	}

	/*
	 * NLU识别结果
	 */
	public void OnRecognizeResult(string msg){
		VoiceManager.Instance.OnRecognizeResult (msg);
	}

	/*
	 * 流媒体开始播放
	 */
	public void OnAudioPlayStart(){
		VoiceManager.Instance.OnAudioPlayStart ();
	}

	/*
	 * 流媒体播放停止
	 */
	public void OnAudioPlayStop(){
		VoiceManager.Instance.OnAudioPlayStop ();
	}

	/*
	 * 流媒体播放完成
	 */
	public void OnAudioPlayComplete(){
		VoiceManager.Instance.OnAudioPlayComplete ();
	}

	/*
	 * 开始TTS
	 */
	public void OnTtsStart(){
		VoiceManager.Instance.OnTtsStart ();
	}

	/*
	 * 停止TTS
	 */
	public void OnTtsStop(){
		VoiceManager.Instance.OnTtsStop ();
	}

	/*
	 * 语音识别错误回调
	 */
	public void OnError(string errorCode){
		VoiceManager.Instance.OnError (errorCode);
	}

	/*
	 * 打开app的回调
	 */
	public void OnOpenApp(string msg){
		VoiceManager.Instance.OnOpenApp (msg);
	}

}
