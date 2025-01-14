using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    #region Save Data IO
    public UnityEvent saveRequested;
    public UnityEvent loadRequested;
    #endregion

    #region Flags and Triggers
    public UnityEvent<EventFlag, bool> setFlag;
    public UnityEvent<string> dialogueEnded;
    // True for main menu, false for in-game scene
    public UnityEvent<bool> unitySceneChanged;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
