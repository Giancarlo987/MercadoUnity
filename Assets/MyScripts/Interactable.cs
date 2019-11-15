using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Interactable : MonoBehaviour
{
    private Material newMaterial;
    private Material oldMaterial;
    private AudioSource audioSource;
    public VideoPlayer video;

    // Start is called before the first frame update
    void Start()
    {
        oldMaterial = transform.gameObject.GetComponent<Renderer>().material;
        newMaterial = Resources.Load<Material>("MyMaterials/CubeMaterial4");
        audioSource = transform.GetComponent<AudioSource>();
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
        oldMaterial = transform.gameObject.GetComponent<Renderer>().material;
    }

    public void PlaySound() {
        audioSource.Play();
    }

    public void StopSound() {
        audioSource.Stop();
    }

    public void PlayVideo() {
        //transform.GetComponentInChildren<VideoPlayer>().Play();
        video.Play();
    }

    public void StopVideo(){
        //transform.GetComponentInChildren<VideoPlayer>().Stop();
        video.Stop();
    }
}
