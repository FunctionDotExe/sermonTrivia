using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 300f; // 5 minutes in seconds
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    public Button mainMenuButton;
    
    private bool isGameOver = false;
    private ScoreManager scoreManager;
    
    void Start()
    {
        // Find the ScoreManager
        scoreManager = ScoreManager.Instance;
        
        // Setup buttons if they exist
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
        
        // Hide game over panel at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    void Update()
    {
        if (isGameOver) return;
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            GameOver();
        }
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    void GameOver()
    {
        isGameOver = true;
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            // Update final score text
            if (finalScoreText != null && scoreManager != null)
            {
                finalScoreText.text = $"Final Score: {scoreManager.GetScore()}";
            }
        }
        
        // Pause the game
        Time.timeScale = 0;
    }
    
    void RestartGame()
    {
        // Reset time scale
        Time.timeScale = 1;
        
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void ReturnToMainMenu()
    {
        // Reset time scale
        Time.timeScale = 1;
        
        // Load main menu scene
        SceneManager.LoadScene("MainMenu");
    }
} 