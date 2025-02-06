using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cuckoo : IInteractable
{
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private PlayerInput input;
    [SerializeField] private Transform hoursArm;
    [SerializeField] private Transform minutesArm;
    [SerializeField] private Renderer hoursArmMaterial;
    [SerializeField] private Renderer minutesArmMaterial;
    [SerializeField] private float selectionSpeed = 1f;

    private enum states
    {
        HOURS_SELECTED,
        MINUTES_SELECTED,
        DESELECTED
    }
    private states state = states.DESELECTED;

    void Awake(){
        input = GetComponent<PlayerInput>();
        input.enabled=true;
        //input.DeactivateInput();
    }
    public override void Interact()
    {
        if (state == states.DESELECTED)
        {
            isSelectable = false;
            fpc.playerState=FirstPersonController.PlayerStates.IDLE;

            input.ActivateInput();
            state = states.HOURS_SELECTED;
        }
    }



    public void OnClockSwitchArm(InputAction.CallbackContext context)
    {
        print("prova");
        if (context.started)
        {
            print("pisello");
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
    }

    public void OnClockExit(InputAction.CallbackContext context)
    {
        if (state == states.DESELECTED)
        {
            return;
        }
        if (context.started)
        {
            //isSelectable = false;
            //hoursArmMaterial.material.color = Color.white;
            //minutesArmMaterial.material.color = Color.white;
            //state = states.DESELECTED;
            //GetComponent<PlayerInput>().enabled = false;
            //fpc.GetComponent<PlayerInput>().enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(input.actions["ClockControlArm"].WasPressedThisFrame()){
            print("ma vaffanculo");
        }
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
}
