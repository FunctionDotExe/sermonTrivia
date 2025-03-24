using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DirectButtonHandler : MonoBehaviour
{
    private Button[] mysteryButtons;
    
    void Start()
    {
        mysteryButtons = GetComponentsInChildren<Button>();
        
        // Setup mystery buttons
        for (int i = 0; i < mysteryButtons.Length; i++)
        {
            int buttonIndex = i; // Capture for lambda
            mysteryButtons[i].onClick.AddListener(() => OnMysteryButtonClick(buttonIndex));
            
            // Also handle number keys
            if (i < 4) // Only for first 4 buttons
            {
                int keyIndex = i + 1; // For keys 1-4
                mysteryButtons[i].GetComponentInChildren<Text>().text = $"Mystery {keyIndex}";
            }
        }
    }
    
    void Update()
    {
        // Handle numeric key inputs (1-4)
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                OnMysteryButtonClick(i);
            }
        }
    }
    
    void OnMysteryButtonClick(int buttonIndex)
    {
        if (MultiplayerGameManager.Instance != null)
        {
            MultiplayerGameManager.Instance.StartPlayerTurn(buttonIndex);
        }
    }
} 