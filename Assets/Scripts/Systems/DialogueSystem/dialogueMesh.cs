using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DialogueMesh : IInteractable
{
    public TextMeshPro textReference;
    public AudioSource audioSource;
    public ParticleSystem particles;
    public DialogueManager dialogueManager;
    public DialogueComponent dc;
    public FirstPersonController fpc;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = fpc.transform.position - transform.position;

        // Applica una rotazione di 180 gradi (inverte l'asse Z)
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = lookRotation;
    }
    public void setTextColor(Color c){
        textReference.fontMaterial.SetColor("_GlowColor",c);
    }
    public override void Interact()
    {
        if (isSelected)
        {
            if (dc is DialogueLine)
            {
                textReference.DOColor(new Color(0.2f,0.2f,0.2f,0.5f),1.5f);
                dialogueManager.ContinueDialogue(dc);
                isSelectable = false;
                Deselect();
                GetComponent<CapsuleCollider>().enabled=false;
            }
            else if (dc is DialogueAnswer)
            {
                dialogueManager.EndDialogue();
                dialogueManager.StartDialogue((dc as DialogueAnswer).next_dialogue);
                isSelectable = false;
                Deselect();
                GetComponent<CapsuleCollider>().enabled=false;
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
            
            //particles.Play();
            textReference.fontMaterial.DOFloat(0.1f,"_GlowPower",1f);
            base.Select();
        }
    }

    public override void Deselect()
    {
        //particles.Stop();
        textReference.fontMaterial.DOFloat(0f,"_GlowPower",1f);
        base.Deselect();
    }


}
