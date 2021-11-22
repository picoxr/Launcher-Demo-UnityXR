using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using OneP.InfinityScrollView;
[CustomEditor(typeof(InfinityScrollView))]
public class InfinityScrollViewInspector : Editor {

	private InfinityScrollView infinityScrollView;
	private int count = 0;

	// Use this for initialization
	public override void OnInspectorGUI()
	{

		EditorGUILayout.BeginVertical();
		infinityScrollView = (InfinityScrollView)target;
		GUILayout.Label("Setting Auto", EditorStyles.boldLabel);
		infinityScrollView.isAutoLinking= EditorGUILayout.Toggle("Auto Setup reference object",infinityScrollView.isAutoLinking);
		Color color = GUI.color;
		GUI.color = Color.blue;
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = color;

		GUILayout.Label("Linking references", EditorStyles.boldLabel);
		infinityScrollView.scrollRect = (ScrollRect)EditorGUILayout.ObjectField("ScrollRect",infinityScrollView.scrollRect,typeof(ScrollRect),true,null);
		infinityScrollView.content = (RectTransform)EditorGUILayout.ObjectField("Content",infinityScrollView.content,typeof(RectTransform),true,null);
		infinityScrollView.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab Item",infinityScrollView.prefab,typeof(GameObject),true,null);

		GUI.color = Color.blue;
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = color;

		GUILayout.Label("Setting Type of Infinity scrollview", EditorStyles.boldLabel);
		infinityScrollView.type = (InfinityType)EditorGUILayout.EnumPopup("ScrollView Type",infinityScrollView.type);
		if (infinityScrollView.type == InfinityType.Vertical) {
			infinityScrollView.verticalType = (VerticalType)EditorGUILayout.EnumPopup("Direction",infinityScrollView.verticalType);
		}
		else {
			infinityScrollView.horizontalType = (HorizontalType)EditorGUILayout.EnumPopup("Direction",infinityScrollView.horizontalType);
		}
		infinityScrollView.isOverrideSettingScrollbar= EditorGUILayout.Toggle("Also Change Direction Scrollbar",infinityScrollView.isOverrideSettingScrollbar);
		GUI.color = Color.blue;
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = color;
		GUILayout.Label("Setting Custom data", EditorStyles.boldLabel);
		if (infinityScrollView.type == InfinityType.Vertical) {
			infinityScrollView.itemSize = EditorGUILayout.FloatField ("Item Height", infinityScrollView.itemSize);
		} 
		else {
			infinityScrollView.itemSize = EditorGUILayout.FloatField ("Item Width", infinityScrollView.itemSize);
		}



		infinityScrollView.itemGenerate= EditorGUILayout.IntField("Item in Screen",infinityScrollView.itemGenerate);
		if (infinityScrollView.itemGenerate <= 0) {
			infinityScrollView.itemGenerate=0;
		}
		infinityScrollView.totalNumberItem= EditorGUILayout.IntField("Total Items",infinityScrollView.totalNumberItem);
		if (infinityScrollView.totalNumberItem <= 0) {
			infinityScrollView.totalNumberItem=0;
		}
		if (infinityScrollView.totalNumberItem <= 0) {
			infinityScrollView.totalNumberItem=0;
		}
		infinityScrollView.setupOnAwake=EditorGUILayout.Toggle("Setup On Awake",infinityScrollView.setupOnAwake);
		infinityScrollView.locationType = (DetemineLocationType)EditorGUILayout.EnumPopup("Setting Item Location",infinityScrollView.locationType);
		if (infinityScrollView.locationType == DetemineLocationType.OverrideLocation) {
			if (infinityScrollView.type == InfinityType.Vertical) {
				infinityScrollView.overrideX= EditorGUILayout.FloatField("Init pos X",infinityScrollView.overrideX);
			}
			else
			{
				infinityScrollView.overrideY= EditorGUILayout.FloatField("Init pos Y",infinityScrollView.overrideY);
			}
		}
		infinityScrollView.extraContentLength= EditorGUILayout.FloatField("Extra Content Length",infinityScrollView.extraContentLength);

		GUI.color = Color.blue;
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
		GUI.color = color;

		SetSkipIndex ();
		EditorGUILayout.EndVertical();
		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
			FixValue();
		}
		else if(count==0){
			FixValue();
		}
		count++;
	}
	#region Fix Data value
	public void FixValue(){
		if (!infinityScrollView.isAutoLinking) {
			return;
		}
		//Debug.LogError("FixValue");
		if (infinityScrollView.scrollRect == null) {
			infinityScrollView.scrollRect=infinityScrollView.GetComponent<ScrollRect>();	
		}
		if (infinityScrollView.scrollRect != null) {

			if (infinityScrollView.type == InfinityType.Vertical) {
				infinityScrollView.scrollRect.vertical=true;
				infinityScrollView.scrollRect.horizontal=false;
				Scrollbar scrollBar=infinityScrollView.scrollRect.verticalScrollbar;
				if(scrollBar!=null &&infinityScrollView.isOverrideSettingScrollbar){
					if (infinityScrollView.verticalType == VerticalType.TopToBottom) {
						scrollBar.direction=Scrollbar.Direction.BottomToTop;

					}
					else{
						scrollBar.direction=Scrollbar.Direction.BottomToTop;
					}
				}	
			}
			else{
				infinityScrollView.scrollRect.vertical=false;
				infinityScrollView.scrollRect.horizontal=true;
				Scrollbar scrollBar=infinityScrollView.scrollRect.horizontalScrollbar;
				if(scrollBar!=null&&infinityScrollView.isOverrideSettingScrollbar){
					if (infinityScrollView.horizontalType == HorizontalType.LeftToRight) {
						scrollBar.direction=Scrollbar.Direction.LeftToRight;
					}
					else{
						scrollBar.direction=Scrollbar.Direction.RightToLeft;
					}
				}
			}
			if(infinityScrollView.content==null){
				Transform tran=infinityScrollView.scrollRect.transform;
				if(tran.Find("Viewport")!=null&&tran.Find("Viewport").Find("Content")!=null){
					GameObject obj=tran.Find("Viewport").Find("Content").gameObject;
					RectTransform rect=obj.GetComponent<RectTransform>();
					if(rect!=null)
					{
					}
				}
			}
			if(infinityScrollView.content!=null){
				RectTransform rect=infinityScrollView.content;
				if (infinityScrollView.type == InfinityType.Vertical) {
					if (infinityScrollView.verticalType == VerticalType.TopToBottom) {
						Vector2 min=rect.anchorMax;
						Vector2 max=rect.anchorMin;
						rect.anchorMax=new Vector2(max.x,1);
						rect.anchorMin=new Vector2(min.x,1);

						Vector3 pivot=rect.pivot;
						rect.pivot=new Vector2(pivot.x,1);
					}
					else{
						Vector2 min=rect.anchorMax;
						Vector2 max=rect.anchorMin;
						rect.anchorMax=new Vector2(max.x,0);
						rect.anchorMin=new Vector2(min.x,0);
						Vector3 pivot=rect.pivot;
						rect.pivot=new Vector2(pivot.x,0);
					}
						
				}
				else{
					if (infinityScrollView.horizontalType == HorizontalType.LeftToRight) {
						Vector2 min=rect.anchorMax;
						Vector2 max=rect.anchorMin;
						rect.anchorMax=new Vector2(0,min.y);
						rect.anchorMin=new Vector2(0,max.y);
						Vector3 pivot=rect.pivot;
						rect.pivot=new Vector2(0,pivot.y);
					}
					else{
						Vector2 min=rect.anchorMax;
						Vector2 max=rect.anchorMin;
						rect.anchorMax=new Vector2(1,min.y);
						rect.anchorMin=new Vector2(1,max.y);
						Vector3 pivot=rect.pivot;
						rect.pivot=new Vector2(1,pivot.y);
					}
				}
			}
		}
		EditorUtility.SetDirty(target);
	}
	#endregion

	#region Draw Skip Index
	public void SetSkipIndex(){

		GUILayout.Label("Custom Ignore Object Item", EditorStyles.largeLabel);
		int size=infinityScrollView.list_skip_Index.Count;
	
		size=Mathf.Clamp(EditorGUILayout.IntField("Size:",size,GUILayout.MaxWidth(5000f)), 0, 50);
		if(size<0)
		{
			size=0;
		}
		if(size!=infinityScrollView.list_skip_Index.Count) 
		{
			if(size==0)
			{
				infinityScrollView.list_skip_Index.Clear();
				infinityScrollView.list_skip_Object.Clear();
			}
			else
			{
				if(size>infinityScrollView.list_skip_Index.Count)
				{
					for(int i=infinityScrollView.list_skip_Index.Count;i<size;i++)
					{
						infinityScrollView.list_skip_Index.Add(0);
						infinityScrollView.list_skip_Object.Add(null);
					}
				}
				else
				{
					int total=infinityScrollView.list_skip_Index.Count;
					for(int i=size;i<total;i++)
					{
						infinityScrollView.list_skip_Index.RemoveAt(infinityScrollView.list_skip_Index.Count-1);
						infinityScrollView.list_skip_Object.RemoveAt(infinityScrollView.list_skip_Object.Count-1);
					}
				}
			}
		}
		if (infinityScrollView.list_skip_Index.Count > 0) {
			BeginContentsMaxHeight ();
			for (int i=0; i<infinityScrollView.list_skip_Index.Count; i++) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Skip " + (i + 1).ToString (), GUILayout.MaxWidth (100));
					infinityScrollView.list_skip_Index [i] = EditorGUILayout.IntField ("", infinityScrollView.list_skip_Index [i], GUILayout.MaxWidth (50));
					infinityScrollView.list_skip_Object [i] = (GameObject)EditorGUILayout.ObjectField ("", infinityScrollView.list_skip_Object [i], typeof(GameObject), true, GUILayout.Width (150));
					EditorGUILayout.EndHorizontal ();
			}
			EndContents ();
		}
	}
	public void BeginContentsMaxHeight ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal("Button", GUILayout.MaxHeight(20000f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	public void EndContents ()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}
	#endregion
}
