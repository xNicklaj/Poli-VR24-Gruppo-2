using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSetFlag : MonoBehaviour, IInteractable
{
    public EventFlag flag;
    public bool value;

    private EventManager _em;

    void Awake()
    {
        _em = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFlag(EventFlag e, bool b)
    {
        _em.setFlag.Invoke(e, b);
    }

    void IInteractable.Interact()
    {
        SetFlag(flag, value);
    }
}
