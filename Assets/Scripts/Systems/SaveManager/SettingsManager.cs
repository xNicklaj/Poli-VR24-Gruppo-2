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

    void Awake()
    {
        DontDestroyOnLoad(this);
        SETTINGS_PATH = Application.persistentDataPath + "/settings.json";
        settings = new Dictionary<Settings, SettingsMode>();
        settings.Add(Settings.VSync, SettingsMode.SettingsMode0);
        settings.Add(Settings.DoFpsLimiter, SettingsMode.SettingsMode0);
        settings.Add(Settings.FpsLimit, SettingsMode.SettingsMode1);
        HandleSettingsChanged();
    }

    void Start()
    {
        EventManager.Instance.settingsChanged.AddListener(HandleSettingsChanged);
        if (!File.Exists(SETTINGS_PATH)) 
            SaveSettings();
        else
        {
            settings = LoadSettings();
            EventManager.Instance.settingsChanged.Invoke();
        }
            
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
            data = JsonConvert.DeserializeObject<Dictionary<Settings, SettingsMode>>(File.ReadAllText(SETTINGS_PATH));
        if (data == null) 
            Debug.LogWarning("Settings file not found in " + SETTINGS_PATH);
        return data;
    }

    private void HandleSettingsChanged()
    {
        Debug.Log("Reloading settings...");
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
        if (settings.TryGetValue(Settings.VSync, out SettingsMode value))
        {
            // Handle VSync toggle
            QualitySettings.vSyncCount = (value == SettingsMode.SettingsMode0 || value2 == SettingsMode.SettingsMode1) ? 0 : 1;
        }
    }

    public void SetSetting(Settings setting, SettingsMode mode)
    {
        settings[setting] = mode;
        SaveSettings();
        EventManager.Instance.settingsChanged.Invoke();
    }
    public SettingsMode GetSetting(Settings setting)
    {
        return settings[setting];
    }

    public void EnableVSync()
    {
        SetSetting(Settings.VSync, SettingsMode.SettingsMode1);
    }

    public void DisableVSync()
    {
        SetSetting(Settings.VSync, SettingsMode.SettingsMode0);
    }

    public void EnableFpsLimiter()
    {
        SetSetting(Settings.DoFpsLimiter, SettingsMode.SettingsMode1);
    }

    public void DisableFpsLimiter()
    {
        SetSetting(Settings.DoFpsLimiter, SettingsMode.SettingsMode0);
    }

    public void SetFpsLimit30()
    {
        SetSetting(Settings.FpsLimit, SettingsMode.SettingsMode0);
    }

    public void SetFpsLimit60()
    {
        SetSetting(Settings.FpsLimit, SettingsMode.SettingsMode1);
    }

    public void SetFpsLimit90()
    {
        SetSetting(Settings.FpsLimit, SettingsMode.SettingsMode2);
    }

    public void SetFpsLimit120()
    {
        SetSetting(Settings.FpsLimit, SettingsMode.SettingsMode3);
    }

    public void SetFpsLimit144()
    {
        SetSetting(Settings.FpsLimit, SettingsMode.SettingsMode4);
    }

}
