// PlayerStates.cs

using UnityEngine;

public enum PlayerState
{
    Initializing,
    Active,
    Inactive,
    None
}

public enum PlayerMovementState
{
    Idle,
    Walking,
}

public enum ItemType
{
    WeaponItem,
    HealingItem,
    DeliveryItem
}

public enum ItemInterestPointType
{
    Distributor,
    Collector
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

public enum InputDirection
{
    Up,
    Right,
    Down,
    Left
}
