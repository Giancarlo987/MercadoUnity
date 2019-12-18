using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Interactable : MonoBehaviour
{
    private Material newMaterial;
    private Material oldMaterial;
    private AudioSource audioSource;

    public bool isAudioPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        oldMaterial = transform.gameObject.GetComponent<Renderer>().material;
        newMaterial = Resources.Load<Material>("MyMaterials/CubeMaterial2");
        audioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor()
    {
        transform.gameObject.GetComponent<Renderer>().material = newMaterial;
        newMaterial = oldMaterial;
        oldMaterial = transform.gameObject.GetComponent<Renderer>().material;
    }

    public void PlaySound()
    {
        audioSource.Play();
        isAudioPlaying = true;
    }

    public void StopSound()
    {
        audioSource.Stop();
        isAudioPlaying = false;
    }
}
