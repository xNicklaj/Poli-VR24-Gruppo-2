using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.Events;
using System;

public class SaveManager : Singleton<SaveManager>
{
    private static string SAVE_PATH = "";
    private static DateTime lastModified;

    private void Awake()
    {
        lastModified = DateTime.UnixEpoch;
        SAVE_PATH = Application.persistentDataPath + "/eventFlags.json";
        if(File.Exists(SAVE_PATH))
            lastModified = System.IO.File.GetLastWriteTime(SAVE_PATH);
    }

    public async static Task SaveEventFlags(EventFlags flags)
    {
        Debug.Log("Saving game...");
        // Save the JSON converted EventFlags object into the file
        await File.WriteAllTextAsync(SAVE_PATH, JsonConvert.SerializeObject(flags));
        lastModified = DateTime.Now;
        return;
    }

    public async static Task<EventFlags> LoadEventFlags()
    {
        EventFlags data = null;

        // Convert JSON to EventFlags object
        if (File.Exists(SAVE_PATH))
            data = JsonConvert.DeserializeObject<EventFlags>(await File.ReadAllTextAsync(SAVE_PATH));

        if (data == null) 
            Debug.LogWarning("Save file not found in " + SAVE_PATH);

        return new EventFlags();
    }

    public static DateTime GetLastModified()
    {
        return lastModified;
    }
}
