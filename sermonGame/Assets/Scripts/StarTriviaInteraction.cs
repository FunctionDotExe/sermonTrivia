using UnityEngine;
using UnityEngine.Events;

public class StarTriviaInteraction : NPCInteraction
{
    [Header("Trivia Settings")]
    public string triviaQuestion;
    [TextArea(2, 5)]
    public string[] answerOptions = new string[4];
    public int correctAnswerIndex;
    public int pointsForCorrectAnswer = 10;
    
    [Header("Visual Settings")]
    public float rotationSpeed = 50f;
    public Color starColor = Color.yellow;
    public float pulseSpeed = 1f;
    public float pulseIntensity = 0.2f;
    
    [Header("Collection Effect")]
    public GameObject collectEffectPrefab; // Assign your particle effect prefab here
    public float effectDuration = 3f;
    public bool addLightToEffect = true;
    public Color lightColor = new Color(1f, 0.7f, 0.3f, 1f); // Orange-yellow light color
    public float lightIntensity = 2f;
    
    private bool hasBeenCollected = false;
    private Material starMaterial;
    private bool isInteracting = false;
    
    public UnityEvent<int> OnChoiceMade = new UnityEvent<int>();
    private GameManager gameManager;
    
    // Use 'new' keyword to explicitly hide the base class field
    // Only uncomment if you need to override the base class field
    // [SerializeField] new private int correctChoiceIndex = 0;

    void Awake()
    {
        // Set up the NPC interaction properties
        npcName = "Star Trivia";
        dialogueText = triviaQuestion;
        
        // Set up the choices
        choiceTexts = new string[answerOptions.Length];
        responseTexts = new string[answerOptions.Length];
        
        for (int i = 0; i < answerOptions.Length; i++)
        {
            choiceTexts[i] = answerOptions[i];
            
            if (i == correctAnswerIndex)
            {
                responseTexts[i] = "Correct! You've gained spiritual wisdom!";
            }
            else
            {
                responseTexts[i] = "That's not quite right. The correct answer was: " + answerOptions[correctAnswerIndex];
            }
        }
    }
    
    protected override void Start()
    {
        base.Start();
        
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
            
            // Find the DialogueManager
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            
            if (dialogueManager != null)
            {
                // Start checking for dialogue completion
                StartCoroutine(CheckDialogueCompletion(dialogueManager));
                
                // Try to start the dialogue directly
                TriggerDialogue(dialogueManager);
            }
        }
    }
    
    void TriggerDialogue(DialogueManager dialogueManager)
    {
        // Try different approaches to trigger the dialogue
        
        // Approach 1: Use reflection to find and call a method in NPCInteraction
        System.Type npcType = typeof(NPCInteraction);
        System.Reflection.MethodInfo[] methods = npcType.GetMethods(
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.NonPublic);
        
        foreach (System.Reflection.MethodInfo method in methods)
        {
            // Look for methods that might trigger dialogue
            if (method.Name.Contains("Interact") || method.Name.Contains("Dialogue") || 
                method.Name.Contains("Talk") || method.Name.Contains("Start"))
            {
                try
                {
                    Debug.Log("Trying to call method: " + method.Name);
                    method.Invoke(this, null);
                    break;
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Failed to call " + method.Name + ": " + e.Message);
                }
            }
        }
        
        // Approach 2: Try to call StartDialogue directly on the DialogueManager
        try
        {
            // Create a DialogueScenario
            DialogueScenario scenario = new DialogueScenario(npcName, dialogueText, CreateChoices());
            
            // Try to call StartDialogue with different parameter combinations
            System.Reflection.MethodInfo startDialogueMethod = dialogueManager.GetType().GetMethod("StartDialogue");
            
            if (startDialogueMethod != null)
            {
                System.Reflection.ParameterInfo[] parameters = startDialogueMethod.GetParameters();
                
                if (parameters.Length == 1)
                {
                    startDialogueMethod.Invoke(dialogueManager, new object[] { scenario });
                }
                else if (parameters.Length == 2)
                {
                    startDialogueMethod.Invoke(dialogueManager, new object[] { scenario, transform });
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to start dialogue directly: " + e.Message);
        }
    }
    
    DialogueChoice[] CreateChoices()
    {
        DialogueChoice[] choices = new DialogueChoice[answerOptions.Length];
        
        for (int i = 0; i < answerOptions.Length; i++)
        {
            string responseText = (i == correctAnswerIndex) 
                ? "Correct! You've gained spiritual wisdom!" 
                : "That's not quite right. The correct answer was: " + answerOptions[correctAnswerIndex];
                
            bool isCorrect = (i == correctAnswerIndex);
            choices[i] = new DialogueChoice(
                answerOptions[i],  // choiceText
                responseText,      // responseText
                isCorrect ? pointsForCorrectAnswer : 0,  // pointValue
                isCorrect          // isCorrect
            );
        }
        
        return choices;
    }
    
    System.Collections.IEnumerator CheckDialogueCompletion(DialogueManager dialogueManager)
    {
        // Wait a moment for dialogue to start
        yield return new WaitForSeconds(0.5f);
        
        // Wait until the dialogue panel is no longer active
        while (dialogueManager.dialoguePanel.activeSelf)
        {
            yield return null;
        }
        
        // Mark as collected
        hasBeenCollected = true;
        isInteracting = false;
        
        // Play collection effect
        PlayCollectionEffect();
        
        // Hide the star
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        
        // Optional: Add points or track collection
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

    // Create a new method with a different name to avoid conflicts
    protected void OnChoiceSelected(int choiceIndex)
    {
        // Add our score logic
        if (gameManager != null && pointsAwarded.Length > choiceIndex)
        {
            Debug.Log($"Adding {pointsAwarded[choiceIndex]} points for choice {choiceIndex}");
            gameManager.AddPoints(pointsAwarded[choiceIndex]);
        }
        else
        {
            if (gameManager == null)
            {
                Debug.LogError("Cannot add points - GameManager is null!");
                // Try to find it again
                gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null && pointsAwarded.Length > choiceIndex)
                {
                    gameManager.AddPoints(pointsAwarded[choiceIndex]);
                }
            }
        }
        
        OnChoiceMade?.Invoke(choiceIndex);
    }
}