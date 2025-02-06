using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BonsaiPot : IInteractable
{
    [SerializeField] private GameObject HouseStonePortal;
    private AudioSource HouseStonePortalSoundSource;
    [SerializeField] private GameObject Trunk;
    [SerializeField] private GameObject Leaves;
    [SerializeField] private GameObject Flowers;
    [SerializeField] private Transform seed;
    private enum plantState
    {
        NOT_PLANTED,
        PLANTED,
        GROWN,
    }
    private plantState state;
    void Start()
    {
        HouseStonePortalSoundSource = HouseStonePortal.GetComponent<AudioSource>();
        Trunk.SetActive(false);
        Leaves.SetActive(false);
        Flowers.SetActive(false);
        seed.position = this.transform.position - Vector3.up * 2;
        state = plantState.NOT_PLANTED;


    }

    public override void Interact()
    {
        switch (state)
        {
            case plantState.NOT_PLANTED:
                if (GameManager.Instance.eventFlags.GetFlag(EventFlag.HasSeed))
                {
                    HouseStonePortalSoundSource.Play();
                    HouseStonePortal.transform.DOMoveY(-5, 1.5f);

                    seed.position = this.transform.position + Vector3.up * 1.5f;
                    seed.DOMoveY(0.8f, 1f);
                    EventManager.Instance.setFlag.Invoke(EventFlag.HasSeed,false);
                    state = plantState.PLANTED;
                }
                break;
            case plantState.PLANTED:
                if (GameManager.Instance.eventFlags.GetFlag(EventFlag.HasWateringCan) &&
                !GameManager.Instance.eventFlags.GetFlag(EventFlag.HasSeed))
                {
                    float trunkScale = Trunk.transform.localScale.x;
                    Trunk.transform.localScale = Vector3.zero;
                    Trunk.SetActive(true);
                    Trunk.transform.DOScale(trunkScale, 1.5f);


                    Leaves.SetActive(true);
                    foreach (Transform leaf in Leaves.transform)
                    {
                        float leafScale = leaf.localScale.x;
                        leaf.localScale = Vector3.zero;
                        leaf.transform.DOScale(leafScale, 3f);
                    }

                    Flowers.SetActive(true);
                    foreach (Transform flower in Flowers.transform)
                    {
                        float flowerScale = flower.localScale.x;
                        flower.localScale = Vector3.zero;
                        flower.transform.DOScale(flowerScale, 5f);
                    }
                }
                break;
            case plantState.GROWN:
                break;

        }
    }
}
