using UnityEngine;

public class RangeWeapon : Weapon
{
    public float reloadDuration; // Renamed from attackSpeed
    public float fireRate; // Fire rate in shots per second
    public float damageFallOffRange; // Range at which damage falls off

    public override void Initialize()
    {
        base.Initialize(); // Call the base class's Initialize method

        // Initialize weapon-specific properties here
        damage = 10; // Default damage value
        reloadDuration = 1.0f; // Default reload duration
        fireRate = 2.0f; // Default fire rate (2 shots per second)
        damageFallOffRange = 10.0f; // Default damage fall-off range (10 units)
        movementSpeedPenalty = 30.0f;
    }

    // Weapon-specific functionality (e.g., attacking)
    public void Attack()
    {
        Debug.Log("Attacking with " + ItemName + ". Damage: " + damage);
        // Implement weapon-specific attack logic here
    }
}