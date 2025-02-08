using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "ScriptableObjects/DialogueLine", order = 1)]
public class DialogueLine : DialogueComponent
{
    [TextArea][SerializeField] private String textString;
    [SerializeField] private int textSize = 8;
    
    [SerializeField] private List<DialogueAnswer> answers;
    
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
    public int getSize(){
        return textSize;
    }
    
}
