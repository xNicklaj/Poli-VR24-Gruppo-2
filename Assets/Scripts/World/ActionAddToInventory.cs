using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAddToInventory : ActionSetFlag, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        SetFlag(flag, value);
        Destroy(this.gameObject);
    }
}