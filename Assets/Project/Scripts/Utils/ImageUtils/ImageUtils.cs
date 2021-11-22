using UnityEngine;
using System.Collections;
using BestHTTP;
using System;
using System.IO;
using System.Threading;

public class ImageUtils : MonoBehaviour {

	public delegate void TextureCallback (HTTPRequest request, HTTPResponse response);
	
	public static TextureCallback onTextureCallback;

	private static ImageUtils instance;

	public static ImageUtils Instance {
		get {
			if(instance == null){
				GameObject go = new GameObject("ImageUtils");
				instance = go.AddComponent<ImageUtils>();
			}
			return instance;
		}
		set {
			instance = value;
		}
	}

	double timeOut = 20;
	public double TimeOut {
		get {
			return timeOut;
		}
		set {
			if(value <= 0){
				timeOut = 20;
			}
			timeOut = value;
		}
	}

	double connectTimeOut = 10;
	public double ConnectTimeOut {
		get {
			return connectTimeOut;
		}
		set {
			if(value <= 0){
				connectTimeOut = 10;
			}
			connectTimeOut = value;
		}
	}

	public void LoadTexture(string url){
		if (string.IsNullOrEmpty(url)) {
			Debug.LogWarning("The url is null.");
			return;
		}
		HTTPRequest request = new HTTPRequest(new Uri(url), OnRequestFinished);
		request.Timeout = TimeSpan.FromSeconds(timeOut);
		request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeOut);
		request.Send ();
	}

	void OnRequestFinished(HTTPRequest request, HTTPResponse response){
		string imageUrl = request.Uri.ToString ();
		switch (request.State){
		case HTTPRequestStates.Finished:
			if (!response.IsSuccess){
				return;
			}
			if(onTextureCallback != null){
				Debug.Log ("The request is success and url is "+imageUrl);
				onTextureCallback(request, response);
				//save to local
				Texture2D image = response.DataAsTexture2D;
				if (image.width <= 8 || image.height <= 8) {
					//There is something wrong with the image.
					return;
				}
				SaveToLocal(imageUrl, response);
			}else{
				Debug.LogWarning("ImageLoaderManager->onTextureCallback = null");
			}
			break;
		case HTTPRequestStates.Error:
			Debug.LogWarning("The request finished with an unexpected error. The request's Exception property may contain more info about the error.");
			break;
		case HTTPRequestStates.Aborted:
			Debug.LogWarning("The request aborted, initiated by the user.");
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			Debug.LogWarning("Connecting to the server is timed out.");
			return;
		case HTTPRequestStates.TimedOut:
			Debug.LogWarning("The request didn't finished in the given time.");
			return;
		default:
			return;
		}
	}

	private void SaveToLocal(string imageUrl, HTTPResponse response)
	{
		string imagePath = LauncherUtils.GetImageCachePath() + imageUrl.GetHashCode ();
		if(!File.Exists (imagePath)){
			ThreadStart threadStart = new ThreadStart(delegate(){
				if (!Directory.Exists(LauncherUtils.GetImageCachePath())) {  
					Directory.CreateDirectory(LauncherUtils.GetImageCachePath());  
				} 
				Texture2D image = response.DataAsTexture2D;
				byte[] pngData = image.EncodeToPNG();    
				File.WriteAllBytes(imagePath, pngData);
			});
			new Thread(threadStart).Start();
		}
	}

}
