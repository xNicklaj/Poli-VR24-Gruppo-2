using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

public class SaveManager : Singleton<SaveManager>
{
    private static string SAVE_PATH = Application.persistentDataPath + "/eventFlags.json";

    public async static Task SaveEventFlags(EventFlags flags)
    {
        Debug.Log("Saving game...");
        await File.WriteAllTextAsync(SAVE_PATH, JsonConvert.SerializeObject(flags));
        return;
    }

    public async static Task<EventFlags> LoadEventFlags()
    {
        EventFlags data = null;

        if(File.Exists(SAVE_PATH)) // Check if the file exists
            data = JsonConvert.DeserializeObject<EventFlags>(await File.ReadAllTextAsync(SAVE_PATH)); // Convert JSON to EventFlags object
        if (data == null) 
            Debug.LogWarning("Save file not found in " + SAVE_PATH);

        return new EventFlags();
    }
}
