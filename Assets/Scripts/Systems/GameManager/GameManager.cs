using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{

    public EventFlags eventFlags;
    public GameObject savingPlane;
    public FirstPersonController player;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    async void Start()
    {
        // Loads up all the event flags
        eventFlags = new EventFlags(await SaveManager.LoadEventFlags());
    }

    // Update is called once per frame
    async void Update()
    {
        if(false) // Code willingly unreachable just to show how to save. Will probably be replaced with a proper SaveManager though.
        {
            // Save the game
            await SaveGame();
        }
    }

    // Save the game directly via GameManager
    private async Task SaveGame()
    {
        await SaveManager.SaveEventFlags(eventFlags);
    }
}
