using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuEvents : MonoBehaviour
{
    #region Events
    public static UnityAction OnTouchpadTouchDown = null;
    public static UnityAction OnTouchpadTouchUp = null;

    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;
    #endregion

    #region Anchors
    public GameObject m_LeftAnchor;
    public GameObject m_RightAnchor;
    public GameObject m_HeadAnchor;
    #endregion

    #region Input
    private Dictionary<OVRInput.Controller, GameObject> m_ControllerSets = null;
    private OVRInput.Controller m_InputSource = OVRInput.Controller.None;
    private OVRInput.Controller m_Controller = OVRInput.Controller.None;
    private bool m_InputActive = true;
    #endregion

    private void Awake()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        m_ControllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for avtive input
        if (!m_InputActive)
            return;

        // Check if a controller exists
        CheckForController();

        // Check for input source 
        CheckInputSource();

        // Check for actual input
        Input();
    }

    private void CheckForController()
    {
        OVRInput.Controller controllerCheck = m_Controller;

        // Right remotre 
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
        {
            controllerCheck = OVRInput.Controller.RTrackedRemote;
        }

        // Left remote 
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            controllerCheck = OVRInput.Controller.LTrackedRemote;
        }

        // Headset
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote)
            && !OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
        {
            controllerCheck = OVRInput.Controller.Touchpad;
        }

        // Update
        m_Controller = UpdateSource(controllerCheck, m_Controller);
    }

    private void CheckInputSource()
    {
        //Update
        m_InputSource = UpdateSource(OVRInput.GetActiveController(), m_InputSource);
    }

    private void Input()
    {
        // Touchpad touch pressed
        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))
        {
            if (OnTouchpadTouchDown != null)
                OnTouchpadTouchDown();
        }

        // Touchpad touch unpressed
        if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
        {
            if (OnTouchpadTouchUp != null)
                OnTouchpadTouchUp();
        }
    }

    private OVRInput.Controller UpdateSource(OVRInput.Controller check, OVRInput.Controller previous)
    {
        // if the same values, return 
        if (check == previous)
            return previous;

        // get controller object
        GameObject controllerObject = null;
        m_ControllerSets.TryGetValue(check, out controllerObject);

        // if no controllers, set the head
        if (controllerObject == null)
            controllerObject = m_HeadAnchor;

        // send out event
        if (OnControllerSource != null)
            OnControllerSource(check, controllerObject);

        // return new value
        return check;
    }

    private void PlayerFound()
    {
        m_InputActive = true;
    }

    private void PlayerLost()
    {
        m_InputActive = false;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets()
    {
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>() {
            {OVRInput.Controller.LTrackedRemote, m_LeftAnchor},
            {OVRInput.Controller.RTrackedRemote, m_RightAnchor},
            {OVRInput.Controller.Touch, m_HeadAnchor}
        };

        return newSets;
    }
}

