using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "ScriptableObjects/DialogueLine", order = 1)]
public class DialogueLine : DialogueComponent
{
    [TextArea][SerializeField] private String textString;
    [SerializeField] private Vector3 RelativePosition = new Vector3(0,0,0);
    [SerializeField] private List<DialogueAnswer> answers;
    public bool positionRelativeToPlayer;
    public string get_line(){
        return textString;
    }
    public bool has_answers(){
        if (answers.Count==0){
            return false;
        }
        else return true;
    }
    public List<DialogueAnswer> get_answers(){
        return answers;
    }
    public Vector3 get_relative_position(){
        return RelativePosition;
    }
}
