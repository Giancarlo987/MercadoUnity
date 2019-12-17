using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributions : MonoBehaviour
{

    private List<GameObject> products1 = new List<GameObject>();
 
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] myListProducts1 = GameObject.FindGameObjectsWithTag("Product1");

        products1 = new List<GameObject>();

        for (int i = 0; i < myListProducts1.Length; i++)
        {
            if (myListProducts1[i] != transform)
                products1.Add(myListProducts1[i]);
        }

        for (int i = 0; i < products1.Count; i++)
        {
         
            Debug.Log(products1[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
