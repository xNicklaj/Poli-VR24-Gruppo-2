using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Candle : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [SerializeField] private Light light_emitter;
    [SerializeField] private ParticleSystem particles;

    public void Deselect()
    {
        ;
    }

    public void Interact()
    {
        light_emitter.DOIntensity(0.1f,1.5f);
        particles.Play();
    }

    public void Select()
    {
        ;
    }

    void Start()
    {
        light_emitter.intensity = 0f;
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
