using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueComponent : ScriptableObject
{
    public DialogueLine next = null;
    protected DialogueManager dm;
    private bool selectable;

    [SerializeField] protected AudioSource audioToPlay;

    public void setDialogueManager(DialogueManager d){
        dm = d;
    }
    public void setSelectable(bool value){
        selectable = value;
    }
}
