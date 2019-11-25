using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPath : MonoBehaviour
{
    private Color lineColor = Color.white;
    private List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] myPathNodes = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < myPathNodes.Length; i++) 
        {
            if (myPathNodes[i] != transform)
                nodes.Add(myPathNodes[i]);
        }

        for (int i = 0; i < nodes.Count; i++) 
        {
            Vector3 currentNode, previousNode;
            previousNode = Vector3.zero;
            currentNode = nodes[i].position;
            if (i > 0) {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previousNode = nodes[nodes.Count - 1].position;
            }
            Gizmos.DrawLine(previousNode, currentNode);
        }
    }

}
