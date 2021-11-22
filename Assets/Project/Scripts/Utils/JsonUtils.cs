using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public class JsonUtils {

	public static List<BaseModel> JsonToList(string json)
	{
		if (string.IsNullOrEmpty(json)) {
			return null;
		}
		List<BaseModel> modelList = new List<BaseModel>();
		JsonData jsonData = JsonMapper.ToObject (json);
		IDictionary dictionary = jsonData as IDictionary;
		if (dictionary.Contains ("list")) {
			JsonData jsonArrayData = jsonData["list"];
			if(!jsonArrayData.IsArray){
				return null;
			}
			int count = jsonArrayData.Count;
			for(int i=0; i<count; i++){
				JsonData jsonObjectData = jsonArrayData[i];
				IDictionary dic = jsonObjectData as IDictionary;
				BaseModel itemModel = new BaseModel();
				if(dic.Contains("mid") && jsonObjectData["mid"] != null){
					itemModel.mid = int.Parse(jsonObjectData["mid"].ToString());
				}
				if(dic.Contains("type") && jsonObjectData["type"] != null){
					itemModel.dataType = (DataType)int.Parse(jsonObjectData["type"].ToString());
				}
				if(dic.Contains("title") && jsonObjectData["title"] != null){
					itemModel.title = jsonObjectData["title"].ToString();
				}
				if(dic.Contains("focusimage") && jsonObjectData["focusimage"] != null){
					itemModel.imageUrl = jsonObjectData["focusimage"].ToString();
				}
				if(dic.Contains("url") && jsonObjectData["url"] != null){
					itemModel.url = jsonObjectData["url"].ToString();
				}
				if(dic.Contains("newsubtype") && jsonObjectData["newsubtype"] != null){
					itemModel.videoType = int.Parse(jsonObjectData["newsubtype"].ToString());
				}
				if(dic.Contains("packagename") && jsonObjectData ["packagename"] != null){
					itemModel.packageName = jsonObjectData["packagename"].ToString();
				}
				if(dic.Contains("pid") && jsonObjectData["pid"] != null){
					itemModel.pid = int.Parse(jsonObjectData["pid"].ToString());
				}
				if(dic.Contains("content") && jsonObjectData["content"] != null){
					itemModel.content = jsonObjectData["content"].ToString();
				}
				if(dic.Contains("date") && jsonObjectData["date"] != null){
					itemModel.date = jsonObjectData["date"].ToString();
				}
				if(dic.Contains("button") && jsonObjectData["button"] != null){
					itemModel.button = jsonObjectData["button"].ToString();
				}
				modelList.Add(itemModel);
			}
		}
		return modelList;
	}
	
}
