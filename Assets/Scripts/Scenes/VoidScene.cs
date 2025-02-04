using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class VoidScene : MonoBehaviour
{
    [SerializeField] private GameObject playerReference;

    private List<GameObject> candles = new List<GameObject>();
    [SerializeField] private GameObject nameInputSystem;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject dome;
    [Header("Presets")]
    [SerializeField] private GameObject matchBoxPreset;
    [SerializeField] private GameObject candlePreset;
    [SerializeField] private GameObject SeedPreset;
    private GameObject seedInstance;
    [SerializeField] private GameObject StoneDoorVoid;
    [SerializeField] private GameObject StoneDoorVoidCollisionsOnly;
    [SerializeField] private AudioSource StoneDoorVoidAudioSource;
    [SerializeField] private GameObject HouseDoorVoidCollisionsOnly;
    [SerializeField] private AudioSource HouseDoorVoidAudioSource;


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
        EventManager.Instance.flagHasBeenSet.AddListener(inventorySetFlagSorting);
        floor.GetComponent<Renderer>().material.color = blackRoomColor;
        dome.GetComponent<Renderer>().material.color = blackRoomColor;

        if (GameManager.Instance.eventFlags.GetFlag(EventFlag.IntroDialogueEnded)){
            print("yeah");
        }
        else{
            print(DialogueManager.Instance);
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(2f);
            sequence.AppendCallback(()=>MakeSeedAppear());
            
        }
    }
    public void DialogueEndedFunctions(string dialogueName){
        switch (dialogueName)
        {
            case "Intro Dialogue 1":
                showNameInput();
                break;
            case "Intro Dialogue 3":
                showNameInput();
                break;
            case "God Dialogue 1":
                changeColor();
                break;
            case "God Dialogue 3" or "God Dialogue 4":
                showGodDialogue();
                break;
            case "God Dialogue 6":
                getSilent();
                break;
            case "Self Dialogue":
                StartSeedDialogue();
                break;
            case "Seed Dialogue 1":
                MakeSeedAppear();
                break;
            case "Seed Dialogue 2":
                MakeSeedSelectable();
                break;


        }
    }
    private void showNameInput(){
        Debug.Log("anche senza dai");
        nameInputSystem.SetActive(true);
        nameInputSystem.GetComponent<NameInputSystem>().Appear();
    }
    private void changeColor(){
        gameSounds.GetComponent<AudioSource>().clip=colorChangeSound;
        gameSounds.GetComponent<AudioSource>().Play();
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(whiteRoomColor,3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(whiteRoomColor,3.5f));
        sequence.Join(GetComponent<AudioSource>().DOFade(0,3.5f));
        sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 2")));
    }

    private void showGodDialogue(){
        GetComponent<AudioSource>().volume=0f;
        gameSounds.GetComponent<AudioSource>().clip=godKilledSound;
        gameSounds.GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().clip = godKilledSoundtrack;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(redRoomColor,3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(redRoomColor,3.5f));
        sequence.JoinCallback(()=>GetComponent<AudioSource>().Play());
        sequence.Join(GetComponent<AudioSource>().DOFade(1f,4f));
        sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 6")));
    }
    private void getSilent(){
        gameSounds.GetComponent<AudioSource>().clip=fluteSound;
        gameSounds.GetComponent<AudioSource>().Play();
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor,6f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor,6f));
        sequence.Join(GetComponent<AudioSource>().DOFade(0f,4f));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(()=>throwMatchBox());
    }
    private void throwMatchBox(){
        Vector3 matchBoxPosition = playerReference.transform.position - playerReference.transform.forward*3f+playerReference.transform.up*3f;
        var matchBoxInstance = Instantiate(matchBoxPreset,matchBoxPosition, new Quaternion());
        matchBoxInstance.GetComponent<Rigidbody>().AddForce((playerReference.transform.forward+playerReference.transform.up)*20f);
    }

    private void inventorySetFlagSorting(EventFlag e, bool status){
        if ((e == EventFlag.HasLighter)&&(status == true)){
            startSelfDialogue(new Vector3(0f,0f,1f).normalized*7,"Self 1");
        }
        if ((e == EventFlag.HasSeed)&&(status == true)){
            Vector3 rot = new Vector3(-playerReference.transform.forward.x,0,-playerReference.transform.forward.z);
            StoneDoorVoidCollisionsOnly.transform.rotation = Quaternion.LookRotation(rot,Vector3.up);
            StoneDoorVoid.transform.rotation = Quaternion.LookRotation(rot,Vector3.up);
            Vector3 pos = new Vector3(playerReference.transform.position.x,0f,playerReference.transform.position.z);
            Vector3 posNoY = new Vector3(playerReference.transform.position.x,StoneDoorVoid.transform.position.y,playerReference.transform.position.z);
            StoneDoorVoidCollisionsOnly.transform.position=pos;
            StoneDoorVoid.transform.position = posNoY;
            StoneDoorVoidAudioSource.Play();
            StoneDoorVoid.transform.DOMoveY(0,1f).OnComplete(()=>Destroy(StoneDoorVoidCollisionsOnly.gameObject));
        }
    }
    public void startSelfDialogue(Vector3 pos, string lineName){
        GameObject candleInstance = Instantiate(candlePreset,pos,new Quaternion());
        candleInstance.GetComponent<Candle>().dialogueLine = Resources.Load<DialogueLine>("Dialogues/Self Dialogue/"+lineName);
        candleInstance.GetComponent<Candle>().setVoidScene(this);
        candles.Add(candleInstance);
    }
    public void destroyCandles(){
        foreach(GameObject candle in candles){
            Destroy(candle.gameObject);
        }
    }
    private void StartSeedDialogue(){
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(2.5f);
        sequence.AppendCallback(()=>DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Seed Dialogue/Seed Dialogue 1")));
    }
    private void MakeSeedAppear(){
        seedInstance = Instantiate(SeedPreset,new Vector3(-10f,2f,0f),new Quaternion());
        DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Seed Dialogue/Seed Dialogue 2"));

    }
    private void MakeSeedSelectable(){
        seedInstance.GetComponent<seed>().particles.Play();
        seedInstance.GetComponent<ActionSetFlag>().isSelectable = true;
    }
}
