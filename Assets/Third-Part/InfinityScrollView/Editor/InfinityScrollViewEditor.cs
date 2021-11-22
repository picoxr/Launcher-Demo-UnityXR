using UnityEngine;
using System.Collections;
using UnityEditor;
using OneP.InfinityScrollView;
public class InfinityScrollViewEditor {
	
	[MenuItem("GameObject/UI/OneP Extras/Infinity ScrollView/Vertical/TopToDown")]
	public static void CreateVerticalTopDown()
	{
		CreateInfinityScrollView (1);
	}

	[MenuItem("GameObject/UI/OneP Extras/Infinity ScrollView/Vertical/DownToTop")]
	public static void CreateVerticalDownTop()
	{
		CreateInfinityScrollView (2);
	}

	[MenuItem("GameObject/UI/OneP Extras/Infinity ScrollView/Horizontal/LeftToRight")]
	public static void CreateVerticalLeftToRight()
	{
		CreateInfinityScrollView (3);
	}
	[MenuItem("GameObject/UI/OneP Extras/Infinity ScrollView/Horizontal/RightToLeft")]
	public static void CreateVerticalRightToLeft()
	{
		CreateInfinityScrollView (4);
	}

	private static void CreateInfinityScrollView(int type){
		string path = "Assets/InfinityScrollView/Editor/BaseObject/";
		switch (type) {
			case 1: path+="Infinity_VTD.prefab";
				break;
				case 2: path+="Infinity_VDT.prefab";
						break;
				case 3: path+="Infinity_HLR.prefab";
						break;
				default: path+="Infinity_HRL.prefab";
				break;
		}
		try
		{
			Object obj=AssetDatabase.LoadAssetAtPath (path, typeof(Object));
			if(obj==null)
			{
				EditorUtility.DisplayDialog("Error","Can not Find Prefab in Path:"+path,"ok");
			}
			else
			{
				GameObject objPrefab=(GameObject)obj;
				GameObject infinityObj= GameObject.Instantiate(objPrefab) as GameObject;
				Vector3 scale=infinityObj.transform.localScale;
				infinityObj.name="Infinity ScrollView";
				if(Selection.activeGameObject!=null){
				infinityObj.transform.SetParent(Selection.activeGameObject.transform);
				infinityObj.transform.localScale=scale;
				infinityObj.transform.localPosition=Vector3.zero;

				}
			}
		}
		catch{
			EditorUtility.DisplayDialog("Error","Can not Find Prefab in Path:"+path,"ok");		
		}

	}
}
