using UnityEngine;

public interface IScrollViewListItem
{
    public GameObject SelectionIndicator { get; }

    public void ActivateSelectionIndicator(bool arg);
}