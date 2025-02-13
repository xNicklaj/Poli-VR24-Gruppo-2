using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetFirstSelectedOnSelection : MonoBehaviour, ISelectHandler
{
    public EventSystem ev;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnSelect(BaseEventData baseEventData)
    {
        ev.firstSelectedGameObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
