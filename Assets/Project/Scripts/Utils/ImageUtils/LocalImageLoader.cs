using UnityEngine;
using System.Collections;

namespace ImageLoaderPlugin
{
	public class LocalImageLoader
	{
		public delegate void OnRequestFinishedDelegate (LocalImageLoader localImageLoader,LocalImageLoaderResponse response);

		public string Url { get; set;}
		public bool IsAndroidType{ get; set;}
		public bool IsAlive { get; set;}
		/// <summary>
		/// Any object can be passed with the request with this property. (eq. it can be identified, etc.)
		/// </summary>
		public object Tag { get; set; }

		public LocalImageLoaderResponse Response { get; internal set;}
		private ImageLoadManager imageLoadManager;
		/// <summary>
		/// The callback function that will be called when a request is fully processed.
		/// </summary>
		public OnRequestFinishedDelegate Callback { get; set; }

		public LocalImageLoader (string localUrl, OnRequestFinishedDelegate callback)
		:this(localUrl, true, callback)
		{	
		}

		public LocalImageLoader (string localUrl, bool isAndroid, OnRequestFinishedDelegate callback)
		{
//			Debug.Log ("construct LocalImageLoader: ");
			this.Callback = callback;
			this.IsAlive = false;
			this.Url = localUrl;
			this.IsAndroidType = isAndroid;
			imageLoadManager = ImageLoadManager.GetIntance ();

		}

		public void StartLoad ()
		{
			ImageLoadManager.AsyncLoadImage (this);
		}

		public void StopLoad ()
		{
			if (IsAlive) {
			
			}
		}

		public void CallCallback ()
		{
			if(Callback != null){
				Callback(this, Response);
			}
		}

		internal void GetResponse(LocalImageLoaderResponse response){
			Response = response;
		}
	}
}
