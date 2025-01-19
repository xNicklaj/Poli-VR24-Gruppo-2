using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;

public class VoidScene : MonoBehaviour
{
    [SerializeField] private GameObject nameInputSystem;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject dome;

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
            sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 1")));
            
        }
    }
    public void showNameInput(){
        Debug.Log("anche senza dai");
        nameInputSystem.SetActive(true);
        nameInputSystem.GetComponent<NameInputSystem>().Appear();
    }
    public void changeColor(){
        Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(Color.white,5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(Color.white,5f));
    }
}
