using UnityEngine;
using System.Collections.Generic;
using System;

public class Backpack : MonoBehaviour
{
    private bool isInitialized = false;

    private InputController inputController;

    // Maximum number of items that the backpack can hold (excluding weapons)
    public int maxCapacity = 10;

    // Propert to check if the backpack is full (excluding weapons)
    public bool IsFull => items.Count >= maxCapacity;

    // List to store the items in the backpack (excluding weapons)
    private List<BaseItem> items = new List<BaseItem>();

    // Weapon slot to hold one weapon
    private Weapon weaponSlot;

    public static Action<BaseItem> OnItemAdd;
    public static Action<BaseItem> OnItemUse;
    public static Action<BaseItem> OnItemRemove;
    public static Action<Weapon> OnWeaponEquip;
    public static Action<Weapon> OnWeaponUnequip;
    public static Action<bool> OnShowBackpackInfo;

    void Awake()
    {
        inputController = FindObjectOfType<InputController>();
    }

    void OnEnable()
    {
        inputController.OnItemUse += UseItem;
        inputController.OnItemDrop += RemoveItem;
        inputController.OnWeaponCharge += ChargeWeapon;
        inputController.OnWeaponDrop += UnequipWeapon;
        inputController.OnPrimaryFire += UseWeapon;
        inputController.OnInfoDisplay += ShowBackPackInfoScreen;
    }

    void OnDisable()
    {
        inputController.OnItemUse -= UseItem;
        inputController.OnItemDrop -= RemoveItem;
        inputController.OnWeaponCharge -= ChargeWeapon;
        inputController.OnWeaponDrop -= UnequipWeapon;
        inputController.OnPrimaryFire -= UseWeapon;
        inputController.OnInfoDisplay -= ShowBackPackInfoScreen;
    }

    public void Initialize()
    {
        isInitialized = true;
        GetComponent<Collider>().enabled = false;

    }

    // Method to add an item to the backpack (excluding weapons)
    public bool AddItem(BaseItem item)
    {
        if (!IsFull)
        {
            item.gameObject.transform.position = Vector3.zero;
            item.gameObject.SetActive(false);

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

    //   Method to Use Weapon for attacking
    public void UseWeapon()
    {
        if(weaponSlot != null)
        {
            weaponSlot.Attack();
        }
    }

    //   Method to Charge Weapon for stronger attacks
    public void ChargeWeapon(Weapon weapon)
    {
        // Weapon Charging implementation
        if(!weapon.isCharged)
        {
            weapon.Charge();
        }
    }

    // Method to equip a weapon in the weapon slot
    public bool EquipWeapon(Weapon newWeapon)
    {
        if (weaponSlot == null)
        {
            weaponSlot = newWeapon;

            newWeapon.Initialize();
            newWeapon.RefreshState(true);

            OnWeaponEquip?.Invoke(newWeapon);

            string msg = $"Equipped weapon: {newWeapon.ItemName}";
            NotificationManager.Instance.ShowNotification(msg);

            return true;
        }
        else
        {
            string msg = $"A weapon is already equipped. Unequip it first.";
            NotificationManager.Instance.ShowNotification(msg);

            return false;
        }
    }

    // Method to unequip the currently equipped weapon
    public void UnequipWeapon(Weapon weapon)
    {
        if (weaponSlot != null)
        {
            weaponSlot = null;
            weapon.RefreshState(false);

            OnWeaponUnequip?.Invoke(weapon);

            string msg = $"Unequipped weapon: {weapon.ItemName}";
            NotificationManager.Instance.ShowNotification(msg);

            ProcessDroppedItem(weapon, false);
        }
        else
        {
            Debug.LogWarning("No weapon is currently equipped.");
        }
    }

    // Display Backpack Info Screen
    void ShowBackPackInfoScreen(bool arg)
    {
        if (isInitialized)
        {
            OnShowBackpackInfo?.Invoke(arg);
        }
    }

    private void ProcessDroppedItem(BaseItem item, bool isUsed)
    {
        if (item.ItemType == ItemType.WeaponItem)
        {
            item.gameObject.SetActive(true);
            item.gameObject.transform.parent = null;
        }
        else if(isUsed)
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
}
