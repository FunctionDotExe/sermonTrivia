using UnityEngine;

public class KeyboardButtonTrigger : MonoBehaviour
{
    private NumberButtonHandler[] buttonHandlers;
    
    void Start()
    {
        buttonHandlers = FindObjectsOfType<NumberButtonHandler>();
        Debug.Log($"KeyboardButtonTrigger: Found {buttonHandlers.Length} buttons");
    }
    
    void Update()
    {
        // Number keys 1-9 will trigger corresponding buttons
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                Debug.Log($"KeyboardButtonTrigger: Key {i+1} pressed");
                
                // Find button with matching value
                foreach (var handler in buttonHandlers)
                {
                    if (handler != null && handler.buttonValue == i+1)
                    {
                        Debug.Log($"KeyboardButtonTrigger: Triggering button with value {i+1}");
                        handler.HandleButtonClick();
                        return;
                    }
                }
            }
        }
    }
} 