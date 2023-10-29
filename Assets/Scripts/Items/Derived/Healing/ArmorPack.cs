using UnityEngine;

public class ArmorPack : HealingAid
{
    // Additional properties specific to armor packs
    public int armorAmount;

    // Use this method for initialization
    public override void Initialize()
    {
        base.Initialize(); // Call the base class's Initialize method

        // Initialize armor pack-specific properties here
        armorAmount = 10; // Default armor amount
    }

    // Armor pack-specific functionality (e.g., applying armor)
    public void Use()
    {
        Debug.Log("Using " + ItemName + ". Armor amount: " + armorAmount);
        // Implement armor pack-specific functionality here (e.g., applying armor)
    }
}
