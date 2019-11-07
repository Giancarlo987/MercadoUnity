using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private float speed = 10.0f;
    
    private Vector3 inital_Position, target_Position;

    // Start is called before the first frame update
    void Start()
    {
        inital_Position = transform.position;
        target_Position = new Vector3(inital_Position.x, inital_Position.y, inital_Position.z + 15);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object
        Movement();
    }

    public void Movement()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move

        transform.position = Vector3.MoveTowards(transform.position, target_Position, step);
        
        // Check if the position of the object and target are approximately equal.
        if (Vector3.Distance(transform.position, target_Position) < 0.001f)
        {
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z));

            //transform.position -= transform.forward * step;

            target_Position = inital_Position;
            inital_Position = transform.position;
        }
    }
}
