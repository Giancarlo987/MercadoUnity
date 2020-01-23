using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TestMenuPointer : MonoBehaviour
{
    private float m_Distance = 20.0f;
    private LineRenderer m_LineRenderer = null;

    public LayerMask m_EverythingMask = 0;

    public Canvas m_SubtitleCanvas = null;
    public TextMeshProUGUI m_Subtitle = null;
    
    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;

    private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;

    private bool audioPlay = false;

    // Start 
    private void Start()
    {
        m_LineRenderer = transform.GetComponent<LineRenderer>();
        m_SubtitleCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hitPoint = UpdateLine();
        m_CurrentObject = UpdatePointerStatus();

        ChekObjetPointed();

        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_CurrentObject);
    }

    private void ChekObjetPointed()
    {
        if (!m_CurrentObject)
            return;
        if (!m_LineRenderer.enabled)
            return;

        if (m_CurrentObject.tag.Equals("Salesman"))
        {
            m_SubtitleCanvas.enabled = true;

            if (!audioPlay)
                m_Subtitle.text = "Press the trigger to PLAY the audio";
            else
                m_Subtitle.text = "Press the trigger to STOP the audio";
        }
        else
        {
            m_SubtitleCanvas.enabled = false;
        }
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
        TestMarketEvents.OnControllerSource += UpdateOrigin;
        TestMarketEvents.OnTriggerDown += ProcessTriggerDown;
        TestMarketEvents.OnBackDown += BackButtonDown;
        TestMarketEvents.OnTouchpadDown += ProcessTouchpadDown;

        TestMarketEvents.OnTouchpadTouchDown += ChangeLinerendererPressedColor;
        TestMarketEvents.OnTouchpadTouchUp += ChangeLinerendererUnpressedColor;
    }

    private void OnDestroy()
    {
        TestMarketEvents.OnControllerSource -= UpdateOrigin;
        TestMarketEvents.OnTriggerDown -= ProcessTriggerDown;
        TestMarketEvents.OnBackDown -= BackButtonDown;
        TestMarketEvents.OnTouchpadDown -= ProcessTouchpadDown;

        TestMarketEvents.OnTouchpadTouchDown -= ChangeLinerendererPressedColor;
        TestMarketEvents.OnTouchpadTouchUp -= ChangeLinerendererUnpressedColor;
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
        RaycastHit hit = CreateRaycast(m_EverythingMask);

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

        Color startColor = Color.white;
        Color endColor = Color.white;
        endColor.a = 0.0f;

        m_LineRenderer.endColor = endColor;
        m_LineRenderer.startColor = startColor;
    }

    private void ProcessTriggerDown()
    {
        if (!m_CurrentObject)
            return;

        if (m_LineRenderer.enabled)
        {
            Salesman salesman = m_CurrentObject.GetComponent<Salesman>();

            // Play salesman's audio
            if (!salesman.isAudioPlaying)
                salesman.PlaySound();
            else
                salesman.StopSound();
        }
    }

    private void ProcessTouchpadDown()
    {
        if (!m_LineRenderer)
            return;

        m_LineRenderer.enabled = !m_LineRenderer.enabled;
        m_SubtitleCanvas.enabled = !m_SubtitleCanvas.enabled;
    }

    public void BackButtonDown()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene(0);
            StaticDistribution.dist1 = false;
            StaticDistribution.dist2 = false;
        }

        SceneManager.LoadScene(0);

    }

    private void ChangeLinerendererPressedColor()
    {
        if (!m_LineRenderer)
            return;

        Color startColor = Color.blue;
        m_LineRenderer.startColor = startColor;
    }

    private void ChangeLinerendererUnpressedColor()
    {
        if (!m_LineRenderer)
            return;

        Color startColor = Color.white;
        m_LineRenderer.startColor = startColor;
    }
}
