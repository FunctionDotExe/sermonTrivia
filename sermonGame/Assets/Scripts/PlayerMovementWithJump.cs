using UnityEngine;

public class PlayerMovementWithJump : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    public float rotationSpeed = 2f;
    public Transform cameraTransform;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Add CharacterController if it doesn't exist
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            Debug.Log("Added CharacterController to player");
        }

        // Get camera reference if not set
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
        }
    }
    
    void Update()
    {
        if (cameraTransform == null) return;

        // Check if grounded
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
        
        // Get input (controller and keyboard)
        float horizontalInput = Input.GetAxis("RightStickHorizontal");
        float verticalInput = Input.GetAxis("RightStickVertical");
        
        // Fallback to keyboard input if no controller input
        if (horizontalInput == 0) horizontalInput = Input.GetAxis("Horizontal");
        if (verticalInput == 0) verticalInput = Input.GetAxis("Vertical");
        
        // Get camera forward and right
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        // Project vectors onto the horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        // Create movement vector relative to camera orientation
        Vector3 movement = (right * horizontalInput + forward * verticalInput).normalized;
        
        // Apply movement
        if (movement != Vector3.zero)
        {
            controller.Move(movement * moveSpeed * Time.deltaTime);
            
            // Smoothly rotate to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
            Debug.Log("Jump!");
        }
        
        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;
        
        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }
} 