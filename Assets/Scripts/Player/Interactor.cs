using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform source;
    public float range = 3f;

    private PlayerInputActions pc;
    private InputAction interact;

    public FirstPersonController fpc;

    private void Awake()
    {
        pc = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = pc.Player.Interact;

        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(source.position, source.forward * range, Color.red);
        if (Physics.Raycast(source.position, source.forward, out hit, range) && 
            hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactObj))
        {
            fpc.DisplayCrosshair();
            Debug.Log("Interactable object found");
            if (interact.IsPressed())
            {
                interactObj.Interact();
            }
        }
        else
            fpc.HideCrosshair();
    }
}
