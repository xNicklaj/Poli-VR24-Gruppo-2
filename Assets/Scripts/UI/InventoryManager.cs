using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameObject[] items;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.flagHasBeenSet.AddListener(EvalInventoryWrapper);
        EvalInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EvalInventoryWrapper(EventFlag e, bool status)
    {
        EvalInventory();
    }

    private void EvalInventory()
    {
        // Destroy all current children
        for (int i = 0; i < this.transform.childCount; i++) 
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
        // Recalculate inventory
        for (int i = 0; i < items.Length; i++)
        {
            InventoryItem item = items[i].GetComponent<InventoryItem>();
            if (GameManager.Instance.eventFlags.GetFlag(item.flag))
            {
                Instantiate(item, this.transform);
            }
        }
    }
}
