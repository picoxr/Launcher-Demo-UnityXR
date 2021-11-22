using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// Wraps UnityEngine.EventSystems.ExecuteEvents.
/// Also, exposes event delegates to allow global handling of events.
public class PvrEventExecutor : IPvrEventExecutor
{

    public delegate void EventDelegate(GameObject target, PointerEventData eventData);

    /// Fired when a Click occurs on any object.
    public event EventDelegate OnPointerClick
    {
        add
        {
            AddEventDelegate<IPointerClickHandler>(value);
        }
        remove
        {
            RemoveEventDelegate<IPointerClickHandler>(value);
        }
    }

    // Fired when a Down event occurs on any object.
    public event EventDelegate OnPointerDown
    {
        add
        {
            AddEventDelegate<IPointerDownHandler>(value);
        }
        remove
        {
            RemoveEventDelegate<IPointerDownHandler>(value);
        }
    }

    // Fired when an Up event occurs on any object.
    public event EventDelegate OnPointerUp
    {
        add
        {
            AddEventDelegate<IPointerUpHandler>(value);
        }
        remove
        {
            RemoveEventDelegate<IPointerUpHandler>(value);
        }
    }

    // Fired when an Enter event occurs on any object.
    public event EventDelegate OnPointerEnter
    {
        add
        {
            AddEventDelegate<IPointerEnterHandler>(value);
        }
        remove
        {
            RemoveEventDelegate<IPointerEnterHandler>(value);
        }
    }

    // Fired when an Exit event occurs on any object.
    public event EventDelegate OnPointerExit
    {
        add
        {
            AddEventDelegate<IPointerExitHandler>(value);
        }
        remove
        {
            RemoveEventDelegate<IPointerExitHandler>(value);
        }
    }

    // Fired when a Scroll event occurs on any object.
    public event EventDelegate OnScroll
    {
        add
        {
            AddEventDelegate<IScrollHandler>(value);
        }
        remove
        {
            RemoveEventDelegate<IScrollHandler>(value);
        }
    }

    /// Stores delegates for events.
    private Dictionary<Type, EventDelegate> eventTable;

    public PvrEventExecutor()
    {
        eventTable = new Dictionary<Type, EventDelegate>();
    }

    public bool Execute<T>(GameObject target,
      BaseEventData eventData,
      ExecuteEvents.EventFunction<T> functor)
      where T : IEventSystemHandler
    {
        bool result = ExecuteEvents.Execute<T>(target, eventData, functor);
        CallEventDelegate<T>(target, eventData);

        return result;
    }

    public GameObject ExecuteHierarchy<T>(GameObject root,
      BaseEventData eventData,
      ExecuteEvents.EventFunction<T> callbackFunction)
      where T : IEventSystemHandler
    {
        GameObject result = ExecuteEvents.ExecuteHierarchy<T>(root, eventData, callbackFunction);
        CallEventDelegate<T>(root, eventData);

        return result;
    }

    public GameObject GetEventHandler<T>(GameObject root)
      where T : IEventSystemHandler
    {
        return ExecuteEvents.GetEventHandler<T>(root);
    }

    private void CallEventDelegate<T>(GameObject target, BaseEventData eventData)
      where T : IEventSystemHandler
    {
        Type type = typeof(T);

        EventDelegate eventDelegate;
        if (eventTable.TryGetValue(type, out eventDelegate))
        {
            PointerEventData pointerEventData = eventData as PointerEventData;
            if (pointerEventData == null)
            {
                Debug.LogError("Event data must be PointerEventData.");
                return;
            }

            eventDelegate(target, pointerEventData);
        }
    }

    private void AddEventDelegate<T>(EventDelegate eventDelegate)
    {
        Type type = typeof(T);

        EventDelegate existingDelegate;
        if (eventTable.TryGetValue(type, out existingDelegate))
        {
            eventTable[type] = existingDelegate + eventDelegate;
        }
        else
        {
            eventTable[type] = eventDelegate;
        }
    }

    private void RemoveEventDelegate<T>(EventDelegate eventDelegate)
    {
        Type type = typeof(T);

        EventDelegate existingDelegate;
        if (!eventTable.TryGetValue(type, out existingDelegate))
        {
            return;
        }

        eventDelegate = existingDelegate - eventDelegate;
        if (eventDelegate != null)
        {
            eventTable[type] = eventDelegate;
        }
        else
        {
            eventTable.Remove(type);
        }
    }
}
