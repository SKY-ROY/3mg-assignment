using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLayoutHandler : MonoBehaviour//, IScrollViewListItem
{
    BaseItem item;
    public BaseItem Item { get => item; }

    [SerializeField] TMP_Text itemNameText;
    [SerializeField] Image itemIconImage;
    [SerializeField] Button useItemButton;
    [SerializeField] Button dropItemButton;

    public static Action<ItemLayoutHandler> OnItemUseRequest;
    public static Action<ItemLayoutHandler> OnItemDropRequest;

    public void RefreshLayoutData(BaseItem pickedItem)
    {
        item = pickedItem;
        itemNameText.text = item.ItemName;
        itemIconImage.sprite = item.ItemIcon;

        useItemButton.onClick.AddListener(OnClickUseItemHandler);
        dropItemButton.onClick.AddListener(OnClickDropItemHandler);
    }

    private void OnClickUseItemHandler()
    {
        OnItemUseRequest?.Invoke(this);
    }

    private void OnClickDropItemHandler()
    {
        Debug.Log("Drop request initiated");
        OnItemDropRequest?.Invoke(this);
    }
}
