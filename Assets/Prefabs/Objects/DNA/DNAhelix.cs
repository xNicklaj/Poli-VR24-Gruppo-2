using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAHelix : MonoBehaviour
{
    [SerializeField]private float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,speed*Time.deltaTime,0);
    }
}
