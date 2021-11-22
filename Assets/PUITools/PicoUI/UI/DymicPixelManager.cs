using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DymicPixelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject gameObject = GameObject.Find ("UI Root");
		if (gameObject == null) {
			Debug.LogError ("DymicPixelManager --- UI Root is null !");
			return;
		}
		CanvasScaler canvasScaler = gameObject.GetComponent<CanvasScaler> (); 
		if (canvasScaler == null) {
			Debug.LogError ("DymicPixelManager --- CanvasScaler is null !");
			return;
		}
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass androidClass = new AndroidJavaClass ("android.os.Build");
			string productName = androidClass.GetStatic<string> ("PRODUCT");
			Debug.Log ("DymicPixelManager --- Product name is " + productName);
			if (string.IsNullOrEmpty (productName)) {
				return;
			}
			if (productName.Equals ("A7910")) {
				//4K
				canvasScaler.dynamicPixelsPerUnit = 0.72f;
			} else {
				canvasScaler.dynamicPixelsPerUnit = 1.0f;
			}
		} 
	}

}
