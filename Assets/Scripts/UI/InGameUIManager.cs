using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;

    private PlayerInputActions inputActions;
    private InputAction exitMenu;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        pauseMenu.SetActive(false);
    }

    public void OnEnable()
    {
        exitMenu = inputActions.UI.ExitMenu;
        exitMenu.performed += OnExitMenu;

        exitMenu.Enable();
    }

    public void OnDisable()
    {
        exitMenu.Disable();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnExitMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Exit Menu");
        if (context.performed)
        {
            if (GameManager.Instance.IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        GameManager.Instance.PauseGame(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.PauseGame(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
