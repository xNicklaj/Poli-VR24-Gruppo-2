using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialogueMesh : MonoBehaviour
{
    public TextMeshPro textReference;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
