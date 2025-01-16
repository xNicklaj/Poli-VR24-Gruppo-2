using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum ItemType
{
    Lantern,
    Seed,
    Lighter,
    Crayon,
    Can
}

public class InventoryItem : MonoBehaviour
{
    public EventFlag flag;
    public bool flagStatus;
    public ItemType itemIcon;

    [SerializeField] private GameObject iconGo;

    private void Awake()
    {
        string iconName = "none";
        switch (itemIcon)
        {
            case ItemType.Lighter:
                iconName = "Lighter";
                break;
            case ItemType.Lantern:
                iconName = "Lantern";
                break;
            case ItemType.Seed:
                iconName = "Seed";
                break;
            case ItemType.Crayon:
                iconName = "Crayon";
                break;
            case ItemType.Can:
                iconName = "Can";
                break;
        }
        Debug.Log("Loading " + "Textures/UI/InventoryIcons/" + iconName);
        iconGo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/InventoryIcons/" + iconName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
