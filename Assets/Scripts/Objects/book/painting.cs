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
    private DG.Tweening.Sequence sequence;
    public bool visited;

    private bool isVisible = false;
    public float distanceToHide = 4.5f;

    public void Awake()
    {
        sequence = DOTween.Sequence();
    }

    public void Update()
    {
        Vector3 v1 = this.transform.position;
        v1.y = 0;

        Vector3 v2 = Camera.main.transform.position;
        v2.y = 0;

        if (isVisible && Vector3.Distance(v1, v2) > distanceToHide) Hide();
    }

    public override void Interact()
    {
        visited=true;
        if (isVisible) Hide();
        else Show();
    }

    public void Show()
    {
        Transform test = canvasReference.transform.Find("StatueText(Clone)");
        if (test != null)
        {
            Destroy(test.gameObject);
        }
        textInstance = Instantiate(UIPreset, canvasReference.transform);
        textInstance.transform.SetAsFirstSibling();
        textInstance.GetComponent<CanvasGroup>().alpha = 0f;
        textInstance.GetComponent<paintingText>().textReference.text = text;
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(1, 1f));
        //sequence.AppendInterval(persistenceTime);
        isVisible = true;
    }

    public void Hide()
    {
        sequence.Append(textInstance.GetComponent<CanvasGroup>().DOFade(0, 0.5f)).OnComplete(() => Destroy(textInstance.gameObject));
        isVisible = false;
    }
}
