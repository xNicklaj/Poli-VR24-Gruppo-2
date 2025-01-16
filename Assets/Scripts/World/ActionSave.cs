using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSave : MonoBehaviour, IInteractable
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Select()
    {
        ;
    }
    public void Deselect() {; }

    void IInteractable.Interact()
    {
        EventManager.Instance.saveRequested.Invoke();
    }
}
