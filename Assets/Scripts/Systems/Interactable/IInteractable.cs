using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    public bool isSelectable = true;
    public bool isSelected = false;
    public abstract void Interact();

    public virtual void Select()
    {
        if (isSelectable && !isSelected)
        {
            isSelected = true;
        }
    }

    public virtual void Deselect()
    {
        isSelected = true;
    }
}