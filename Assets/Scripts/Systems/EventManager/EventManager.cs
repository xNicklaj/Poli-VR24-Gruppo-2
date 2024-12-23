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
    #endregion

    private void Awake()
    {
           
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
