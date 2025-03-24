using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StarTriviaSetup : MonoBehaviour
{
    [System.Serializable]
    public class TriviaQuestion
    {
        public string question;
        public string[] answers = new string[4];
        public int correctAnswerIndex;
    }
    
    public TriviaQuestion[] triviaQuestions = new TriviaQuestion[18];
    public GameObject starPrefab;
    
    void Awake()
    {
        // This script is just for setup, not needed at runtime
        enabled = false;
    }
    
#if UNITY_EDITOR
    [ContextMenu("Setup All Stars")]
    public void SetupAllStars()
    {
        // Check if star prefab is assigned
        if (starPrefab == null)
        {
            Debug.LogError("Star prefab is not assigned! Please assign a prefab to the starPrefab field.");
            return;
        }
        
        // Check if trivia questions are set up
        if (triviaQuestions == null)
        {
            Debug.LogError("Trivia questions array is null!");
            return;
        }
        
        Debug.Log("Setting up " + triviaQuestions.Length + " stars");
        
        // Check if questions are initialized
        int initializedCount = 0;
        for (int i = 0; i < triviaQuestions.Length; i++)
        {
            if (triviaQuestions[i] != null && !string.IsNullOrEmpty(triviaQuestions[i].question))
            {
                initializedCount++;
            }
        }
        
        Debug.Log("Found " + initializedCount + " initialized trivia questions");
        
        if (initializedCount == 0)
        {
            Debug.LogError("No initialized trivia questions found! Make sure SermonOnTheMountTrivia script has run.");
            return;
        }
        
        // Clear existing stars
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        
        // Create new stars
        int createdStars = 0;
        for (int i = 0; i < triviaQuestions.Length; i++)
        {
            if (triviaQuestions[i] != null && !string.IsNullOrEmpty(triviaQuestions[i].question))
            {
                if (CreateStar(triviaQuestions[i], i))
                {
                    createdStars++;
                }
            }
            else
            {
                Debug.LogWarning("Trivia question at index " + i + " is null or empty. Skipping this star.");
            }
        }
        
        Debug.Log("Successfully created " + createdStars + " stars with trivia questions");
    }
    
    bool CreateStar(TriviaQuestion question, int index)
    {
        if (starPrefab == null)
        {
            Debug.LogError("Star prefab is not assigned!");
            return false;
        }
        
        Debug.Log("Creating star " + index + " for question: " + question.question);
        
        // Create star using standard Instantiate
        GameObject star = null;
        try {
            star = Instantiate(starPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Star " + index + " instantiated successfully");
        }
        catch (System.Exception e) {
            Debug.LogError("Error instantiating star prefab: " + e.Message);
            return false;
        }
        
        if (star == null)
        {
            Debug.LogError("Failed to instantiate star prefab!");
            return false;
        }
        
        star.transform.SetParent(transform);
        
        // Position star (you can customize this)
        float angle = (360f / triviaQuestions.Length) * index;
        float radius = 20f; // Adjust based on your scene
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        star.transform.position = new Vector3(x, 1.5f, z);
        
        // Set up trivia
        StarTriviaInteraction trivia = star.GetComponent<StarTriviaInteraction>();
        if (trivia != null)
        {
            trivia.triviaQuestion = question.question;
            trivia.answerOptions = new string[question.answers.Length];
            for (int i = 0; i < question.answers.Length; i++)
            {
                trivia.answerOptions[i] = question.answers[i];
            }
            trivia.correctAnswerIndex = question.correctAnswerIndex;
            
            // Name the star for easy identification
            star.name = "Star_" + (index + 1) + "_" + question.question.Substring(0, Mathf.Min(20, question.question.Length));
            Debug.Log("Successfully configured star " + index);
            return true;
        }
        else
        {
            Debug.LogError("Star prefab does not have a StarTriviaInteraction component! Make sure to add the StarTriviaInteraction script to your prefab.");
            DestroyImmediate(star);
            return false;
        }
    }
#endif
} 