using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PvrInputMoudle : BaseInputModule, IPvrInputModuleController
{

    public static Ray ray;

    public PvrInputMoudleImpl Impl { get; private set; }

    public PvrEventExecutor EventExecutor { get; private set; }

    public Action<RaycastResult> onRefreshCursor;


    /// hover obj
    public GameObject hoverObj
    {

        get
        {
            return Impl == null || Impl.CurrentEventData ==null ? null : Impl.CurrentEventData.pointerEnter; 
        }

    }

    /// press obj
    public GameObject pressObj
    {
        get
        {
            return Impl == null || Impl.CurrentEventData == null ? null : Impl.CurrentEventData.pointerPress;
        }
    }


    public new EventSystem eventSystem
    {
        get
        {
            return base.eventSystem;
        }
    }



    public List<RaycastResult> RaycastResultCache
    {
        get
        {
            return m_RaycastResultCache;
        }
    }

    /// Helper function to find the Event Executor that is part of
    /// the input module if one exists in the scene.
    public static PvrEventExecutor FindEventExecutor()
    {
        PvrInputMoudle pvrInputModule = FindInputModule();
        if (pvrInputModule == null)
        {
            return null;
        }

        return pvrInputModule.EventExecutor;
    }


    /// Helper function to find the input module if one exists in the
    /// scene and it is the active module.
    public static PvrInputMoudle FindInputModule()
    {
        if (EventSystem.current == null)
        {
            return null;
        }

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            return null;
        }

        PvrInputMoudle pvrInputModule =
          eventSystem.GetComponent<PvrInputMoudle>();

        return pvrInputModule;
    }

    /// Convenience function to access what the current RaycastResult.
    public static RaycastResult CurrentRaycastResult
    {
        get
        {
            PvrInputMoudle inputModule = PvrInputMoudle.FindInputModule();
            if (inputModule == null)
            {
                return new RaycastResult();
            }

            if (inputModule.Impl == null)
            {
                return new RaycastResult();
            }

            if (inputModule.Impl.CurrentEventData == null)
            {
                return new RaycastResult();
            }

            return inputModule.Impl.CurrentEventData.pointerCurrentRaycast;
        }
    }


    public override bool ShouldActivateModule()
    {
        return Impl.ShouldActivateModule();
    }

    public override void DeactivateModule()
    {
        Impl.DeactivateModule();
    }

    public override bool IsPointerOverGameObject(int pointerId)
    {
        return Impl.IsPointerOverGameObject(pointerId);
    }

    public override void Process()
    {
        UpdateImplProperties();
        Impl.Process();
        if(onRefreshCursor != null)
        {
            //Debug.Log("执行onRefreshCursor000000事件");
            onRefreshCursor(Impl.CurrentEventData.pointerCurrentRaycast);
        }
    }


    protected override void Awake()
    {
        base.Awake();
        Impl = new PvrInputMoudleImpl();
        EventExecutor = new PvrEventExecutor();
        UpdateImplProperties();
    }

    public bool ShouldActivate()
    {
        return base.ShouldActivateModule();
    }

    public void Deactivate()
    {
        base.DeactivateModule();
    }

    public new GameObject FindCommonRoot(GameObject g1, GameObject g2)
    {
        return BaseInputModule.FindCommonRoot(g1, g2);
    }

    public new BaseEventData GetBaseEventData()
    {
        return base.GetBaseEventData();
    }

    public new RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
    {
        return BaseInputModule.FindFirstRaycast(candidates);
    }



    private void UpdateImplProperties()
    {
        if (Impl == null)
        {
            return;
        }

        //Impl.ScrollInput = scrollInput;
        //Impl.VrModeOnly = vrModeOnly;
        Impl.MoudleController = this;
        Impl.EventExecutor = EventExecutor;
    }
}
