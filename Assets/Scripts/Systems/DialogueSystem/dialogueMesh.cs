using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialogueMesh : MonoBehaviour, IInteractable
{
    public TextMeshPro textReference;
    public AudioSource audioSource;
    public Light lineLight;
    public ParticleSystem particles;
    public bool isSelected = false;
    public bool isSelectable = true;


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
        ;
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
