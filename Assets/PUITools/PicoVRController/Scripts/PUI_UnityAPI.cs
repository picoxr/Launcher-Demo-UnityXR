using UnityEngine;
using System.Collections;
using System;

public class PUI_UnityAPI
{

    public static DeviceMode GetDeviceMode()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("android.os.SystemProperties");
            string deviceMode = jc.CallStatic<string>("get", "ro.pvr.product.name", "Finch");
            Debug.Log("PUI--------------ro.pvr.product.name = " + deviceMode);
            if (deviceMode.Equals("Falcon", StringComparison.InvariantCultureIgnoreCase))
            {
                return DeviceMode.Falcon;
            }
            else if (deviceMode.Equals("Finch", StringComparison.InvariantCultureIgnoreCase))
            {
                return DeviceMode.Finch;
            }
            else if (deviceMode.Equals("Finch2", StringComparison.InvariantCultureIgnoreCase))
            {
                return DeviceMode.Finch;
            }
            else if (deviceMode.Equals("FalconCV", StringComparison.InvariantCultureIgnoreCase))
            {
                return DeviceMode.FalconCV;
            }
            else if (deviceMode.Equals("FalconCV2", StringComparison.InvariantCultureIgnoreCase))
            {
                return DeviceMode.FalconCV2;
            }
            else
            {
                return DeviceMode.Finch;
            }
        }
        return DeviceMode.Other;
    }

    /// <summary>
    /// 是否更新  true为更新，false为不更新
    /// </summary>
    /// <returns></returns>
    public static bool IsUpdateGrade()
    {
        bool isUpdateGrade = true;
        try
        {
#if !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("android.os.SystemProperties");
			int i = jc.CallStatic<int>("getInt", "persist.accept.systemupdates", 1);
			Debug.Log("获取到的是否升级的属性：" + i); 
			isUpdateGrade = i == 1;
#endif
        }
        catch (System.Exception ex)
        {
            Debug.Log("查询是否更新属性出错,错误信息为：" + ex.Message);
        }
        Debug.Log("是否更新： " + isUpdateGrade);
        return isUpdateGrade;
    }

    /*
	 * 是否是强制头控
	 * 		0或者无属性表示非强制
	 * 		1表示强制
	 */
    public static string GetForceHmd()
    {
        return GetSystemProperties("picovr.controller.forcehmd", "0");
    }

    public static string GetSystemProperties(string key, string def)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("android.os.SystemProperties");
            string value = jc.CallStatic<string>("get", key, def);
            Debug.Log("PUI--------------" + key + " = " + value);
            return value;
        }
        return def;
    }


    /*
	 * 手柄是否连接
	 * 参数：CV设备，传0、1
	 *      Goblin设备，传0即可
	 */
    public static bool isControllerConnected(int hand)
    {
        return true;
        //Annotation
        //Pvr_UnitySDKAPI.Controller.UPvr_GetControllerState(hand) == Pvr_UnitySDKAPI.ControllerState.Connected;
    }

    /*
	 * 手柄是否连接
	 * 不论平台或主副，只要连接就返回true
	 */
    public static bool isControllerConnected()
    {
        return isControllerConnected(0) || isControllerConnected(1);
    }

}

public enum DeviceMode
{
    Falcon,//PicoNeoDK,PicoNeoDKS
    Finch,//PicoGoblin
    FalconCV,//PicoNeo
    FalconCV2,//PicoNeo2
    Other,
}
