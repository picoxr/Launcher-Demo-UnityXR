using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace ImageLoaderPlugin
{
	public class ImageLoadManager
	{

		private static bool quiteThread = false;
		private static List<LocalImageLoader> taskQueue;
		private static List<LocalImageLoader> callBackQueue;
		Thread workThread;

		LocalImageLoader curImageLoader;
		bool callbackRuning = false;
		private static ImageLoadManager Instance = null;

		public ImageLoadManager ()
		{
//			Debug.Log ("ImageLoadManager-> construct");
			taskQueue = new List<LocalImageLoader> ();
			callBackQueue = new List<LocalImageLoader> ();

			workThread = new Thread (DoTask);
			workThread.Start ();
		}

		public static ImageLoadManager GetIntance(){
			if (Instance == null) {
				Instance = new ImageLoadManager();
			}
			return Instance;
		}

		public static LocalImageLoader AsyncLoadImage (LocalImageLoader localImageLoader)
		{
			Setup ();

			if (localImageLoader == null) {
				Debug.LogError("ImageLoadManager->AsyncLoadImage: input parameter is null!");
				return null;
			}

			taskQueue.Add (localImageLoader);

			return localImageLoader;
		}

		private void DoTask ()
		{
			Debug.Log ("ImageLoadManager->DoTask start");
			while (!quiteThread) {
		
				if (taskQueue.Count > 0) {
					LocalImageLoader imageLoader = taskQueue [0];
					GetDataFromFile(imageLoader);
					callBackQueue.Add(imageLoader);
					taskQueue.RemoveAt (0);
				} else {
					Thread.Sleep (100);
				}
			}
			Debug.Log ("ImageLoadManager->DoTask end!");
		}

		private void GetDataFromFile(LocalImageLoader localImageLoader){
			if(localImageLoader != null){
				string url = localImageLoader.Url;
//				Debug.Log ("ImageLoadManager->GetDataFromFile url= "+url);
				if(File.Exists(url)){
					byte[] data = File.ReadAllBytes(url);
					if(data == null || data.Length <= 0){
						Debug.LogError("ImageLoadManager->GetDataFromFile: read byte data is null, url is "+url);
						return ;
					}
					LocalImageLoaderResponse response = new LocalImageLoaderResponse();
					response.Data = data;
					localImageLoader.Response = response;
				}else{
					Debug.LogWarning("ImageLoadManager->GetDataFromFile: file is not exits, url= "+url);
				}
			}
		}

		private static void Setup(){
			ImageLoaderUpdateDelegator.CheckInstance ();
		}

		public static void OnUpdate(){
			if(callBackQueue.Count > 0){
				LocalImageLoader imageLoader = callBackQueue[0];
				if(imageLoader != null){
					imageLoader.CallCallback();
				}
				callBackQueue.RemoveAt(0);
			}
		}

		public static void OnQuit(){
			//stop work thread
			quiteThread = true;
			taskQueue.Clear ();
			callBackQueue.Clear ();
		}

		public void OnDestory(){
			quiteThread = true;
			taskQueue.Clear ();
			callBackQueue.Clear ();
		}

	}
}
