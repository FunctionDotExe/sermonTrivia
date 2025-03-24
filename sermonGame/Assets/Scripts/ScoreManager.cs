using UnityEngine;
using TMPro; // Required for TextMeshPro components

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ScoreManager");
                    _instance = obj.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }

    private int currentScore = 0;
    public TMP_Text scoreText; // Use TMP_Text for TextMeshPro

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreDisplay(); // Call to update the UI
    }

    public void AddTime(int time) // Add this method
    {
        // Implement your logic for adding time here
        Debug.Log($"Time added: {time}");
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore; // Update the text component
        }
        else
        {
            Debug.LogError("Score text is not assigned!"); // Error log if scoreText is not assigned
        }
    }
}