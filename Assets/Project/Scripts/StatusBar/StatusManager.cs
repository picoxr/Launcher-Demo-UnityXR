using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.XR.PXR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StatusManager : MonoBehaviour {

	private static StatusManager instance;
	public static StatusManager Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<StatusManager> ();
			}
			return instance;
		}
	}
	//Wifi Status
	private string m_wifiStatus;
	private Wifi wifi = new Wifi();
	public Image wifiImage;
	//Bluetooth Status
	private string m_bluetoothStatus;
	private Bluetooth bluetooth = new Bluetooth();
	public Image bluetoothImage;
	//Battery
	private Battery battery = new Battery();
	public Image batteryImage;
	public Text batteryText;
	//Time 
	private DateTime now;
	public Text timeText;
	private SystemTime systemTime = new SystemTime();

	private Volume volume = new Volume();
	public GameObject volumeObj;
	public Image volumeImage;
	private float startShowVolumeTime = 0.0f;

	public Image handleImage;

    public GameObject settingObj;

    public GridLayoutGroup mGrid;

	private AndroidJavaObject ajo;
	private AndroidJavaObject context;
	
	// Use this for initialization
	void Start () {
        // Controller Connection Delegations
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
        InputDevices.deviceConnected += InputDevices_deviceConnected;
		
        // Controller Icon refresh once at begining
        if (PXR_Input.IsControllerConnected(PXR_Input.Controller.LeftController) || PXR_Input.IsControllerConnected(PXR_Input.Controller.RightController))
        {
            UpdateControllerIcon(true);
        }
        if ((PXR_Input.IsControllerConnected(PXR_Input.Controller.LeftController) == false) && (PXR_Input.IsControllerConnected(PXR_Input.Controller.RightController) == false))
        {
            UpdateControllerIcon(false);
        }
		
		//Initialize Data
		wifi.NetworkState = (NetworkState)OtherUtils.Instance.GetNetworkConnectionState();
		wifi.WifiState = (WifiState)OtherUtils.Instance.GetWifiSwitchState();
		wifi.Level = OtherUtils.Instance.GetWifiLevel ().ToString();
		UpdateWifiUI ();
		bluetooth.BluetoothState = (BluetoothState)OtherUtils.Instance.GetBluetoothStatus ();
		UpdateBluetoothUI ();
		PXR_System.InitAudioDevice();

		//Icons and UI Refresh Coroutine
		StartCoroutine("WifiAndBluetoothIconsRefresh");
		StartCoroutine("TimeUIRefresh");
		StartCoroutine("BatteryUIRefresh");
	}

	//Controller Connected Delegate
	private void InputDevices_deviceConnected(InputDevice obj)
	{
		//Controller Icon Refresh
		UpdateControllerIcon(true);
		if (obj.name == "PicoXR Controller-Left")
		{
			GameObject.Find("LeftHand Controller").GetComponent<XRInteractorLineVisual>().enabled = true;
		}
		if (obj.name == "PicoXR Controller-Right")
		{
			GameObject.Find("RightHand Controller").GetComponent<XRInteractorLineVisual>().enabled = true;
		}
	}
	//Controller Disconnected Delegate
	private void InputDevices_deviceDisconnected(InputDevice obj)
	{
		//Controller Icon Refresh
		UpdateControllerIcon(false);
		if (obj.name == "PicoXR Controller-Left")
		{
			GameObject.Find("LeftHand Controller").GetComponent<XRInteractorLineVisual>().enabled = false;
		}
		if (obj.name == "PicoXR Controller-Right")
		{
			GameObject.Find("RightHand Controller").GetComponent<XRInteractorLineVisual>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		//if (startShowVolumeTime != 0.0f && Time.time - startShowVolumeTime > 1.0f)
		//{
		//	volumeObj.SetActive(false);
		//	startShowVolumeTime = 0.0f;
		//}
		////Time UI
		//systemTime.Time = OtherUtils.Instance.GetCurrentTime();
		//UpdateTimeUI();
		////Battery UI
		//battery.Level = OtherUtils.Instance.GetBattery().ToString();
		//UpdateBatteryUI();

	}
	void OnApplicationPause(bool pauseStatus){
		if (!pauseStatus) {
			UpdateTime (OtherUtils.Instance.GetCurrentTime());
		}
	}
	IEnumerator WifiAndBluetoothIconsRefresh()
    {
        while (true)
        {
			//Bluetooth Icon Status 
			m_bluetoothStatus = PXR_Plugin.System.UPxr_StateGetDeviceInfo(SystemInfoEnum.DEVICE_BLUETOOTH_STATUS);
			if (m_bluetoothStatus == "已连接")
			{
				bluetoothImage.sprite = Resources.Load("Image/bluetooth_on", typeof(Sprite)) as Sprite;
			}
			else if (m_bluetoothStatus == "未连接")
			{
				bluetoothImage.sprite = Resources.Load("Image/bluetooth_off", typeof(Sprite)) as Sprite;
			}
			//Wifi Icon Status 
			m_wifiStatus = PXR_Plugin.System.UPxr_StateGetDeviceInfo(SystemInfoEnum.DEVICE_WIFI_STATUS);
			if (m_wifiStatus == "已连接")
			{
				wifiImage.sprite = Resources.Load("Image/wifi_connected_4", typeof(Sprite)) as Sprite;
			}
			else if (m_wifiStatus == "未连接")
			{
				wifiImage.sprite = Resources.Load("Image/wifi_disconnected", typeof(Sprite)) as Sprite;
			}
			
			
			yield return new WaitForSeconds(1f);
		}
	}
	IEnumerator TimeUIRefresh()
    {
		now = DateTime.Now;
		if (now.Minute < 10)
		{
			timeText.text = string.Format("{0}:0{1}", now.Hour, now.Minute);
		}
		else
		{
			timeText.text = string.Format("{0}:{1}", now.Hour, now.Minute);
		}
		yield return new WaitForSeconds(60f - now.Second);
		while (true)
		{
			now = DateTime.Now;
			if (now.Minute < 10)
			{
				timeText.text = string.Format("{0}:0{1}", now.Hour, now.Minute);
			}
			else
			{
				timeText.text = string.Format("{0}:{1}", now.Hour, now.Minute);
			}
			yield return new WaitForSeconds(60f);
		}
	}
	IEnumerator BatteryUIRefresh()
    {
        while (true)
        {
			//Battery UI
			battery.Level = OtherUtils.Instance.GetBattery().ToString();
			UpdateBatteryUI();
			yield return new WaitForSeconds(100f);
		}
    }
	public void SetActive(bool isShow)
	{
		this.gameObject.SetActive (isShow);
	}

	public void setAudio(string s)
    {
		UpdateVolume(s);
	}

	private void UpdateWifiUI()
	{
		if (wifi.WifiState == WifiState.WIFI_STATE_ENABLED){
			if(wifi.NetworkState == NetworkState.STATE_DISCONNECTED){
				wifiImage.sprite = Resources.Load("Image/wifi_disconnected", typeof(Sprite)) as Sprite;
			}else{
				wifiImage.sprite = Resources.Load("Image/wifi_connected_"+ wifi.Level, typeof(Sprite)) as Sprite;
			}
		}else{
			wifiImage.sprite = Resources.Load("Image/wifi_disabled", typeof(Sprite)) as Sprite;
		}
	}

	
	private void UpdateBluetoothUI()
	{
		if (bluetooth.BluetoothState == BluetoothState.STATE_ON) {
			if (bluetooth.BluetoothProfile == BluetoothProfile.STATE_CONNECTED) {
				bluetoothImage.sprite = Resources.Load("Image/bluetooth_connected", typeof(Sprite)) as Sprite;
			} else {
				bluetoothImage.sprite = Resources.Load("Image/bluetooth_on", typeof(Sprite)) as Sprite;
			}
		} else {
			bluetoothImage.sprite = Resources.Load("Image/bluetooth_off", typeof(Sprite)) as Sprite;
		}
	}

	
	private void UpdateBatteryUI()
	{
		switch (battery.BatteryState) {

		case BatteryState.BATTERY_STATUS_CHARGING:
			batteryImage.sprite = Resources.Load("Image/electric_charging", typeof(Sprite)) as Sprite;
			batteryText.text = battery.Level + "%";
			break;
		case BatteryState.BATTERY_STATUS_DISCHARGING:
		case BatteryState.BATTERY_STATUS_NOT_CHARGING:

		case BatteryState.BATTERY_STATUS_FULL:
				float num = float.Parse (battery.Level) / 20;
			int level = Mathf.CeilToInt (num);
				batteryImage.sprite = Resources.Load("Image/electric_" + level.ToString(), typeof(Sprite)) as Sprite;
            batteryText.text = battery.Level + "%";
                break;
		}
	}

	
	private void UpdateTimeUI()
	{
		timeText.text = systemTime.Time;
	}

	
	private void UpdateVolumeUI()
	{
		if (startShowVolumeTime == 0.0f) {
			volumeObj.SetActive(true);
			volumeImage.fillAmount = float.Parse(volume.VolumeData)/(float)100;
			startShowVolumeTime = Time.time;
		}
	}

	public void UpdateNetworkState(string state)
	{
		wifi.NetworkState = (NetworkState)Enum.Parse (typeof(NetworkState), state);
		UpdateWifiUI ();
	}

	public void UpdateWifiState(string state)
	{
		wifi.WifiState = (WifiState)Enum.Parse (typeof(WifiState), state);
		UpdateWifiUI ();
	}

	public void UpdateWifiLevel(string level)
	{
		wifi.Level = level;
		UpdateWifiUI ();
	}

	public void UpdateBluetoothState(string state)
	{
		bluetooth.BluetoothState = (BluetoothState)Enum.Parse (typeof(BluetoothState), state);
		UpdateBluetoothUI ();
	}

	public void UpdateBluetoothConnectState(string state)
	{
		bluetooth.BluetoothProfile = (BluetoothProfile)Enum.Parse (typeof(BluetoothProfile), state);
		UpdateBluetoothUI ();
	}

	public void UpdateBatteryState(string state)
	{
		battery.BatteryState = (BatteryState)Enum.Parse (typeof(BatteryState), state);
		UpdateBatteryUI ();
	}
		
	public void UpdateBatteryLevel(string level)
	{
		battery.Level = level;
		UpdateBatteryUI ();
	}

	public void UpdateTime(string time)
	{
		systemTime.Time = time;
		UpdateTimeUI ();
	}

	public void UpdateVolume(string data)
	{
		volume.VolumeData = data;
		startShowVolumeTime = 0.0f;
        if (PUI_UnityAPI.GetDeviceMode() != DeviceMode.FalconCV2) {
            UpdateVolumeUI();
        }
	}

	public void UpdateControllerIcon(bool isConnect)
	{
		handleImage.sprite = Resources.Load(isConnect?"Image/handle_connect":"Image/handle_close", typeof(Sprite)) as Sprite;
	}

	
	public void ClickWifi()
	{
		AppUtils.Instance.OpenAppByAction (Constant.SETTINGS_WIFI_ACTION);

        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	
	public void ClickBluetooth()
	{
		AppUtils.Instance.OpenAppByAction (Constant.SETTINGS_BLUETOOTH_ACTION);

        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	
	public void ClickHandle()
	{
		AppUtils.Instance.OpenAppByAction (Constant.SETTINGS_HANDLE_ACTION);

        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	public void ClickSettings()
	{
		AppUtils.Instance.OpenAppByComponentName (Constant.SETTINGS_PACKAGE_NAME, Constant.SETTINGS_ACTIVITY_NAME);
		
        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	
	public void ClickUserCenter()
	{
		AppUtils.Instance.OpenAppByComponentName (Constant.USERCENTER_PACKAGE_NAME, Constant.USERCENTER_ACTIVITY_NAME);
	}
	
    private void OnDestroy()
    {
		InputDevices.deviceConnected -= InputDevices_deviceConnected;
		InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
	}
}

