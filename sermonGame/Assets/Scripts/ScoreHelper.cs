using UnityEngine;

public class ScoreHelper : MonoBehaviour
{
    // Static method that can be called from anywhere
    public static void AwardPoints(int points)
    {
        ScoreTracker.Instance.AddPoints(points);
        Debug.Log($"ScoreHelper: Awarded {points} points");
    }
} 