using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seed : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject seedModel;

    public void Deselect()
    {
        throw new NotImplementedException();
    }

    public void Interact()
    {
        throw new NotImplementedException();
    }

    public void Select()
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
