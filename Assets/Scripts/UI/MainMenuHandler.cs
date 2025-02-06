using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        if(continueButton != null)
            continueButton.GetComponent<Button>().interactable = SaveManager.GetLastModified() != DateTime.UnixEpoch;
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void PlayGame()
    {
        GameManager.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);     
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        GameManager.Instance.eventFlags.PrintFlags();
        GameManager.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
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
