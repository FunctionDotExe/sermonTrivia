using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Button testButton;
    public GameObject testPanel;
    
    void Start()
    {
        Debug.Log("ButtonTest script started");
        
        if (testButton == null)
        {
            Debug.LogError("Test button is not assigned!");
            return;
        }
        
        if (testPanel == null)
        {
            Debug.LogError("Test panel is not assigned!");
            return;
        }
        
        // Make sure panel is visible
        testPanel.SetActive(true);
        
        // Add click listener
        testButton.onClick.AddListener(() => {
            Debug.Log("Button clicked!");
            testPanel.SetActive(false);
        });
        
        Debug.Log("Button listener added");
    }
    
    void Update()
    {
        // Check for mouse clicks
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked at: " + Input.mousePosition);
        }
        
        // Check for keyboard input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");
            testPanel.SetActive(!testPanel.activeSelf);
        }
    }
} 