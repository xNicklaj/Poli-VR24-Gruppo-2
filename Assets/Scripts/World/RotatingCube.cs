using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 45.0f;
    // Floating speed in units per second
    public float floatSpeed = 0.8f; 
    // Floating amplitude (max height change)
    public float floatAmplitude = 0.2f;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        if(GetComponent<ActionSetFlag>() != null)
        {
            ActionSetFlag asf = GetComponent<ActionSetFlag>();
            bool state = GameManager.Instance.eventFlags.GetFlag(asf.flag);
            if (state)
            {
                this.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                this.GetComponent<Renderer>().material.color = Color.red;
            }
            EventManager.Instance.flagHasBeenSet.AddListener(IsFlagSet);
        }
        
    }

    private void IsFlagSet(EventFlag flag, bool value)
    {
        ActionSetFlag asf = GetComponent<ActionSetFlag>();
        if (asf.flag == flag && asf.value == value)
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ActionSetFlag>() != null)
        {
            ActionSetFlag asf = GetComponent<ActionSetFlag>();
            bool state = GameManager.Instance.eventFlags.GetFlag(asf.flag);
            if (state)
            {
                this.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                this.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        // Rotate the cube around its local Y axis at 1 degree per second
        transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
        // Calculate the new vertical position
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude; 
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}