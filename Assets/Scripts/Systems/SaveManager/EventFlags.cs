using System.Collections;
using System.Collections.Generic;
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
    private List<bool> events = new List<bool>();

    public EventFlags()
    {
        
        for (int i = 0; i < System.Enum.GetValues(typeof(EventFlag)).Length; i++)
        {
            events.Add(false);
        }
    }

    public EventFlags(EventFlags eventFlags)
    {
        for (int i = 0; i < eventFlags.events.Count; i++)
        {
            events.Add(eventFlags.events[i]);
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
    }
}
