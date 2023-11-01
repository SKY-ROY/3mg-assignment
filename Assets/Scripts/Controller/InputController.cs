using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] bool useKeyboard = false;
    [SerializeField] VariableJoystick variableJoystick;

    // Define events for various actions
    public Action<float, float> OnHorizontalMovement;
    public event Action OnJump;
    public event Action<bool> OnInfoDisplay;
    public event Action<BaseItem> OnItemPick;
    public event Action<BaseItem> OnItemUse;
    public event Action<BaseItem> OnItemDrop;
    public event Action OnPrimaryFire;

    private void Update()
    {
        // Get input values for movement
        float horizontalInput = useKeyboard ? Input.GetAxis("Horizontal") : variableJoystick.Horizontal;
        float verticalInput = useKeyboard ? Input.GetAxis("Vertical") : variableJoystick.Vertical;

        // Debug.Log($"Movement Input Captured. ({horizontalInput}, {verticalInput})");
        OnHorizontalMovement?.Invoke(horizontalInput, verticalInput);

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
    }
}
