using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Material newMaterial;
    private Material oldMaterial;
    // Start is called before the first frame update
    void Start()
    {
        oldMaterial = transform.gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressed() {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        bool flip = !renderer.enabled;

        renderer.enabled = flip;
    }

    public void ChangeColor() {

        transform.gameObject.GetComponent<Renderer>().material = newMaterial;
        newMaterial = oldMaterial;
        
    }
}
