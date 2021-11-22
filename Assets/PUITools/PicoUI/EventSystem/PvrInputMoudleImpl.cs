using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// Implementation of _PvrInputModule_
public class PvrInputMoudleImpl
{
    /// <summary>
    /// Interface for controlling the actual InputModule.
    /// </summary>
    public IPvrInputModuleController MoudleController { get; set; }

    /// <summary>
    /// Interface for executing events.
    /// </summary>
    public IPvrEventExecutor EventExecutor { get; set; }


    public PointerEventData CurrentEventData { get; set; }

    public Vector3 RayDirection;


    private Vector2 lastPose;
    private bool isPointerHovering = false;


    private bool isActive = false;

    public bool ShouldActivateModule()
    {
        bool activeState = MoudleController.ShouldActivate();

        if (isActive != activeState)
            isActive = activeState;

        return isActive;
    }


    public void DeactivateModule()
    {

        MoudleController.Deactivate();
        if (CurrentEventData != null)
        {
            HandlePendingClick();
            HandlePointerExitAndEnter(CurrentEventData, null);
            CurrentEventData = null;
        }
        MoudleController.eventSystem.SetSelectedGameObject(null, MoudleController.GetBaseEventData());
    }


    public bool IsPointerOverGameObject(int pointerId)
    {
        return CurrentEventData != null && CurrentEventData.pointerEnter != null;
    }


    public void Process()
    {
        // If the pointer is inactive, make sure it is exited if necessary.
        //if (!IsPointerActiveAndAvailable())
        //{
        //    TryExitPointer();
        //}

        // Save the previous Game Object
        GameObject previousObject = GetCurrentGameObject();

        CastRay();
        UpdateCurrentObject(previousObject);
        UpdatePointer(previousObject);

        // True during the frame that the trigger has been pressed.
        bool triggerDown = false;
        // True if the trigger is held down.
        bool triggering = false;

        triggerDown = InputController.GetInstance().TriggerDown;  /*Debug.Log("triggerDown    " + triggerDown.ToString());*/
        triggering = InputController.GetInstance().Triggering; /*Debug.Log("triggering    " + triggering.ToString());*/

        //if(triggerDown)
        //{
        //    Debug.Log("triggerDowntriggerDowntriggerDowntriggerDown");
        //}


        //if(triggering)
        //{
        //    Debug.Log("triggeringtriggeringtriggeringtriggeringtriggeringtriggering");
        //}
        //if (IsPointerActiveAndAvailable())
        //{
        //    triggerDown = Pointer.TriggerDown;
        //    triggering = Pointer.Triggering;
        //}

        bool handlePendingClickRequired = !triggering;

        // Handle input
        if (!triggerDown && triggering)
        {
            HandleDrag();
        }
        else if (triggerDown && !CurrentEventData.eligibleForClick)
        {
            // New trigger action.
            HandleTriggerDown();
        }
        else if (handlePendingClickRequired)
        {
            // Check if there is a pending click to handle.
            HandlePendingClick();
        }

        /**暂未接入滑动的东西**/
        //ScrollInput.HandleScroll(GetCurrentGameObject(), CurrentEventData, Pointer, EventExecutor);
    }



