using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PortalDisappearArea : MonoBehaviour
{
    [SerializeField]private Transform StonePortalVoid;
    private void OnTriggerExit(Collider other)
    {
        // Verifica se l'oggetto che esce è il giocatore
        if (other.CompareTag("Player"))
        {
            StonePortalVoid.DOLocalMoveY(-5,2);
            StonePortalVoid.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
