using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSetFlag : IInteractable
{
    public EventFlag flag;
    public bool value;
    public bool saveOnSet = false;

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
        GameManager.Instance.eventFlags.SetFlag(e, b);
    }

    public override void Interact()
    {
        SetFlag(flag, value);
        isSelectable = false;
        if(saveOnSet){
            EventManager.Instance.saveRequested.Invoke();
        }
    }
}
