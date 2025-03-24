using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizPlaceholder : MonoBehaviour
{
    public Button returnButton;
    
    void Start()
    {
        // Set up the return button
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnToMainScene);
        }
    }
    
    public void ReturnToMainScene()
    {
        // Return to the main scene
        SceneManager.LoadScene("DemoScene");
    }
} 