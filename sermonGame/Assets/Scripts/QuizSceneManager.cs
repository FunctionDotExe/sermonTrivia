using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizSceneManager : MonoBehaviour
{
    public Button returnButton;
    public string mainSceneName = "DemoScene";
    
    void Start()
    {
        // Unlock cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Setup return button
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnToMainScene);
        }
        else
        {
            Debug.LogWarning("Return button not assigned in QuizSceneManager!");
        }
    }
    
    public void ReturnToMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
} 