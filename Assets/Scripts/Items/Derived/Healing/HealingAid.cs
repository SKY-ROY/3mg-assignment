using UnityEngine;

public class HealingAid : BaseItem
{
    [SerializeField] HealingAidType healingAidType;

    // Use this method for initialization
    public override void Initialize()
    {
        base.Initialize(); // Call the base class's Initialize method
    }

    // Armor pack-specific functionality (e.g., applying armor)
    public void Use()
    {
        Debug.Log($"Using {ItemName}. of type: {healingAidType}");
        // Implement armor pack-specific functionality here (e.g., applying armor)
    }
}