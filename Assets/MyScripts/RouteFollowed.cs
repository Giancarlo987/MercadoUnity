using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFollowed : MonoBehaviour
{
    private GameObject car;
    private float speed;
    private float Timer;
    private int currentNode;

    private bool obstacle;


    private static Vector3 currentPositionHolder;
    private static Quaternion targetRotation;

    private Node [] NodePath;


    private void Awake()
    {
        ObstacleTrigger.OnObstacleEnter += reduceSpeed;
        ObstacleTrigger.OnObstacleExit += returnSpeed;
    }

    private void OnDestroy()
    {
        ObstacleTrigger.OnObstacleEnter -= reduceSpeed;
        ObstacleTrigger.OnObstacleExit -= returnSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        NodePath = GetComponentsInChildren<Node>();
        car = GameObject.Find("Car");
        speed = .7f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime * speed;
        Movement();
    }

    private void CheckNode() 
    {
        Timer = 0;
        currentPositionHolder = NodePath[currentNode].transform.position;
        targetRotation = Quaternion.LookRotation(currentPositionHolder - car.transform.position);
    }

    private void Movement() 
    {
        car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetRotation, Timer);
        if (car.transform.position != currentPositionHolder) 
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, currentPositionHolder, Timer);

        }
        else 
        {
            if (currentNode < NodePath.Length - 1) 
            {
                currentNode++;

            } 
            else 
            {
                currentNode = 0;
            }
            CheckNode();
        }

    }

    public void reduceSpeed () 
    {
        speed = .01f;
        Debug.Log(speed);
    }

    public void returnSpeed()
    {
        speed = .7f;
        Debug.Log(speed);
    }
}
