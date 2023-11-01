using UnityEngine;
using System;
// using System.Numerics; // Import the System namespace for the Action delegate

public class Player : MonoBehaviour
{
    [SerializeField] GameObject backpackHolder;
    private PlayerMovementController playerController;
    private Backpack backpack; // Reference to the player's backpack
    public Backpack BackPack => backpack;
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
        playerController = GetComponent<PlayerMovementController>();

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
        backpack = backpackHolder.GetComponent<Backpack>();
        if (backpack == null)
        {
            Debug.LogError("Backpack component not found on the same GameObject.");
        }

        ChangeState(PlayerState.Active);
    }

    private void OnEnable()
    {
        playerController.OnInteract += PickupItemInteractionHandler;
    }

    private void OnDisable()
    {
        playerController.OnInteract -= PickupItemInteractionHandler;

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
        // NotificationManager.Instance.ShowNotification("Press 'F' to interact!");

        ItemInterestPoint interestPoint = other.gameObject.GetComponent<ItemInterestPoint>();
        if (interestPoint != null && interestPoint.InterestPointType == ItemInterestPointType.Distributor)
        {
            interestPoint.OnItemSpawn += PickupItemInteractionHandler;
        }
        if (interestPoint != null && interestPoint.InterestPointType == ItemInterestPointType.Collector)
        {
            interestPoint.OnItemCollect += DropItemInteractionHandler;
        }
    }

    void OnTriggerExit(Collider other)
    {
        ItemInterestPoint interestPoint = other.gameObject.GetComponent<ItemInterestPoint>();
        if (interestPoint != null && interestPoint.InterestPointType == ItemInterestPointType.Distributor)
        {
            interestPoint.OnItemSpawn -= PickupItemInteractionHandler;
        }
        if (interestPoint != null && interestPoint.InterestPointType == ItemInterestPointType.Collector)
        {
            interestPoint.OnItemCollect -= DropItemInteractionHandler;
        }
    }

    private void PickupItemInteractionHandler(GameObject obj)
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

    private void DropItemInteractionHandler()
    {
        if (backpack != null)
        {
            Debug.Log($"Item drop request");
            backpack.RemoveItem();
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
                case ItemType.DeliveryItem:
                    // Attempt to pick up the item and stack it to the backpack
                    bool itemPicked = backpack.AddItem(item, true);
                    if (itemPicked)
                    {
                        string msg = $"Picked up item: {item.ItemName}";
                        NotificationManager.Instance.ShowNotification(msg);
                    }
                    else
                    {
                        string msg = $"Backpack is full. Cannot pick up item: {item.ItemName}";
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
