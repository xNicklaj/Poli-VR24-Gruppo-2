using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class Candle : IInteractable
{
    // Start is called before the first frame update
    public DialogueLine dialogueLine;
    private DialogueMesh dialogueMesh;
    public GameObject body;

    [SerializeField] private Light fire_light_emitter;
    [SerializeField] private Light base_light_emitter;
    private AudioSource audioSource;
    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private AudioClip appearAudio;
    [SerializeField] private VoidScene voidScene;
    [SerializeField] private ParticleSystem particles;
    public void setVoidScene(VoidScene value){
        voidScene = value;
    }
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playAppearanceSound(){
        audioSource.Play();
    }

    public override void Interact()
    {
        audioSource.clip=fireAudio;
        audioSource.volume=0.5f;
        base_light_emitter.DOIntensity(0f,1.5f);
        fire_light_emitter.DOIntensity(0.1f,1.5f);
        audioSource.Play();
        particles.Play();
        dialogueMesh=DialogueManager.Instance.DisplayCandleLine(dialogueLine, transform.position + new Vector3(0,2.5f,0f));
        Vector3 position;
        isSelectable=false;
        switch(dialogueLine.name){
            case "Self 1":
                position = new Vector3(1f,0f,1f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 2");
                break;
            case "Self 2":
                position = new Vector3(1f,0f,0f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 3");
                break;
            case "Self 3":
                position = new Vector3(1f,0f,-1f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 4");
                break;
            case "Self 4":
                position = new Vector3(0f,0f,-1f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 5");
                break;
            case "Self 5":
                position = new Vector3(-1f,0f,-1f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 6");
                break;
            case "Self 6":
                position = new Vector3(-1f,0f,0f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 7");
                break;
            case "Self 7":
                position = new Vector3(-1f,0f,1f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 8");
                break;
            case "Self 8":
                position = new Vector3(0f,0f,0f).normalized*7;
                voidScene.startSelfDialogue(position,"Self 9");
                break;
        }
    }


    void Start()
    {
        fire_light_emitter.intensity = 0f;
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
