using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToastController : MonoBehaviour {

	private static Transform rootTran;
	private static Transform headTran;
	private static GameObject obj;
	private static float duration = 5.0f;
	private static float startTime = 0.0f; 
	private static GameObject controllerObj;

	private static ToastController m_ToastController;

	void Update(){
		if (Time.time - startTime > duration && this.gameObject.activeSelf) {
			Close();
		}
		if (startTime != 0.0f && headTran != null) {
			this.transform.eulerAngles = new Vector3(0,headTran.eulerAngles.y,0);
		}
	}

	public static void Show(bool isConnected, string tip = "")
	{
		//Annotation
		//if (rootTran == null){
		//	rootTran = FindObjectOfType<Pvr_UnitySDKManager>().transform;
		//}
		//if (headTran == null) {
		//	headTran = FindObjectOfType<Pvr_UnitySDKEyeManager>().transform;
		//}
		if (obj == null){
			GameObject go = Resources.Load("Prefab/ControllerToast") as GameObject;
			obj = Instantiate(go);
			obj.transform.parent = rootTran;
			obj.transform.eulerAngles = Vector3.zero;
			obj.transform.position = Vector3.zero;

			m_ToastController = obj.GetComponent<ToastController>();
			#region  没有适配多种手柄的逻辑
			//         GameObject cvObj = obj.transform.GetChild(0).FindChild("CV").gameObject;
			//GameObject goblinObj = obj.transform.GetChild(0).FindChild("Goblin").gameObject;

			//if (PUI_UnityAPI.GetDeviceMode() == DeviceMode.FalconCV) {
			//	cvObj.SetActive(true);
			//	goblinObj.SetActive(false);
			//	controllerObj = cvObj;
			//} else {
			//	cvObj.SetActive(false);
			//	goblinObj.SetActive(true);
			//	controllerObj = goblinObj;
			//}
			#endregion
		}
		m_ToastController.FindControllerTexture();

		if (rootTran == null || headTran == null || 
			obj == null /*|| controllerObj == null*/){
			Debug.LogError("ToastController is null !!!");
			return;
		}

		//UILabel label = obj.transform.GetChild(0).FindChild("Label").GetComponent<UILabel>();
		//UITexture bg = obj.transform.GetChild(0).FindChild("Bg").GetComponent<UITexture>();
		if (m_ToastController.m_Label == null || m_ToastController.m_Bg == null) {
			Debug.LogError("label or bg is null");
			return;
		}
		if (isConnected){
			m_ToastController.m_Label.text = Localization.Get("Controller_Connected");
			m_ToastController.m_Bg.rectTransform.sizeDelta = new Vector2(m_ToastController.m_Bg.rectTransform.sizeDelta.x, 380f);
			if (controllerObj != null)
				controllerObj.SetActive(true);
		}
		else{
			m_ToastController.m_Label.text = Localization.Get("Controller_DisConnected");
			m_ToastController.m_Bg.rectTransform.sizeDelta = new Vector2(m_ToastController.m_Bg.rectTransform.sizeDelta.x, 115f);
			if (controllerObj != null)
				controllerObj.SetActive(false);
		}
		//if (!string.IsNullOrEmpty (tip)) {
		//          m_ToastController.m_Label.text = tip;
		//}
		startTime = Time.time;
		obj.SetActive (true);
		if (m_ToastController.m_Label != null)
			m_ToastController.m_Label.gameObject.SetActive(true);
		if (m_ToastController.m_Bg != null)
			m_ToastController.m_Bg.gameObject.SetActive(true);

	}

	void Close(){
		this.gameObject.SetActive (false);
		startTime = 0.0f;
		this.transform.eulerAngles = Vector3.zero;
	}

	private RawImage m_Bg;

	private Text m_Label;

	private int m_lastControlerType = -1;

	/// <summary>
	/// 查询Controller对应的图片
	/// </summary>
	private void FindControllerTexture()
	{
		int controllerType = CursorManager.GetInstance().GetControllerType();
		Transform contentTran = transform.GetChild(0);
		int contentChildCount = contentTran.childCount;
		for(int i = 0; i < contentChildCount; i++)
		{
			Transform tmpTran = contentTran.GetChild(i);
			if (m_Bg == null)
			{
				if (tmpTran.name.Equals("Bg"))
				{
					m_Bg = tmpTran.GetComponent<RawImage>();
					continue;
				}
			}
			if (m_Label == null)
			{
				if (tmpTran.name.Equals("Label"))
				{
					m_Label = tmpTran.GetComponent<Text>();
					continue;
				}
			}


			if (controllerType == 0)
			{
				if (controllerObj != null)
					controllerObj.SetActive(false);
			}
			else
			{
				if (m_lastControlerType != controllerType) /**当前的手柄类型与上一次的不一样，重新查找图**/
				{
					if (controllerObj != null)  /**上一个手柄用的图标**/
						controllerObj.SetActive(false);

					if (controllerType == 1)
					{
						if (tmpTran.name.Equals("Goblin"))
						{
							controllerObj = tmpTran.gameObject;
						}
						else
							tmpTran.gameObject.SetActive(false);
					}
					else if (controllerType == 2)
					{
						if (tmpTran.name.Equals("CV"))
						{
							controllerObj = tmpTran.gameObject;
						}
						else
							tmpTran.gameObject.SetActive(false);
					}
					else if (controllerType == 3)
					{
						if (tmpTran.name.Equals("Goblin2"))
						{
							controllerObj = tmpTran.gameObject;
						}
						else
							tmpTran.gameObject.SetActive(false);
					}
				}

				if(controllerObj != null)
					controllerObj.SetActive(true);
			}
		}
		if(m_lastControlerType != controllerType && controllerType != 0)
			m_lastControlerType = controllerType;

	}



	void OnApplicationFocus(bool focus)
	{
		if (!focus)
			Close();
	}

}
