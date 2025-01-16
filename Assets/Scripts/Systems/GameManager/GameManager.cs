using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


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

    private PlayerInputActions pia;
    private InputAction loadAction;

    [SerializeField] private bool isGamePaused = false;

    private String player_name {get;set;}

    private void Awake()
    {
        DontDestroyOnLoad(this);

        // Load SceneManager and DialogueManager from scene, the rest is instanced.
        GameObject goEm;
        GameObject goSm;
        _sm = GameObject.FindGameObjectWithTag("SceneManager")?.GetComponent<SceneManager>();
        _dm = GameObject.FindGameObjectWithTag("DialogueManager")?.GetComponent<DialogueManager>();
        
        goEm = Instantiate(EventManagerPrefab);
        _em = goEm.GetComponent<EventManager>();
        goSm = Instantiate(SaveManagerPrefab);
        _svm = goSm.GetComponent<SaveManager>();

        // When saveRequested is invoked by any script, SaveGame is called
        EventManager.Instance.saveRequested.AddListener(SaveGame);

        // When the unity scene changes, reload the managers that need to be reloaded
        _em.unitySceneChanged.AddListener(HandleSceneChange);
        
        // Initialize event flags. Why not in constructor? STFU just do it here or everything breaks.
        eventFlags = new EventFlags();
        eventFlags.InitializeFlags();
#if UNITY_EDITOR
        pia = new PlayerInputActions();
#endif
    }

#region DEBUG_LOAD
#if UNITY_EDITOR
    private void OnEnable()
    {
        loadAction = pia.UI.LoadGame;
        loadAction.Enable();
        loadAction.performed += LoadGameWrapper;
    }

    private void OnDisable()
    {
        loadAction?.Disable();
    }

    public void LoadGameWrapper(InputAction.CallbackContext callbackContext)
    {
        LoadGame();
    }
#endif
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _em.setFlag.AddListener(eventFlags.SetFlag);

        //_em.dialogueEnded.AddListener(_dm.func);
    }

    private void HandleSceneChange(bool isMenu)
    {
        // Reload manager if scene has changed but it's not in the main menu
        if(!isMenu)
            ReloadManagers();
    }

    // Does exactly what it says, plus sets the scene index to 0. You only need to call this when changing unity scene.
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

    public void LoadGame()
    {
        eventFlags = new EventFlags(SaveManager.LoadEventFlags());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        PauseGame(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void PauseGame(bool isPaused)
    {
        isGamePaused = isPaused;
        Time.timeScale = isGamePaused ? 0 : 1;
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    public bool GetFlagStatus(EventFlag flag)
    {
        return eventFlags.GetFlag(flag);
    }
}
