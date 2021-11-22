using UnityEngine;
using System.Collections;
namespace OneP.InfinityScrollView
{
	public class InfinityBaseItem : MonoBehaviour {
		protected InfinityScrollView infinityScrollView;
		private int index = int.MinValue;
		public int Index{
			private set{ 
				index=value;
			}
			get{ 
				return index;
			}
		}
		public InfinityScrollView GetInfinityScrollView(){
			return infinityScrollView;
		}
		//// Use this for initialization
		//void Start () {
		
		//}
		
		//// Update is called once per frame
		//void Update () {
		
		//}


		// using for setup data
		public virtual void Reload(InfinityScrollView infinity,int _index){
			infinityScrollView = infinity;
			Index = _index;
            //Debug.Log("Index: " + Index);
			//todo
		}

		public virtual void SelfReload(){
			if (Index != int.MinValue) {
				//todo
			}
		}
	}
}
