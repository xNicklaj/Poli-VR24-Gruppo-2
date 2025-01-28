using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seed : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject seedModel;
    public ParticleSystem particles;
    void Start()
    {
        EventManager.Instance.flagHasBeenSet.AddListener(destroySeed);
    }

    // Update is called once per frame
    void Update()
    {
        seedModel.transform.position += Vector3.up * 0.0001f*(float)Math.Sin(Time.time);
        seedModel.transform.Rotate(0,150*Time.deltaTime,0);
    }

    private void destroySeed(EventFlag e, bool status){
        if ((e == EventFlag.HasSeed)&&(status == true)){
            Destroy(gameObject);
        }
    }
}

