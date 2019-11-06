using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuPointer : MonoBehaviour
{
    public float m_Distance = 500.0f;
    public LineRenderer m_LineRenderer = null;
    public LayerMask m_EverythingMask = 0;
    public LayerMask m_UIMask = 0;

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

    #region Events
    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;
    #endregion

    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;

    private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;

    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;

        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        m_ControllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;    

        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for avtive input
        if (!m_InputActive)
            return;

        // Check if a controller exists
        CheckForController();

        // Check for input source 
        CheckInputSource();

        // Check for actual input
        //Input();

        Vector3 hitPoint = UpdateLine();
        m_CurrentObject = UpdatePointerStatus();

        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_CurrentObject);
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

    private Vector3 UpdateLine()
    {
        // Create ray
        RaycastHit hit = CreateRaycast(m_EverythingMask);

        // Default end
        Vector3 endPosition = m_CurrentOrigin.position + (m_CurrentOrigin.forward * m_Distance);

        // Check hit
        if (hit.collider != null)
            endPosition = hit.point;

        // Set Position
        m_LineRenderer.SetPosition(0, m_CurrentOrigin.position);
        m_LineRenderer.SetPosition(1, endPosition);
        return endPosition;
    }
    
    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        // Set origin 
        m_CurrentOrigin = controllerObject.transform;

        // Laser visible
        if (controller == OVRInput.Controller.Touchpad)
        {
            m_LineRenderer.enabled = false;
        }
        else
        {
            m_LineRenderer.enabled = true;
        }
    }

    private GameObject UpdatePointerStatus()
    {
        // Create Raycaster
        RaycastHit hit = CreateRaycast(m_UIMask);
        // Check hit
        if (hit.collider)
            return hit.collider.transform.gameObject;
        // return
        return null;
    }

    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(m_CurrentOrigin.position, m_CurrentOrigin.forward);
        Physics.Raycast(ray, out hit, m_Distance, layer);
        return hit;
    }

    private void SetLineColor()
    {
        if (!m_LineRenderer)
            return;

        Color startColor = Color.blue;
        Color endColor = Color.white;
        endColor.a = 0.0f;

        m_LineRenderer.endColor = endColor;
        m_LineRenderer.startColor = startColor;
    }
}
