using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcFloatingRotation : MonoBehaviour
{
    // Start is called before the first frame update

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
            this.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            this.transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);
        }
        if(doFloat)
        {
            this.transform.position = new Vector3(initialTransform.position.x, initialTransform.position.y + Mathf.Sin(Time.time * floatingSpeed) * floatingIntensity, initialTransform.position.z);
        }
    }
}
