using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] GameObject avatar; // The avatar GameObject to rotate
    [SerializeField] Animator avatarAnimator;
    [SerializeField] InputController inputController;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpForce = 8.0f;
    [SerializeField] float gravity = 20.0f;

    private CharacterController characterController;
    private PlayerMovementState currentMovementState;

    public Action<GameObject> OnInteract;
    public Action<PlayerMovementState> OnPlayerMovementStateChanged;

    // Use this method for initialization
    public void Initialize()
    {
        inputController = GetComponent<InputController>();
        if (inputController == null)
        {
            inputController = FindObjectOfType<InputController>();
        }

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on the same GameObject.");
        }

        // ChangeMovementState(PlayerMovementState.Idle);
    }

    private void OnEnable()
    {
        // Subscribe to the movement events when the component is enabled
        if (inputController != null)
        {
            inputController.OnHorizontalMovement += HandleHorizontalMovement;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the events when the component is disabled
        if (inputController != null)
        {
            inputController.OnHorizontalMovement -= HandleHorizontalMovement;
        }
    }

    private void HandleHorizontalMovement(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            Vector3 moveDirection = transform.right * moveHorizontal + transform.forward * moveVertical;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Adjust avatar's rotation
            if (avatar != null)
            {
                Vector3 lookDirection = avatar.transform.position + moveDirection;
                avatar.transform.LookAt(lookDirection, Vector3.up);
            }

            if (currentMovementState != PlayerMovementState.Walking)
            {
                ChangeMovementState(PlayerMovementState.Walking);
            }
        }
        else
        {
            if (currentMovementState != PlayerMovementState.Idle)
            {
                ChangeMovementState(PlayerMovementState.Idle);
            }
        }
    }

    private void ChangeMovementState(PlayerMovementState newState)
    {
        currentMovementState = newState;
        Debug.Log("Player movement state changed to: " + currentMovementState);

        switch (currentMovementState)
        {
            case PlayerMovementState.Idle:
                avatarAnimator.SetTrigger("idle");
                break;
            case PlayerMovementState.Walking:
                avatarAnimator.SetTrigger("walk");
                break;
        }

        OnPlayerMovementStateChanged?.Invoke(currentMovementState);
    }
}
