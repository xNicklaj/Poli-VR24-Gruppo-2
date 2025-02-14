using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Settings
{
    VSync,
    DoFpsLimiter,
    FpsLimit
}

public enum SettingsMode
{
    SettingsMode0,
    SettingsMode1,
    SettingsMode2,
    SettingsMode3,
    SettingsMode4,
    SettingsMode5
}

public class SettingsManager : Singleton<SettingsManager>
{
    private static string SETTINGS_PATH = "";
    private Dictionary<Settings, SettingsMode> settings;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SETTINGS_PATH = Application.persistentDataPath + "/settings.json";
        EventManager.Instance.settingsChanged.AddListener(HandleSettingsChanged);
        settings = new Dictionary<Settings, SettingsMode>();
        settings.Add(Settings.VSync, SettingsMode.SettingsMode0);
        settings.Add(Settings.DoFpsLimiter, SettingsMode.SettingsMode0);
        settings.Add(Settings.FpsLimit, SettingsMode.SettingsMode1);
        if (!File.Exists(SETTINGS_PATH)) SaveSettings();
    }

    public void SaveSettings()
    {
        Debug.Log("Saving settings...");
        // Save the JSON converted EventFlags object into the file
        File.WriteAllText(SETTINGS_PATH, JsonConvert.SerializeObject(settings));
        return;
    }

    public Dictionary<Settings, SettingsMode> LoadSettings()
    {
        Dictionary<Settings, SettingsMode> data = null;
        Debug.Log("Loading settings data...");
        if(File.Exists(SETTINGS_PATH)) Debug.Log(File.ReadAllText(SETTINGS_PATH));
        // Convert JSON to EventFlags object
        if (File.Exists(SETTINGS_PATH))
        {
            data = JsonConvert.DeserializeObject<Dictionary<Settings, SettingsMode>>(File.ReadAllText(SETTINGS_PATH));
            EventManager.Instance.settingsChanged.Invoke();
        }
        if (data == null) 
            Debug.LogWarning("Settings file not found in " + SETTINGS_PATH);
        return data;
    }

    private void HandleSettingsChanged()
    {
        if (settings.TryGetValue(Settings.VSync, out SettingsMode value))
        {
            // Handle VSync toggle
            QualitySettings.vSyncCount = value == SettingsMode.SettingsMode0 ? 0 : 1;
        }
        if (settings.TryGetValue(Settings.DoFpsLimiter, out SettingsMode value2))
        {
            // Handle FPS limiter toggle
            if (value2 == SettingsMode.SettingsMode1 && settings.TryGetValue(Settings.FpsLimit, out SettingsMode value3))
            {
                // Handle FPS limit
                switch (value3)
                {
                    case SettingsMode.SettingsMode0:
                        Application.targetFrameRate = 30;
                        break;
                    case SettingsMode.SettingsMode1:
                        Application.targetFrameRate = 60;
                        break;
                    case SettingsMode.SettingsMode2:
                        Application.targetFrameRate = 90;
                        break;
                    case SettingsMode.SettingsMode3:
                        Application.targetFrameRate = 120;
                        break;
                    case SettingsMode.SettingsMode4:
                        Application.targetFrameRate = 144;
                        break;
                }
            }
            else if (value2 == SettingsMode.SettingsMode0)
            {
                // Unlimited framerate
                Application.targetFrameRate = -1;
            }
        }
    }
}
