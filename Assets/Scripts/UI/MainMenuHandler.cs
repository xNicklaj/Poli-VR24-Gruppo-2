using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
