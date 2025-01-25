using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSave : IInteractable
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact()
    {
        EventManager.Instance.saveRequested.Invoke();
    }
}
