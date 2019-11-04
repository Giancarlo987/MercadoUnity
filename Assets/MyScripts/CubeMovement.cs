using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    // Spped of the object movement
    public float speed = 1.0f;

    // Target Object 
    private Transform target;

    private Vector3 initalPosition, endPosition, targetPosition;

    private void Awake()
    {
        // Position the cube at the origin.
        //transform.position = new Vector3(0.0f, 0.25f, 0.0f);

        // Create and position the cylinder. Reduce the size.
        //GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Grab cylinder values and place on the target.
        /*target = cylinder.transform;
        target.transform.localScale = new Vector3(0.15f, 1.0f, 0.15f);
        target.transform.position = new Vector3(1.0f, 0.25f, 1.0f);*/
    }

    // Start is called before the first frame update
    void Start()
    {
        initalPosition = transform.position;
        endPosition = initalPosition * -1.0f;

        targetPosition = initalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the cube and sphere are approximately equal.

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            // Swap the position of the cylinder.
            targetPosition = new Vector3(targetPosition.x * -1, targetPosition.y, targetPosition.z * -1);
        }

        /*
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            // Swap the position of the cylinder.
            target.position = new Vector3(target.position.x * -1 , target.position.y, target.position.z * -1);
        }*/
    }
}
