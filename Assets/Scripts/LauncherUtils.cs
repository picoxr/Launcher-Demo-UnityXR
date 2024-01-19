using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class LauncherUtils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PXR_Enterprise.InitEnterpriseService();
        PXR_Enterprise.BindEnterpriseService();
    }

    public void OpenWifiSettingsPanel()
    {
        PXR_Enterprise.StartVrSettingsItem(StartVRSettingsEnum.START_VR_SETTINGS_ITEM_WIFI, true, 0);
    }

    public void LaunchVideoPlayer()
    {
        PXR_Enterprise.StartActivity("", "", "picovr.intent.action.player", "", new string[] { }, new int[] { });
    }

    public void StartFloorSetting()
    {
        PXR_Enterprise.GotoSeeThroughFloorSetting();
    }


    //This is a protected API. You need to add <meta-data android:name="pico_advance_interface" android:value="0"/> to the app's AndroidManifest.xml file for calling this API, after which the app is unable to be published on the PICO Store.
    public void Shutdown()
    {
        PXR_Enterprise.ControlSetDeviceAction(DeviceControlEnum.DEVICE_CONTROL_SHUTDOWN, (result) => Debug.Log("Reboot failed: " + result));
    }

    //This is a protected API. You need to add <meta-data android:name="pico_advance_interface" android:value="0"/> to the app's AndroidManifest.xml file for calling this API, after which the app is unable to be published on the PICO Store.
    public void Reboot()
    {
        PXR_Enterprise.ControlSetDeviceAction(DeviceControlEnum.DEVICE_CONTROL_REBOOT, (result) => Debug.Log("Reboot failed: " + result));
    }

    public void SetAsLauncherApp()
    {
        PXR_Enterprise.SetAPPAsHome(SwitchEnum.S_ON, "com.PICO.LauncherDemo");
    }

    public void ResetLauncherApp()
    {
        PXR_Enterprise.SetAPPAsHome(SwitchEnum.S_OFF, "com.PICO.LauncherDemo");
    }
    
}
