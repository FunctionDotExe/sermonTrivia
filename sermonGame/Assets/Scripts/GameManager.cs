using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    
    private float gameTimer = 300f; // 5 minutes
    private int score = 0;
    private bool isGameActive = true;
    private int starsCollected = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find UI elements if not assigned
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
            if (scoreText == null)
            {
                Debug.LogError("Score Text not found! Please create a UI Text element named 'ScoreText'");
            }
        }
        
        if (timerText == null)
        {
            timerText = GameObject.Find("TimerText")?.GetComponent<TextMeshProUGUI>();
        }
        
        UpdateUI();
        Debug.Log("GameManager started. Initial score: " + score);
    }

    void Update()
    {
        if (isGameActive)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                EndGame();
            }
            UpdateUI();
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log($"Score changed: {score - points} -> {score} (+{points})");
        UpdateUI();
    }
    
    public void StarCollected()
    {
        starsCollected++;
        Debug.Log($"Star collected! Total stars: {starsCollected}");
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("Score Text is null, can't update UI");
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer / 60f);
            int seconds = Mathf.FloorToInt(gameTimer % 60f);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    void EndGame()
    {
        isGameActive = false;
        Debug.Log($"Game Over! Final Score: {score}, Stars Collected: {starsCollected}");
        // You can add game over UI or restart functionality here
    }
} 