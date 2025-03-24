using UnityEngine;
using UnityEngine.UI;

public class ScoreIntegrator : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private int currentScore = 0;
    
    // Singleton pattern
    private static ScoreIntegrator _instance;
    public static ScoreIntegrator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreIntegrator>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ScoreIntegrator");
                    _instance = obj.AddComponent<ScoreIntegrator>();
                }
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        // Find score text if not assigned
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
            if (scoreText == null)
            {
                Debug.LogWarning("ScoreIntegrator: Score text not found!");
            }
        }
        
        UpdateScoreDisplay();
    }
    
    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        Debug.Log($"ScoreIntegrator: Added {points} points. New score: {currentScore}");
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
} 