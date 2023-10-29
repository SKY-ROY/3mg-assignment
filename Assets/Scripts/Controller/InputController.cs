using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Define events for various actions
    public event Action<float> OnMoveForward;
    public event Action<float> OnMoveBackward;
    public event Action<float> OnMoveRight;
    public event Action<float> OnMoveLeft;
    public event Action OnJump;
    public event Action<GameObject> OnInteract;
    public event Action<bool> OnInfoDisplay;
    public event Action<BaseItem> OnItemPick;
    public event Action<BaseItem> OnItemUse;
    public event Action<BaseItem> OnItemDrop;
    public event Action<Weapon> OnWeaponCharge;
    public event Action<Weapon> OnWeaponDrop;
    public event Action OnPrimaryFire;
    public event Action<float> OnRotateHorizontal;

    [SerializeField] float rotationSensitivity = 2.0f; // Adjust this value to control the rotation speed


    void OnEnable()
    {
        ItemLayoutHandler.OnItemUseRequest += OnItemUseRequestHandler;
        ItemLayoutHandler.OnItemDropRequest += OnItemDropRequestHandler;
    }

    void OnDisable()
    {
        ItemLayoutHandler.OnItemUseRequest += OnItemUseRequestHandler;
        ItemLayoutHandler.OnItemDropRequest += OnItemDropRequestHandler;
    }

    private void Update()
    {
        // Get input values for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check for movement events
        if (verticalInput > 0)
        {
            OnMoveForward?.Invoke(verticalInput);
        }
        else if (verticalInput < 0)
        {
            OnMoveBackward?.Invoke(-verticalInput); // Pass positive value for backward movement
        }

        if (horizontalInput > 0)
        {
            OnMoveRight?.Invoke(horizontalInput);
        }
        else if (horizontalInput < 0)
        {
            OnMoveLeft?.Invoke(-horizontalInput); // Pass positive value for left movement
        }

        // Check for jump event
        if (Input.GetButtonDown("Jump"))
        {
            OnJump?.Invoke();
        }

        if (Input.GetButtonDown("Info"))
        {
            OnInfoDisplay?.Invoke(true);
        }

        if (Input.GetButtonUp("Info"))
        {
            OnInfoDisplay?.Invoke(false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            OnPrimaryFire?.Invoke();
        }

        // Check for horizontal mouse input to control player rotation
        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0)
        {
            // Calculate the rotation angle based on mouse input
            float rotationAngle = mouseX * rotationSensitivity;
            OnRotateHorizontal?.Invoke(rotationAngle);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // implement Pickup with interact action
        if (Input.GetButton("Interact"))
        {
            OnInteract?.Invoke(other.gameObject);
        }
    }

    void OnItemPickRequestHandler()
    {
        OnItemPick?.Invoke(null);
    }

    void OnItemUseRequestHandler(ItemLayoutHandler layoutHandler)
    {
        if (layoutHandler.Item.ItemType == ItemType.HealingItem)
        {
            Debug.Log($"Item Use request Handled for {layoutHandler}");
            OnItemUse?.Invoke(layoutHandler.Item);
        }
        else
        {
            Debug.Log($"Weapon Charge request Handled for {layoutHandler}");
            OnWeaponCharge?.Invoke((Weapon)layoutHandler.Item);
        }

    }

    void OnItemDropRequestHandler(ItemLayoutHandler layoutHandler)
    {
        if (layoutHandler.Item.ItemType == ItemType.HealingItem)
        {
            Debug.Log($"Item Drop request Handled for {layoutHandler}");
            OnItemDrop?.Invoke(layoutHandler.Item);
        }
        else
        {
            Debug.Log($"Weapon Drop request Handled for {layoutHandler}");
            OnWeaponDrop?.Invoke((Weapon)layoutHandler.Item);
        }
    }
}
