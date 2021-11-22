using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using BestHTTP;
using ImageLoaderPlugin;

public class HomeItem : MonoBehaviour {

	private BaseModel model;
	public int index;
	public RawImage image;
	public Text titleText;

	// Use this for initialization
	void Start () {
		ImageUtils.onTextureCallback += OnTextureCallback;
		GetComponent<Button> ().onClick.AddListener (delegate() {
			LauncherUtils.DoSomething (this.model);
		});

        initData();
    }

    void initData() {
        BaseModel model = new BaseModel();
        if (this.index == 0)
        {
            model.mid = -1;
            model.dataType = DataType.VIDEO;
            model.title = "DemoVideo";
            model.imageUrl = "poster/DemoVideoImage";
            model.url = "/sdcard/pre_resource/video/Video_Demo.mp4";
            model.videoType = 0;
        }
        else {
            model.mid = -1;
            model.dataType = DataType.APP;
            model.title = "FileManager";
            model.imageUrl = "poster/FileManagerImage";
            model.packageName = "com.pvr.filemanager";
			model.className = "com.pvr.filemanager.refactor.view.activity.MainActivity";
        }

        OnItemDataChangeCallback(model);
    }


	private void OnItemDataChangeCallback(BaseModel model)
	{
		if (model == null) {
			return;
		}
		if (this.model != null && model.Equals (this.model)) {
			return;
		}
		this.model = model;
		SetUI ();
	}

	
	private void SetUI()
	{
		LogUtils.Log (this.model.title);
		titleText.text = this.model.title;
		LoadImage();
	}

	
	private void LoadImage()
	{
		string imageUrl = this.model.imageUrl;
		if (string.IsNullOrEmpty (imageUrl)) {
			LogUtils.LogError (this.gameObject.name+" image url is null!");
			return;
		}
		if (imageUrl.StartsWith ("poster")) {
			LogUtils.Log (this.model.title+" load image from <poster>");
			image.texture = Resources.Load (imageUrl) as Texture;
		} else if (imageUrl.StartsWith ("sdcard")) {
			LogUtils.Log (this.model.title+" load image from <sdcard>");
			new LocalImageLoader (imageUrl, true, OnLocalTextureCallback).StartLoad();
		} else {
			string imageLocalPath = LauncherUtils.GetImageCachePath() + GetImageUrl().GetHashCode();
			if(!File.Exists (imageLocalPath)){
				LogUtils.Log (this.model.title+" load image from <network>");
				ImageUtils.Instance.LoadTexture (GetImageUrl());
			}else{
				LogUtils.Log (this.model.title+" load image from <cache>");
				new LocalImageLoader(imageLocalPath, true, OnLocalTextureCallback).StartLoad();
			}
		}
	}

	
	private void OnTextureCallback(HTTPRequest req, HTTPResponse resp)
	{
		if (req == null || this.model == null) {
			return;
		}
		if (!req.Uri.ToString ().Equals (GetImageUrl())) {
			return;
		}
		Texture2D texture = resp.DataAsTexture2D;
		if (texture.width <= 8 || texture.height <= 8) {
			LogUtils.LogError (this.model.title + " image width or height less than 8!");
			return;
		}
		image.texture = texture;
	}

	private void OnLocalTextureCallback(LocalImageLoader localImageLoader, LocalImageLoaderResponse response)
	{
		image.texture = response.DataAsTexture2D;
	}

	void OnDestroy(){
		ImageUtils.onTextureCallback -= OnTextureCallback;
	}

	
	private string GetImageUrl()
	{
		if (index == 0)
			return this.model.imageUrl + "?x-oss-process=image/resize,w_434,h_210";
		return this.model.imageUrl + "?x-oss-process=image/resize,w_210,h_210";
	}
		
}
