using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Book : IInteractable
{
    [SerializeField] private GameObject UIPreset;
    [SerializeField] private GameObject cuckoo;
    [SerializeField] private GameObject playerCanvas;
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
        textInstance.GetComponent<CanvasGroup>().alpha=0f;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(1,1f));
        sequence.AppendInterval(3f);
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(0,1f)).OnComplete(()=>Destroy(textInstance.gameObject));

    }
}
