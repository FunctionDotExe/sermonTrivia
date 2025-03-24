using UnityEngine;

public class ScoreHelper : MonoBehaviour
{
    // Static method that can be called from anywhere
    public static void AwardPoints(int points)
    {
        // Use the ScoreManager to add points
        ScoreManager.Instance.AddPoints(points);
        Debug.Log($"ScoreHelper: Awarded {points} points");
    }
    
    // Add a method for time bonuses
    public static void AwardTime(float seconds)
    {
        // Convert float seconds to int before adding
        int timeInSeconds = Mathf.RoundToInt(seconds);
        // Use the ScoreManager to add time
        ScoreManager.Instance.AddTime(timeInSeconds);
        Debug.Log($"ScoreHelper: Awarded {seconds} seconds");
    }
} 