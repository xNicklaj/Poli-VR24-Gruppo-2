using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public GameObject quitToMenu;
    public GameObject quitToDesktop;
    public GameObject saveDate;
    public GameObject lastSaveWrapper;

    private PlayerInputActions inputActions;
    private InputAction exitMenu;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        HideAllPauseMenus();
        lastSaveWrapper.SetActive(false);
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
                RecalcSaveDate();
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        ShowPauseMenu();
        GameManager.Instance.PauseGame(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        HideAllPauseMenus();
        GameManager.Instance.PauseGame(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        quitToDesktop.SetActive(false);
        quitToMenu.SetActive(false);
        lastSaveWrapper.SetActive(false);
    }

    public void ShowQuitToMenu()
    {
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(false);
        quitToMenu.SetActive(true);
        lastSaveWrapper.SetActive(true);
    }

    public void ShowQuitToDesktop()
    {
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(true);
        quitToMenu.SetActive(false);
        lastSaveWrapper.SetActive(true);
    }

    public void HideAllPauseMenus()
    {
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(false);
        quitToMenu.SetActive(false);
        lastSaveWrapper.SetActive(false);
    }

    private void RecalcSaveDate()
    {
        if (SaveManager.GetLastModified() != DateTime.UnixEpoch)
            saveDate.GetComponent<TextMeshPro>().text = SaveManager.GetLastModified().ToString("dd/MM/YYYY HH:mm:ss");
    }
}
