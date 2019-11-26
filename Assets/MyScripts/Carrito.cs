using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrito : MonoBehaviour
{
    public Transform carrito;
    private float x, z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = carrito.position.x / 2;
        z = carrito.position.z /2;
        transform.localPosition = new Vector3(x, z, 0);


    }
}
