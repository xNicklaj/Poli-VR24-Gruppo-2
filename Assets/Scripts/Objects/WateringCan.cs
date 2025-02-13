using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : ActionSetFlag
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter (Collision collision){
        if(collision.relativeVelocity.magnitude > 3f){
            audioSource.Play();
        }
    }
    public override void Interact()
    {
        GameManager.Instance.eventFlags.SetFlag(flag, value);
        isSelectable =false;
        Destroy(this.gameObject);
    }
}
