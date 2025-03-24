using UnityEngine;
using UnityEngine.UI;

public class DirectButtonHandler : MonoBehaviour
{
    private SimpleMultiplayerManager manager;
    private NumberButtonHandler[] buttonHandlers;
    private RectTransform[] buttonRectTransforms;
    private Camera uiCamera;

    void Start()
    {
        manager = FindObjectOfType<SimpleMultiplayerManager>();
        buttonHandlers = FindObjectsOfType<NumberButtonHandler>();
        
        // Cache the RectTransforms
        buttonRectTransforms = new RectTransform[buttonHandlers.Length];
        for (int i = 0; i < buttonHandlers.Length; i++)
        {
            buttonRectTransforms[i] = buttonHandlers[i].GetComponent<RectTransform>();
        }
        
        // Find the UI camera (if using WorldSpace or ScreenSpaceCamera)
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }
        
        Debug.Log($"DirectButtonHandler initialized with {buttonHandlers.Length} buttons");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log($"DirectButtonHandler: Mouse clicked at {mousePos}");
            
            // Check each button
            for (int i = 0; i < buttonHandlers.Length; i++)
            {
                if (buttonHandlers[i] != null && buttonRectTransforms[i] != null)
                {
                    // Check if mouse is over this button
                    if (RectTransformUtility.RectangleContainsScreenPoint(buttonRectTransforms[i], mousePos, uiCamera))
                    {
                        Debug.Log($"DirectButtonHandler: Hit on button {buttonHandlers[i].buttonIndex}!");
                        buttonHandlers[i].HandleButtonClick();
                        return;
                    }
                }
            }
            
            // If we got here, no button was hit
            Debug.Log("DirectButtonHandler: No button hit");
            
            // Debug the button positions relative to the click
            for (int i = 0; i < buttonHandlers.Length; i++)
            {
                if (buttonHandlers[i] != null && buttonRectTransforms[i] != null)
                {
                    Vector3[] corners = new Vector3[4];
                    buttonRectTransforms[i].GetWorldCorners(corners);
                    
                    bool xInRange = mousePos.x >= corners[0].x && mousePos.x <= corners[2].x;
                    bool yInRange = mousePos.y >= corners[0].y && mousePos.y <= corners[2].y;
                    
                    Debug.Log($"Button {buttonHandlers[i].buttonIndex}: X in range: {xInRange}, Y in range: {yInRange}");
                    Debug.Log($"Button {buttonHandlers[i].buttonIndex} corners: {corners[0]}, {corners[1]}, {corners[2]}, {corners[3]}");
                }
            }
        }
    }
} 