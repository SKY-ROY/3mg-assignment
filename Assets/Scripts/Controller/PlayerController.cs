using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputController inputController;
    private CharacterController characterController;
    private Transform cameraTransform;

    public float moveSpeed = 5.0f;
    public float rotationSpeed = 120.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    public Action<GameObject> OnInteract;

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

        // Assuming the camera is a child of the player or follows the player's rotation
        cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        // Subscribe to the movement events when the component is enabled
        if (inputController != null)
        {
            inputController.OnMoveForward += HandleMoveForward;
            inputController.OnMoveBackward += HandleMoveBackward;
            inputController.OnMoveRight += HandleMoveRight;
            inputController.OnMoveLeft += HandleMoveLeft;
            inputController.OnJump += HandleJump;
            inputController.OnInteract += HandleInteract;
            inputController.OnRotateHorizontal += HandleRotateHorizontal;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the events when the component is disabled
        if (inputController != null)
        {
            inputController.OnMoveForward -= HandleMoveForward;
            inputController.OnMoveBackward -= HandleMoveBackward;
            inputController.OnMoveRight -= HandleMoveRight;
            inputController.OnMoveLeft -= HandleMoveLeft;
            inputController.OnJump -= HandleJump;
            inputController.OnInteract -= HandleInteract;
            inputController.OnRotateHorizontal -= HandleRotateHorizontal;
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

    private void HandleMoveForward(float inputValue)
    {
        // Calculate the movement vector based on the input
        Vector3 moveDirection = cameraTransform.forward;
        moveDirection.y = 0; // Ignore vertical movement
        moveDirection.Normalize();

        // Move the character
        characterController.Move(moveDirection * inputValue * moveSpeed * Time.deltaTime);
    }

    private void HandleMoveBackward(float inputValue)
    {
        // Calculate the movement vector based on the input
        Vector3 moveDirection = -cameraTransform.forward;
        moveDirection.y = 0; // Ignore vertical movement
        moveDirection.Normalize();

        // Move the character
        characterController.Move(moveDirection * inputValue * moveSpeed * Time.deltaTime);
    }

    private void HandleMoveRight(float inputValue)
    {
        // Calculate the movement vector based on the input
        Vector3 moveDirection = cameraTransform.right;
        moveDirection.y = 0; // Ignore vertical movement
        moveDirection.Normalize();

        // Move the character
        characterController.Move(moveDirection * inputValue * moveSpeed * Time.deltaTime);
    }

    private void HandleMoveLeft(float inputValue)
    {
        // Calculate the movement vector based on the input
        Vector3 moveDirection = -cameraTransform.right;
        moveDirection.y = 0; // Ignore vertical movement
        moveDirection.Normalize();

        // Move the character
        characterController.Move(moveDirection * inputValue * moveSpeed * Time.deltaTime);
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

    private void HandleRotateHorizontal(float rotationAngle)
    {
        // Calculate the rotation based on the rotation angle and speed
        Vector3 rotation = Vector3.up * rotationAngle * rotationSpeed * Time.deltaTime;

        // Apply the rotation to the player
        transform.Rotate(rotation);
    }
}
