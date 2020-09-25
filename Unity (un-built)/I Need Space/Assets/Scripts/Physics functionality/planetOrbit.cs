using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetOrbit : MonoBehaviour
{
    public float planetSpeed = 5f;
    public GameObject anchor;

    private void FixedUpdate()
    {
        transform.RotateAround(anchor.transform.position, new Vector3(0f, 0f, 5f), planetSpeed * Time.deltaTime); //makes gameObject rotate around "anchor" object at speed specified 
    }
}
