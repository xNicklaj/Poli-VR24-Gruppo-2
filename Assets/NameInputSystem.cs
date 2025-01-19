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
    [SerializeField] FirstPersonController player;
    [SerializeField] TMP_InputField inputField;
    private bool canSubmit;
    public void Appear(){
        canSubmit = true;
        player.cameraCanMove=false;
        player.playerState=FirstPersonController.PlayerStates.IDLE;
        Sequence sequence = DOTween.Sequence();
        inputField.transform.position = inputField.transform.position-new Vector3(0.0f,10.0f,0.0f);
        canvasGroup.alpha = 0f;
        sequence.Append(inputField.transform.DOMoveY(inputField.transform.position.y+ 10.0f,1f));
        sequence.Join(canvasGroup.DOFade(1f,1f)).onComplete=inputField.ActivateInputField;
    }
    private void Disappear(){

        player.cameraCanMove=true;
        player.playerState=FirstPersonController.PlayerStates.MOVE;
        Debug.Log(inputField.text);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && canSubmit){
            canSubmit=false;
            Submit();
        };
    }

    void Submit(){
        inputField.DeactivateInputField();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(inputField.transform.DOMoveY(inputField.transform.position.y- 10.0f,1f));
        sequence.Join(canvasGroup.DOFade(0f,1f)).OnComplete(()=>Disappear());
    }
    
}
