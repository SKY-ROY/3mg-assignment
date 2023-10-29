using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackVisualizer : MonoBehaviour
{
    [SerializeField] ScrollViewLayoutHandler itemViewLayoutHandler;
    [SerializeField] ScrollViewLayoutHandler weaponViewLayoutHandler;
    [SerializeField] GameObject itemViewPrefab;
    [SerializeField] GameObject weaponViewPrefab;
    [SerializeField] GameObject backPackInfoPanel;
    // List<ItemLayoutHandler> itemLayoutHandlers;
    Dictionary<int, ItemLayoutHandler> itemLayoutHandlers;

    void Awake()
    {
        // itemLayoutHandlers = new List<ItemLayoutHandler>();
        itemLayoutHandlers = new Dictionary<int, ItemLayoutHandler>();
    }

    void OnEnable()
    {
        Backpack.OnItemAdd += AddItemVisually;
        Backpack.OnItemUse += UseItemVisually;
        Backpack.OnItemRemove += RemoveItemVisually;
        Backpack.OnWeaponEquip += EquipWeaponVisually;
        Backpack.OnWeaponUnequip += UnequipWeaponVisually;
        Backpack.OnShowBackpackInfo += ShowBackPackInfoScreen;
    }

    void OnDisable()
    {
        Backpack.OnItemAdd -= AddItemVisually;
        Backpack.OnItemUse -= UseItemVisually;
        Backpack.OnItemRemove -= RemoveItemVisually;
        Backpack.OnWeaponEquip -= EquipWeaponVisually;
        Backpack.OnWeaponUnequip -= UnequipWeaponVisually;
        Backpack.OnShowBackpackInfo -= ShowBackPackInfoScreen;
    }

    private void AddItemVisually(BaseItem item)
    {
        GameObject obj = itemViewLayoutHandler.PopulateScrollViewList(itemViewPrefab);

        ItemLayoutHandler currentItemLayout = obj.GetComponent<ItemLayoutHandler>();

        if (currentItemLayout != null)
        {
            item.AssignedListingIndex = currentItemLayout.gameObject.GetInstanceID();

            itemLayoutHandlers.Add(item.AssignedListingIndex, currentItemLayout);
            currentItemLayout.RefreshLayoutData(item);
        }
    }

    private void UseItemVisually(BaseItem item)
    {
        // play any fanfare effects
    }

    private void RemoveItemVisually(BaseItem item)
    {
        GameObject obj = itemLayoutHandlers[item.AssignedListingIndex].gameObject;
        itemLayoutHandlers.Remove(item.AssignedListingIndex);
        itemViewLayoutHandler.RemoveElementFromScollViewList(obj);
    }

    private void EquipWeaponVisually(Weapon weapon)
    {
        GameObject obj = weaponViewLayoutHandler.PopulateScrollViewList(weaponViewPrefab);

        ItemLayoutHandler currentWeaponLayout = obj.GetComponent<ItemLayoutHandler>();

        if (currentWeaponLayout != null)
        {
            weapon.AssignedListingIndex = currentWeaponLayout.gameObject.GetInstanceID();

            itemLayoutHandlers.Add(weapon.AssignedListingIndex, currentWeaponLayout);
            currentWeaponLayout.RefreshLayoutData(weapon);
        }
    }

    private void UnequipWeaponVisually(Weapon weapon)
    {
        GameObject obj = itemLayoutHandlers[weapon.AssignedListingIndex].gameObject;
        itemLayoutHandlers.Remove(weapon.AssignedListingIndex);
        itemViewLayoutHandler.RemoveElementFromScollViewList(obj);
    }

    private void ShowBackPackInfoScreen(bool arg)
    {
        backPackInfoPanel.SetActive(arg);
    }
}
