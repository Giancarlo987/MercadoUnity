using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    public Pointer m_Pointer;
    public SpriteRenderer m_DotRenderer;

    public Sprite m_Sprite;

    public Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_Camera.gameObject.transform);
    }

    private void Awake()
    {
        m_Pointer.OnPointerUpdate += UpdateSprite;
    }

    private void OnDestroy()
    {
        m_Pointer.OnPointerUpdate -= UpdateSprite;
    }

    private void UpdateSprite(Vector3 point, GameObject hitObject)
    {
        transform.position = point;

        if (hitObject)
        {
            m_DotRenderer.sprite = m_Sprite;
        }
    }
}
