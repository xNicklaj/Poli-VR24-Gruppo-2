using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Answer", menuName = "ScriptableObjects/DialogueAnswer", order = 1)]
public class DialogueAnswer : DialogueComponent
{
    
    [TextArea][SerializeField] private String answer_text_string;
    public string get_answer_text(){
        return answer_text_string;
    }
}