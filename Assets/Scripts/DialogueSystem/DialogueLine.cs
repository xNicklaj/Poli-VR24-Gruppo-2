using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "ScriptableObjects/DialogueLine", order = 1)]
public class DialogueLine : DialogueComponent
{
    [TextArea][SerializeField] private String textString;
    [SerializeField] private Vector3 Position = new Vector3(0,0,0);
    [SerializeField] private List<DialogueAnswer> answers;
    [SerializeField] private Color lightColor;
    public bool positionRelativeToPlayer;
    public string getLine(){
        return textString;
    }
    public bool hasAnswers(){
        if (answers.Count==0){
            return false;
        }
        else return true;
    }
    public List<DialogueAnswer> getAnswers(){
        return answers;
    }
    public Vector3 getPosition(){
        return Position;
    }
    public Color getLightColor(){
        return lightColor;
    }
}
