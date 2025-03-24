using UnityEngine;

public class FixedCameraController : MonoBehaviour
{
    // Original position and rotation
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    // Camera component
    private Camera cameraComponent;
    
    void Awake()
    {
        // Store original transform
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        
        // Get camera component
        cameraComponent = GetComponent<Camera>();
        
        // Make this the main camera
        if (cameraComponent != null)
        {
            cameraComponent.tag = "MainCamera";
        }
        
        // Ensure this camera is not a child of any other object
        transform.SetParent(null);
        
        Debug.Log("Fixed Camera Controller initialized");
    }
    
    void LateUpdate()
    {
        // Force camera back to original position and rotation every frame
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
} 