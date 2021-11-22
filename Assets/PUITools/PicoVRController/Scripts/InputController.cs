using UnityEngine;
using System.Collections.Generic;
using System;
public class InputController : MonoBehaviour {

    private static InputController m_Instance;


    private DeviceMode currentDevice = DeviceMode.Falcon;

    private Dictionary<string, LinkedList<Action>> _listners;

    private int m_Hand = 0;

    public int Hand
    {
        get
        {
            return m_Hand;
        }
		set
		{
            if (value < 0 || value > 1)
            {
                m_Hand = 0;
                Debug.Log("value: " + value + "  ");
                return;
            }
            m_Hand = value;
            Hand_Second = 1 - m_Hand;
        }
    }


    private int m_Hand_Second = 1;

    public int Hand_Second
    {
        get
        {
            return m_Hand_Second;
        }
        private set
        {
            m_Hand_Second = value;
        }
    }

    



    public static InputController GetInstance()
    {
        if(m_Instance == null)
        {
            m_Instance = FindObjectOfType<InputController>();
            if (m_Instance == null)
                m_Instance = new GameObject("InputController").AddComponent<InputController>();
            m_Instance.GetDeviceInfo();
            try
            {
                if (m_Instance.currentDevice == DeviceMode.FalconCV || m_Instance.currentDevice == DeviceMode.FalconCV2)
                    m_Instance.GetMainController();
            }
            catch (System.Exception error)
            {
                Debug.Log("Main Controller Error：  " + error.Message);
            }

    }
        return m_Instance;
    }


    private void GetDeviceInfo()
    {
        currentDevice = PUI_UnityAPI.GetDeviceMode();
    }


  
    private void GetMainController()
    {
        //Annotation
        //m_Hand = Pvr_UnitySDKAPI.Controller.UPvr_GetMainHandNess();
    }


  
    private void OnChangeController(int hand)
    {
        this.m_Hand = hand;
    }




  
    public void AddListener(ListenerEventType eventName, Action handler)
    {
        string key = eventName.ToString();
        if (_listners == null)
        {
            _listners = new Dictionary<string, LinkedList<Action>>();
        }
        if (_listners.ContainsKey(key))
        {
            _listners[key].AddLast(handler);
        }
        else
        {
            LinkedList<Action> handlers = new LinkedList<Action>();
            handlers.AddLast(handler);
            _listners.Add(key, handlers);
        }
    }


   
    public virtual void RemoveListener(ListenerEventType eventName, Action handler)
    {
        string key = eventName.ToString();
        if (_listners != null)
        {
            if (_listners.ContainsKey(key))
            {
                LinkedList<Action> handlers = _listners[key];
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    _listners.Remove(key);
                }
            }
        }
    }


   
    public virtual void DispatchEvent(ListenerEventType eventName)
    {
        string key = eventName.ToString();
        if (_listners != null && _listners.ContainsKey(key))
        {
            LinkedList<Action> list = _listners[key];
            for (LinkedListNode<Action> handler = list.First; handler != null; handler = handler.Next)
            {
                handler.Value();
            }
        }
    }



    
    public virtual void RemoveListeners(ListenerEventType eventName)
    {
        if (_listners != null && _listners.ContainsKey(eventName.ToString()))
        {
            _listners.Remove(eventName.ToString());
        }
    }

    public virtual void ClearAllListener()
    {
        if (_listners != null)
        {
            _listners.Clear();
        }
    }




    // Use this for initialization
    void Start () {



    }
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
        InputListenerPC();
#else
         switch(currentDevice)
        {
			case DeviceMode.Falcon:
                InputListenerDKBox();
                SlideListenerBox();
                break;
			case DeviceMode.Finch:
			case DeviceMode.FalconCV:
            case DeviceMode.FalconCV2:
                InputListenerController();
                SlideListenerController();
                InputListenerAndroid();  
                break;
        }
