using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSetFlag : IInteractable
{
    public EventFlag flag;
    public bool value;

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
        EventManager.Instance.setFlag.Invoke(e, b);
    }

    public override void Interact()
    {
        SetFlag(flag, value);
        isSelectable=false;
    }
}
