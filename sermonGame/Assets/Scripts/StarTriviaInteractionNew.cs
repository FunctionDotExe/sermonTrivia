using UnityEngine;
using UnityEngine.Events;

public class StarTriviaInteractionNew : MonoBehaviour
{
    [Header("Trivia Settings")]
    public string npcName = "Star Trivia";
    public string triviaQuestion = "What is the capital of France?";
    [TextArea(2, 5)]
    public string[] answerOptions = new string[4] { "Paris", "London", "Berlin", "Rome" };
    public int correctAnswerIndex = 0;
    public int pointsForCorrectAnswer = 10;
    
    [Header("Visual Settings")]
    public float rotationSpeed = 50f;
    public Color starColor = Color.yellow;
    public float pulseSpeed = 1f;
    public float pulseIntensity = 0.2f;
    
    [Header("Collection Effect")]
    public GameObject collectEffectPrefab;
    public float effectDuration = 3f;
    public bool addLightToEffect = true;
    public Color lightColor = new Color(1f, 0.7f, 0.3f, 1f);
    public float lightIntensity = 2f;
    
    private bool hasBeenCollected = false;
    private Material starMaterial;
    private bool isInteracting = false;
    
    public UnityEvent<int> OnChoiceMade = new UnityEvent<int>();
    private GameManager gameManager;
    
    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found! Make sure there's a GameManager in your scene.");
        }
        
        // Set up star appearance
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            starMaterial = renderer.material;
            starMaterial.color = starColor;
            starMaterial.EnableKeyword("_EMISSION");
        }
        
        // Add a rotating animation
        StartCoroutine(AnimateStar());
    }
    
    System.Collections.IEnumerator AnimateStar()
    {
        float pulseTime = 0f;
        
        while (!hasBeenCollected)
        {
            // Rotate the star
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            
            // Pulse the star
            if (starMaterial != null)
            {
                pulseTime += Time.deltaTime * pulseSpeed;
                float pulse = 1f + Mathf.Sin(pulseTime) * pulseIntensity;
                starMaterial.SetColor("_EmissionColor", starColor * pulse);
            }
            
            yield return null;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenCollected && !isInteracting)
        {
            isInteracting = true;
            ShowTriviaQuestion();
        }
    }
    
    void ShowTriviaQuestion()
    {
        // Create a simple UI for the trivia question
        GameObject canvasObj = new GameObject("TriviaCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Create question panel
        GameObject panelObj = new GameObject("QuestionPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        UnityEngine.UI.Image panel = panelObj.AddComponent<UnityEngine.UI.Image>();
        panel.color = new Color(0, 0, 0, 0.8f);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.2f, 0.2f);
        panelRect.anchorMax = new Vector2(0.8f, 0.8f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Create question text
        GameObject questionObj = new GameObject("QuestionText");
        questionObj.transform.SetParent(panelObj.transform, false);
        TMPro.TextMeshProUGUI questionText = questionObj.AddComponent<TMPro.TextMeshProUGUI>();
        questionText.text = triviaQuestion;
        questionText.fontSize = 24;
        questionText.color = Color.white;
        questionText.alignment = TMPro.TextAlignmentOptions.Center;
        RectTransform questionRect = questionText.GetComponent<RectTransform>();
        questionRect.anchorMin = new Vector2(0.1f, 0.7f);
        questionRect.anchorMax = new Vector2(0.9f, 0.9f);
        questionRect.offsetMin = Vector2.zero;
        questionRect.offsetMax = Vector2.zero;
        
        // Create answer buttons
        for (int i = 0; i < answerOptions.Length; i++)
        {
            int index = i; // Capture for lambda
            
            GameObject buttonObj = new GameObject("AnswerButton" + i);
            buttonObj.transform.SetParent(panelObj.transform, false);
            UnityEngine.UI.Button button = buttonObj.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image buttonImage = button.GetComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            // Position the button
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            float buttonHeight = 0.1f;
            float spacing = 0.05f;
            float startY = 0.6f - (buttonHeight + spacing) * i;
            buttonRect.anchorMin = new Vector2(0.2f, startY);
            buttonRect.anchorMax = new Vector2(0.8f, startY + buttonHeight);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;
            
            // Add text to button
            GameObject buttonTextObj = new GameObject("ButtonText");
            buttonTextObj.transform.SetParent(buttonObj.transform, false);
            TMPro.TextMeshProUGUI buttonText = buttonTextObj.AddComponent<TMPro.TextMeshProUGUI>();
            buttonText.text = answerOptions[i];
            buttonText.fontSize = 18;
            buttonText.color = Color.white;
            buttonText.alignment = TMPro.TextAlignmentOptions.Center;
            RectTransform textRect = buttonText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            // Add click handler
            button.onClick.AddListener(() => {
                HandleAnswer(index, canvasObj);
            });
        }
    }
    
    void HandleAnswer(int choiceIndex, GameObject canvasObj)
    {
        bool isCorrect = (choiceIndex == correctAnswerIndex);
        
        // Show result
        GameObject resultObj = new GameObject("ResultText");
        resultObj.transform.SetParent(canvasObj.transform, false);
        TMPro.TextMeshProUGUI resultText = resultObj.AddComponent<TMPro.TextMeshProUGUI>();
        
        if (isCorrect)
        {
            resultText.text = "Correct! You've gained spiritual wisdom!";
            resultText.color = Color.green;
            
            // Add points
            if (gameManager != null)
            {
                gameManager.AddPoints(pointsForCorrectAnswer);
            }
        }
        else
        {
            resultText.text = "That's not quite right. The correct answer was: " + answerOptions[correctAnswerIndex];
            resultText.color = Color.red;
        }
        
        resultText.fontSize = 24;
        resultText.alignment = TMPro.TextAlignmentOptions.Center;
        RectTransform resultRect = resultText.GetComponent<RectTransform>();
        resultRect.anchorMin = new Vector2(0.2f, 0.1f);
        resultRect.anchorMax = new Vector2(0.8f, 0.2f);
        resultRect.offsetMin = Vector2.zero;
        resultRect.offsetMax = Vector2.zero;
        
        // Destroy canvas after delay
        Destroy(canvasObj, 3f);
        
        // Mark as collected
        hasBeenCollected = true;
        isInteracting = false;
        
        // Play collection effect
        PlayCollectionEffect();
        
        // Hide the star
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        
        // Notify any listeners
        OnChoiceMade?.Invoke(choiceIndex);
        
        // Update GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StarCollected();
        }
    }
    
    void PlayCollectionEffect()
    {
        if (collectEffectPrefab != null)
        {
            // Instantiate the effect at the star's position
            GameObject effect = Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
            
            // Add a light if requested
            if (addLightToEffect && effect.GetComponent<Light>() == null)
            {
                Light light = effect.AddComponent<Light>();
                light.color = lightColor;
                light.intensity = lightIntensity;
                light.range = 5f;
            }
            
            // Destroy the effect after the specified duration
            Destroy(effect, effectDuration);
        }
        else
        {
            Debug.LogWarning("No collection effect prefab assigned to star!");
        }
    }
} 