    private void CastRay()
    {
        Vector2 currentPos /*= lastPose*/;

        currentPos = NormalizedCartesianToSpherical(RayDirection);
        if(CurrentEventData == null)
        {
            CurrentEventData = new PointerEventData(MoudleController.eventSystem);
            lastPose = currentPos;
        }

        // Store the previous raycast result.
        RaycastResult previousRaycastResult = CurrentEventData.pointerCurrentRaycast;

        // Cast a ray into the scene
        CurrentEventData.Reset();

        // Set the position to the center of the camera.
        // This is only necessary if using the built-in Unity raycasters.
        RaycastResult raycastResult;
        CurrentEventData.position = GetViewportCenter();
        //if (isPointerActiveAndAvailable)
        //{
            RaycastAll();
            raycastResult = MoudleController.FindFirstRaycast(MoudleController.RaycastResultCache);
        //}
        //else
        //{
        //    raycastResult = new RaycastResult();
        //    raycastResult.Clear();
        //}

        // If we were already pointing at an object we must check that object against the exit radius
        // to make sure we are no longer pointing at it to prevent flicker.
        if (previousRaycastResult.gameObject != null
            && raycastResult.gameObject != previousRaycastResult.gameObject)
        {
            RaycastAll();
            RaycastResult firstResult = MoudleController.FindFirstRaycast(MoudleController.RaycastResultCache);
            if (firstResult.gameObject == previousRaycastResult.gameObject)
            {
                raycastResult = firstResult;
            }
        }

        if (raycastResult.gameObject != null && raycastResult.worldPosition == Vector3.zero)
        {
            raycastResult.worldPosition =
              GetIntersectionPosition(CurrentEventData.enterEventCamera, raycastResult);
        }

        CurrentEventData.pointerCurrentRaycast = raycastResult;


        // Find the real screen position associated with the raycast
        // Based on the results of the hit and the state of the pointerData.
        if (raycastResult.gameObject != null)
        {
            CurrentEventData.position = raycastResult.screenPosition;
        }
        else if ( CurrentEventData.enterEventCamera != null)
        {
            //Vector3 pointerPos = Pointer.MaxPointerEndPoint;
            //CurrentEventData.position = CurrentEventData.enterEventCamera.WorldToScreenPoint(pointerPos);
            if (raycastResult.module != null && !(raycastResult.module is PvrGraphicRaycaster))
            {
                Vector3 pointerPos = (raycastResult.module as PvrGraphicRaycaster).MaxPointerEndPoint;
                CurrentEventData.position = CurrentEventData.enterEventCamera.WorldToScreenPoint(pointerPos);
            }
        }

        MoudleController.RaycastResultCache.Clear();
        CurrentEventData.delta = currentPos - lastPose;
        lastPose = currentPos;

        // Check to make sure the Raycaster being used is a GvrRaycaster.
        if (raycastResult.module != null
            && !(raycastResult.module is PvrGraphicRaycaster)
            && !(raycastResult.module is PvrGraphicRaycaster))
        {
            Debug.LogWarning("Using Raycaster (Raycaster: " + raycastResult.module.GetType() +
              ", Object: " + raycastResult.module.name + "). It is recommended to use " +
              "GvrPointerPhysicsRaycaster or GvrPointerGrahpicRaycaster with GvrPointerInputModule.");
        }
    }


    private void UpdateCurrentObject(GameObject previousObject)
    {
        if (CurrentEventData == null)
        {
            return;
        }
        // Send enter events and update the highlight.
        GameObject currentObject = GetCurrentGameObject(); // Get the pointer target
        HandlePointerExitAndEnter(CurrentEventData, currentObject);

        // Update the current selection, or clear if it is no longer the current object.
        var selected = EventExecutor.GetEventHandler<ISelectHandler>(currentObject);
        if (selected == MoudleController.eventSystem.currentSelectedGameObject)
        {
            EventExecutor.Execute(MoudleController.eventSystem.currentSelectedGameObject, MoudleController.GetBaseEventData(),
              ExecuteEvents.updateSelectedHandler);
        }
        else
        {
            MoudleController.eventSystem.SetSelectedGameObject(null, CurrentEventData);
        }

        /**Google VR定义的Hover事件，暂不清楚具体机制，放弃；UGUI的Hover事件是pointerExitHandler 和 pointerEnterHandler来实现的**/
        // Execute hover event.   
        //if (currentObject != null && currentObject == previousObject)
        //{
        //    EventExecutor.ExecuteHierarchy(currentObject, CurrentEventData, GvrExecuteEventsExtension.pointerHoverHandler);
        //}
    }


    private void UpdatePointer(GameObject previousObject)
    {
        if (CurrentEventData == null)
        {
            return;
        }

        GameObject currentObject = GetCurrentGameObject(); // Get the pointer target
        //bool isPointerActiveAndAvailable = IsPointerActiveAndAvailable();
        
        bool isInteractive = CurrentEventData.pointerPress != null ||
                             EventExecutor.GetEventHandler<IPointerClickHandler>(currentObject) != null ||
                             EventExecutor.GetEventHandler<IDragHandler>(currentObject) != null;

        if (isPointerHovering && currentObject != null && currentObject == previousObject)
        {
            //if (isPointerActiveAndAvailable)
            //{
            //    Pointer.OnPointerHover(CurrentEventData.pointerCurrentRaycast, isInteractive);
            //}
        }
        else
        {
            // If the object's don't match or the hovering object has been destroyed
            // then the pointer has exited.
            if (previousObject != null || (currentObject == null && isPointerHovering))
            {
                //if (isPointerActiveAndAvailable)
                //{
                //    Pointer.OnPointerExit(previousObject);
                //}
                isPointerHovering = false;
            }

            if (currentObject != null)
            {
                //    if (isPointerActiveAndAvailable)
                //    {
                //        Pointer.OnPointerEnter(CurrentEventData.pointerCurrentRaycast, isInteractive);
                //    }
                isPointerHovering = true;
            }
        }
    }


