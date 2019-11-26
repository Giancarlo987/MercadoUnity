using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    private List<Transform> nodes;

    public WheelCollider wheelFR, wheelFL, wheelRR, wheelRL ;

    private int currentNode = 0;
    private float maxSteerAngle, maxMotorTorque, maxBrakeTorque, currentSpeed, maxSpeed;


    private float sensorLenght;
    private float sideSensorPosition, frontSensorAngle;
    private Vector3 frontSensorPosition;

    private bool braking = false;
    private bool avoiding = false;

    public AudioSource claxonSound;

    // Start is called before the first frame update
    private void Start()
    {
        Transform[] myPathNodes = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < myPathNodes.Length; i++)
        {
            if (myPathNodes[i] != path.transform)
                nodes.Add(myPathNodes[i]);
        }

        maxSteerAngle = 55f;
        maxSpeed = 220.0f;
        maxMotorTorque = 200.0f;
        maxBrakeTorque = 180.0f;
        sensorLenght = 5.0f;
        frontSensorPosition = new Vector3(0.0f, 0.4f, 1.5f);
        sideSensorPosition = 0.9f;
        frontSensorAngle = 20.0f;

    }


    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckPointDistance();
        Braking();
        Sensors();
    }

    // Wheels orientation 
    private void ApplySteer() 
    {
        if (avoiding) return;
        Vector3 relativePosition = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativePosition.x / relativePosition.magnitude) * maxSteerAngle;

        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    // Car Movement into path
    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        if (currentSpeed < maxSpeed && !braking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }

    }

    // Update node
    private void CheckPointDistance() 
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 2f) { 
            if (currentNode == nodes.Count - 1) 
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    // Braking
    private void Braking() 
    {
        if (braking)
        {
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
            ClaxonOn();
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0 ;
            ClaxonOff();
        }
    }

    // Front sensors
    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        braking = false;
        avoiding = false;

        float avoidMultiplier = 0;

        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        // Right sensor
        sensorStartPos += transform.right * sideSensorPosition;
        if (Physics.Raycast(sensorStartPos, this.transform.forward, out hit, sensorLenght))
        {
            if (hit.collider.CompareTag("Person"))
            {
                braking = true;
            }
            if (hit.collider.CompareTag("Vehicle"))
            {
                avoiding = true;
                avoidMultiplier -= 1f;
            }
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // Right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * this.transform.forward, out hit, sensorLenght))
        {
            if (hit.collider.CompareTag("Person"))
            {
                braking = true;
            }
            if (hit.collider.CompareTag("Vehicle"))
            {
                avoiding = true;
                avoidMultiplier -= .7f;
            }
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // Left sensor
        sensorStartPos -= transform.right * sideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, this.transform.forward, out hit, sensorLenght))
        {
            if (hit.collider.CompareTag("Person"))
            {
                braking = true;
            }
            if (hit.collider.CompareTag("Vehicle"))
            {
                avoiding = true;
                avoidMultiplier += 1;
            }
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // Left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * this.transform.forward, out hit, sensorLenght))
        {
            if (hit.collider.CompareTag("Person"))
            {
                braking = true;
            }
            if (hit.collider.CompareTag("Vehicle"))
            {
                avoiding = true;
                avoidMultiplier += .7f;
            }
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // Front Sensor
        sensorStartPos += transform.right * sideSensorPosition;
        if (Physics.Raycast(sensorStartPos, this.transform.forward, out hit, sensorLenght))
        {
            if (hit.collider.CompareTag("Person"))
            {
                braking = true;
            }
            if (hit.collider.CompareTag("Vehicle"))
            {
                if (avoidMultiplier < 0.1 && avoidMultiplier > -0.1)
                {
                    avoiding = true;
                    if (hit.normal.x > 0)
                    {
                        avoidMultiplier += 1;
                    } else
                    {
                        avoidMultiplier -= 1;
                    }
                }   
            }
            Debug.DrawLine(sensorStartPos, hit.point);
        }


        if (avoiding)
        {
            wheelFL.steerAngle = maxSteerAngle * avoidMultiplier;
            wheelFR.steerAngle = maxSteerAngle * avoidMultiplier;
        }
    }

    private void ClaxonOn()
    {
        claxonSound.Play();
    }

    private void ClaxonOff() 
    {
        claxonSound.Stop();
    }
}
