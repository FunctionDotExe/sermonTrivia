using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MultiplayerGameManager : MonoBehaviour
{
    [Header("Player Settings")]
    public int numberOfPlayers = 4;
    public int numberOfRounds = 5;
    public float timeMultiplier = 20f; // Seconds per number
    
    [Header("UI References")]
    public GameObject buttonSelectionPanel;
    public Button[] numberButtons = new Button[4];
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI[] playerScoreTexts = new TextMeshProUGUI[4];
    public TextMeshProUGUI roundText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;
    
    [Header("Player Control")]
    public PlayerController playerController;
    public GameObject[] playerIndicators = new GameObject[4]; // Visual indicators for current player
    
    // Game state
    private int currentPlayerIndex = 0;
    private int currentRound = 1;
    private int[] playerScores = new int[4];
    private int scoreAtTurnStart = 0;
    private float remainingTime = 0f;
    private bool isTimerRunning = false;
    private bool isGameOver = false;
    
    // Singleton instance
    public static MultiplayerGameManager Instance { get; private set; }
    
    void Awake()
    {
        Debug.Log("MultiplayerGameManager Awake called");
        
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MultiplayerGameManager instance created");
        }
        else
        {
            Debug.Log("Destroying duplicate MultiplayerGameManager");
            Destroy(gameObject);
            return;
        }
        
        // Initialize player scores
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i] = 0;
        }
    }
    
    void Start()
    {
        Debug.Log("MultiplayerGameManager Start called");
        
        // Check if UI elements are assigned
        if (buttonSelectionPanel == null)
        {
            Debug.LogError("Button Selection Panel is not assigned!");
        }
        
        if (numberButtons == null || numberButtons.Length == 0)
        {
            Debug.LogError("Number Buttons array is empty or null!");
        }
        else
        {
            Debug.Log($"Found {numberButtons.Length} number buttons");
            
            // Set up button listeners
            for (int i = 0; i < numberButtons.Length; i++)
            {
                if (numberButtons[i] == null)
                {
                    Debug.LogError($"Button at index {i} is null!");
                    continue;
                }
                
                int buttonIndex = i; // Capture the index for the lambda
                Debug.Log($"Setting up listener for button {i}");
                numberButtons[i].onClick.AddListener(() => {
                    Debug.Log($"Button {buttonIndex} clicked!");
                    OnNumberButtonClicked(buttonIndex);
                });
            }
        }
        
        // Start the first turn
        StartNewTurn();
        
        // Hide game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned!");
        }
    }
    
    void Update()
    {
        if (isTimerRunning)
        {
            // Update timer
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();
            
            // Check if time is up
            if (remainingTime <= 0)
            {
                EndPlayerTurn();
            }
        }
    }
    
    void StartNewTurn()
    {
        Debug.Log($"Starting new turn for Player {currentPlayerIndex + 1}");
        
        // Update UI
        UpdatePlayerDisplay();
        UpdateRoundDisplay();
        
        // Show button selection panel
        if (buttonSelectionPanel != null)
        {
            buttonSelectionPanel.SetActive(true);
            Debug.Log("Button selection panel activated");
        }
        else
        {
            Debug.LogError("Button Selection Panel is null!");
        }
        
        // Randomize button numbers
        RandomizeButtonNumbers();
        
        // Disable player control until they select a number
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("Player controller disabled");
        }
        else
        {
            Debug.LogWarning("Player Controller is not assigned!");
        }
        
        // Store current score to calculate difference at end of turn
        scoreAtTurnStart = GetCurrentPlayerScore();
    }
    
    void RandomizeButtonNumbers()
    {
        // Create a list of available numbers (1-4)
        System.Collections.Generic.List<int> availableNumbers = new System.Collections.Generic.List<int> { 1, 2, 3, 4 };
        
        // Assign random numbers to buttons
        for (int i = 0; i < numberButtons.Length; i++)
        {
            // Get a random index from the available numbers
            int randomIndex = Random.Range(0, availableNumbers.Count);
            int numberValue = availableNumbers[randomIndex];
            
            // Remove the selected number from available numbers
            availableNumbers.RemoveAt(randomIndex);
            
            // Update button text
            TextMeshProUGUI buttonText = numberButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = numberValue.ToString();
            }
            
            // Store the number value in the button's tag
            numberButtons[i].tag = numberValue.ToString();
        }
    }
    
    void OnNumberButtonClicked(int buttonIndex)
    {
        // Get the number value from the button's tag
        int numberValue = int.Parse(numberButtons[buttonIndex].tag);
        
        // Calculate time for this turn
        remainingTime = numberValue * timeMultiplier;
        
        // Hide button selection panel
        buttonSelectionPanel.SetActive(false);
        
        // Enable player control
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        
        // Start the timer
        isTimerRunning = true;
        UpdateTimerDisplay();
        
        // Update player indicator
        UpdatePlayerIndicator();
    }
    
    void EndPlayerTurn()
    {
        // Stop the timer
        isTimerRunning = false;
        
        // Calculate points earned this turn
        int currentScore = GetCurrentPlayerScore();
        int pointsEarned = currentScore - scoreAtTurnStart;
        
        // Add points to player's score
        playerScores[currentPlayerIndex] += pointsEarned;
        
        // Update score display
        UpdateScoreDisplay();
        
        // Move to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        
        // Check if we've completed a round
        if (currentPlayerIndex == 0)
        {
            currentRound++;
            
            // Check if game is over
            if (currentRound > numberOfRounds)
            {
                EndGame();
                return;
            }
        }
        
        // Start the next turn
        StartNewTurn();
    }
    
    void EndGame()
    {
        isGameOver = true;
        
        // Disable player control
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Find the winner
        int winnerIndex = 0;
        int highestScore = playerScores[0];
        
        for (int i = 1; i < playerScores.Length; i++)
        {
            if (playerScores[i] > highestScore)
            {
                winnerIndex = i;
                highestScore = playerScores[i];
            }
        }
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (winnerText != null)
            {
                winnerText.text = "Player " + (winnerIndex + 1) + " wins with " + highestScore + " points!";
            }
        }
    }
    
    // Helper methods for UI updates
    
    void UpdatePlayerDisplay()
    {
        if (currentPlayerText != null)
        {
            currentPlayerText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
        }
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();
        }
    }
    
    void UpdateRoundDisplay()
    {
        if (roundText != null)
        {
            roundText.text = "Round " + currentRound + "/" + numberOfRounds;
        }
    }
    
    void UpdateScoreDisplay()
    {
        for (int i = 0; i < playerScoreTexts.Length; i++)
        {
            if (playerScoreTexts[i] != null)
            {
                playerScoreTexts[i].text = "P" + (i + 1) + ": " + playerScores[i];
            }
        }
    }
    
    void UpdatePlayerIndicator()
    {
        // Hide all indicators
        foreach (GameObject indicator in playerIndicators)
        {
            if (indicator != null)
            {
                indicator.SetActive(false);
            }
        }
        
        // Show current player's indicator
        if (currentPlayerIndex < playerIndicators.Length && playerIndicators[currentPlayerIndex] != null)
        {
            playerIndicators[currentPlayerIndex].SetActive(true);
        }
    }
    
    // Helper method to get current player's score
    int GetCurrentPlayerScore()
    {
        // This should be replaced with your actual scoring system
        if (GameManager.Instance != null)
        {
            return GameManager.Instance.GetPlayerScore();
        }
        return 0;
    }
    
    // Public method for other scripts to call when points are earned
    public void AddPoints(int points)
    {
        if (!isGameOver)
        {
            playerScores[currentPlayerIndex] += points;
            UpdateScoreDisplay();
        }
    }
    
    // Public method to manually end a turn (for testing)
    public void ForceEndTurn()
    {
        if (isTimerRunning)
        {
            EndPlayerTurn();
        }
    }
} 