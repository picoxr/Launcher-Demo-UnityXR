using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {

    private static CursorManager m_cursorManger;

    private Transform m_curController;

    [SerializeField]
    private GameObject m_headController;

    /*
    * used in UICamera.cs
    */
    private Ray ray;
    public Ray GetRay()
    {
        return ray;
    }


    public static CursorManager GetInstance()
    {
        if (m_cursorManger == null)
        {
            m_cursorManger = FindObjectOfType<CursorManager>();
            if (m_cursorManger == null)
            {
                Debug.LogError("Error: CursorManager 没有挂在场景中的管理手柄物体（CursorController）上");
            }
            else
            {
                //m_cursorManger.gameObject.transform.position = Vector3.zero;
                //m_cursorManger.gameObject.transform.eulerAngles = Vector3.zero;
            }
        }
        return m_cursorManger;
    }


    void Awake()
    {
        //Annotation
        //     InputController.GetInstance().AddListener(ListenerEventType.TRIGGER_SECOND, () => {
        //         if (!isControllerConnected(0) || !isControllerConnected(1))
        //         {
        //             return;
        //         }
        //         int mainController = Pvr_UnitySDKAPI.Controller.UPvr_GetMainHandNess();
        //         Debug.Log(" ----- Controller ------  " + "PUI--------------TrggerClick---currentMainController:" + mainController);
        //         Pvr_UnitySDKAPI.Controller.UPvr_SetMainHandNess(mainController == 0 ? 1 : 0);
        //     });

        //     Pvr_ControllerManager.ChangeMainControllerCallBackEvent += (string str) => 
        //     {
        //         Debug.Log("主手柄切换回调");

        //         OnControllerStateChange();
        //     };
        //     Pvr_ControllerManager.PvrServiceStartSuccessEvent += () =>
        //     {
        //         Debug.Log(" ----- Controller ------  " + "进入应用第一次或者后台唤醒到前台");
        //         OnControllerStateChange();
        ////add by wayne --- start
        //StatusManager.Instance.UpdateHandle(isControllerConnected());
        ////add by wayne --- end
        //     };
        //     Pvr_ControllerManager.PvrControllerStateChangedEvent += (string str) =>
        //     {
        //         Debug.Log("此处弹出toast提示");
        //         ControlToastOnFinch(isControllerConnected());
        //         OnControllerStateChange();
        ////add by wayne --- start
        //StatusManager.Instance.UpdateHandle(isControllerConnected());
        ////add by wayne --- end
        //     };
        //     Pvr_ControllerManager.ChangeMainControllerCallBackEvent += (string str) => { OnControllerStateChange(); };
    }



    /// <summary>
    /// 手柄状态发生改变 / 进入应用后通知 / 后台唤醒到前台 的回调方法  (暂定)
    /// </summary>
    private void OnControllerStateChange()
    {
        LoadController();
    }


    /*
	 * 在Finch手柄连接回调里处理toast
	 */
    private void ControlToastOnFinch(bool isConnect)
    {
        ToastController.Show(isConnect);
    }


    /*
    * 手柄是否连接
    * 参数：CV设备，传0、1
    *      Goblin设备，传0即可
    */
    public bool isControllerConnected(int hand)
    {
        //Annotation
        return true;
            //Pvr_UnitySDKAPI.Controller.UPvr_GetControllerState(hand) == Pvr_UnitySDKAPI.ControllerState.Connected;

    }


    /*
	 * 是否连接
	 * 不论平台或主副，只要连接就返回true
	 */
    public bool isControllerConnected()
    {
        return isControllerConnected(0) || isControllerConnected(1);
    }


    //Pvr_Controller pvr_controller;

    private void LoadController()
    {
        //Annotation
        //        if (pvr_controller == null)
        //            pvr_controller = GetComponent<Pvr_Controller>();
        //        if (!isControllerConnected())
        //        {
        //            if(m_curController != null)
        //                ControlCVRayShow(m_curController.gameObject, false);
        //            m_headController.SetActive(true);
        //        }
        //        else
        //        {
        //			InputController.GetInstance().Hand = Pvr_UnitySDKAPI.Controller.UPvr_GetMainHandNess();
        //            m_headController.SetActive(false);
        //            if (isControllerConnected(0) && !isControllerConnected(1))  /**两种情况：1，双手柄，连了1，没连2；2，单手柄**/
        //            {
        //                if (pvr_controller == null)
        //                    Debug.Log("pvr_controller is null");
        //                if(m_curController != null)
        //                    ControlCVRayShow(m_curController.gameObject, false);
        //                m_curController = pvr_controller.controller0.transform;
        //                ControlCVRayShow(m_curController.gameObject, true);
        //            }
        //            else if (!isControllerConnected(0) && isControllerConnected(1)) /**一定是双手柄，练了1，没有连2**/
        //            {
        //                if (m_curController != null)
        //                    ControlCVRayShow(m_curController.gameObject, false);
        //                m_curController = pvr_controller.controller1.transform;
        //                ControlCVRayShow(m_curController.gameObject, true);
        //            }
        //            else if (isControllerConnected(0) && isControllerConnected(1)) /**一定是连了双手柄**/
        //            {
        //				ControlCVRayShow(pvr_controller.controller0, false);
        //				ControlCVRayShow(pvr_controller.controller1, false);
        //                if (InputController.GetInstance().Hand == 0)
        //                {
        ////                    if (m_curController != null)
        ////                        ControlCVRayShow(m_curController.gameObject, false);
        //                    m_curController = pvr_controller.controller0.transform;
        //                    ControlCVRayShow(m_curController.gameObject, true);
        //                }
        //                else
        //                {
        ////                    if (m_curController != null)
        ////                        ControlCVRayShow(m_curController.gameObject, false);
        //                    m_curController = pvr_controller.controller1.transform;
        //                    ControlCVRayShow(m_curController.gameObject, true);
        //                }
        //            }
        //        }
    }



    /// <summary>
    /// 获取连接的手柄类型 0：没有连接手柄； 1： HB Controller ； 2： CV Controller ；3 ： HB2 Controller
    /// </summary>
    /// <returns></returns>
    public int GetControllerType()
    {
        int controllerType = 0;
        //Annotation
        //if (Pvr_ControllerManager.controllerlink != null)
        //{
        //    controllerType = Pvr_ControllerManager.controllerlink.GetDeviceType();
        //    Debug.Log("----- Controller ------ SDK获取到的type： " + controllerType);
        //    if (controllerType == -1)  //SDK定义的默认值是-1，但是我们需要的是默认未连接手柄的状态,所以设置为0;
        //        controllerType = 0;
        //}
        Debug.Log(" ----- Controller ------  " + "controllerType : " + controllerType);
        return controllerType;
    }

    void Update()
    {
        //if (m_curController != null)
        //{
        //    this.ray = GetControllerRay(m_curController.gameObject);
        //}

        //Debug.Log(" ----- Controller ------  " + "Hand: " + InputController.GetInstance().Hand);
        //if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(InputController.GetInstance().Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD))
        //{
        //    Debug.Log(" ----- Controller ------  " + "TouchPad键抬起");
        //}
        //if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(1 - InputController.GetInstance().Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD))
        //{
        //    Debug.Log(" ----- Controller ------  " + "TouchPad键抬起");
        //}
    }




    ////获取手柄上的射线
    //private Ray GetControllerRay(GameObject obj)
    //{
    //    if (obj == null)
    //    {
    //        Debug.LogError("Controller--------------GetControllerRay---obj is null!!!");
    //        return new Ray();
    //    }
    //    RayController rayController = obj.GetComponent<RayController>();
    //    if (rayController == null)
    //        rayController = obj.AddComponent<RayController>();
    //    rayController.Init();
    //    if (rayController == null)
    //    {
    //        Debug.LogError("Controller--------------GetControllerRay---RayController is null!!!");
    //        return new Ray();
    //    }
    //    return rayController.GetRay();
    //}


    /*
    * 控制CV手柄射线的显示隐藏
    */
    private void ControlCVRayShow(GameObject obj, bool isShow)
    {
        if (obj == null)
        {
            Debug.LogError("Controller--------------ControlCVRayShow---obj is null!!!");
            return;
        }
        RayController rayController = obj.GetComponent<RayController>();
        if (rayController == null)
        {
            Debug.LogError("Controller--------------ControlCVRayShow---RayController is null!!!");
            //return;
            rayController = obj.AddComponent<RayController>();
            rayController.Init();
        }
        Debug.Log(" ----- Controller ------  " + "PUI--------------ControlCVRayShow : " + obj.name + " " + isShow);
        if (isShow)
        {
            rayController.SetEnable();
        }
        else
        {
            rayController.SetDisable();
        }
    }



    /// <summary>
    /// 是否强制HMD
    /// </summary>
    /// <returns></returns>
    public bool IsForceHmd()
    {
        return false;
    }
}
  