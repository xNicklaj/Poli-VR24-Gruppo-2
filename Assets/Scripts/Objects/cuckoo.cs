using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cuckoo : IInteractable
{
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject playerCanvas;

    [SerializeField] private Transform hoursArm;
    [SerializeField] private Transform minutesArm;
    [SerializeField] private Renderer hoursArmMaterial;
    [SerializeField] private Renderer minutesArmMaterial;
    [SerializeField] private float selectionSpeed = 1f;
    [SerializeField] private GameObject ClockTutorialUI;
    private GameObject textInstance;

    [HideInInspector]public bool completed=false;
    [SerializeField] private GameObject wateringCanPrefab;


    [Header("Bird Components")]
    [SerializeField] private Transform door;
    [SerializeField] private Transform arm;
    [SerializeField] private Transform wings;


    private enum states
    {
        HOURS_SELECTED,
        MINUTES_SELECTED,
        DESELECTED
    }
    private states state = states.DESELECTED;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        input.enabled = true;
        input.DeactivateInput();
    }
    public override void Interact()
    {
        if (state == states.DESELECTED && completed==false)
        {
            isSelectable = false;
            fpc.playerState = FirstPersonController.PlayerStates.IDLE;
            input.ActivateInput();
            state = states.HOURS_SELECTED;
            fpc.HideCrosshair();
            fpc.GetComponent<Interactor>().enabled=false;

            textInstance = Instantiate(ClockTutorialUI,Vector2.zero, new Quaternion(),playerCanvas.transform);
            textInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            textInstance.GetComponent<CanvasGroup>().alpha=0f;
            textInstance.GetComponent<CanvasGroup>().DOFade(1,0.5f);
        }
    }
    public void OnClockSwitchArm()
    {
        switch (state)
        {
            case states.HOURS_SELECTED:
                hoursArmMaterial.material.color = Color.white;
                state = states.MINUTES_SELECTED;

                break;
            case states.MINUTES_SELECTED:
                minutesArmMaterial.material.color = Color.white;
                state = states.HOURS_SELECTED;

                break;

        }
    }

    public void OnClockExit()
    {
        if (state == states.DESELECTED)
        {
            return;
        }
        textInstance.GetComponent<CanvasGroup>().DOFade(0,0.5f).OnComplete(()=>Destroy(textInstance.gameObject));
        
        fpc.playerState=FirstPersonController.PlayerStates.MOVE;
        hoursArmMaterial.material.color = Color.white;
        minutesArmMaterial.material.color = Color.white;
        state = states.DESELECTED;
        input.DeactivateInput();
        isSelectable = true;
        fpc.GetComponent<Interactor>().enabled=true;
        
        
        float hoursArmRotation = hoursArm.localRotation.x*180;
        float minutesArmRotation = minutesArm.localRotation.x*180;
        if(hoursArmRotation>15&&hoursArmRotation<45&&minutesArmRotation<-75&&minutesArmRotation>-105){
            //print("orario corretto");
            completed = true;
            cuckooAnimation();
            throwWateringCan();
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case states.HOURS_SELECTED:
                hoursArmMaterial.material.color = Color.Lerp(Color.red, Color.white, (Mathf.Sin(Time.time * selectionSpeed) + 1) / 2f);
                hoursArm.Rotate(input.actions["ClockControlArm"].ReadValue<float>(), 0f, 0f);
                break;
            case states.MINUTES_SELECTED:
                minutesArmMaterial.material.color = Color.Lerp(Color.red, Color.white, (Mathf.Sin(Time.time * selectionSpeed) + 1) / 2f);
                minutesArm.Rotate(input.actions["ClockControlArm"].ReadValue<float>(), 0f, 0f);
                break;
        }
    }
    void cuckooAnimation(){
        Sequence sequence = DOTween.Sequence();
        sequence.Append(door.transform.DOLocalRotate(new Vector3(0f,-90f,0f),0.5f));
        sequence.Join(arm.DOLocalMoveX(-0.02f,0.5f));
        sequence.JoinCallback(()=>arm.GetComponent<AudioSource>().Play());
        sequence.AppendInterval(1f);
        sequence.Append(door.transform.DOLocalRotate(new Vector3(0f,0,0f),0.5f));
        sequence.Join(arm.DOLocalMoveX(-0.002f,0.5f));
    }
    
    void throwWateringCan(){
        Vector3 pos = fpc.transform.position - fpc.transform.forward*2f+fpc.transform.up*2f;
        var wateringCanInstance = Instantiate(wateringCanPrefab,pos, new Quaternion());
        wateringCanInstance.GetComponent<Rigidbody>().AddForce((fpc.transform.forward+fpc.transform.up)*10f);
    }
}