    private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
    {
        if (!useDragThreshold)
            return true;

        return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
    }


    private void HandleDrag()
    {
        bool moving = CurrentEventData.IsPointerMoving();
        bool shouldStartDrag = ShouldStartDrag(CurrentEventData.pressPosition,
                                 CurrentEventData.position,
                                 MoudleController.eventSystem.pixelDragThreshold,
                                 CurrentEventData.useDragThreshold);
        var go = CurrentEventData.pointerCurrentRaycast.gameObject;

        if (moving && shouldStartDrag && CurrentEventData.pointerDrag != null && !CurrentEventData.dragging)
        {
            EventExecutor.Execute(CurrentEventData.pointerDrag, CurrentEventData,
              ExecuteEvents.beginDragHandler);
            CurrentEventData.dragging = true;
        }

        // Drag notification
        if (CurrentEventData.dragging && moving && CurrentEventData.pointerDrag != null)
        {
            // Before doing drag we should cancel any pointer down state
            // And clear selection!
            if (CurrentEventData.pointerPress != CurrentEventData.pointerDrag)
            {
                EventExecutor.Execute(CurrentEventData.pointerPress, CurrentEventData, ExecuteEvents.pointerUpHandler);

                CurrentEventData.eligibleForClick = false;
                CurrentEventData.pointerPress = null;
                CurrentEventData.rawPointerPress = null;
            }

            EventExecutor.Execute(CurrentEventData.pointerDrag, CurrentEventData, ExecuteEvents.dragHandler);
        }
    }


    private void HandlePendingClick()
    {
        if (CurrentEventData == null || (!CurrentEventData.eligibleForClick && !CurrentEventData.dragging))
        {
            return;
        }

        //if (IsPointerActiveAndAvailable())
        //{
        //    Pointer.OnPointerClickUp();
        //}

        var go = CurrentEventData.pointerCurrentRaycast.gameObject;

        if (go == null)
        {
            if (CurrentEventData.eligibleForClick)
                ClearClickState();
            return;
        }
        //Debug.Log("执行Click！！！！！！！  Click gameobject name  "+go.name);
        // Send pointer up and click events.
        EventExecutor.Execute(CurrentEventData.pointerPress, CurrentEventData, ExecuteEvents.pointerUpHandler);

        GameObject pointerClickHandler = EventExecutor.GetEventHandler<IPointerClickHandler>(go);
        if (CurrentEventData.pointerPress == pointerClickHandler && CurrentEventData.eligibleForClick)
        {
            EventExecutor.Execute(CurrentEventData.pointerPress, CurrentEventData, ExecuteEvents.pointerClickHandler);
        }

        if (CurrentEventData.pointerDrag != null && CurrentEventData.dragging)
        {
            EventExecutor.ExecuteHierarchy(go, CurrentEventData, ExecuteEvents.dropHandler);
            EventExecutor.Execute(CurrentEventData.pointerDrag, CurrentEventData, ExecuteEvents.endDragHandler);
        }

        // Clear the click state.
        //CurrentEventData.pointerPress = null;
        //CurrentEventData.rawPointerPress = null;
        //CurrentEventData.eligibleForClick = false;
        //CurrentEventData.clickCount = 0;
        //CurrentEventData.clickTime = 0;
        //CurrentEventData.pointerDrag = null;
        //CurrentEventData.dragging = false;
        ClearClickState();
    }


    private void ClearClickState()
    {
        // Clear the click state.
        CurrentEventData.pointerPress = null;
        CurrentEventData.rawPointerPress = null;
        CurrentEventData.eligibleForClick = false;
        CurrentEventData.clickCount = 0;
        CurrentEventData.clickTime = 0;
        CurrentEventData.pointerDrag = null;
        CurrentEventData.dragging = false;
    }


