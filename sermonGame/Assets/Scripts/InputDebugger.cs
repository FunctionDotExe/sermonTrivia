using UnityEngine;

public class InputDebugger : MonoBehaviour
{
    void Update()
    {
        // Check for keyboard input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            Debug.Log($"Input detected - Horizontal: {horizontal}, Vertical: {vertical}");
        }
        
        // Check for any key press
        if (Input.anyKeyDown)
        {
            Debug.Log("Key pressed: " + Input.inputString);
        }
    }
} 