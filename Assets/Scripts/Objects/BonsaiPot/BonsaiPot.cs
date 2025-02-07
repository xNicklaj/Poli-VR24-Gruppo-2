using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BonsaiPot : IInteractable
{
    [SerializeField] private AudioClip treeGrowingAudio;
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
        state = plantState.PLANTED;
        DialogueManager.Instance.dialogueEnded.AddListener(SortDialogue);
        GameManager.Instance.eventFlags.SetFlag(EventFlag.HasWateringCan,true);


    }

    public override void Interact()
    {
        switch (state)
        {
            case plantState.NOT_PLANTED:
                if (GameManager.Instance.eventFlags.GetFlag(EventFlag.HasSeed))
                {
                    HouseStonePortalSoundSource.Play();
                    HouseStonePortal.transform.DOMoveY(-5, 3f);

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
                    AudioSource.PlayClipAtPoint(treeGrowingAudio,transform.position,0.5f);
                    float trunkScale = Trunk.transform.localScale.x;
                    Trunk.transform.localScale = Vector3.zero;
                    Trunk.SetActive(true);
                    Trunk.transform.DOScale(trunkScale, 2.5f);


                    Leaves.SetActive(true);
                    foreach (Transform leaf in Leaves.transform)
                    {
                        float leafScale = leaf.localScale.x;
                        leaf.localScale = Vector3.zero;
                        leaf.transform.DOScale(leafScale, 5f);
                    }

                    Flowers.SetActive(true);
                    foreach (Transform flower in Flowers.transform)
                    {
                        float flowerScale = flower.localScale.x;
                        flower.localScale = Vector3.zero;
                        flower.transform.DOScale(flowerScale, 6f);
                    }
                    DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Tree Dialogue/Tree Dialogue 1"));
                }
                break;
            case plantState.GROWN:
                break;

        }
    }
    void SortDialogue(string dialogueName){
        switch(dialogueName){
            case "Tree Dialogue 1":
                foreach(Transform leaf in Leaves.transform){
                    leaf.gameObject.GetComponent<MeshCollider>().convex=true;
                    leaf.gameObject.GetComponent<Rigidbody>().isKinematic=false;

                    //leaf.gameObject.GetComponent<Rigidbody>().useGravity=true;

                    leaf.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-15f, 15f),1f,Random.Range(-15f, 15f)));



                }
                break;
        }
    }
}
