using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueComponent : ScriptableObject
{
    public DialogueLine next = null;
    protected DialogueManager dm;
    private bool selectable;

    [SerializeField] protected AudioClip audioToPlay;
    [SerializeField] protected Vector3 Position = new Vector3(0, 0, 0);
    [ColorUsage(true,true)] [SerializeField] protected Color lightColor;

    public void setDialogueManager(DialogueManager d)
    {
        dm = d;
    }
    public void setSelectable(bool value)
    {
        selectable = value;
    }
    public AudioClip GetAudio()
    {
        return audioToPlay;
    }
    public Vector3 getPosition()
    {
        return Position;
    }
    public Color getLightColor(){
        return lightColor;
    }
}
