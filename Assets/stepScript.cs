using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepScript : MonoBehaviour
{
    [SerializeField] private List<AudioClip> voidStep = new List<AudioClip>();
    [SerializeField] private List<AudioClip> houseStep = new List<AudioClip>();
    [SerializeField] private List<AudioClip> museumStep = new List<AudioClip>();
    private List<AudioClip> selectedSteps = new List<AudioClip>();

    [SerializeField] private AudioSource audioSource;
    public void playStepSound(){
        audioSource.pitch=Random.Range(0.8f,1.2f);
        audioSource.clip=selectedSteps[Random.Range(0,3)];
        audioSource.Play();
    }
    void Update()
    {
        if (GameManager.Instance.IsGamePaused()) return;
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up*2.5f, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2.5f) 
        && hit.collider.gameObject.TryGetComponent<Renderer>(out Renderer mat)){
                print(mat.material.name);
                switch(mat.material.name){
                    case "provetta (Instance)":
                        selectedSteps = voidStep;
                        break;
                    case "New Material (Instance)":
                        selectedSteps = houseStep;
                        break;
                    case "WoodFloor Variant (Instance)":
                        selectedSteps = museumStep;
                        break;
                }
            }
    }
}
