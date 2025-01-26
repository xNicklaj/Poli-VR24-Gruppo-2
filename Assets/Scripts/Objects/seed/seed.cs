using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seed : IInteractable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject seedModel;

    public override void Interact()
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        seedModel.transform.position += Vector3.up * 0.0001f*(float)Math.Sin(Time.time);
        seedModel.transform.Rotate(0,150*Time.deltaTime,0);
    }
}
