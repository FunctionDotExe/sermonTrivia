using UnityEngine;
using Cinemachine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    
    [Header("Camera Settings")]
    public float lookSensitivity = 0.5f;
    public float controllerLookSensitivity = 25f;
    public Transform cameraTarget;
    
    // Components
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private CinemachineVirtualCamera virtualCamera;
    private float cameraRotationX = 0f;
    
    void Start()
    {
        // Get or add CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
        }
        
        // Find camera target if not assigned
        if (cameraTarget == null)
        {
            // Create a camera target if it doesn't exist
            GameObject targetObj = new GameObject("CameraTarget");
            targetObj.transform.SetParent(transform);
            targetObj.transform.localPosition = new Vector3(0, 1.6f, 0); // Head height
            cameraTarget = targetObj.transform;
        }
        
        // Find virtual camera
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = cameraTarget;
            virtualCamera.LookAt = cameraTarget;
        }
        else
        {
            Debug.LogError("No Cinemachine Virtual Camera found in scene!");
        }
    }
    
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
    }
    
    void HandleMovement()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Get input from keyboard or controller
        float horizontalInput = Input.GetAxis("Horizontal"); // Works with controller left stick
        float verticalInput = Input.GetAxis("Vertical");     // Works with controller left stick
        
        // Create movement vector relative to player orientation
        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        
        // Apply movement
        controller.Move(movement * moveSpeed * Time.deltaTime);
        
        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void HandleRotation()
    {
        // Get input from mouse or controller right stick
        float mouseX = Input.GetAxis("Mouse X") + Input.GetAxis("RightStickHorizontal");
        float mouseY = Input.GetAxis("Mouse Y") + Input.GetAxis("RightStickVertical");
        
        // Apply controller sensitivity if using controller
        if (Mathf.Abs(Input.GetAxis("RightStickHorizontal")) > 0.1f || 
            Mathf.Abs(Input.GetAxis("RightStickVertical")) > 0.1f)
        {
            mouseX *= controllerLookSensitivity * Time.deltaTime;
            mouseY *= controllerLookSensitivity * Time.deltaTime;
        }
        else
        {
            mouseX *= lookSensitivity;
            mouseY *= lookSensitivity;
        }
        
        // Rotate player horizontally
        transform.Rotate(Vector3.up, mouseX);
        
        // Rotate camera vertically
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f);
        cameraTarget.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
    
    void HandleJump()
    {
        // Jump with space or controller button
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("JoystickJump")) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
        }
    }
} 