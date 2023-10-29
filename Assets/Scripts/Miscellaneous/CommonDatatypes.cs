// PlayerStates.cs

using UnityEngine;

public enum PlayerState
{
    Initializing,
    Active,
    Inactive,
    None
}

public enum ItemType
{
    WeaponItem,
    HealingItem,
}

public enum HealingAidType
{
    HealthPack,
    ArmorPack,
}

public enum WeaponType
{
    MeleeWeapon,
    RangeWeapon
}

[System.Serializable]
public class EnvironmentObject
{
    public GameObject spawnObject;
    public int count;
}
