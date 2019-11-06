using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetunrPosition : MonoBehaviour
{

    private Vector3 initialPosition, driftPosition;
    private Quaternion initialRotation, driftRotation;
    private Rigidbody rb;

    private float count = 0;

    private float driftSeconds;
    private float driftTimer;
    private bool isDrifting;

    // Start is called before the first frame update
    void Start()
    {
        driftTimer = 0;
        driftSeconds = 3;
        isDrifting = false;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != initialPosition) 
        {
            if (!rb.isKinematic)
            {
                count += Time.deltaTime;
                if (count >= 5)
                {
                    StartDrift();
                }
            }
            else
            {
                count = 0;
            }
        }

        if (isDrifting)
        {
            driftTimer += Time.deltaTime;

            if (driftTimer > driftSeconds)
            {
                StopDrift();
            }
            else
            {
                float ratio = driftTimer / driftSeconds;
                transform.position = Vector3.Lerp(driftPosition, initialPosition, ratio);
                transform.rotation = Quaternion.Slerp(driftRotation, initialRotation, ratio);
            }
        }
        
    }

    private void StartDrift()
    {
        isDrifting = true;
        rb.isKinematic = true;

        driftTimer = 0;

        driftPosition = transform.position;
        driftRotation = transform.rotation;

        rb.velocity = Vector3.zero;
        count = 0;
    }

    private void StopDrift()
    {
        isDrifting = false;
        rb.isKinematic = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        rb.velocity = Vector3.zero;
        count = 0;
    }
}
