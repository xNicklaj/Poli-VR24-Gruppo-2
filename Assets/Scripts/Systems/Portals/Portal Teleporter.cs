using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform receiver;
    [SerializeField]private bool arrival;
    private bool active = true;

    private bool playerIsOverlapping = false;

    void Start(){
        if (arrival){
            active = !active;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
            if (dotProduct > 0f && active)
            {
                float rotationDiff = Quaternion.Angle(transform.rotation, receiver.rotation);
                if(arrival){
                    rotationDiff *=-1f;
                }
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);
                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f)*portalToPlayer;
                player.position = receiver.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
            active = !active;
        }
    }
}
