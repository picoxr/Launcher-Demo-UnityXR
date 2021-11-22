using UnityEngine;
using UnityEngine.EventSystems;

public class PUIEventListener : EventTrigger {


    public System.Action<GameObject> onClick;

    public System.Action<GameObject, bool> onHover;

    public System.Action<GameObject, bool> onPress;



    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        //base.OnPointerUp(eventData);
        if (onPress != null)
        {
            onPress(gameObject, false);
        }
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        if (onPress != null)
        {
            onPress(gameObject, true);
        }
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        //base.OnPointerClick(eventData);
        if(onClick != null)
        {
            onClick(gameObject);
        }

    }



    public override void OnUpdateSelected(BaseEventData eventData)
    {
        base.OnUpdateSelected(eventData);
    }


    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //base.OnPointerEnter(eventData);
        if(onHover != null)
        {
            onHover(gameObject, true);
        }

    }


    public override void OnPointerExit(PointerEventData eventData)
    {
        //base.OnPointerExit(eventData);
        if (onHover != null)
        {
            onHover(gameObject, false);
        }
    }



    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        base.OnInitializePotentialDrag(eventData);
    }


    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }



    static public PUIEventListener Get(GameObject go)
    {
        PUIEventListener listener = go.GetComponent<PUIEventListener>();
        if (listener == null) listener = go.AddComponent<PUIEventListener>();
        return listener;
    }




}
