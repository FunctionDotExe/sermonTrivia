using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    // Update is called once per frame
    void Update()
    {
        // Get input
        float horizontalInput = 0;
        float verticalInput = 0;
        
        // Check for WASD or arrow key input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) verticalInput = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) verticalInput = -1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1;
        
        // Create movement vector
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        
        // Apply movement
        transform.position += movement * moveSpeed * Time.deltaTime;
        
        // Optional: Rotate to face movement direction
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }
} 