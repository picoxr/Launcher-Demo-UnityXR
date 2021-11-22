using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{

	public Text labInfo;
	private static List<Toast> instanceList = new List<Toast> ();
	private static Transform rootTran;
	private static Toast instance;

	public static void Show (string info)
	{
		Show (info, new Color (0.13f, 0.13f, 0.13f));
	}

	public static void Show (string info, Color color)
	{
		if (rootTran == null) {
			rootTran = GameObject.Find ("UI Root").transform;
		}

		if (instance == null || (instance != null && !info.Equals (instance.labInfo.text))) {
			if (instanceList.Count > 0) {
				instance = instanceList [0];
				instanceList.RemoveAt (0);
			} else {
				GameObject go = Resources.Load ("Prefab/Toast") as GameObject;
				GameObject obj = Instantiate (go);
				obj.transform.parent = rootTran;
				obj.gameObject.SetActive (false);
				instance = obj.GetComponent<Toast> ();
				instance.gameObject.transform.localScale = Vector3.one;
			}
           
			instance.transform.localEulerAngles = Vector3.zero;
			instance.transform.localPosition = new Vector3 (0f, 0f, -200f);
			instance.gameObject.SetActive (true);
			instance.labInfo.text = info;
			instance.labInfo.color = color;
			instance.StartCoroutine (instance.HideTips (2f));
			if (instance == null) {
				Debug.LogError ("Toast is null");
			}
		}

	}


	IEnumerator HideTips (float dua)
	{
		yield return new WaitForSeconds (dua);
		StartCoroutine (AddInstanceList (1.01f));

	}

	IEnumerator AddInstanceList (float dua)
	{
		yield return new WaitForSeconds (dua);
		if (!instanceList.Contains (this)) {
			gameObject.SetActive (false);
			instanceList.Add (this);
			instance = null;
		}
		if (instanceList.Count > 5) {
			for (int i = 0; i < instanceList.Count - 5; i++) {
				GameObject go = instanceList [0].gameObject;
				instanceList.RemoveAt (0);
				Destroy (go);
				i--;
			}
		}
	}
}
