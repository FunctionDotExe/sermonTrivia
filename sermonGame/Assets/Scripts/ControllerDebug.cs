using UnityEngine;
using UnityEngine.UI;

public class ControllerDebug : MonoBehaviour
{
    public Text debugText;

    void Update()
    {
        string text = "Controller Debug:\n";
        
        // Check all possible axes (0-20)
        for (int i = 0; i < 20; i++)
        {
            try {
                float axisValue = Input.GetAxis("Joystick Axis " + i);
                if (Mathf.Abs(axisValue) > 0.1f)
                {
                    text += $"Axis {i}: {axisValue}\n";
                }
            } catch {}
        }
        
        // Check all possible buttons (0-20)
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKey("joystick button " + i))
            {
                text += $"Button {i} pressed\n";
            }
        }
        
        if (debugText != null)
        {
            debugText.text = text;
        }
        else
        {
            Debug.Log(text);
        }
    }
} 