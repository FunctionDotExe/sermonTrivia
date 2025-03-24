using UnityEngine;
using UnityEngine.UI;

public class ButtonPositionFixer : MonoBehaviour
{
    public RectTransform buttonPanel;
    public RectTransform[] buttonRectTransforms;
    
    void Start()
    {
        // Find the button panel if not assigned
        if (buttonPanel == null)
        {
            GameObject panel = GameObject.Find("ButtonPanel");
            if (panel != null)
            {
                buttonPanel = panel.GetComponent<RectTransform>();
            }
        }
        
        // Find all buttons if not assigned
        if (buttonRectTransforms == null || buttonRectTransforms.Length == 0)
        {
            NumberButtonHandler[] handlers = FindObjectsOfType<NumberButtonHandler>();
            buttonRectTransforms = new RectTransform[handlers.Length];
            for (int i = 0; i < handlers.Length; i++)
            {
                buttonRectTransforms[i] = handlers[i].GetComponent<RectTransform>();
            }
        }
        
        // Reposition buttons to the center of the panel
        RepositionButtons();
    }
    
    void RepositionButtons()
    {
        if (buttonPanel == null || buttonRectTransforms == null || buttonRectTransforms.Length == 0)
        {
            Debug.LogError("ButtonPositionFixer: Missing button panel or buttons!");
            return;
        }
        
        // Calculate the center of the panel
        Vector2 panelCenter = new Vector2(0, 0); // Local coordinates center
        
        // Calculate button layout
        int numButtons = buttonRectTransforms.Length;
        float totalWidth = numButtons * 220; // 200px button width + 20px spacing
        float startX = -totalWidth / 2 + 110; // Center the first button
        
        Debug.Log($"ButtonPositionFixer: Repositioning {numButtons} buttons");
        Debug.Log($"ButtonPositionFixer: Panel center: {panelCenter}, Start X: {startX}");
        
        // Position buttons in a row at the center of the panel
        for (int i = 0; i < numButtons; i++)
        {
            RectTransform rectTransform = buttonRectTransforms[i];
            if (rectTransform != null)
            {
                // Set anchors to center
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                
                // Position the button
                float xPos = startX + i * 220;
                rectTransform.anchoredPosition = new Vector2(xPos, 0);
                
                // Make sure the button is the right size
                rectTransform.sizeDelta = new Vector2(200, 200);
                
                Debug.Log($"ButtonPositionFixer: Button {i} positioned at {rectTransform.anchoredPosition}");
                
                // Make sure the button is active and interactable
                Button button = rectTransform.GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = true;
                }
                rectTransform.gameObject.SetActive(true);
            }
        }
        
        // Force layout update
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonPanel);
    }
} 