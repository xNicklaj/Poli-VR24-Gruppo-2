using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Button startButton;
    public Button continueButton;
    public Button quitButton;
    public Button museumButton;

    public TextMeshProUGUI subText;

    public GameObject optionsMenu;
    public GameObject mainMenu;


    // Start is called before the first frame update
    void Start()
    {
        if (continueButton != null)
            continueButton.GetComponent<Button>().interactable = SaveManager.GetLastModified() != DateTime.UnixEpoch;
        EventFlags eventFlags = SaveManager.LoadEventFlags();
        if (eventFlags != null && subText)
        {
            if (eventFlags.playerName != null)
            {
                if ((!(eventFlags.playerName == "null")) && !(eventFlags.playerName == "Player") && !(eventFlags.playerName == "nome_nullo"))
                {
                    subText.gameObject.SetActive(true);
                    subText.text = "Cosa vuoi fare della tua vita, " + eventFlags.playerName + "?";
                }
            }
            if (eventFlags.GetFlag(EventFlag.MuseumExited))
            {
                print("MUSEO FINITO");
                museumButton.gameObject.SetActive(true);
            }
        }
    }

    public void showOptions(){
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void showMainMenu(){
        EventManager.Instance.settingsChanged.Invoke();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        GameManager.Instance.eventFlags = new EventFlags();
        GameManager.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1, false);
    }
    public void goToMuseum()
    {
        GameManager.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1, true);
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        GameManager.Instance.eventFlags.PrintFlags();
        GameManager.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1, false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenItchPage()
    {
        Application.OpenURL("https://nicklaj.itch.io/domande");
    }

    public void Fart()
    {
        AudioSource fart = GameObject.Find("FartSource").GetComponent<AudioSource>();
        Debug.Log("Farting...");
        fart.Play();
    }
}
