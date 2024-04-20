using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
   
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public Transform cameraTransform; // Reference to the camera's transform

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Get input from the horizontal and vertical axes (keyboard or controller)
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        // Transform input vector into world space relative to the camera orientation
        Vector3 move = cameraTransform.forward * inputDirection.z + cameraTransform.right * inputDirection.x;
        move.y = 0; // Ensure movement is strictly horizontal

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Rotate towards the movement direction using the look rotation vector
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Jump mechanics
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(0.3f * 3.0f * -3.0f);
        }

        // Apply gravity
        playerVelocity.y += -3.0f * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
