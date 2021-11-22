using UnityEngine;
using System.Collections;

namespace ImageLoaderPlugin{
	

	public class LocalImageLoaderResponse {

		public byte[] Data { get; internal set; }

		/// <summary>
		/// Cached converted data.
		/// </summary>
		protected UnityEngine.Texture2D texture;
		/// <summary>
		/// The data loaded to a Texture2D.
		/// </summary>
		public UnityEngine.Texture2D DataAsTexture2D
		{
			get
			{
				if (Data == null)
					return null;
				
				if (texture != null)
					return texture;
				
				texture = new UnityEngine.Texture2D(0, 0, UnityEngine.TextureFormat.ETC_RGB4, false);
				texture.LoadImage(Data);

				return texture;
			}
		}

		public LocalImageLoaderResponse (){
		
		}
		
	}

}


