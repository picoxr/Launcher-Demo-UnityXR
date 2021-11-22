using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RayController : MonoBehaviour {
    [SerializeField]
	private Transform rayalpha;
    [SerializeField]
	private Transform dot;
    [SerializeField]
	private Transform start;
    [SerializeField]
	private GameObject point;
    private Camera camera;
	private Transform head;
    private Ray ray;

    private bool isInit = false;

    void Start()
    {
        Init();
    }


    public void Init()
    {
        if (isInit)
            return;
        isInit = true;
        //Annotation
        //if (head == null)
        //    head = FindObjectOfType<Pvr_UnitySDKEyeManager>().transform;
        int childCount = transform.childCount;
        Transform tranTemp;
        for (int i = 0; i < childCount; i++)
        {
            tranTemp = transform.GetChild(i);
            if (tranTemp.name.Equals("dot"))
            {
                dot = tranTemp;
                Debug.Log("PUI------- DOT is not null");
            }
            else if (tranTemp.name.Equals("ray_alpha"))
            {
                rayalpha = tranTemp;
                Debug.Log("PUI------- ray_alpha is not null");
            }
            else if (tranTemp.name.Equals("start"))
            {
                start = tranTemp;
                Debug.Log("PUI------- start is not null");
            }
            else if (tranTemp.name.Equals("point"))
            {
                point = tranTemp.gameObject;
                Debug.Log("PUI------- point is not null");
            }
        }
        ray = new Ray { origin = start.position };
        //Annotation
        //camera = Pvr_UnitySDKManager.SDK.gameObject.transform.Find("Head").GetComponent<Camera>();
        //head = FindObjectOfType<Pvr_UnitySDKEyeManager>().transform;

        //PvrInputMoudle.FindInputModule().onRefreshCursor += RefreshCursor;
    }



    // Update is called once per frame
    void Update () {
        ray.direction = (dot.position - camera.transform.position).normalized;
        ray.origin = camera.transform.position;
        PvrInputMoudle.ray = this.ray; 
    }

    void RefreshCursor(UnityEngine.EventSystems.RaycastResult raycastResult)
    {
        if (raycastResult.gameObject != null)
        {

            dot.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        }
        else
        {
            dot.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);

        }
    }

//    void RefreshCursor(UnityEngine.EventSystems.RaycastResult raycastResult)
//    {
//        if(raycastResult.gameObject != null)
//        {
////            if (!point.activeInHierarchy)
////            {
//                point.SetActive(true);
//                point.transform.position = raycastResult.worldPosition;
//				float distance = Vector3.Distance(point.transform.position, head.position);
//                float scale = 0.012f * distance / 3.3f;
//                point.transform.localScale = scale * Vector3.one;
//                dot.gameObject.SetActive(false);
////            }
//        }
//        else
//        {
////            if (point.activeSelf)
////            {
//                point.transform.localScale = Vector3.zero;
//                point.SetActive(false);
//                dot.gameObject.SetActive(true);
////            }
//        }
//    }



    /// <summary>
    /// 启用该组件
    /// </summary>
    public void SetEnable()
    {
        gameObject.GetComponent<RayController>().enabled = true;
		IsShowRay (true);
        PvrInputMoudle.FindInputModule().onRefreshCursor += RefreshCursor;
    }

    /// <summary>
    /// 禁用该组件
    /// </summary>
    public void SetDisable()
    {
        gameObject.GetComponent<RayController>().enabled = false;
		IsShowRay (false);
        PvrInputMoudle.FindInputModule().onRefreshCursor -= RefreshCursor;
    }

	private void IsShowRay(bool isShow)
	{
		rayalpha.gameObject.SetActive (isShow);
		dot.gameObject.SetActive (isShow);
		point.gameObject.SetActive (isShow);
	}

}
