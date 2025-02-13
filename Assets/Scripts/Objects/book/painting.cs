using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Painting : IInteractable
{
    [SerializeField] private GameObject UIPreset;
    [SerializeField] private Canvas canvasReference;
    [SerializeField][TextArea(10,15)] private string text;
    [SerializeField]private float persistenceTime=3f;
    private GameObject textInstance;
    public override void Interact()
    {

        Transform test = canvasReference.transform.Find("StatueText(Clone)");
        if(test!=null){
            Destroy(test.gameObject);
            }
        textInstance = Instantiate(UIPreset,canvasReference.transform);
        textInstance.transform.SetAsFirstSibling();
        textInstance.GetComponent<CanvasGroup>().alpha=0f;
        textInstance.GetComponent<paintingText>().textReference.text=text;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(1,1f));
        sequence.AppendInterval(persistenceTime);
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(0,1f)).OnComplete(()=>Destroy(textInstance.gameObject));

    }
}
