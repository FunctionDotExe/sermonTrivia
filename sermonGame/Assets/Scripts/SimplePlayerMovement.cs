using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public Transform cameraTransform; // Reference to the camera
    
    private void Start()
    {
        // If no camera transform is assigned, try to find the main camera
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

        // Get input
        float horizontalInput = Input.GetAxis("RightStickHorizontal");
        float verticalInput = Input.GetAxis("RightStickVertical");
        
        // Add keyboard input as fallback
        if (horizontalInput == 0) horizontalInput = Input.GetAxis("Horizontal");
        if (verticalInput == 0) verticalInput = Input.GetAxis("Vertical");
        
        // Create movement vector relative to camera direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        // Zero out the y component to keep movement horizontal
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        // Calculate movement direction
        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized;
        
        // Apply movement
        if (movement != Vector3.zero)
        {
            transform.position += movement * moveSpeed * Time.deltaTime;
            
            // Smoothly rotate to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
} 