using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Book : IInteractable
{
    [SerializeField] private GameObject UIPreset;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private AudioClip bookPageFlipAudio;
    [SerializeField][TextArea] private string text;
    [SerializeField]private float persistenceTime=3f;
    private GameObject textInstance;
    public override void Interact()
    {
        Transform test = playerCanvas.transform.Find("bookText(Clone)");
        if(test!=null){
            Destroy(test.gameObject);
            }
        textInstance = Instantiate(UIPreset,Vector2.zero, new Quaternion(),playerCanvas.transform);
        textInstance.transform.SetAsFirstSibling();
        AudioSource.PlayClipAtPoint(bookPageFlipAudio,new Vector3(-4f,1.2f,75f),0.5f);
        textInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        textInstance.GetComponent<CanvasGroup>().alpha=0f;
        textInstance.GetComponent<TextMeshProUGUI>().text=text;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(1,1f));
        sequence.AppendInterval(persistenceTime);
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(0,1f)).OnComplete(()=>Destroy(textInstance.gameObject));

    }
}
