using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RaycastDebugger : MonoBehaviour
{
    private static List<RaycastResult> raycastResults = new List<RaycastResult>();
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked at: " + Input.mousePosition);
            
            // Check what's under the cursor
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            
            EventSystem.current.RaycastAll(eventData, raycastResults);
            
            if (raycastResults.Count == 0)
            {
                Debug.Log("No UI elements found under cursor!");
            }
            else
            {
                Debug.Log($"Found {raycastResults.Count} UI elements under cursor:");
                foreach (var result in raycastResults)
                {
                    Debug.Log($"Hit: {result.gameObject.name} (Layer: {result.gameObject.layer})");
                }
            }
        }
    }
} 