using UnityEngine;
using System.Collections;

namespace ImageLoaderPlugin
{
	public class ImageLoaderUpdateDelegator : MonoBehaviour
	{

		private static ImageLoaderUpdateDelegator instance;
		private static bool IsCreated;
		
		public static void CheckInstance()
		{
			try
			{
				if (!IsCreated)
				{
					instance = UnityEngine.Object.FindObjectOfType(typeof(ImageLoaderUpdateDelegator)) as ImageLoaderUpdateDelegator;
					
					if (instance == null)
					{
						GameObject go = new GameObject("LocalImageLoader Update Delegator");
						go.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
						UnityEngine.Object.DontDestroyOnLoad(go);
						
						instance = go.AddComponent<ImageLoaderUpdateDelegator>();
					}
					IsCreated = true;
				}
			}
			catch
			{
				Debug.LogError("ImageLoaderUpdateDelegator->Please call the LocalImageLoader.ImageLoadManager.Setup() from one of Unity's event(eg. awake, start) before you start AsyncLoadImage!");
			}
		}


		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			ImageLoadManager.OnUpdate ();
		}

		#if UNITY_EDITOR
		void OnDisable()
		{
			ImageLoadManager.OnQuit();
		}
		#else
		void OnApplicationQuit()
		{
			ImageLoadManager.OnQuit();
		}
		#endif
	}
}