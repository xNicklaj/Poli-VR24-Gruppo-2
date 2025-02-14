using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsSync : MonoBehaviour
{
    public GameObject VSyncToggle;
    public GameObject LimiterToggle;
    public GameObject TargetDropdown;

    public EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        VSyncToggle.GetComponent<Toggle>().isOn = SettingsManager.Instance.GetSetting(Settings.VSync) == SettingsMode.SettingsMode1;
        LimiterToggle.GetComponent<Toggle>().isOn = SettingsManager.Instance.GetSetting(Settings.DoFpsLimiter) == SettingsMode.SettingsMode1;
        switch(SettingsManager.Instance.GetSetting(Settings.FpsLimit)){
            case SettingsMode.SettingsMode0:
                TargetDropdown.GetComponent<TMP_Dropdown>().value = 0;
                break;
            case SettingsMode.SettingsMode1:
                TargetDropdown.GetComponent<TMP_Dropdown>().value = 1;
                break;
            case SettingsMode.SettingsMode2:
                TargetDropdown.GetComponent<TMP_Dropdown>().value = 2;
                break;
            case SettingsMode.SettingsMode3:
                TargetDropdown.GetComponent<TMP_Dropdown>().value = 3;
                break;
            case SettingsMode.SettingsMode4:
                TargetDropdown.GetComponent<TMP_Dropdown>().value = 4;
                break;
        }

    }

    void OnEnable()
    {
        eventSystem.firstSelectedGameObject = VSyncToggle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
