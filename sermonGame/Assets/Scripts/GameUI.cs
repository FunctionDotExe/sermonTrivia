using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currentPlayerText;
    
    // Timer value that we'll update manually
    private float timer = 0f;

    void Update()
    {
        // Update timer manually
        timer += Time.deltaTime;
        
        // Update UI elements
        if (timerText != null)
        {
            timerText.text = $"Time: {timer:F1}";
        }
        
        // Update score text (placeholder)
        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }
        
        // Update current player text (placeholder)
        if (currentPlayerText != null)
        {
            currentPlayerText.text = "Player 1";
        }
    }
} 