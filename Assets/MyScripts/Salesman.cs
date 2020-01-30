using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salesman : MonoBehaviour
{
    private AudioSource audioSource;

    public bool isAudioPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            isAudioPlaying = false;
        }
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
