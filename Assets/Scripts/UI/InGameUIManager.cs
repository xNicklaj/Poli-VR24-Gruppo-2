using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject quitToMenu;
    public GameObject quitToDesktop;
    public GameObject museumExitForm;
    [Header("Menu First Buttons")]
    [SerializeField] private Button QuitMenuFirstButton;
    [SerializeField] private Button QuitDesktopMenuFirstButton;
    [SerializeField] private Button EscMenuFirstButton;
    [SerializeField] private Button MuseumFormFirstButton;
    [Header("Save Parameters")]
    public GameObject saveDate;
    public GameObject lastSaveWrapper;
    [Header("Other Parameters")]
    [SerializeField] private GameObject playerReference;

    private PlayerInputActions inputActions;
    private InputAction exitMenu;
    [Header("Audio Parameters")]
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
        playerReference.GetComponent<Interactor>().enabled=false;
        pauseSource.mute = false;
        pauseSource.Play();
        GameManager.Instance.PauseGame(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitMuseum(){
        showMuseumExitForm();
        GameManager.Instance.PauseGame(true);
        Cursor.lockState = CursorLockMode.None;
    }
    

    public void ResumeGame()
    {
        if(!museumExitForm.activeInHierarchy){
            unpauseSource.mute = false;
            unpauseSource.Play();
        }
        HideAllPauseMenus();
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance.PauseGame(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerReference.GetComponent<Interactor>().enabled=true;
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
        museumExitForm.SetActive(false);
        if(GameManager.Instance.currentDevice != GameManager.DeviceType.Keyboard){
        EventSystem.current.SetSelectedGameObject(EscMenuFirstButton.gameObject);
        }
    }
    public void showMuseumExitForm(){
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(false);
        quitToMenu.SetActive(false);
        lastSaveWrapper.SetActive(false);
        museumExitForm.SetActive(true);
        if(GameManager.Instance.currentDevice != GameManager.DeviceType.Keyboard){
        EventSystem.current.SetSelectedGameObject(MuseumFormFirstButton.gameObject);
        }
    }

    public void ShowQuitToMenu()
    {
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(false);
        quitToMenu.SetActive(true);
        lastSaveWrapper.SetActive(true);
        museumExitForm.SetActive(false);
        if(GameManager.Instance.currentDevice != GameManager.DeviceType.Keyboard){
        EventSystem.current.SetSelectedGameObject(QuitMenuFirstButton.gameObject);
        }
    }

    public void ShowQuitToDesktop()
    {
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(true);
        quitToMenu.SetActive(false);
        lastSaveWrapper.SetActive(true);
        museumExitForm.SetActive(false);
        if(GameManager.Instance.currentDevice != GameManager.DeviceType.Keyboard){
        EventSystem.current.SetSelectedGameObject(QuitDesktopMenuFirstButton.gameObject);
        }

    }

    public void HideAllPauseMenus()
    {
        pauseMenu.SetActive(false);
        quitToDesktop.SetActive(false);
        quitToMenu.SetActive(false);
        lastSaveWrapper.SetActive(false);
        museumExitForm.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

    }

    private void RecalcSaveDate()
    {
        saveDate.GetComponent<TextMeshPro>();
        if (SaveManager.GetLastModified() != DateTime.UnixEpoch)
            saveDate.GetComponent<TextMeshProUGUI>().text = SaveManager.GetLastModified().ToString("dd/MM/yyyy HH:mm:ss");
    }
}
