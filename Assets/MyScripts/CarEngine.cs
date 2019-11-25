using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    private List<Transform> nodes;

    public WheelCollider wheelFR, wheelFL, wheelRR, wheelRL ;

    private int currentNode = 0;
    public float maxSteerAngle, maxMotorTorque, maxBrakeTorque, currentSpeed, maxSpeed;


    public float sensorLenght;
    public float sideSensorPosition, frontSensorAngle;
    private Vector3 frontSensorPosition;

    public bool braking = false;

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

        maxSteerAngle = 50f;
        maxSpeed = 220.0f;
        maxMotorTorque = 0.0f;
        maxBrakeTorque = 180.0f;
        sensorLenght = 10.0f;
        frontSensorPosition = new Vector3(0.0f, 0.2f, 2.0f);
        //frontSensorPosition = 0.5f;
        sideSensorPosition = 0.2f;
        frontSensorAngle = 30.0f;

    }


    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckPointDistance();
        Braking();
        Sensors();
    }

    private void ApplySteer() 
    {
        Vector3 relativePosition = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativePosition.x / relativePosition.magnitude) * maxSteerAngle;

        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

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

    private void Braking() 
    {
        if (braking)
        {
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0 ;
        }
    }

    private void Sensors()
    {
        RaycastHit hit;
        //Vector3 sensorStartPos = transform.position;

        Vector3 sensorStartPos = transform.position + frontSensorPosition;


        //sensorStartPos.z += frontSensorPosition;
        if (Physics.Raycast(sensorStartPos, this.transform.forward, out hit, sensorLenght))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);

        // Right sensor
        sensorStartPos.x += sideSensorPosition;
        if (Physics.Raycast(sensorStartPos, this.transform.forward, out hit, sensorLenght))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);

        // Right angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * this.transform.forward, out hit, sensorLenght))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);

        // Left sensor
        sensorStartPos.x -= sideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, this.transform.forward, out hit, sensorLenght))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);

        // Left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * this.transform.forward, out hit, sensorLenght))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);
    }
}
