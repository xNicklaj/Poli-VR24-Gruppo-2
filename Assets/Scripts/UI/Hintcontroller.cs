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

    public PlayerInputActions pc;
    private DeviceType currentType = DeviceType.Keyboard;

    enum DeviceType
    {
        Keyboard,
        Gamepad
    }

    void Awake()
    {
        pc = new PlayerInputActions();

        InputSystem.onEvent.Call((_) =>
        {
            var device = InputSystem.GetDeviceById(_.deviceId);
            if (device is Gamepad)
            {
                currentType = DeviceType.Gamepad;
            }
            if (device is Keyboard || device is Mouse)
            {
                currentType = DeviceType.Keyboard;
            }
        });
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentType == DeviceType.Keyboard)
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
