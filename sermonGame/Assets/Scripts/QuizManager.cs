using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI[] answerTexts;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI scoreText;
    public GameObject questionPanel; // Parent panel containing questions
    public GameObject completionPanel; // Panel to show when quiz is complete
    public TextMeshProUGUI completionText; // Text to show final results
    public Button returnButton; // Button to return to main scene
    
    [Header("Quiz Settings")]
    public int pointsForCorrectAnswer = 10;
    public int pointsForWrongAnswer = -15;
    public float timeBonus = 30f;
    public float timePenalty = -15f;
    public bool allowKeyboardInput = true;
    public string mainSceneName = "DemoScene"; // Name of your main scene
    
    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private QuizQuestion currentQuestion;
    private int currentQuestionIndex = 0;
    private bool canAnswer = true;
    private int currentScore = 0;
    private int correctAnswers = 0;
    private int totalQuestionsAnswered = 0;
    
    // Static variables to preserve state between scene loads
    private static bool isReturningFromQuiz = false;
    private static Vector3 playerPosition;
    private static Quaternion playerRotation;
    
    void Start()
    {
        // Initialize quiz questions
        InitializeQuestions();
        
        // Setup UI
        SetupButtons();
        
        // Store player position if not already returning from quiz
        if (!isReturningFromQuiz && GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerPosition = player.transform.position;
            playerRotation = player.transform.rotation;
        }
        
        // Hide completion panel initially
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Completion panel is not assigned!");
        }
        
        // Make sure question panel is active
        if (questionPanel != null)
        {
            questionPanel.SetActive(true);
        }
        
        // Setup return button
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnToMainScene);
        }
        
        // Get current score from GameManager if available
        if (GameManager.Instance != null)
        {
            PlayerData currentPlayer = GameManager.Instance.GetCurrentPlayer();
            if (currentPlayer != null)
            {
                currentScore = currentPlayer.score;
                UpdateScoreDisplay();
            }
        }
        
        // Show first question
        ShowQuestion(0);
        
        // Make cursor visible in quiz scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void Update()
    {
        // Check for keyboard input if enabled and can answer
        if (allowKeyboardInput && canAnswer)
        {
            // Check for number keys 1-4 (both main keyboard and numpad)
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SelectAnswer(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SelectAnswer(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SelectAnswer(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                SelectAnswer(3);
            }
            
            // Allow returning with Escape key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToMainScene();
            }
        }
    }
    
    void InitializeQuestions()
    {
        // Add some sample questions
        questions.Add(new QuizQuestion(
            "What is the capital of France?",
            new string[] { "Paris", "London", "Berlin", "Rome" },
            0 // Correct answer index
        ));
        
        questions.Add(new QuizQuestion(
            "Which planet is known as the Red Planet?",
            new string[] { "Venus", "Mars", "Jupiter", "Saturn" },
            1 // Correct answer index
        ));
        
        questions.Add(new QuizQuestion(
            "What is the largest mammal?",
            new string[] { "Elephant", "Giraffe", "Blue Whale", "Hippopotamus" },
            2 // Correct answer index
        ));
        
        // Add more questions as needed
    }
    
    void SetupButtons()
    {
        // Setup answer button click events
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }
    }
    
    void ShowQuestion(int index)
    {
        if (index < 0 || index >= questions.Count)
        {
            // No more questions, show completion panel
            ShowCompletionPanel();
            return;
        }
        
        // Reset can answer flag
        canAnswer = true;
        
        // Set current question
        currentQuestionIndex = index;
        currentQuestion = questions[index];
        
        // Update UI
        questionText.text = currentQuestion.question;
        
        // Set answer texts
        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                answerTexts[i].text = (i+1) + ". " + currentQuestion.answers[i]; // Add number prefix
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].interactable = true;
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
        
        // Clear feedback
        feedbackText.text = "";
    }
    
    void SelectAnswer(int answerIndex)
    {
        // Ignore if we can't answer or index is out of range
        if (!canAnswer || answerIndex >= answerButtons.Length || !answerButtons[answerIndex].gameObject.activeSelf)
            return;
        
        // Prevent multiple answers
        canAnswer = false;
        
        // Increment total questions answered
        totalQuestionsAnswered++;
        
        // Check if answer is correct
        bool isCorrect = (answerIndex == currentQuestion.correctAnswerIndex);
        
        if (isCorrect)
        {
            // Increment correct answers
            correctAnswers++;
            
            // Correct answer
            feedbackText.text = "Correct! +10 points";
            feedbackText.color = Color.green;
            
            // Add points locally
            currentScore += pointsForCorrectAnswer;
            
            // Add points and time bonus to GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPoints(pointsForCorrectAnswer);
                GameManager.Instance.AddTime(timeBonus);
            }
        }
        else
        {
            // Wrong answer
            feedbackText.text = "Wrong! -15 points. The correct answer is: " + 
                currentQuestion.answers[currentQuestion.correctAnswerIndex];
            feedbackText.color = Color.red;
            
            // Deduct points locally
            currentScore += pointsForWrongAnswer; // This will subtract 15 points
            
            // Apply point deduction and time penalty to GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPoints(pointsForWrongAnswer);
                GameManager.Instance.AddTime(timePenalty);
            }
        }
        
        // Update score display
        UpdateScoreDisplay();
        
        // Disable buttons temporarily
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }
        
        // Wait and show next question or return
        Invoke("ShowNextQuestion", 2f);
    }
    
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
    
    void ShowNextQuestion()
    {
        // Show next question
        ShowQuestion(currentQuestionIndex + 1);
    }
    
    void ShowCompletionPanel()
    {
        Debug.Log("Showing completion panel");
        
        // Hide question panel
        if (questionPanel != null)
        {
            questionPanel.SetActive(false);
            Debug.Log("Question panel hidden");
        }
        else if (questionText != null && questionText.transform.parent != null)
        {
            questionText.transform.parent.gameObject.SetActive(false);
            Debug.Log("Question parent hidden");
        }
        
        // Show completion panel
        if (completionPanel != null)
        {
            // Force panel to front
            completionPanel.transform.SetAsLastSibling();
            completionPanel.SetActive(true);
            Debug.Log("Completion panel activated");
            
            // Update completion text
            if (completionText != null)
            {
                string resultText = "Quiz Complete!\n\n";
                resultText += "Correct Answers: " + correctAnswers + "/" + totalQuestionsAnswered + "\n";
                resultText += "Final Score: " + currentScore + "\n\n";
                resultText += "Press the Return button or Escape to continue your adventure!";
                
                completionText.text = resultText;
                Debug.Log("Set completion text: " + resultText);
            }
            else
            {
                Debug.LogWarning("Completion text is null!");
            }
        }
        else
        {
            Debug.LogError("Completion panel is null! Returning to main scene after delay.");
            // If no completion panel, return after a delay
            Invoke("ReturnToMainScene", 3f);
        }
    }
    
    public void ReturnToMainScene()
    {
        Debug.Log("Returning to main scene");
        
        // Cancel any pending invokes
        CancelInvoke();
        
        // Reset cursor state before returning
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Load main scene
        SceneManager.LoadScene(mainSceneName);
    }
    
    // This method is called when the scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isReturningFromQuiz && scene.name == mainSceneName)
        {
            // Find player and restore position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
                player.transform.rotation = playerRotation;
                Debug.Log("Restored player position: " + playerPosition);
            }
            
            // Reset the flag
            isReturningFromQuiz = false;
        }
    }
    
    void OnEnable()
    {
        // Register for scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        // Unregister from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

// Class to store quiz questions
[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
    
    public QuizQuestion(string question, string[] answers, int correctAnswerIndex)
    {
        this.question = question;
        this.answers = answers;
        this.correctAnswerIndex = correctAnswerIndex;
    }
}