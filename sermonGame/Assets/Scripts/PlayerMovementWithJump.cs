using UnityEngine;

public class PlayerMovementWithJump : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    
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
    }
    
    void Update()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
        
        // Get input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Get camera forward and right
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        
        // Project vectors onto the horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        // Create movement vector relative to camera orientation
        Vector3 movement = right * horizontalInput + forward * verticalInput;
        
        // Apply movement
        controller.Move(movement * moveSpeed * Time.deltaTime);
        
        // Rotate player to face movement direction
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
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