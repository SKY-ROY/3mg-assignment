using UnityEngine;
using System;
// using System.Numerics; // Import the System namespace for the Action delegate

public class Player : MonoBehaviour
{
    [SerializeField] GameObject backpackHolder;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject itemHolder;

    private PlayerController playerController;
    private Backpack backpack; // Reference to the player's backpack

    // Use camelCase naming convention for variables
    private PlayerState currentState;

    // Event to notify about player state changes
    public event Action<PlayerState> OnPlayerStateChanged;

    private void Awake()
    {
        // Initialize player variables and components
        Initialize();
    }

    private void Initialize()
    {
        ChangeState(PlayerState.Initializing);

        // Get the PlayerController and PlayerController components
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found on the same GameObject.");
        }
        else
        {
            // Initialize the PlayerController with the InputController
            playerController.Initialize();
        }

        // Find and initialize the player's backpack
        // backpack = GetComponent<Backpack>();
        // if (backpack == null)
        // {
        //     Debug.LogError("Backpack component not found on the same GameObject.");
        // }

        ChangeState(PlayerState.Active);
    }

    private void OnEnable()
    {
        // playerController.OnInteract += ItemInteractionHandler;
    }

    private void OnDisable()
    {
        // playerController.OnInteract -= ItemInteractionHandler;

        // Transition to the Inactive state
        ChangeState(PlayerState.Inactive);
    }

    // Method to change the player's state
    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
        Debug.Log("Player state changed to: " + newState);

        // Notify about the state change through the event
        OnPlayerStateChanged?.Invoke(newState);
    }

    void OnTriggerEnter(Collider other)
    {
        NotificationManager.Instance.ShowNotification("Press 'F' to interact!");

        ItemDistributor itemDistributor = other.gameObject.GetComponent<ItemDistributor>();
        if (itemDistributor != null)
        {
            itemDistributor.OnItemSpawn += ItemDistributionSpawnHandler;
        }
    }

    void OnTriggerExit(Collider other)
    {
        ItemDistributor itemDistributor = other.gameObject.GetComponent<ItemDistributor>();
        if (itemDistributor != null)
        {
            itemDistributor.OnItemSpawn -= ItemDistributionSpawnHandler;
        }
    }

    private void ItemDistributionSpawnHandler()
    {
        Debug.Log($"Item Spawn Recorded");
    }

    private void ItemInteractionHandler(GameObject obj)
    {
        if (backpack != null)
        {
            PickItem(obj);
        }
        else
        {
            PickBackPack(obj);
        }
    }

    void PickItem(GameObject pickUpObject)
    {
        // Check if the collided object has a BaseItem component
        BaseItem item = pickUpObject.GetComponent<BaseItem>();
        if (item != null)
        {
            switch (item.ItemType)
            {
                case ItemType.HealingItem:
                    // Attempt to pick up the item and add it to the backpack
                    bool pickedUp = backpack.AddItem(item);
                    if (pickedUp)
                    {
                        string msg = $"Picked up item: {item.ItemName}";
                        NotificationManager.Instance.ShowNotification(msg);

                        // You can add more logic here, such as playing a sound, disabling the item in the scene, etc.
                        SetPickedObjectOrientation(pickUpObject, itemHolder.transform);
                    }
                    else
                    {
                        string msg = $"Backpack is full. Cannot pick up item: {item.ItemName}";
                        NotificationManager.Instance.ShowNotification(msg);
                    }
                    break;
                case ItemType.WeaponItem:
                    bool equipped = backpack.EquipWeapon((Weapon)item);
                    if (equipped)
                    {
                        SetPickedObjectOrientation(pickUpObject, weaponHolder.transform);
                    }
                    else
                    {
                        string msg = $"Weapon already Equipped. Cannot equip weapon: {item.ItemName}";
                        NotificationManager.Instance.ShowNotification(msg);
                    }
                    break;
            }
        }
    }

    void PickBackPack(GameObject pickUpObject)
    {
        Backpack pickBackPack = pickUpObject.GetComponent<Backpack>();

        if (pickBackPack != null)
        {
            backpack = pickBackPack;
            backpack.Initialize();
            SetPickedObjectOrientation(pickUpObject, backpackHolder.transform);
        }
    }

    void SetPickedObjectOrientation(GameObject pickedUpObject, Transform parent)
    {
        pickedUpObject.gameObject.transform.parent = parent;
        pickedUpObject.transform.localPosition = Vector3.zero;
        pickedUpObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
}
