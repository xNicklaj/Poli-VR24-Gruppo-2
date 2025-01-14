using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{

    public EventFlags eventFlags;
    public FirstPersonController player;

    public GameObject EventManagerPrefab;
    public GameObject SaveManagerPrefab;

    private EventManager _em;
    private SceneManager _sm;
    private DialogueManager _dm;
    private SaveManager _svm;

    private String player_name {get;set;}

    private void Awake()
    {
        DontDestroyOnLoad(this);

        GameObject goEm;
        GameObject goSm;
        _sm = GameObject.FindGameObjectWithTag("SceneManager")?.GetComponent<SceneManager>();
        _dm = GameObject.FindGameObjectWithTag("DialogueManager")?.GetComponent<DialogueManager>();
        
        goEm = Instantiate(EventManagerPrefab);
        _em = goEm.GetComponent<EventManager>();
        goSm = Instantiate(SaveManagerPrefab);
        _svm = goSm.GetComponent<SaveManager>();

        // When saveRequested is invoked by any script, SaveGame is called
        _em.saveRequested.AddListener(SaveGame);

        Debug.Log("Binding scene change event...");
        // When the unity scene changes, reload the managers that need to be reloaded
        _em.unitySceneChanged.AddListener(ReloadManagers);
    }

    // Start is called before the first frame update
    async void Start()
    {
        // Loads up all the event flags
        eventFlags = new EventFlags(await SaveManager.LoadEventFlags());
        // When setFlag is invoked by any script, SetFlag is called
        _em.setFlag.AddListener(eventFlags.SetFlag);

        //_em.dialogueEnded.AddListener(_dm.func);
    }

    private void ReloadManagers()
    {
        Debug.Log("Reloading Managers...");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        _sm = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        _dm = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        _sm.enabled = true;
        _dm.enabled = true;
        _sm.SetSceneByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _em.GetComponent<EventManager>().unitySceneChanged.Invoke();
        }

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

    public async void LoadGame()
    {
        eventFlags = new EventFlags(await SaveManager.LoadEventFlags());
    }
}
