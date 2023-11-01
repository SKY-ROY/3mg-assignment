using UnityEngine;
using System.Collections.Generic;
using System;

public class Backpack : MonoBehaviour
{
    // [SerializeField] GameObject itemHolder;
    private bool isInitialized = false;

    private InputController inputController;

    // Maximum number of items that the backpack can hold (excluding weapons)
    [SerializeField] int maxCapacity = 10;

    // Propert to check if the backpack is full (excluding weapons)
    public bool IsFull => items.Count >= maxCapacity;

    // List to store the items in the backpack (excluding weapons)
    private List<BaseItem> items = new List<BaseItem>();

    public static Action<BaseItem> OnItemAdd;
    public static Action<BaseItem> OnItemUse;
    public static Action<BaseItem> OnItemRemove;

    void Awake()
    {
        inputController = FindObjectOfType<InputController>();
    }

    void OnEnable()
    {
        inputController.OnItemUse += UseItem;
        inputController.OnItemDrop += RemoveItem;
    }

    void OnDisable()
    {
        inputController.OnItemUse -= UseItem;
        inputController.OnItemDrop -= RemoveItem;
    }

    public void Initialize()
    {
        isInitialized = true;
        GetComponent<Collider>().enabled = false;
    }

    // Method to add an item to the backpack (excluding weapons)
    public bool AddItem(BaseItem item, bool allowStacking = false)
    {
        if (!IsFull)
        {
            SetPickedObjectOrientation(item.gameObject, transform);

            item.gameObject.SetActive(allowStacking);

            items.Add(item);
            OnItemAdd?.Invoke(item);

            string msg = $"Added item {item.ItemName} to the backpack.";
            NotificationManager.Instance.ShowNotification(msg);

            return true;
        }
        else
        {
            string msg = $"Backpack is full. Cannot add item {item.ItemName}.";
            NotificationManager.Instance.ShowNotification(msg);

            return false;
        }
    }

    // Method to remove an item from the backpack (excluding weapons)
    public void RemoveItem(BaseItem item, bool isUsed)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            OnItemRemove?.Invoke(item);

            string msg = $"Removed item {item.ItemName} from the backpack..";
            NotificationManager.Instance.ShowNotification(msg);

            ProcessDroppedItem(item, isUsed);
        }
        else
        {
            Debug.LogWarning("Item " + item.ItemName + " not found in the backpack.");
        }
    }

    public void RemoveItem(BaseItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            OnItemRemove?.Invoke(item);

            string msg = $"Removed item {item.ItemName} from the backpack..";
            NotificationManager.Instance.ShowNotification(msg);

            ProcessDroppedItem(item, false);
        }
        else
        {
            Debug.LogWarning("Item " + item.ItemName + " not found in the backpack.");
        }
    }

    public void RemoveItem()
    {
        if (items.Count > 0)
        {
            BaseItem itemToRemove = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            OnItemRemove?.Invoke(itemToRemove);

            string msg = $"Removed item {itemToRemove.ItemName} from the backpack..";
            NotificationManager.Instance.ShowNotification(msg);

            ProcessDroppedItem(itemToRemove, true);
        }
        else
        {
            Debug.LogWarning("Backpack empty.");
        }
    }

    // Method to use an item from the backpack (e.g., consume a consumable item)
    public void UseItem(BaseItem item)
    {
        if (items.Contains(item))
        {
            item.Use(); // Call the item's Use method
            OnItemUse?.Invoke(item);

            string msg = $"Used item {item.ItemName} from the backpack.";
            NotificationManager.Instance.ShowNotification(msg);

            RemoveItem(item, true); // Remove the item from the backpack after use
        }
        else
        {
            Debug.LogWarning("Item " + item.ItemName + " not found in the backpack.");
        }
    }

    private void ProcessDroppedItem(BaseItem item, bool allowActivation)
    {
        if (item.ItemType == ItemType.WeaponItem)
        {
            item.gameObject.SetActive(true);
            item.gameObject.transform.parent = null;
        }
        else if (allowActivation)
        {
            item.gameObject.SetActive(false);
            item.gameObject.transform.parent = null;
        }
        else
        {
            string objectSpawnId = item.gameObject.name;

            objectSpawnId = objectSpawnId.Remove(objectSpawnId.Length - 7, 7);

            GameObject droppedObject = ObjectPooler.Instance.GetPooledObject(objectSpawnId, transform.position, Quaternion.identity, true);

            droppedObject.transform.parent = null;
        }
    }

    void SetPickedObjectOrientation(GameObject pickedUpObject, Transform parent)
    {
        pickedUpObject.gameObject.transform.parent = parent;
        pickedUpObject.transform.localPosition = new Vector3(0f, items.Count * 1.5f * pickedUpObject.transform.localScale.y, 0f);
        pickedUpObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
}
