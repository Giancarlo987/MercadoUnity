using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributions : MonoBehaviour
{
    private List<GameObject> prod1 = new List<GameObject>();
    private List<Vector3> prod1Dist1 = new List<Vector3>();
    private List<Vector3> prod1Dist2 = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] myListProducts1 = GameObject.FindGameObjectsWithTag("Product1");

        prod1 = new List<GameObject>();

        prod1Dist1 = new List<Vector3>();
        prod1Dist2 = new List<Vector3>();

        for (int i = 0; i < myListProducts1.Length; i++)
        {
            if (myListProducts1[i] != transform)
            {
                prod1.Add(myListProducts1[i]);
                prod1Dist1.Add(prod1[i].transform.position);
                prod1Dist2.Add(prod1[i].transform.position + new Vector3(-4, 0, 0));
            }
        }

        for (int i = 0; i < prod1.Count; i++)
        {
            Debug.Log(prod1Dist1[i]);
            Debug.Log(prod1[i].name);
            Debug.Log(prod1Dist2[i]);
        }

        // Sorting products 
        if (StaticDistribution.dist1)
        {
            DoDist1();
        }
        else if (StaticDistribution.dist2)
        {
            DoDist2();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoDist1()
    {
        for (int i = 0; i < prod1.Count; i++)
        {
            prod1[i].transform.position = prod1Dist1[i];
        }
    }

    public void DoDist2()
    {
        for (int i = 0; i < prod1.Count; i++)
        {
            prod1[i].transform.position = prod1Dist2[i];
        }
    }
}
