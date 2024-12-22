using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_SaveOnCollision : MonoBehaviour
{
    EventManager _em;

    // Start is called before the first frame update
    void Awake()
    {
        _em = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _em.saveRequested.Invoke(); // Tell the GameManager to save the game
        }
    }
}
