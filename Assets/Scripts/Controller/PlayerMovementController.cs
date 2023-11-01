using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] InputController inputController;
    private CharacterController characterController;
    private Transform cameraTransform;

    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;

    private PlayerMovementState currentMovementState;
    private Vector3 moveDirection = Vector3.zero;

    public Action<GameObject> OnInteract;
    public Action<PlayerMovementState> OnPlayerMovementStateChanged;

    public GameObject avatar; // The avatar GameObject to rotate
    [SerializeField] private Animator animator;


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

        // if (avatar != null)
        // {
        //     animator = avatar.GetComponent<Animator>();
        // }

        // Assuming the camera is a child of the player or follows the player's rotation
        cameraTransform = Camera.main.transform;

        // ChangeMovementState(PlayerMovementState.Idle);
    }

    private void OnEnable()
    {
        // Subscribe to the movement events when the component is enabled
        if (inputController != null)
        {
            inputController.OnHorizontalMovement += HandleHorizontalMovement;
            inputController.OnJump += HandleJump;
            inputController.OnInteract += HandleInteract;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the events when the component is disabled
        if (inputController != null)
        {
            inputController.OnHorizontalMovement -= HandleHorizontalMovement;
            inputController.OnJump -= HandleJump;
            inputController.OnInteract -= HandleInteract;
        }
    }

    private void Update()
    {
        // Apply gravity to the character
        //if (characterController.isGrounded)
        //{
        //    moveDirection.y = 0; // Reset vertical velocity if grounded
        //}
        //else
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
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

    private void HandleJump()
    {
        Debug.Log("Jump Input Received");
        Debug.Log($"grounded: {characterController.isGrounded}");
        // Check if the character is grounded before jumping
        if (characterController.isGrounded)
        {
            // Apply jump force to the character
            moveDirection.y = jumpForce;
        }
    }

    private void HandleInteract(GameObject obj)
    {
        // Implement your interact logic here
        Debug.Log("Player is interacting with an item.");
        OnInteract?.Invoke(obj);
    }

    private void ChangeMovementState(PlayerMovementState newState)
    {
        currentMovementState = newState;
        Debug.Log("Player movement state changed to: " + currentMovementState);

        switch (currentMovementState)
        {
            case PlayerMovementState.Idle:
                animator.SetTrigger("idle");
                break;
            case PlayerMovementState.Walking:
                animator.SetTrigger("walk");
                break;
        }

        OnPlayerMovementStateChanged?.Invoke(currentMovementState);
    }
}
