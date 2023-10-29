using UnityEngine;

public class HealthPack : HealingAid
{
    // Additional properties specific to health packs
    public int healingAmount;

    // Use this method for initialization
    public override void Initialize()
    {
        base.Initialize(); // Call the base class's Initialize method

        // Initialize health pack-specific properties here
        healingAmount = 20; // Default healing amount
    }

    // Health pack-specific functionality (e.g., healing)
    public void Use()
    {
        Debug.Log("Using " + ItemName + ". Healing amount: " + healingAmount);
        // Implement health pack-specific functionality here (e.g., healing logic)
    }
}
