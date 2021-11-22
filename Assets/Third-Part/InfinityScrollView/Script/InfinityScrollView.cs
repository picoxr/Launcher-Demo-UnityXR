using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace OneP.InfinityScrollView
{
	public enum InfinityType
	{
		Vertical,
		Horizontal
	}

	public enum VerticalType
	{
		TopToBottom,
		BottomToTop
	}

	public enum HorizontalType
	{
		LeftToRight,
		RigthToLeft
	}

	public enum DetemineLocationType{
		BaseOnObjectCreate,
		OverrideLocation
	}

	public class InfinityScrollView : MonoBehaviour {
		[Header("Setting Reference object")]
		public GameObject prefab; // link object item
		public ScrollRect scrollRect;// link to UGUI scrollRect
		public RectTransform content;// link to content that contain all item in scrollrect

		[Header("Setting For Custom Scroll View")]
		public InfinityType type=InfinityType.Vertical;// type scrollview
		public VerticalType verticalType=VerticalType.TopToBottom;
		public HorizontalType horizontalType=HorizontalType.LeftToRight;
		public DetemineLocationType locationType=DetemineLocationType.OverrideLocation;
		public float overrideX=0;
		public float overrideY=0;
		public float extraContentLength=0;


		[Header("Setting For Custom Data")]
		public float itemSize=100; // size of an item 
		public int itemGenerate=10; // number item generate, note: only need create +2 more item appear, if max item appear in screen is 5 =>itemGenerate =7 is enough
		public int totalNumberItem=100;// total item of scrollview


		[Header("Setting if want to skip some index item")]
		public List<int> list_skip_Index=new List<int>(); // contain location of skip object
		public List<GameObject> list_skip_Object=new List<GameObject>(); // object want to skip 

		[Header("flat check auto setup references")]
		public bool isAutoLinking=true;
		public bool isOverrideSettingScrollbar=true;
		public bool setupOnAwake=false;

		private List<GameObject> listItem = new List<GameObject> ();
		private GameObject[] arrayCurrent = null;
		private int cacheOld=-1;
		private bool isInit=false;


		public void Setup(int numberItem){
			totalNumberItem = numberItem;
			if (totalNumberItem<0) {
				totalNumberItem = 0;
			}
			Setup ();
		}


		public void Setup(){
			if (prefab == null) {
				Debug.LogWarning("No prefab/Gameobject Item linking");
				return;		
			}
			if (type == InfinityType.Vertical) {
				int totalHeight = (int)((totalNumberItem + list_skip_Index.Count) * itemSize);
				content.SetHeight (totalHeight+extraContentLength);
			} else {
				int totalWidth = (int)((totalNumberItem + list_skip_Index.Count) * itemSize);
				content.SetWidth (totalWidth+extraContentLength);
			}
			//reset Array
			arrayCurrent = new GameObject[totalNumberItem];

			for (int i = 0; i < itemGenerate; i++) {
				GameObject obj = null;
				if (!isInit) {
					//if (i < totalNumberItem) {  /**修改**/
						obj = GameObject.Instantiate (prefab) as GameObject;
						obj.name = "item_" + (i);
						obj.transform.SetParent (content.transform, false);
						obj.transform.localScale = Vector3.one;
						listItem.Add (obj);
                        obj.SetActive(true);
						if(type==InfinityType.Vertical)
						{
							RectTransform rect=obj.GetComponent<RectTransform>();
							if(rect!=null){
								Vector2 anchor=rect.pivot;
								if(verticalType==VerticalType.BottomToTop)
								{
									Vector2 min=rect.anchorMin;
									Vector2 max=rect.anchorMax;
									rect.anchorMin=new Vector2(min.x,0);
									rect.anchorMax=new Vector2(max.x,0);
									rect.pivot=new Vector2(anchor.x,0);
								}
								else{
									rect.pivot=new Vector2(anchor.x,1);
								}
							}
						}
						else if(type==InfinityType.Horizontal)
						{
							RectTransform rect=obj.GetComponent<RectTransform>();
							if(rect!=null){
								Vector2 anchor=rect.pivot;
								if(horizontalType==HorizontalType.RigthToLeft)
								{
									Vector2 min=rect.anchorMin;
									Vector2 max=rect.anchorMax;
									rect.anchorMin=new Vector2(1,min.y);
									rect.anchorMax=new Vector2(1,max.y);
									rect.pivot=new Vector2(1,anchor.y);
								}
								else{
									rect.pivot=new Vector2(0,anchor.y);
								}
							}
						}
						Reload (obj, i);
					if(i < totalNumberItem) /**修改**/
                        arrayCurrent [i] = obj;
					//}
				}
				else
				{
					if (i < totalNumberItem) {
						obj = listItem [i];
						obj.SetActive (true);
						Reload (obj, i);
						arrayCurrent [i] = obj;
					} else {
						obj = listItem [i];
						obj.SetActive (false);
					}
				}
			}
			isInit = true;
			int add = 0;
			for (int i = 0; i < list_skip_Index.Count; i++) {
				if (i < list_skip_Object.Count&&list_skip_Object[i]!=null) {
					int index = list_skip_Index [i];
					if(index>totalNumberItem){
						index=totalNumberItem;
					}
					index+=add;
					add++;
					Vector3 scale=list_skip_Object[i].transform.localScale;
					if(list_skip_Object[i].activeInHierarchy==false)//invisible or prefab
					{
						GameObject obj=GameObject.Instantiate(list_skip_Object[i]) as GameObject;
						obj.SetActive(true);
						list_skip_Object[i]=obj;
					}
					list_skip_Object[i].transform.SetParent(content.transform);
					list_skip_Object[i].transform.localScale=scale;
					if(type==InfinityType.Vertical)
					{
						RectTransform rect=list_skip_Object[i].GetComponent<RectTransform>();
						if(rect!=null){
							Vector2 anchor=rect.pivot;
							if(verticalType==VerticalType.BottomToTop)
							{
								Vector2 min=rect.anchorMin;
								Vector2 max=rect.anchorMax;
								rect.anchorMin=new Vector2(min.x,0);
								rect.anchorMax=new Vector2(max.x,0);
								rect.pivot=new Vector2(anchor.x,0);
							}
							else{
								rect.pivot=new Vector2(anchor.x,1);
							}
						}
					}
					else if(type==InfinityType.Horizontal)
					{
						RectTransform rect=list_skip_Object[i].GetComponent<RectTransform>();
						if(rect!=null){
							Vector2 anchor=rect.pivot;
							if(horizontalType==HorizontalType.RigthToLeft)
							{
								Vector2 min=rect.anchorMin;
								Vector2 max=rect.anchorMax;
								rect.anchorMin=new Vector2(1,min.y);
								rect.anchorMax=new Vector2(1,max.y);
								rect.pivot=new Vector2(1,anchor.y);
							}
							else{
								rect.pivot=new Vector2(0,anchor.y);
							}
						}
					}
					Vector3 vec=Vector3.zero;
					if(locationType==DetemineLocationType.BaseOnObjectCreate){
						if(listItem.Count>0&&listItem[0]!=null)
						{
							vec=listItem[0].transform.localPosition;
						}
					}
					else
					{
						vec=new Vector2(overrideX,overrideY);
					}
					list_skip_Object [i].transform.localPosition=GetLocationAppear (vec, index);
					
				}
			}
		}
		private float GetContentSize(){
			return content.GetHeight ();
		}

		public int GetLocaltionWithSkip(int index){
			int location = index;
			for (int i = 0; i < list_skip_Index.Count; i++) {
				if (list_skip_Index [i] <= index) {
					location++;
				}
			}
			return location;
		}

		public int GetIndexRejectSkip(int index){
			int location = index;
			for (int i = 0; i < list_skip_Index.Count; i++) {
				if (list_skip_Index [i] <= index) {
					location--;
				}
			}

			if (location < 0) {
				return 0;
			}

			return location;
		}


		void Awake(){
            if(scrollRect == null)
			    scrollRect = GetComponent<ScrollRect> ();
			scrollRect.onValueChanged.AddListener (OnScrollChange);
			if (setupOnAwake) {
				Setup ();
			}
		}

		private int GetCurrentIndex(){
			int index = -1;
			if (type == InfinityType.Vertical) {
				if(verticalType==VerticalType.TopToBottom)
				{
					index = (int)(content.anchoredPosition.y / itemSize);
				}
				else
				{
					index = (int)(-content.anchoredPosition.y / itemSize);
				}
			} else {
				if(horizontalType==HorizontalType.LeftToRight)
				{
					index = (int)(-content.anchoredPosition.x / itemSize);

				}
				else
				{
					index = (int)(content.anchoredPosition.x / itemSize);
				}
			}
			if (index < 0)
				index = 0;
			if (index > totalNumberItem-1) {
				index = totalNumberItem - 1;
			}
			return index;
		}
		public void InternalReload(){
			
			int index = GetCurrentIndex ();
			index = GetIndexRejectSkip (index);
			FixFastReload (index);
		}
		public void OnScrollChange(Vector2 vec){
			if (arrayCurrent.Length < 1) {
				return;
			}

			int index = GetCurrentIndex ();
			index = GetIndexRejectSkip (index);
			if (cacheOld != index) {
				cacheOld = index;
			} else {
				return;
			}
			if (!FixFastReload (index)) {
				GameObject objIndex = arrayCurrent [index];
				if (objIndex == null) {// truot len
					int next = index + itemGenerate;
					if (next > totalNumberItem - 1) {
						return;
					} else {
						GameObject objNow = arrayCurrent [next];
						if (objNow != null) {//swap
							arrayCurrent [next] = objIndex;
							arrayCurrent [index] = objNow;
							Reload (arrayCurrent [index], index);
						}
					}
				} else {// truot xuong
					if (index > 0) {
						GameObject obj = arrayCurrent [index - 1];
						if (obj == null) {
							return;
						}
						int next = index - 1 + itemGenerate;
						if (next > totalNumberItem - 1) {
							return;
						} else {
							GameObject objNow = arrayCurrent [next];
							if (objNow == null) {//swap
								arrayCurrent [next] = obj;
								arrayCurrent [index - 1] = objNow;
								Reload (arrayCurrent [next], next);
							}
						}
					}
				}
			}
		}
		
		public bool FixFastReload(int index){
			bool isNeedFix = false;

			int add = index + 1;
			for (int i = add; i < add + itemGenerate - 2; i++) {
				if (i < totalNumberItem) {
					GameObject obj = arrayCurrent [i];
					if (obj == null) {
						isNeedFix = true;
						break;
					} else if (!obj.name.Equals ("item_" + i)) {
						isNeedFix = true;
						break;
					}
				}
			}

			if (isNeedFix) {
				for (int i = 0; i < totalNumberItem; i++) {
					arrayCurrent [i] = null;
				}

				int start = index;
				if (start + itemGenerate > totalNumberItem ) {
					start = totalNumberItem  - itemGenerate;
				}
				//Debug.LogError ("Fix Fast reload:"+start+","+index);
				for (int i = 0; i <  itemGenerate; i++) {
					arrayCurrent [start + i] = listItem [i];
					Reload (arrayCurrent [start + i], start + i);
				}
				return true;
			}
			return false;
		}

		protected virtual void Reload(GameObject obj,int indexReload){
			obj.transform.name = "item_" + indexReload;
			int location = GetLocaltionWithSkip (indexReload);
			Vector3 vec=Vector3.zero;
			if(locationType==DetemineLocationType.BaseOnObjectCreate){
				vec.x=obj.transform.localPosition.x;
				vec.y=obj.transform.localPosition.y;
			}
			else
			{
				vec=new Vector2(overrideX,overrideY);
			}
			vec = GetLocationAppear (vec, location);
			obj.transform.localPosition=vec;
			InfinityBaseItem baseItem = obj.GetComponent<InfinityBaseItem> ();
			if (baseItem != null) {
				baseItem.Reload (this,indexReload);
			}
		}

		private Vector3 GetLocationAppear(Vector2 initVec,int location){
			Vector3 vec=initVec;
			if (type == InfinityType.Vertical) {
				if (verticalType == VerticalType.TopToBottom) {
					vec = new Vector3 (vec.x, -itemSize * location/* - itemSize / 2*/, 0);
				} else {
					vec = new Vector3 (vec.x, itemSize * location /*+ itemSize / 2*/, 0);
				}
			} else {
				if(horizontalType==HorizontalType.LeftToRight){
					vec=new Vector3(itemSize*location/*+itemSize/2*/,vec.y,0);
				}
				else
				{
					vec=new Vector3(-itemSize*location/*-itemSize/2*/,vec.y,0);
				}
			}
			return vec;
		}
		//// Use this for initialization
		//void Start () {
		
		//}
		
		//// Update is called once per frame
		//void Update () {
		
		//}
	}
}
