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
    [Header ("Color Parameters")]
    [SerializeField] [ColorUsage(false)]private Color totalBlackRoomColor;
    [SerializeField] [ColorUsage(false)]private Color blackRoomColor;
    [SerializeField] [ColorUsage(false)]private Color whiteRoomColor;
    [SerializeField] [ColorUsage(false)]private Color redRoomColor;
    [Header ("Audio Parameters")]
    [SerializeField] private GameObject gameSounds;
    [SerializeField] private AudioClip colorChangeSound;
    [SerializeField] private AudioClip godKilledSound;
    [SerializeField] private AudioClip godKilledSoundtrack;
    [SerializeField] private AudioClip fluteSound;




    // Start is called before the first frame update
    void Start()
    {
        floor.GetComponent<Renderer>().material.color = blackRoomColor;
        dome.GetComponent<Renderer>().material.color = blackRoomColor;

        if (GameManager.Instance.eventFlags.GetFlag(EventFlag.IntroDialogueEnded)){
            print("yeah");
        }
        else{
            print(DialogueManager.Instance);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 1")));
            
        }
    }
    public void showNameInput(){
        Debug.Log("anche senza dai");
        nameInputSystem.SetActive(true);
        nameInputSystem.GetComponent<NameInputSystem>().Appear();
    }
    public void changeColor(){
        gameSounds.GetComponent<AudioSource>().clip=colorChangeSound;
        gameSounds.GetComponent<AudioSource>().Play();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(whiteRoomColor,3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(whiteRoomColor,3.5f));
        sequence.Join(GetComponent<AudioSource>().DOFade(0,3.5f));
        sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 2")));
    }

    public void showGodDialogue(){
        GetComponent<AudioSource>().volume=0f;
        gameSounds.GetComponent<AudioSource>().clip=godKilledSound;
        gameSounds.GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().clip = godKilledSoundtrack;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(redRoomColor,3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(redRoomColor,3.5f));
        sequence.JoinCallback(()=>GetComponent<AudioSource>().Play());
        sequence.Join(GetComponent<AudioSource>().DOFade(1f,4f));
        sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 6")));
    }
    public void getSilent(){
        gameSounds.GetComponent<AudioSource>().clip=fluteSound;
        gameSounds.GetComponent<AudioSource>().Play();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor,6f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor,6f));
        sequence.Join(GetComponent<AudioSource>().DOFade(0f,4f));
        //sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 6")));
    }
}
