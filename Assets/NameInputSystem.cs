using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NameInputSystem : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    [SerializeField] TMP_InputField inputField;
    public void Appear(){
        Sequence sequence = DOTween.Sequence();
        inputField.transform.position = inputField.transform.position-new Vector3(0.0f,10.0f,0.0f);
        canvasGroup.alpha = 0f;
        sequence.Append(inputField.transform.DOMoveY(inputField.transform.position.y+ 10.0f,1f));
        sequence.Join(canvasGroup.DOFade(1f,1f)).onComplete=inputField.ActivateInputField;
        

    }
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            Appear();
        };
        if (Input.GetButtonDown("Submit")){
            Submit();
        };
    }

    void Submit(){
        //TODO;
    }
    
}