#endif



    }



  
    public void InputListenerAndroid()
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton0))  
        {
            Debug.Log("Confirm button");
        }


        if (Input.GetKeyUp(KeyCode.JoystickButton1)) 
        {
            Debug.Log("Menu Button");
        }
    }


  
    public void OnHomeKeyEvent(string str)
    {
        Debug.Log("str received： " + str); 

       if(str.Equals("recentapps"))  
        {
            DispatchEvent(ListenerEventType.LONGPRESS_HOME);  
        }
       else if(str.Equals("homekey"))  
        {

        }

    }


   
    public void InputListenerDKBox()
    {
        if(Input.GetButtonUp("Cancel"))
        {
            DispatchEvent(ListenerEventType.CANCEL);
        }

        if(Input.GetButtonUp("Recenter"))
        {
            DispatchEvent(ListenerEventType.RECENTER);
        }

        if(Input.GetButtonUp("Submit"))
        {
            DispatchEvent(ListenerEventType.SUBMIT);
        }
    }





   
    public void InputListenerController()
    {
        //Annotation
        //      if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.APP))  
        //      {
        //          DispatchEvent(ListenerEventType.APP);
        //      }

        //      if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD))
        //      {
        //          DispatchEvent(ListenerEventType.TOUCHPAD);
        //      }

        //      if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyLongPressed(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.HOME))
        //      {
        //          DispatchEvent(ListenerEventType.LONGPRESS_HOME);
        //      }

        //if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER))
        //{
        //	DispatchEvent(ListenerEventType.TRIGGER);
        //          Debug.Log("PUI________________TRIGGER   Click ...........................");
        //}

        //      if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand_Second, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER))
        //      {
        //          DispatchEvent(ListenerEventType.TRIGGER_SECOND);
        //          Debug.Log("PUI________________TRIGGER_SECOND   Click ...........................");
        //      }

    }



 
    public void SlideListenerController()
    {
        //Annotation
        //if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeDown)
        //{
        //    DispatchEvent(ListenerEventType.SLIDE_DOWN);
        //    Debug.Log("-----InputController----------- Slide Down");
        //}
        //else if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeUp)
        //{
        //    DispatchEvent(ListenerEventType.SLIDE_UP);
        //    Debug.Log("-----InputController----------- Slide Up");
        //}
        //else if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeLeft)
        //{
        //    DispatchEvent(ListenerEventType.SLIDE_LEFT);
        //    Debug.Log("-----InputController----------- Slide Left");
        //}
        //else if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeRight)
        //{
        //    DispatchEvent(ListenerEventType.SLIDE_RIGHT);
        //    Debug.Log("-----InputController----------- Slide Right");
        //}

      
    }


    private float lastTime;


   
    public void SlideListenerBox()
    {
        if (Input.GetAxis("Vertical") > 0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_UP);
            }
        }
        else if (Input.GetAxis("Vertical") < -0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_DOWN);
            }
        }


        if (Input.GetAxis("Horizontal") > 0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_LEFT);
            }
        }
        else if (Input.GetAxis("Horizontal") < -0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_RIGHT);
            }
        }
    }


    
    private void InputListenerPC()
    {
        if(Input.GetKey(KeyCode.UpArrow))  
        {
            if (Time.time - lastTime > 1.0f)
            {
                DispatchEvent(ListenerEventType.SLIDE_UP);
                lastTime = Time.time;
            }
        }
        else if(Input.GetKey(KeyCode.DownArrow))   
        {
            DispatchEvent(ListenerEventType.SLIDE_DOWN);
            lastTime = Time.time;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))   
        {
            if (Time.time - lastTime > 1.0f)
            {
                DispatchEvent(ListenerEventType.SLIDE_LEFT);
                lastTime = Time.time;
            }
        }
        else if(Input.GetKey(KeyCode.RightArrow))  
        {
            if (Time.time - lastTime > 1.0f)
            {
                DispatchEvent(ListenerEventType.SLIDE_RIGHT);
                lastTime = Time.time;
            }
        }

        if(Input.GetKeyUp(KeyCode.KeypadEnter))   
        {
            DispatchEvent(ListenerEventType.SUBMIT); /*DispatchEvent(ListenerEventType.TOUCHPAD);*/
        }

        if(Input.GetKeyUp(KeyCode.Backspace))   
        {
            DispatchEvent(ListenerEventType.CANCEL); /*DispatchEvent(ListenerEventType.APP);*/
        }

        if(Input.GetKeyUp(KeyCode.Space))  
        {
            DispatchEvent(ListenerEventType.RECENTER);/* DispatchEvent(ListenerEventType.LONGPRESS_HOME);*/
        }
    }


    public bool TriggerDown
    {
        get
        {

#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            //Annotation
            return false;
            //Input.GetKeyDown(KeyCode.JoystickButton0) ||
            //    Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) ||
            //      Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) ||
            //    Input.GetButtonDown("Submit");
#endif
        }
    }



    public bool Triggering
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
//Annotation
            return false; 
            //Input.GetKey(KeyCode.Joystick1Button0) ||
            //    Pvr_UnitySDKAPI.Controller.UPvr_GetKey(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) ||
            //    Pvr_UnitySDKAPI.Controller.UPvr_GetKey(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) ||
            //    Input.GetButton("Submit");
#endif

        }
    }
}



public enum ListenerEventType
{
   
    APP,

   
    TOUCHPAD,

   
    LONGPRESS_HOME,

	TRIGGER,

   
    CANCEL,

  
    RECENTER,

   
    SUBMIT,

  
    SLIDE_UP,

  
    SLIDE_DOWN,

 
    SLIDE_LEFT,


    SLIDE_RIGHT,

 
    TRIGGER_SECOND
}