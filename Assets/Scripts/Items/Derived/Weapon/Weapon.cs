using UnityEngine;
using System.Collections;

public class Weapon : BaseItem
{
    // Additional properties specific to weapons
    public int damage;
    public float movementSpeedPenalty;
    public bool isCharged;
    public float chargeDuration;

    // Use this method for initialization
    public override void Initialize()
    {
        base.Initialize(); // Call the base class's Initialize method

        // Initialize weapon-specific properties here        
        
        isCharged = false;
    }

    // Weapon-specific functionality (e.g., attacking)
    public void Attack()
    {
        int dmg = damage;
        if(isCharged)
        {
            dmg *= 2;
        }
        string msg = $"Attacking with {ItemName} inflicting {dmg} Damage.";
        NotificationManager.Instance.ShowNotification(msg);
        // Implement weapon-specific attack logic here
    }

    public void Charge()
    {
        StartCoroutine(ReleaseCharge(chargeDuration));
        string msg = $"Charged weapon to deal twice Damage for {chargeDuration} seconds.";
        NotificationManager.Instance.ShowNotification(msg);
    }

    public void RefreshState(bool isEquipped)
    {
        GetComponent<Collider>().enabled = !isEquipped;
    }

    IEnumerator ReleaseCharge(float duration)
    {
        isCharged=true;
        yield return new WaitForSeconds(duration);

        isCharged=false;
    }
}
