using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{

    public EventFlags eventFlags;
    public FirstPersonController player;

    private EventManager _em;
    private SceneManager _sm;

    private void Awake()
    {
        _em = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManager>();
        _sm = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        // When saveRequested is invoked by any script, SaveGame is called
        _em.saveRequested.AddListener(SaveGame); 
        
    }

    // Start is called before the first frame update
    async void Start()
    {
        // Loads up all the event flags
        eventFlags = new EventFlags(await SaveManager.LoadEventFlags());
        // When setFlag is invoked by any script, SetFlag is called
        _em.setFlag.AddListener(eventFlags.SetFlag);

        _sm.SetSceneByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Code willingly unreachable just to show how to save. Will probably be replaced with a proper SaveManager though.
        if (false) 
        {
            // Save the game
            _em.saveRequested.Invoke();
        }
    }

    // Save the game directly via GameManager
    private async void SaveGame()
    {
        await SaveManager.SaveEventFlags(eventFlags);
    }
}
