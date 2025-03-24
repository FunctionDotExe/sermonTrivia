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
    private float remainingTime = 0f;
    private bool isTimerRunning = false;
    private bool isGameOver = false;
    
    // Singleton instance
    public static MultiplayerGameManager Instance { get; private set; }
    
    private MultiplayerPlayerData[] players;
    
    public GameObject initialPanel;
    public GameObject gameplayPanel;
    public GameObject endRoundPanel;
    public TextMeshProUGUI scoreText;
    
    private bool isGameActive = false;
    
    private void Awake()
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
        
        InitializeGame();
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
    
    void InitializeGame()
    {
        players = new MultiplayerPlayerData[4];
        for (int i = 0; i < 4; i++)
        {
            players[i] = new MultiplayerPlayerData(i + 1);
        }
        
        ShowInitialPanel();
    }
    
    public void StartPlayerTurn(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= players.Length) return;
        
        currentPlayerIndex = playerIndex;
        isGameActive = true;
        
        // Hide initial panel and show gameplay panel
        if (initialPanel != null) initialPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);
        
        // Enable player movement
        EnablePlayerMovement(true);
        
        StartCoroutine(PlayerTurnTimer());
    }
    
    IEnumerator PlayerTurnTimer()
    {
        while (players[currentPlayerIndex].remainingTime > 0 && isGameActive)
        {
            players[currentPlayerIndex].remainingTime -= Time.deltaTime;
            UpdateUI();
            yield return null;
        }
        
        EndPlayerTurn();
    }
    
    void EndPlayerTurn()
    {
        isGameActive = false;
        EnablePlayerMovement(false);
        
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Length)
        {
            currentPlayerIndex = 0;
            currentRound++;
            
            if (currentRound > numberOfRounds)
            {
                EndGame();
                return;
            }
            
            ShowEndRoundPanel();
        }
        else
        {
            StartPlayerTurn(currentPlayerIndex);
        }
    }
    
    void ShowInitialPanel()
    {
        if (initialPanel != null) initialPanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (endRoundPanel != null) endRoundPanel.SetActive(false);
    }
    
    void ShowEndRoundPanel()
    {
        if (initialPanel != null) initialPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (endRoundPanel != null) endRoundPanel.SetActive(true);
        
        StartCoroutine(WaitAndStartNextRound());
    }
    
    IEnumerator WaitAndStartNextRound()
    {
        yield return new WaitForSeconds(3f);
        StartPlayerTurn(0);
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
        int highestScore = players[0].score;
        
        for (int i = 1; i < players.Length; i++)
        {
            if (players[i].score > highestScore)
            {
                winnerIndex = i;
                highestScore = players[i].score;
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
            timerText.text = "Time: " + Mathf.CeilToInt(players[currentPlayerIndex].remainingTime).ToString();
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
                playerScoreTexts[i].text = "P" + (i + 1) + ": " + players[i].score;
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
    
    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"Time: {players[currentPlayerIndex].remainingTime:F1}";
        
        if (scoreText != null)
            scoreText.text = $"Score: {players[currentPlayerIndex].score}";
        
        if (currentPlayerText != null)
            currentPlayerText.text = $"Player {currentPlayerIndex + 1}";
        
        if (roundText != null)
            roundText.text = $"Round {currentRound}/{numberOfRounds}";
    }
    
    void EnablePlayerMovement(bool enable)
    {
        if (playerController != null)
        {
            playerController.enabled = enable;
        }
    }
    
    void OnNumberButtonClicked(int buttonIndex)
    {
        // Get the number value from the button's tag
        int numberValue = int.Parse(numberButtons[buttonIndex].tag);
        
        // Calculate time for this turn
        players[currentPlayerIndex].remainingTime = numberValue * timeMultiplier;
        
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
    
    // Public method for other scripts to call when points are earned
    public void AddPoints(int points)
    {
        if (!isGameOver)
        {
            players[currentPlayerIndex].score += points;
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

    public MultiplayerPlayerData GetCurrentPlayer()
    {
        if (currentPlayerIndex >= 0 && currentPlayerIndex < players.Length)
        {
            return players[currentPlayerIndex];
        }
        return null;
    }

    void StartNewTurn()
    {
        if (currentPlayerIndex >= 0 && currentPlayerIndex < players.Length)
        {
            StartPlayerTurn(currentPlayerIndex);
        }
    }

    public void AddTime(float timeToAdd)
    {
        if (currentPlayerIndex >= 0 && currentPlayerIndex < players.Length)
        {
            players[currentPlayerIndex].remainingTime += timeToAdd;
            UpdateUI();
        }
    }

    public int GetPlayerScore(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < players.Length)
        {
            return players[playerIndex].score;
        }
        return 0;
    }

    public int GetCurrentPlayerScore()
    {
        return GetPlayerScore(currentPlayerIndex);
    }
} 