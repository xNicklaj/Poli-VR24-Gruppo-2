using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;

public class VoidScene : MonoBehaviour
{
    [SerializeField] private GameObject nameInputSystem;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.eventFlags.GetFlag(EventFlag.IntroDialogueEnded)){
            print("yeah");
        }
        else{
            print(DialogueManager.Instance);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(3f);
            sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Intro Dialogue/Intro Dialogue 1")));
            
        }
    }
    public void showNameInput(){
        nameInputSystem.SetActive(true);
        nameInputSystem.GetComponent<NameInputSystem>().Appear();
    }
}
