using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SimpleMultiplayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    public int numberOfPlayers = 4;
    public int numberOfRounds = 5;
    public float timeMultiplier = 20f;

    [Header("UI References")]
    public GameObject buttonPanel;
    public GameObject[] numberButtonObjects; // Change to GameObject array
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI[] playerScoreTexts;
    public Color[] playerColors = new Color[4] {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };

    [Header("Player Control")]
    public MonoBehaviour playerControllerScript;

    // Game state
    private int currentPlayer = 0;
    private int currentRound = 1;
    private float remainingTime = 0f;
    private bool isTimerRunning = false;
    private int[] playerScores;
    private int scoreAtTurnStart;

    // Store references to button handlers
    private NumberButtonHandler[] buttonHandlers;

    [SerializeField] private int currentTargetNumber = 1; // Default target is 1

    [SerializeField] private float gameTimer = 300f; // 5 minutes default
    private float currentTime;
    private int currentScore = 0;
    private bool gameStarted = false;

    void Awake()
    {
        // Initialize player scores
        playerScores = new int[numberOfPlayers];
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i] = 0;
 }

        // Check for EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogWarning("No EventSystem found! Creating one...");
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

    void Start()
    {
        Debug.Log("SimpleMultiplayerManager started");

        // Check UI components
        if (buttonPanel == null) Debug.LogError("Button panel is null!");
        if (playerText == null) Debug.LogError("Player text is null!");
        if (timerText == null) Debug.LogError("Timer text is null!");
        if (roundText == null) Debug.LogError("Round text is null!");

        // Find all NumberButtonHandler components in the scene
        NumberButtonHandler[] allHandlers = FindObjectsOfType<NumberButtonHandler>();
        Debug.Log($"Found {allHandlers.Length} NumberButtonHandler components in the scene");

        // Create arrays to store button objects and handlers
        numberButtonObjects = new GameObject[4];
        buttonHandlers = new NumberButtonHandler[4];

        // Assign handlers to the correct indices based on their buttonIndex property
        foreach (NumberButtonHandler handler in allHandlers)
        {
            int index = handler.buttonIndex;
            if (index >= 0 && index < 4)
            {
                numberButtonObjects[index] = handler.gameObject;
                buttonHandlers[index] = handler;
                Debug.Log($"Assigned button with index {index} to arrays");
            }
            else
            {
                Debug.LogError($"Button has invalid index: {index}");
            }
        }

        // Verify all buttons were found
        for (int i = 0; i < 4; i++)
        {
            if (buttonHandlers[i] == null)
            {
                Debug.LogError($"No button found with index {i}");
            }
        }

        // Initialize UI
        UpdateUI();
        UpdateScoreDisplay();

        // Make sure button panel is visible
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(true);
            Debug.Log("Button panel activated");
        }

        // Disable player control initially
        if (playerControllerScript != null)
        {
            playerControllerScript.enabled = false;
            Debug.Log("Player controller disabled");
        }

        // Randomize button numbers
        RandomizeButtonNumbers();

        // Store current score
        scoreAtTurnStart = GetCurrentPlayerScore();

        // Fix multiple audio listeners issue
        FixMultipleAudioListeners();
        EnsureUISystemWorks();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            remainingTime -= Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";
            }

            if (remainingTime <= 0)
            {
                EndTurn();
            }
        }

        // Debug button visibility and interactability
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("=== BUTTON DEBUG INFO ===");

            // Check if button panel is active
            if (buttonPanel != null)
            {
                Debug.Log($"Button panel active: {buttonPanel.activeSelf}");

                // Check if button panel is visible (has non-zero size)
                RectTransform panelRect = buttonPanel.GetComponent<RectTransform>();
                if (panelRect != null)
                {
                    Debug.Log($"Button panel size: {panelRect.rect.width} x {panelRect.rect.height}");
                    Debug.Log($"Button panel position: {panelRect.position}");
                }
            }

            // Check button objects
            for (int i = 0; i < numberButtonObjects.Length; i++)
            {
                if (numberButtonObjects[i] != null)
                {
                    Debug.Log($"Button {i} active: {numberButtonObjects[i].activeSelf}");

                    // Check if button is interactable
                    Button button = numberButtonObjects[i].GetComponent<Button>();
                    if (button != null)
                    {
                        Debug.Log($"Button {i} interactable: {button.interactable}");
                    }

                    // Check button position and size
                    RectTransform buttonRect = numberButtonObjects[i].GetComponent<RectTransform>();
                    if (buttonRect != null)
                    {
                        Debug.Log($"Button {i} size: {buttonRect.rect.width} x {buttonRect.rect.height}");
                        Debug.Log($"Button {i} position: {buttonRect.position}");
                        Debug.Log($"Button {i} world corners: {GetWorldCorners(buttonRect)}");
                    }

                    // Check if button has a collider
                    if (numberButtonObjects[i].GetComponent<Collider>() != null ||
                        numberButtonObjects[i].GetComponent<Collider2D>() != null)
                    {
                        Debug.Log($"Button {i} has a collider (this might interfere with UI clicks)");
                    }
                }
                else
                {
                    Debug.Log($"Button {i} is null");
                }
            }

            // Check EventSystem
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null)
            {
                Debug.Log($"EventSystem found: {eventSystem.gameObject.name}");
                Debug.Log($"EventSystem enabled: {eventSystem.enabled}");
                Debug.Log($"Current selected GameObject: {(eventSystem.currentSelectedGameObject != null ? eventSystem.currentSelectedGameObject.name : "None")}");
            }
            else
            {
                Debug.Log("No EventSystem found in the scene!");
            }

            // Check Canvas settings
            Canvas canvas = buttonPanel?.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"Canvas render mode: {canvas.renderMode}");
                Debug.Log($"Canvas scale factor: {canvas.scaleFactor}");
                Debug.Log($"Canvas enabled: {canvas.enabled}");
            }
            else
            {
                Debug.Log("No Canvas found for the button panel!");
            }

            Debug.Log("=== END DEBUG INFO ===");
        }

        // Direct click detection
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log($"Mouse clicked at: {mousePos}");

            // Check each button manually
            foreach (var buttonHandler in buttonHandlers)
            {
                if (buttonHandler != null)
                {
                    RectTransform rectTransform = buttonHandler.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        // Check if mouse is over this button
                        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, null))
                        {
                            Debug.Log($"Manual detection: Mouse is over button {buttonHandler.buttonIndex}");
                            buttonHandler.HandleButtonClick();
                            return;
                        }
                    }
                }
            }
        }

        // Update timer
        UpdateTimer();
    }

    void RandomizeButtonNumbers()
    {
        Debug.Log("Randomizing button numbers");

        // Create a list of available numbers (1-4)
        System.Collections.Generic.List<int> availableNumbers = new System.Collections.Generic.List<int> { 1, 2, 3, 4 };

        // Assign random numbers to buttons
        for (int i = 0; i < buttonHandlers.Length; i++)
        {
            if (buttonHandlers[i] == null)
            {
                Debug.LogError($"Button handler {i} is null during randomization!");
                continue;
            }

            // Get a random index from the available numbers
            int randomIndex = Random.Range(0, availableNumbers.Count);
            int numberValue = availableNumbers[randomIndex];

            // Remove the selected number from available numbers
            availableNumbers.RemoveAt(randomIndex);

            // Update button value
            buttonHandlers[i].SetButtonValue(numberValue);

            Debug.Log($"Button {i} assigned number {numberValue}");
        }
    }

    // This method is called directly from the NumberButtonHandler
    public void ButtonClickedByHandler(int buttonIndex, int buttonValue)
    {
        Debug.Log($"=== BUTTON CLICKED: Index {buttonIndex}, Value {buttonValue} ===");

        // Verify the button value
        if (buttonHandlers != null && buttonIndex >= 0 && buttonIndex < buttonHandlers.Length && buttonHandlers[buttonIndex] != null)
        {
            int storedValue = buttonHandlers[buttonIndex].buttonValue;
            Debug.Log($"Stored value for button {buttonIndex} is {storedValue}");

            // Get the text value as well
            TextMeshProUGUI buttonText = buttonHandlers[buttonIndex].GetComponentInChildren<TextMeshProUGUI>();
            string textValue = buttonText != null ? buttonText.text : "NULL";
            Debug.Log($"Button {buttonIndex} text is: {textValue}");

            if (storedValue != buttonValue)
            {
                Debug.LogWarning($"Button value mismatch! Passed: {buttonValue}, Stored: {storedValue}");
                buttonValue = storedValue; // Use the stored value
            }

            // Try parsing the text value as a fallback
            int parsedValue;
            if (int.TryParse(textValue, out parsedValue))
            {
                Debug.Log($"Parsed text value: {parsedValue}");
                if (parsedValue != buttonValue)
                {
                    Debug.LogWarning($"Text value mismatch! Using: {buttonValue}, Text: {parsedValue}");
                    if (storedValue == 1 && parsedValue > 1)
                    {
                        Debug.Log($"Using text value {parsedValue} instead of stored value {storedValue}");
                        buttonValue = parsedValue;
                    }
                }
            }
        }

        // Calculate time for this turn
        remainingTime = buttonValue * timeMultiplier;
        Debug.Log($"Setting timer to {buttonValue} × {timeMultiplier} = {remainingTime} seconds");

        // Hide button panel
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
            Debug.Log("Button panel hidden");
        }

        // Enable player control
        if (playerControllerScript != null)
        {
            playerControllerScript.enabled = true;
            Debug.Log("Player controller enabled");
        }

        // Start timer
        isTimerRunning = true;

        // Update UI
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";
        }

        // Update player text color to match current player
        if (playerText != null && currentPlayer < playerColors.Length)
        {
            playerText.color = playerColors[currentPlayer];
        }
    }

    void EndTurn()
    {
        Debug.Log("Turn ended");

        // Stop timer
        isTimerRunning = false;

        // Disable player control
        if (playerControllerScript != null)
        {
            playerControllerScript.enabled = false;
        }

        // Calculate points earned this turn
        int currentScore = GetCurrentPlayerScore();
        int pointsEarned = currentScore - scoreAtTurnStart;

        Debug.Log($"Player {currentPlayer + 1} earned {pointsEarned} points this turn");

        // Add points to player's score
        playerScores[currentPlayer] += pointsEarned;

        // Move to next player
        currentPlayer = (currentPlayer + 1) % numberOfPlayers;

        // Check if round is complete
        if (currentPlayer == 0)
        {
            currentRound++;

            // Check if game is over
            if (currentRound > numberOfRounds)
            {
                EndGame();
                return;
            }
        }

        // Update UI
        UpdateUI();
        UpdateScoreDisplay();

        // Show button panel
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(true);
        }

        // Randomize button numbers for next player
        RandomizeButtonNumbers();

        // Store current score for next turn
        scoreAtTurnStart = GetCurrentPlayerScore();
    }

    void EndGame()
    {
        Debug.Log("Game over!");

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

        // Display winner
        if (playerText != null)
        {
            playerText.text = $"Player {winnerIndex + 1} wins with {highestScore} points!";
            playerText.color = playerColors[winnerIndex];
        }

        // Hide timer
        if (timerText != null)
        {
            timerText.text = "";
        }

        // Hide round text
        if (roundText != null)
        {
            roundText.text = "Game Over";
        }
    }

    void UpdateUI()
    {
        if (playerText != null)
        {
            playerText.text = $"Player {currentPlayer + 1}'s Turn";

            // Set color based on current player
            if (currentPlayer < playerColors.Length)
            {
                playerText.color = playerColors[currentPlayer];
            }
        }

        if (roundText != null)
        {
            roundText.text = $"Round {currentRound}/{numberOfRounds}";
        }
    }

    void UpdateScoreDisplay()
    {
        if (playerScoreTexts == null || playerScoreTexts.Length == 0)
            return;

        for (int i = 0; i < playerScoreTexts.Length && i < playerScores.Length; i++)
        {
            if (playerScoreTexts[i] != null)
            {
                playerScoreTexts[i].text = $"P{i + 1}: {playerScores[i]}";

                // Set color based on player
                if (i < playerColors.Length)
                {
                    playerScoreTexts[i].color = playerColors[i];
                }
            }
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
        // Add points to your score variable
        currentScore += points;

        // Call your existing score display update method
        UpdateScoreDisplay(); // Ensure this method is called to update the display

        Debug.Log($"SimpleMultiplayerManager: Added {points} points. New score: {currentScore}");
    }

    // Add this method to update the score
    public void AddScore(int points)
    {
        if (!gameStarted) return;

        currentScore += points;
        UpdateScoreDisplay();
        Debug.Log($"Score increased by {points}. New score: {currentScore}");
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    // Helper method to get world corners of a RectTransform
    string GetWorldCorners(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return $"[{corners[0]}, {corners[1]}, {corners[2]}, {corners[3]}]";
    }

    private void FixMultipleAudioListeners()
    {
        // Find all audio listeners in the scene
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();

        if (listeners.Length > 1)
        {
            Debug.Log($"Found {listeners.Length} audio listeners. Disabling extras.");

            // Keep only the first one
            for (int i = 1; i < listeners.Length; i++)
            {
                Debug.Log($"Disabling extra audio listener on {listeners[i].gameObject.name}");
                listeners[i].enabled = false;
            }
        }
    }

    private void EnsureUISystemWorks()
    {
        // Check if EventSystem exists
        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogError("No EventSystem found in the scene! Creating one.");
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Check if Canvas has a GraphicRaycaster
        if (buttonPanel != null)
        {
            Canvas canvas = buttonPanel.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
                if (raycaster == null)
                {
                    Debug.LogError("Canvas has no GraphicRaycaster! Adding one.");
                    raycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();
                }

                // Force the raycaster to be enabled
                raycaster.enabled = true;

                // Set blocking objects to none to ensure UI elements are detected
                raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

                Debug.Log("GraphicRaycaster configured on Canvas");
            }
        }

        // Check if all buttons have Button components
        foreach (var buttonHandler in buttonHandlers)
        {
            if (buttonHandler != null)
            {
                Button button = buttonHandler.GetComponent<Button>();
                if (button == null)
                {
                    Debug.LogError($"Button {buttonHandler.buttonIndex} has no Button component! Adding one.");
                    button = buttonHandler.gameObject.AddComponent<Button>();
                    button.onClick.AddListener(() => buttonHandler.HandleButtonClick());
                }
            }
        }

        // Add the DirectButtonHandler to the button panel
        if (buttonPanel != null && buttonPanel.GetComponent<DirectButtonHandler>() == null)
        {
            buttonPanel.AddComponent<DirectButtonHandler>();
            Debug.Log("Added DirectButtonHandler to button panel");
        }

        Debug.Log("UI system check complete");
    }

    // Add this method to handle button clicks from NumberButtonHandler
    public void HandleNumberButtonClick(int buttonIndex, int buttonValue)
    {
        Debug.Log($"SimpleMultiplayerManager received click from button {buttonIndex} with value {buttonValue}");

        // Add your button click handling logic here
        Debug.Log($"Button {buttonIndex} with value {buttonValue} was clicked");

        // For now, just handle any button click
        OnButtonPressed(buttonIndex, buttonValue);
    }

    // Add this method to handle what happens when a button is pressed
    private void OnButtonPressed(int buttonIndex, int buttonValue)
    {
        Debug.Log($"Button {buttonIndex} with value {buttonValue} pressed!");

        // Disable all buttons
        foreach (var handler in buttonHandlers)
        {
            if (handler != null)
            {
                handler.gameObject.SetActive(false);
            }
        }

        // Hide button panel
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
            Debug.Log("Button panel hidden");
        }

        // Re-enable player controller
        if (playerControllerScript != null)
        {
            playerControllerScript.enabled = true;
            Debug.Log("Player controller enabled");
        }

        // Start the game
        gameStarted = true;
        currentTime = gameTimer;
        currentScore = 0;
        UpdateScoreDisplay();

        Debug.Log("Game started!");
    }

    // Add this method to update the timer
    void UpdateTimer()
    {
        if (!gameStarted) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }

        // Update timer display
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}