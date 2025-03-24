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
            buttonPanel = GetComponent<RectTransform>();
        }
        
        // Find all buttons if not assigned
        if (buttonRectTransforms == null || buttonRectTransforms.Length == 0)
        {
            NumberButtonHandler[] handlers = GetComponentsInChildren<NumberButtonHandler>();
            buttonRectTransforms = new RectTransform[handlers.Length];
            for (int i = 0; i < handlers.Length; i++)
            {
                buttonRectTransforms[i] = handlers[i].GetComponent<RectTransform>();
            }
        }
        
        // Ensure the Canvas is properly set up
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1;
        }
        
        // Position the buttons
        RepositionButtons();
    }
    
    void RepositionButtons()
    {
        if (buttonRectTransforms == null || buttonRectTransforms.Length == 0)
        {
            Debug.LogError("No buttons found to position!");
            return;
        }
        
        float spacing = 220f;
        float totalWidth = (buttonRectTransforms.Length - 1) * spacing;
        float startX = -totalWidth / 2;
        
        for (int i = 0; i < buttonRectTransforms.Length; i++)
        {
            RectTransform rectTransform = buttonRectTransforms[i];
            if (rectTransform != null)
            {
                // Set anchors to center
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                
                // Position the button
                float xPos = startX + i * spacing;
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