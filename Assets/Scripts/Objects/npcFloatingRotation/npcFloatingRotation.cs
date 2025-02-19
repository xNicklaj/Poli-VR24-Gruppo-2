using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcFloatingRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float amplitude = 0.5f;  // Altezza della fluttuazione
    public float frequency = 1f;    // Velocità della fluttuazione

    private Vector3 startPosition;
    private Transform initialTransform;

    public bool doFloat;
    public bool doRotation;

    public float floatingIntensity = 1;
    public float floatingSpeed = 1;
    public float rotationSpeed = 1;

    void Start()
    {
        initialTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (doRotation)
        {
            transform.Rotate(0,rotationSpeed*Time.deltaTime,0);
        }
        if(doFloat)
        {
            transform.position = initialTransform.position + Vector3.up * floatingIntensity*Mathf.Sin(Time.time*frequency);
            print(Mathf.Sin(Time.time*frequency));
        }
    }
}
