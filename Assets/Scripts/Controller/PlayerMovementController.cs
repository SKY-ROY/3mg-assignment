using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // The speed at which the player moves
    public GameObject avatar; // The avatar GameObject to rotate

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, transform.position.y, moveVertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            Vector3 moveDirection = transform.right * moveHorizontal + transform.forward * moveVertical;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Adjust avatar's rotation
            if (avatar != null)
            {
                Vector3 lookDirection = transform.position + moveDirection;
                avatar.transform.LookAt(lookDirection, Vector3.up);
            }
        }
    }
}
