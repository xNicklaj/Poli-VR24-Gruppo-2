using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMesh : IInteractable
{
    public TextMeshPro textReference;
    public AudioSource audioSource;
    public Light lineLight;
    public ParticleSystem particles;
    public DialogueManager dialogueManager;
    public DialogueComponent dc;
    public FirstPersonController fpc;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lineLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = fpc.transform.position - transform.position;

        // Applica una rotazione di 180 gradi (inverte l'asse Z)
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = lookRotation;
    }
    public override void Interact()
    {
        if (isSelected)
        {
            if (dc is DialogueLine)
            {
                dialogueManager.ContinueDialogue(dc);
                isSelectable = false;
                Deselect();
            }
            else if (dc is DialogueAnswer)
            {
                dialogueManager.EndDialogue();
                dialogueManager.StartDialogue((dc as DialogueAnswer).next_dialogue);
                isSelectable = false;
                Deselect();
            }

        }
    }
    public void toggleSelectability()
    {
        isSelectable = !isSelectable;
    }

    public override void Select()
    {
        if (isSelectable && !isSelected)
        {
            
            particles.Play();
            base.Select();
        }
    }

    public override void Deselect()
    {
        particles.Stop();
        base.Deselect();
    }


}
