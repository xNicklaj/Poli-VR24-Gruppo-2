using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMesh : MonoBehaviour, IInteractable
{
    public TextMeshPro textReference;
    public AudioSource audioSource;
    public Light lineLight;
    public ParticleSystem particles;
    public DialogueManager dialogueManager;
    public bool isSelected = false;
    public bool isSelectable = true;
    public DialogueComponent dc;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lineLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact()
    {
        if (isSelected){
            dialogueManager.ContinueDialogue(dc);
            isSelectable = false;
            Deselect();
        }
    }

    public void Select()
    {
        if(isSelectable && !isSelected){
            particles.Play();
            isSelected= true;
        }
    }

    public void Deselect()
    {
        particles.Stop();
        isSelected = false;
    }


}