    private void HandleTriggerDown()
    {
        var go = CurrentEventData.pointerCurrentRaycast.gameObject;

        // Send pointer down event.
        CurrentEventData.pressPosition = CurrentEventData.position;
        CurrentEventData.pointerPressRaycast = CurrentEventData.pointerCurrentRaycast;
        CurrentEventData.pointerPress =
          EventExecutor.ExecuteHierarchy(go, CurrentEventData, ExecuteEvents.pointerDownHandler) ??
          EventExecutor.GetEventHandler<IPointerClickHandler>(go);

        // Save the pending click state.
        CurrentEventData.rawPointerPress = go;
        CurrentEventData.eligibleForClick = true;
        CurrentEventData.delta = Vector2.zero;
        CurrentEventData.dragging = false;
        CurrentEventData.useDragThreshold = true;
        CurrentEventData.clickCount = 1;
        CurrentEventData.clickTime = Time.unscaledTime;

        // Save the drag handler as well
        CurrentEventData.pointerDrag = EventExecutor.GetEventHandler<IDragHandler>(go);
        if (CurrentEventData.pointerDrag != null)
        {
            EventExecutor.Execute(CurrentEventData.pointerDrag, CurrentEventData, ExecuteEvents.initializePotentialDrag);
        }

        //if (IsPointerActiveAndAvailable())
        //{
        //    Pointer.OnPointerClickDown();
        //}
    }




    private GameObject GetCurrentGameObject()
    {
        if (CurrentEventData != null)
        {
            return CurrentEventData.pointerCurrentRaycast.gameObject;
        }

        return null;
    }


    // Modified version of BaseInputModule.HandlePointerExitAndEnter that calls EventExecutor instead of
    // UnityEngine.EventSystems.ExecuteEvents.
    private void HandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget) {
    // If we have no target or pointerEnter has been deleted then
    // just send exit events to anything we are tracking.
    // Afterwards, exit.
    if (newEnterTarget == null || currentPointerData.pointerEnter == null) {
      for (var i = 0; i < currentPointerData.hovered.Count; ++i) {
        EventExecutor.Execute(currentPointerData.hovered[i], currentPointerData, ExecuteEvents.pointerExitHandler);
      }

      currentPointerData.hovered.Clear();

      if (newEnterTarget == null) {
        currentPointerData.pointerEnter = newEnterTarget;
        return;
      }
    }

    // If we have not changed hover target.
    if (newEnterTarget && currentPointerData.pointerEnter == newEnterTarget) {
      return;
    }

    GameObject commonRoot = MoudleController.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);

    // We already an entered object from last time.
    if (currentPointerData.pointerEnter != null) {
      // Send exit handler call to all elements in the chain
      // until we reach the new target, or null!
      Transform t = currentPointerData.pointerEnter.transform;

      while (t != null) {
        // If we reach the common root break out!
        if (commonRoot != null && commonRoot.transform == t)
          break;

        EventExecutor.Execute(t.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
        currentPointerData.hovered.Remove(t.gameObject);
        t = t.parent;
      }
    }

    // Now issue the enter call up to but not including the common root.
    currentPointerData.pointerEnter = newEnterTarget;
    if (newEnterTarget != null) {
      Transform t = newEnterTarget.transform;

      while (t != null && t.gameObject != commonRoot) {
        EventExecutor.Execute(t.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
        currentPointerData.hovered.Add(t.gameObject);
        t = t.parent;
      }
    }
  }



    private void RaycastAll()
    {
        MoudleController.RaycastResultCache.Clear();
        MoudleController.eventSystem.RaycastAll(CurrentEventData, MoudleController.RaycastResultCache);
    }



    public static Vector2 NormalizedCartesianToSpherical(Vector3 cartCoords)
    {
        cartCoords.Normalize();

        if (cartCoords.x == 0)
        {
            cartCoords.x = Mathf.Epsilon;
        }

        float polar = Mathf.Atan(cartCoords.z / cartCoords.x);

        if (cartCoords.x < 0)
        {
            polar += Mathf.PI;
        }

        float elevation = Mathf.Asin(cartCoords.y);
        return new Vector2(polar, elevation);
    }


    public static Vector2 GetViewportCenter()
    {
        int viewportWidth = Screen.width;
        int viewportHeight = Screen.height;

        return new Vector2(0.5f * viewportWidth, 0.5f * viewportHeight);
    }


    public static Vector3 GetIntersectionPosition(Camera cam, RaycastResult raycastResult)
    {
        // Check for camera
        if (cam == null)
        {
            return Vector3.zero;
        }

        float intersectionDistance = raycastResult.distance + cam.nearClipPlane;
        Vector3 intersectionPosition = cam.transform.position + cam.transform.forward * intersectionDistance;
        return intersectionPosition;
    }

}
