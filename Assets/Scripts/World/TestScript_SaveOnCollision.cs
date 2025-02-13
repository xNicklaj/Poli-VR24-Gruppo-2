using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_SaveOnCollision : MonoBehaviour
{
    EventManager _em;
    public bool onExit = false;
    public bool triggerOnce = true;

    [SerializeField]
    private bool _hasTriggered = false;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onExit) return;
        if (triggerOnce && _hasTriggered) return;
        if (other.tag == "Player")
        {
            EventManager.Instance.saveRequested.Invoke(); // Tell the GameManager to save the game
            if(triggerOnce) _hasTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onExit) return;
        if (triggerOnce && _hasTriggered) return;
        if (other.tag == "Player")
        {
            EventManager.Instance.saveRequested.Invoke(); // Tell the GameManager to save the game
            if (triggerOnce) _hasTriggered = true;
        }
    }
}
