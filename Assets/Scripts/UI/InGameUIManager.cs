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

    public AudioSource pauseSource;
    public AudioSource unpauseSource;
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
        EventManager.Instance?.saveFinished.AddListener(RecalcSaveDate);
        RecalcSaveDate();
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
        ShowPauseMenu();
        pauseSource.mute = false;
        pauseSource.Play();
        GameManager.Instance.PauseGame(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        HideAllPauseMenus();
        unpauseSource.mute = false;
        unpauseSource.Play();
        GameManager.Instance.PauseGame(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BackToMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
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
        saveDate.GetComponent<TextMeshPro>();
        if (SaveManager.GetLastModified() != DateTime.UnixEpoch)
            saveDate.GetComponent<TextMeshProUGUI>().text = SaveManager.GetLastModified().ToString("dd/MM/yyyy HH:mm:ss");
    }
}
