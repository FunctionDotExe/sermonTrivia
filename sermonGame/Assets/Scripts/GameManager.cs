using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float roundTime = 300f; // 5 minutes per round
    public int maxPlayers = 4;
    
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI starsCollectedText;
    
    // Game state
    private float currentTime;
    private int currentPlayerIndex = 0;
    private List<PlayerData> players = new List<PlayerData>();
    private bool isRoundActive = false;
    
    // Singleton instance
    public static GameManager Instance { get; private set; }
    
    // Static flag to track if we're returning from a quiz
    private static bool isReturningFromQuiz = false;
    // Static variables to store player position and rotation
    private static Vector3 savedPlayerPosition;
    private static Quaternion savedPlayerRotation;
    // Static variable to store the star that was interacted with
    private static GameObject lastInteractedStar;
    
    [Header("Game Settings")]
    public int totalStars = 18;
    
    private int score = 0;
    private int starsCollected = 0;

    private SimpleMultiplayerManager multiplayerManager;
    private int[] playerScores;
    private int currentPlayer = 0;

    private ScoreIntegrator scoreManager;

    void Start()
    {
        scoreManager = ScoreIntegrator.Instance;
        
        // Check if we're returning from a quiz
        if (isReturningFromQuiz)
        {
            // Call after a short delay to ensure scene is fully loaded
            Invoke("ReturnFromQuizScene", 0.1f);
        }
        else
        {
            // Normal game start
            StartRound();
        }
        
        UpdateUI();
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayers();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Initialize player scores
        playerScores = new int[4]; // Adjust based on max players
        
        // Find the multiplayer manager
        multiplayerManager = FindObjectOfType<SimpleMultiplayerManager>();
        if (multiplayerManager != null)
        {
            // Get the current player from the multiplayer manager
            currentPlayer = multiplayerManager.GetCurrentPlayer();
        }
    }
    
    private void InitializePlayers()
    {
        // Create player data for each player
        for (int i = 0; i < maxPlayers; i++)
        {
            players.Add(new PlayerData($"Player {i+1}", 0));
        }
    }
    
    public void StartRound()
    {
        currentTime = roundTime;
        isRoundActive = true;
        UpdateUI();
        StartCoroutine(TimerCoroutine());
    }
    
    private IEnumerator TimerCoroutine()
    {
        while (currentTime > 0 && isRoundActive)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            UpdateUI();
        }
        
        // Time's up
        EndRound();
    }
    
    public void EndRound()
    {
        isRoundActive = false;
        
        // Move to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        
        // TODO: Show round end UI and prepare for next player
    }
    
    public void AddPoints(int points)
    {
        if (currentPlayer >= 0 && currentPlayer < playerScores.Length)
        {
            playerScores[currentPlayer] += points;
            
            // Also update the multiplayer manager
            if (multiplayerManager != null)
            {
                multiplayerManager.AddPoints(points);
            }
        }
    }
    
    public void AddTime(float seconds)
    {
        if (isRoundActive)
        {
            currentTime += seconds;
            UpdateUI();
        }
    }
    
    public void StarCollected()
    {
        starsCollected++;
        UpdateUI();
        
        // Check if all stars are collected
        if (starsCollected >= totalStars)
        {
            Debug.Log("All stars collected! Game complete!");
            // You can trigger a completion event here
        }
        
        // Increment score
        score += 10;
        
        // Notify the multiplayer manager if it exists
        if (MultiplayerGameManager.Instance != null)
        {
            MultiplayerGameManager.Instance.AddPoints(10);
        }
    }
    
    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"Time: {Mathf.FloorToInt(currentTime / 60):00}:{Mathf.FloorToInt(currentTime % 60):00}";
        
        if (scoreText != null && currentPlayerIndex < players.Count)
            scoreText.text = $"Score: {players[currentPlayerIndex].score}";
        
        if (currentPlayerText != null && currentPlayerIndex < players.Count)
            currentPlayerText.text = $"Player: {players[currentPlayerIndex].name}";
        
        if (starsCollectedText != null)
        {
            starsCollectedText.text = "Stars: " + starsCollected + " / " + totalStars;
        }
    }
    
    // Get current player data
    public PlayerData GetCurrentPlayer()
    {
        if (currentPlayerIndex < players.Count)
            return players[currentPlayerIndex];
        return null;
    }

    // Call this before loading the quiz scene
    public void PrepareForQuizScene(GameObject star, GameObject player)
    {
        // Save the player's position and rotation
        if (player != null)
        {
            savedPlayerPosition = player.transform.position;
            savedPlayerRotation = player.transform.rotation;
        }
        
        // Save reference to the star
        lastInteractedStar = star;
        
        // Set the returning flag
        isReturningFromQuiz = true;
        
        // Don't destroy this GameManager when loading new scene
        DontDestroyOnLoad(gameObject);
    }

    // Call this when returning from the quiz scene
    public void ReturnFromQuizScene()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && isReturningFromQuiz)
        {
            // Restore player position and rotation
            player.transform.position = savedPlayerPosition;
            player.transform.rotation = savedPlayerRotation;
        }
        
        // Disable the star that was interacted with
        if (lastInteractedStar != null)
        {
            lastInteractedStar.SetActive(false);
        }
        
        // Reset the flag
        isReturningFromQuiz = false;
    }

    public int GetPlayerScore()
    {
        if (currentPlayer >= 0 && currentPlayer < playerScores.Length)
        {
            return playerScores[currentPlayer];
        }
        return 0;
    }

    // Update the current player
    public void SetCurrentPlayer(int player)
    {
        if (player >= 0 && player < playerScores.Length)
        {
            currentPlayer = player;
        }
    }

    // Example method to award points
    public void AwardPoints(int points)
    {
        if (scoreManager != null)
        {
            scoreManager.AddPoints(points);
        }
    }
}

// Class to store player data
[System.Serializable]
public class PlayerData
{
    public string name;
    public int score;
    
    public PlayerData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
} 