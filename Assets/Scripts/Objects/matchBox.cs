using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class matchBox : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        EventManager.Instance.flagHasBeenSet.AddListener(destroyMatchBox);
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter (Collision collision){
        if(collision.relativeVelocity.magnitude > 3f){
            audioSource.Play();
        }
    }
    private void destroyMatchBox(EventFlag e, bool status){
        if ((e == EventFlag.HasLighter)&&(status == true)){
            Destroy(gameObject);
        }
    }
}
