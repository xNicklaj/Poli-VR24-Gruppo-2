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
        textInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        playerCanvas.alpha=0f;
        playerCanvas.DOFade(1,5f);
    }
}
