using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class Service : MonoBehaviour
{
    public GameObject m_derviceGameObject;
    private void Awake()
    {
        PXR_System.InitSystemService(m_derviceGameObject.name);
        PXR_System.BindSystemService();
    }
    
    private void OnDestory()
    {
        PXR_System.UnBindSystemService();
    }
    private void BoolCallback(string value)
    {
        if (PXR_Plugin.System.BoolCallback != null) PXR_Plugin.System.BoolCallback(bool.Parse(value));
        PXR_Plugin.System.BoolCallback = null;
    }
    private void IntCallback(string value)
    {
        if (PXR_Plugin.System.IntCallback != null) PXR_Plugin.System.IntCallback(int.Parse(value));
        PXR_Plugin.System.IntCallback = null;
    }
    private void LongCallback(string value)
    {
        if (PXR_Plugin.System.LongCallback != null) PXR_Plugin.System.LongCallback(int.Parse(value));
        PXR_Plugin.System.LongCallback = null;
    }
    private void StringCallback(string value)
    {
        if (PXR_Plugin.System.StringCallback != null) PXR_Plugin.System.StringCallback(value);
        PXR_Plugin.System.StringCallback = null;
    }
    public void toBServiceBind(string s) { Debug.Log("Bind success."); }

}
