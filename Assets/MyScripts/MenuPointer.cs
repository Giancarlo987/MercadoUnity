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

    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;

    private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hitPoint = UpdateLine();
        m_CurrentObject = UpdatePointerStatus();

        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_CurrentObject);
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

    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
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
