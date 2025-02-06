using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Book : IInteractable
{
    [SerializeField] private GameObject UIPreset;
    [SerializeField] private GameObject cuckoo;
    [SerializeField] private CanvasGroup playerCanvas;
    [SerializeField] private AudioClip bookPageFlipAudio;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        GameObject textInstance = Instantiate(UIPreset,Vector2.zero, new Quaternion(),playerCanvas.transform);
        AudioSource.PlayClipAtPoint(bookPageFlipAudio,new Vector3(-4f,1.2f,75f),0.5f);
        textInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        playerCanvas.alpha=0f;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(playerCanvas.DOFade(1,2f));
        sequence.AppendInterval(3f);
        sequence.Append(playerCanvas.DOFade(0,2f)).OnComplete(()=>Destroy(textInstance.gameObject));

    }
}
