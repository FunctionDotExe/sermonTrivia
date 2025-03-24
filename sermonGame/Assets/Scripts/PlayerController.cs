using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    public float mouseSensitivity = 2f;
    public float rotationSpeed = 100f;
    
    [Header("View Bobbing")]
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.1f;
    
    [Header("Interaction")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    
    // Private variables
    private Rigidbody rb;
    private Camera playerCamera;
    private float defaultCameraY;
    private float bobTimer = 0f;
    private float verticalLookRotation;
    
    private GameObject currentInteractable;
    
    private SimpleMultiplayerManager multiplayerManager;
    
    private CharacterController characterController;
    
    // Reference to the main camera
    private Camera mainCamera;
    // Reference to the player's body that should rotate
    public Transform playerBody;
    // Reference to the camera transform
    private Transform cameraTransform;
    // Original camera position and rotation
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    
    void Awake()
    {
        // Find the multiplayer manager
        multiplayerManager = FindObjectOfType<SimpleMultiplayerManager>();
        
        // Disable controller by default (will be enabled by multiplayer manager)
        enabled = false;
    }
    
    void Start()
    {
        // Try to get either CharacterController or Rigidbody
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
        // Log which movement system we're using
        if (characterController != null)
        {
            Debug.Log("Using CharacterController for movement");
        }
        else if (rb != null)
        {
            Debug.Log("Using Rigidbody for movement");
        }
        else
        {
            Debug.LogWarning("No CharacterController or Rigidbody found on player!");
        }
        
        // Get components
        playerCamera = GetComponentInChildren<Camera>();
        
        if (playerCamera != null)
        {
            defaultCameraY = playerCamera.transform.localPosition.y;
        }
        else
        {
            Debug.LogError("No camera found as child of player!");
        }
        
        // Get the main camera
        mainCamera = Camera.main;
        
        // If no player body is assigned, use this transform
        if (playerBody == null)
        {
            playerBody = transform;
        }
        
        // Find and store the camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            originalCameraPosition = cameraTransform.position;
            originalCameraRotation = cameraTransform.rotation;
            
            // Detach camera from player if it's a child
            if (cameraTransform.IsChildOf(transform))
            {
                cameraTransform.SetParent(null);
                Debug.Log("Detached camera from player");
            }
        }
        
        // Make sure cursor is visible and not locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void Update()
    {
        // Direct keyboard input
        float horizontal = 0f;
        float vertical = 0f;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            vertical = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            vertical = -1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            horizontal = 1f;
        
        // Calculate movement in world space
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        
        // Move the player object
        transform.position += movement * moveSpeed * Time.deltaTime;
        
        // Optional: Rotate player to face movement direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
        
        // Reset camera position and rotation each frame to prevent it from moving with the player
        if (cameraTransform != null)
        {
            cameraTransform.position = originalCameraPosition;
            cameraTransform.rotation = originalCameraRotation;
        }
        
        // Debug output
        if (movement.magnitude > 0.1f)
        {
            Debug.Log($"Moving player: {movement}, Position: {transform.position}");
        }
        
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate player (horizontal)
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate camera (vertical)
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl))
        {
            // Apply upward velocity directly
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
        
        // Check for interactions
        CheckForInteractables();
        
        // Handle interaction input
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            // Just trigger any interaction component
            currentInteractable.SendMessage("TriggerInteraction", SendMessageOptions.DontRequireReceiver);
        }
        
        // Apply view bobbing
        ApplyHeadBob();
    }
    
    void FixedUpdate()
    {
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        // Calculate movement direction
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection = moveDirection.normalized;
        
        // Apply movement - preserve Y velocity for jumping
        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        
        // Apply movement
        rb.velocity = targetVelocity;
    }
    
    void ApplyHeadBob()
    {
        // Get movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Only bob when moving
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Increment timer
            bobTimer += Time.deltaTime * bobbingSpeed;
            
            // Calculate bob amount
            float bobY = Mathf.Sin(bobTimer) * bobbingAmount;
            
            // Apply to camera
            Vector3 newPos = playerCamera.transform.localPosition;
            newPos.y = defaultCameraY + bobY;
            playerCamera.transform.localPosition = newPos;
        }
        else
        {
            // Reset when not moving
            bobTimer = 0;
            
            // Return to default position
            Vector3 newPos = playerCamera.transform.localPosition;
            newPos.y = defaultCameraY;
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                newPos,
                Time.deltaTime * 5f);
        }
    }
    
    void CheckForInteractables()
    {
        // Reset current interactable
        currentInteractable = null;
        
        // Cast a ray to find interactable objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayer))
        {
            // Check if we hit an object with an interaction component
            if (hit.collider.gameObject.GetComponent<MonoBehaviour>() != null)
            {
                currentInteractable = hit.collider.gameObject;
                
                // You can add UI to show interaction prompt here
                // For example: interactionPrompt.SetActive(true);
            }
        }
    }
    
    // Add this method to enable/disable player movement
    public void EnableMovement(bool enable)
    {
        // Implement this based on your movement system
        // For example:
        // movementEnabled = enable;
    }
    
    // Call this when points are earned
    public void AddPoints(int points)
    {
        // Add points to the current player's score
        if (multiplayerManager != null)
        {
            multiplayerManager.AddPoints(points);
        }
    }
    
    void OnDisable()
    {
        // Make sure camera is reset when script is disabled
        if (cameraTransform != null)
        {
            cameraTransform.position = originalCameraPosition;
            cameraTransform.rotation = originalCameraRotation;
        }
    }
}