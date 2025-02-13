using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CuckooSetScheme : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField]
    private string currentScheme;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        EventManager.Instance.deviceChanged.AddListener((useGamepad) =>
        {
            if (useGamepad)
            {
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
                
            }
            else
            {
                playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
            }
        });
        currentScheme = playerInput.currentControlScheme;
    }
}
