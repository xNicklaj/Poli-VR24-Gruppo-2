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
    private PlayerInputActions pc;
    private InputAction submit;
    public void Appear(){
        submit = pc.Player.Submit;
        submit.Enable();
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
        DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Intro Dialogue/Intro Dialogue 2"));
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Awake()
    {
        pc = new PlayerInputActions();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (submit.WasPressedThisFrame()){
            Submit();
        };
    }

    void Submit(){
        submit.Disable();
        inputField.DeactivateInputField();
        GameManager.Instance.player_name=inputField.text;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(inputField.transform.DOMoveY(inputField.transform.position.y- 10.0f,1f));
        sequence.Join(canvasGroup.DOFade(0f,1f)).OnComplete(()=>Disappear());
    }
    
}
