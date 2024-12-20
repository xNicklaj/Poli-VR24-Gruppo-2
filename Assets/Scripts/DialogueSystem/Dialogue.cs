using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [SerializeField] private DialogueLine firstComponent;

    public DialogueLine getFirstComponent(){
        return firstComponent;
    }
}
