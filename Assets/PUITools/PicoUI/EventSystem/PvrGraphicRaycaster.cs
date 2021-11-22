using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(Canvas))]
public class PvrGraphicRaycaster : BaseRaycaster {

    protected const int kNoEventMaskSet = -1;

    public enum BlockingObjects
    {
        None = 0,
        TwoD = 1,
        ThreeD = 2,
        All = 3,
    }


    public override int sortOrderPriority
    {
        get
        {
            // We need to return the sorting order here as distance will all be 0 for overlay.
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                return canvas.sortingOrder;

            return base.sortOrderPriority;
        }
    }

    public override int renderOrderPriority
    {
        get
        {
            // We need to return the sorting order here as distance will all be 0 for overlay.
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                return canvas.renderOrder;

            return base.renderOrderPriority;
        }
    }


    [FormerlySerializedAs("ignoreReversedGraphics")]
    [SerializeField]
    private bool m_IgnoreReversedGraphics = true;
    [FormerlySerializedAs("blockingObjects")]
    [SerializeField]
    private BlockingObjects m_BlockingObjects = BlockingObjects.None;

    public bool ignoreReversedGraphics { get { return m_IgnoreReversedGraphics; } set { m_IgnoreReversedGraphics = value; } }
    public BlockingObjects blockingObjects { get { return m_BlockingObjects; } set { m_BlockingObjects = value; } }

    [SerializeField]
    protected LayerMask m_BlockingMask = kNoEventMaskSet;


    public Vector3 MaxPointerEndPoint { get; private set; }


    private Canvas m_Canvas;


    private Canvas canvas
    {
        get
        {
            if (m_Canvas != null)
                return m_Canvas;

            m_Canvas = GetComponent<Canvas>();
            return m_Canvas;
        }
    }

    Camera _eventCamera;

    public override Camera eventCamera
    {
        get
        {
            //Annotation
            //if (_eventCamera == null)
            //    _eventCamera = Pvr_UnitySDKManager.SDK.gameObject.transform.Find("Head").GetComponent<Camera>();

            return _eventCamera;
        }
    }


    protected PvrGraphicRaycaster()
    { }


   
    [NonSerialized] private List<Graphic> m_RaycastResults = new List<Graphic>();
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        if (canvas == null)
        {
            Debug.LogError("PvrGraphicRaycaster requires that the game object  needs 'Canvas' componet !");
            return /*false*/;
        }

        if(eventCamera == null)
        {
            Debug.LogError("PvrGraphicRaycaster requires that the eventCamera is not null");
            return;
        }

        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogError("PvrGraphicRaycaster requires that the canvas renderMode is set to WorldSpace.");
            return /*false*/;
        }


        Ray ray = PvrInputMoudle.ray;

		if (!PUI_UnityAPI.isControllerConnected())
        {
            ray = new Ray(eventCamera.transform.position, eventCamera.transform.forward);
            PvrInputMoudle.ray = ray;
        }
        PvrInputMoudle.FindInputModule().Impl.RayDirection = ray.direction;

        float dist = 20f;  

        MaxPointerEndPoint = ray.GetPoint(dist);

        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * 5F), Color.red);
        float hitDistance = float.MaxValue;
        if(blockingObjects != BlockingObjects.None)   /**标记为 None 标签的不需要处理*/
        {
            
            if(blockingObjects == BlockingObjects.ThreeD || blockingObjects == BlockingObjects.All)
            {
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit,dist,m_BlockingMask))
                {
                    hitDistance = hit.distance;
                    //Debug.Log("Hit postition : " + hit.point);
                    //Debug.Log("ditance : " + Vector3.Distance(hit.point, ray.origin) + "    Hitdistance : " + hitDistance);
                }

            }

            if(blockingObjects == BlockingObjects.TwoD || blockingObjects == BlockingObjects.All)
            {
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, dist, m_BlockingMask);
                if(hit.collider != null)
                {
                    hitDistance = hit.fraction * dist;
                }
            }
        }

        m_RaycastResults.Clear();
        Raycast(canvas, ray, eventCamera, dist, m_RaycastResults);

        //Debug.Log("RaycastResult 数量 ： " + m_RaycastResults.Count);

        for (var index = 0; index < m_RaycastResults.Count; index++)
        {
            //Debug.Log("RaycastResult 数量 ： " + m_RaycastResults.Count + " name "+m_RaycastResults[index].gameObject.name);
            var go = m_RaycastResults[index].gameObject;
            bool appendGraphic = true;
            if (ignoreReversedGraphics)
            {
                // If we have a camera compare the direction against the cameras forward.
                Vector3 cameraFoward = eventCamera.transform.rotation * Vector3.forward;
                Vector3 dir = go.transform.rotation * Vector3.forward;
                appendGraphic = Vector3.Dot(cameraFoward, dir) > 0;
            }

            if (appendGraphic)
            {
                float resultDistance = 0;

                Transform trans = go.transform;
                Vector3 transForward = trans.forward;
                // http://geomalgorithms.com/a06-_intersect-2.html
                float transDot = Vector3.Dot(transForward, trans.position - ray.origin);
                float rayDot = Vector3.Dot(transForward, ray.direction);
                resultDistance = transDot / rayDot;
                Vector3 hitPosition = ray.origin + (ray.direction * resultDistance);
                resultDistance = resultDistance + 0;

                //Debug.Log("resultDistance : "+ resultDistance+ "      hitDistance : "+ hitDistance + "   dist "+dist);

                // Check to see if the go is behind the camera.
                if (resultDistance < 0 || resultDistance >= hitDistance || resultDistance > dist)
                {
                    continue;
                }

                //Transform pointerTransform =
                //  GvrPointerInputModule.Pointer.PointerTransform;
                //float delta = (hitPosition - pointerTransform.position).magnitude;
                //if (delta < pointerRay.distanceFromStart)
                //{
                //    continue;
                //}

                RaycastResult castResult = new RaycastResult
                {
                    gameObject = go,
                    module = this,
                    distance = resultDistance,
                    worldPosition = hitPosition,
                    screenPosition = eventCamera.WorldToScreenPoint(hitPosition),
                    index = resultAppendList.Count,
                    depth = m_RaycastResults[index].depth,
                    sortingLayer = canvas.sortingLayerID,
                    sortingOrder = canvas.sortingOrder
                };
                resultAppendList.Add(castResult);
            }
        }
        //Debug.Log("resultAppendList.Count   :  " + resultAppendList.Count);
    }




    /// <summary>
    /// Perform a raycast into the screen and collect all graphics underneath it.
    /// </summary>
    [NonSerialized]
    static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();
    private static void Raycast(Canvas canvas, Ray ray, Camera cam, float distance,
                              List<Graphic> results)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(ray.GetPoint(distance));
        //finalRay = cam.ScreenPointToRay(screenPoint);

        // Necessary for the event system
        IList<Graphic> foundGraphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
        for (int i = 0; i < foundGraphics.Count; ++i)
        {
            Graphic graphic = foundGraphics[i];

            // -1 means it hasn't been processed by the canvas, which means it isn't actually drawn
            if (graphic.depth == -1 || !graphic.raycastTarget)
            {
                continue;
            }

            if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, screenPoint, cam))
            {
                continue;
            }

            if (graphic.Raycast(screenPoint, cam))
            {
                s_SortedGraphics.Add(graphic);
            }
        }

        s_SortedGraphics.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));

        for (int i = 0; i < s_SortedGraphics.Count; ++i)
        {
            results.Add(s_SortedGraphics[i]);
        }

        s_SortedGraphics.Clear();
    }

}
