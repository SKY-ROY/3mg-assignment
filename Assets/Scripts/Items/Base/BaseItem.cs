using UnityEngine;

public class BaseItem : MonoBehaviour
{
    // Common properties for items
    [SerializeField] ItemType itemType;
    public ItemType ItemType => itemType;
    [SerializeField] string itemName;
    public string ItemName => itemName;
    [SerializeField] string itemID;
    public string ItemID => itemID;
    [SerializeField] string itemDescription;
    public string ItemDescription => itemDescription;
    // [SerializeField] GameObject itemViewHandlerObject;
    // public GameObject ItemViewHandlerObject => itemViewHandlerObject;

    [SerializeField] Sprite itemIcon;
    public Sprite ItemIcon => itemIcon;

    int assignedListingIndex = -1;
    public int AssignedListingIndex
    {
        get => assignedListingIndex;
        set => assignedListingIndex = value;
    }


    // Use this method for initialization
    public virtual void Initialize()
    {
        // Initialize common item properties here
    }

    // Common functionality for all items (e.g., picking up an item)
    public virtual void Interact()
    {
        Debug.Log("Item " + ItemName + " has been picked up.");
        // Add item-specific functionality here
    }

    // Common functionality for all items (e.g., using an item)
    public virtual void Use()
    {
        Debug.Log("Item " + ItemName + " has been used.");
        // Add item-specific functionality here
    }
}
