using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BatteryListener : MonoBehaviour
{
    private string m_state;
    private StatusManager m_statusManager;
    public Text textlog;

    // Start is called before the first frame update
    void Start()
    {
        
        AndroidJavaObject batteryListener = new AndroidJavaObject("com.picovr.libs.BatteryListener");
        AndroidJavaObject activityContext = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        batteryListener.Call("setContext", activityContext, gameObject.name, "BatteryIsChargingCallBack");
        batteryListener.Call("register", null);

        m_statusManager = GameObject.Find("StatusBar").GetComponent<StatusManager>();
        StartCoroutine("UpdataTime");//时间
        StartCoroutine("UpdataBattery");//电量
    }
    IEnumerator UpdataTime()
    {
        DateTime now = DateTime.Now;
        textlog.text += string.Format("{0}:{1}", now.Hour, now.Minute);
        yield return new WaitForSeconds(60f - now.Second);
        while (true)
        {
            now = DateTime.Now;
            textlog.text += "\n当前系统时间:" + string.Format("{0}:{1}", now.Hour, now.Minute);
            yield return new WaitForSeconds(60f);
        }
    }
    //更新手机电量
    IEnumerator UpdataBattery()
    {
        while (true)
        {
            textlog.text += "\n当前手机电量:" + GetBatteryLevel().ToString();
            yield return new WaitForSeconds(300f);
        }
    }
    int GetBatteryLevel()
    {
        try
        {

            string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
            return int.Parse(CapacityString);


        }
        catch (Exception e)
        {
            Debug.Log("duke 读取失败; " + e.Message);
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void BatteryIsChargingCallBack(string state)
    {
        m_state = state;
        if (m_state.Equals("TRUE"))
        {
            m_statusManager.UpdateBatteryState(BatteryState.BATTERY_STATUS_CHARGING.ToString());
        }
        if (m_state.Equals("FALSE"))
        {
            m_statusManager.UpdateBatteryState(BatteryState.BATTERY_STATUS_DISCHARGING.ToString());
        }
    }
    
}
