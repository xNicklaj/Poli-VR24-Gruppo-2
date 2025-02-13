using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// Put here all the flags you need for the story
public enum EventFlag
{
    TestVar_HasCrossedPlane,
    TestVar_HasInteracted,
    HasLighter,
    HasLantern,
    HasSeed,
    HasCrayon,
    HasWateringCan,
    MuseumEntered,
    MuseumExited,
    IntroDialogueEnded,
    GodDialogueEnded,
    ManDialogueEnded,
    TreeDialogueEnded,
    NakedDialogueEnded,
    PowerDialogueEnded,
    ReturnDialogueEnded,
    SeedDialogueEnded,
    SelfDialogueEnded,
    EndDialogueEnded


}

public class EventFlags
{
    [JsonProperty] private List<bool> events = new List<bool>();
    [JsonProperty] public string playerName;

    public EventFlags()
    {

    }

    public EventFlags(EventFlags eventFlags)
    {
        events.Clear();
        for (int i = 0; i < eventFlags.events.Count; i++)
        {
            events.Add(eventFlags.events[i]);
        }
        this.playerName = eventFlags.playerName;
    }

    public void InitializeFlags()
    {
        events.Clear();
        for (int i = 0; i < System.Enum.GetValues(typeof(EventFlag)).Length; i++)
        {
            events.Add(false);
        }
    }

    public bool GetFlag(EventFlag flag)
    {
        return events[(int)flag];
    }

    public void SetFlag(EventFlag flag, bool value)
    {
        events[(int)flag] = value;
        Debug.Log("Flag " + flag + " set to " + value);
        EventManager.Instance.flagHasBeenSet.Invoke(flag, value);
    }

    public void PrintFlags()
    {
        for (int i = 0; i < events.Count; i++)
        {
            Debug.Log(events[i]);
        }
    }
}
