using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleTrigger : MonoBehaviour
{
    #region Events
    public static UnityAction OnObstacleEnter = null;
    public static UnityAction OnObstacleExit = null;
    #endregion


    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle") 
        {
            Debug.Log(other.name);
            if (OnObstacleEnter != null)
                OnObstacleEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Debug.Log(other.name);
            if (OnObstacleExit != null)
                OnObstacleExit();
        }
    }
}
