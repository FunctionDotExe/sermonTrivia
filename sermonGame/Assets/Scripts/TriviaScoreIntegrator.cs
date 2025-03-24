using UnityEngine;

public class TriviaScoreIntegrator : MonoBehaviour
{
    private SimpleMultiplayerManager gameManager;
    private SermonOnTheMountTrivia triviaManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<SimpleMultiplayerManager>();
        triviaManager = FindObjectOfType<SermonOnTheMountTrivia>();
        
        if (gameManager == null)
        {
            Debug.LogError("TriviaScoreIntegrator: SimpleMultiplayerManager not found!");
        }
        
        if (triviaManager == null)
        {
            Debug.LogError("TriviaScoreIntegrator: SermonOnTheMountTrivia not found!");
        }
    }
    
    // Call this method from your SermonOnTheMountTrivia script when a correct answer is given
    public void AwardPointsForCorrectAnswer(int points)
    {
        if (gameManager != null)
        {
            gameManager.AddPoints(points);
            Debug.Log($"TriviaScoreIntegrator: Awarded {points} points for correct answer");
        }
    }
} 