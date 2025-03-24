using UnityEngine;

public class MouseCameraControl : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    
    private float xRotation = 0f;
    
    void Start()
    {
        // Find player if not assigned
        if (playerBody == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerBody = player.transform;
            }
            else
            {
                Debug.LogError("No player found! Please tag your player as 'Player'");
            }
        }
        
        // Optional: Lock cursor for FPS style
        // Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        if (playerBody == null) return;
        
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Rotate player left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
} 