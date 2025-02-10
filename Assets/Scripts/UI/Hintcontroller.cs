using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Hintcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kmHint;
    public GameObject gpHint;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentDevice == GameManager.DeviceType.Keyboard)
        {
            kmHint.SetActive(true);
            gpHint.SetActive(false);
        }
        else
        {
            kmHint.SetActive(false);
            gpHint.SetActive(true);
        }
    }
}
