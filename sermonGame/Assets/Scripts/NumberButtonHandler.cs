using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberButtonHandler : MonoBehaviour
{
    public int buttonIndex;
    [HideInInspector]
    public int buttonValue = 1; // Store the value directly
    
    private SimpleMultiplayerManager manager;
    private TextMeshProUGUI buttonText;
    
    void Start()
    {
        // Find the manager
        manager = FindObjectOfType<SimpleMultiplayerManager>();
        if (manager == null)
        {
            Debug.LogError("SimpleMultiplayerManager not found!");
            return;
        }
        
        // Get the button component
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found!");
            return;
        }
        
        // Get the text component
        EnsureTextComponentExists();
        
        // Set initial text
        if (buttonText != null)
        {
            buttonText.text = buttonValue.ToString();
        }
        
        // Add click listener
        button.onClick.AddListener(() => {
            Debug.Log($"Button {buttonIndex} clicked via NumberButtonHandler!");
            
            if (manager != null)
            {
                manager.ButtonClickedByHandler(buttonIndex, buttonValue);
            }
        });
        
        Debug.Log($"NumberButtonHandler set up for button {buttonIndex}");
    }
    
    // This can be called by SimpleMultiplayerManager to update the button text
    public void SetButtonValue(int value)
    {
        buttonValue = value;
        
        // Ensure text component exists
        EnsureTextComponentExists();
        
        if (buttonText != null)
        {
            buttonText.text = value.ToString();
            Debug.Log($"Button {buttonIndex} value set to {value}");
        }
        else
        {
            Debug.LogError($"Button {buttonIndex} text component is null when trying to set value {value}");
        }
    }
    
    // Helper method to ensure the text component exists
    private void EnsureTextComponentExists()
    {
        if (buttonText == null)
        {
            // First try to find it in children
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            
            // If still null, try to find a regular Text component
            if (buttonText == null)
            {
                Text legacyText = GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    Debug.LogWarning($"Button {buttonIndex} is using legacy Text component. Converting to TextMeshProUGUI...");
                    
                    // Create a TextMeshProUGUI component
                    GameObject textObj = legacyText.gameObject;
                    string originalText = legacyText.text;
                    Color originalColor = legacyText.color;
                    Font originalFont = legacyText.font;
                    int originalFontSize = legacyText.fontSize;
                    TextAnchor originalAlignment = legacyText.alignment;
                    
                    // Destroy the legacy component
                    DestroyImmediate(legacyText);
                    
                    // Add TextMeshProUGUI component
                    buttonText = textObj.AddComponent<TextMeshProUGUI>();
                    buttonText.text = originalText;
                    buttonText.color = originalColor;
                    buttonText.fontSize = originalFontSize;
                    
                    // Set alignment (conversion from TextAnchor to TextAlignmentOptions)
                    switch (originalAlignment)
                    {
                        case TextAnchor.UpperLeft:
                            buttonText.alignment = TextAlignmentOptions.TopLeft;
                            break;
                        case TextAnchor.UpperCenter:
                            buttonText.alignment = TextAlignmentOptions.Top;
                            break;
                        case TextAnchor.UpperRight:
                            buttonText.alignment = TextAlignmentOptions.TopRight;
                            break;
                        case TextAnchor.MiddleLeft:
                            buttonText.alignment = TextAlignmentOptions.Left;
                            break;
                        case TextAnchor.MiddleCenter:
                            buttonText.alignment = TextAlignmentOptions.Center;
                            break;
                        case TextAnchor.MiddleRight:
                            buttonText.alignment = TextAlignmentOptions.Right;
                            break;
                        case TextAnchor.LowerLeft:
                            buttonText.alignment = TextAlignmentOptions.BottomLeft;
                            break;
                        case TextAnchor.LowerCenter:
                            buttonText.alignment = TextAlignmentOptions.Bottom;
                            break;
                        case TextAnchor.LowerRight:
                            buttonText.alignment = TextAlignmentOptions.BottomRight;
                            break;
                    }
                }
                else
                {
                    // If no text component exists, create one
                    Debug.LogWarning($"Button {buttonIndex} has no text component. Creating one...");
                    
                    // Create a new GameObject for the text
                    GameObject textObj = new GameObject("ButtonText");
                    textObj.transform.SetParent(transform);
                    textObj.transform.localPosition = Vector3.zero;
                    textObj.transform.localScale = Vector3.one;
                    
                    // Add RectTransform component
                    RectTransform rectTransform = textObj.AddComponent<RectTransform>();
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                    
                    // Add TextMeshProUGUI component
                    buttonText = textObj.AddComponent<TextMeshProUGUI>();
                    buttonText.text = buttonValue.ToString();
                    buttonText.color = Color.black;
                    buttonText.fontSize = 36;
                    buttonText.alignment = TextAlignmentOptions.Center;
                }
            }
            
            if (buttonText != null)
            {
                Debug.Log($"Text component found/created for button {buttonIndex}");
            }
            else
            {
                Debug.LogError($"Failed to find or create text component for button {buttonIndex}");
            }
        }
    }

    void OnEnable()
    {
        // Make sure we have a Button component
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError($"Button component missing on {gameObject.name}. Adding one.");
            button = gameObject.AddComponent<Button>();
        }
        
        // Make sure the button has an onClick listener
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(HandleButtonClick);
        
        // Make sure the button is interactable
        button.interactable = true;
        
        Debug.Log($"Button {buttonIndex} enabled and configured with onClick listener");
    }

    // Add this method to handle button clicks
    public void HandleButtonClick()
    {
        Debug.Log($"Button {buttonIndex} clicked with value {buttonValue}");
        
        // Get the manager if we don't have it yet
        if (manager == null)
        {
            manager = FindObjectOfType<SimpleMultiplayerManager>();
        }
        
        // Call the manager's method to handle the button click
        if (manager != null)
        {
            manager.HandleNumberButtonClick(buttonIndex, buttonValue);
        }
        else
        {
            Debug.LogError("Could not find SimpleMultiplayerManager in the scene!");
        }
    }

    // Add this method to check for direct clicks on this button
    void Update()
    {
        // Check for direct clicks on this button
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            RectTransform rectTransform = GetComponent<RectTransform>();
            
            if (rectTransform != null)
            {
                // Convert mouse position to local position
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform,
                    mousePosition,
                    null,
                    out Vector2 localPoint))
                {
                    // Check if point is inside rect
                    if (rectTransform.rect.Contains(localPoint))
                    {
                        Debug.Log($"Direct hit on Button {buttonIndex}!");
                        HandleButtonClick();
                    }
                }
            }
        }
    }
} 