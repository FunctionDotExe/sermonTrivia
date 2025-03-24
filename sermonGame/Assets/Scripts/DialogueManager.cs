using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Singleton instance
    public static DialogueManager Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;
    
    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;
    public float textMargin = 50f; // Margin for text to prevent edge clipping
    
    private Transform playerTransform;
    private Transform currentNPCTransform;
    private DialogueScenario currentScenario;
    private Coroutine typingCoroutine;
    private bool isInDialogue = false;
    
    // Reference to player controller components
    private MonoBehaviour playerMovementScript;
    private MonoBehaviour playerLookScript;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Hide dialogue panel initially
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Dialogue panel reference is missing!");
        }
        
        // Setup choice buttons with proper initialization
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int choiceIndex = i; // Capture the index for the lambda
            Button button = choiceButtons[i];
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                button.interactable = true;
            }
        }
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            
            // Find player controller components
            // These are common names for movement/look scripts - adjust based on your project
            playerMovementScript = playerObj.GetComponent("FirstPersonController") as MonoBehaviour;
            if (playerMovementScript == null)
                playerMovementScript = playerObj.GetComponent("PlayerController") as MonoBehaviour;
                
            playerLookScript = playerObj.GetComponent("MouseLook") as MonoBehaviour;
            
            Debug.Log("Found player movement script: " + (playerMovementScript != null));
            Debug.Log("Found player look script: " + (playerLookScript != null));
        }
    }
    
    public void StartDialogue(DialogueScenario scenario, Transform npcTransform)
    {
        if (isInDialogue) return;
        
        Debug.Log("Starting dialogue with: " + scenario.npcName);
        
        isInDialogue = true;
        currentScenario = scenario;
        currentNPCTransform = npcTransform;
        
        // Show dialogue panel
        dialoguePanel.SetActive(true);
        
        // Make sure cursor is visible and unlocked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Set NPC name
        if (npcNameText != null)
        {
            npcNameText.text = scenario.npcName;
        }
        
        // Type out the dialogue text
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeDialogue(scenario.dialogueText));
        
        // Setup choice buttons
        SetupChoiceButtons(scenario.choices);
        
        // Freeze player movement
        FreezePlayerMovement();
    }
    
    void FreezePlayerMovement()
    {
        // Try to disable player movement using common methods
        if (playerMovementScript != null)
        {
            // Try different common methods to disable movement
            playerMovementScript.SendMessage("EnableMovement", false, SendMessageOptions.DontRequireReceiver);
            playerMovementScript.SendMessage("SetMovementEnabled", false, SendMessageOptions.DontRequireReceiver);
            
            // Try to disable the script directly
            playerMovementScript.enabled = false;
        }
        
        // Try to disable player look
        if (playerLookScript != null)
        {
            playerLookScript.enabled = false;
        }
        
        // Try to find and disable Rigidbody
        if (playerTransform != null)
        {
            Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            
            // Try to find and disable CharacterController
            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
            }
        }
        
        Debug.Log("Player movement frozen");
    }
    
    void UnfreezePlayerMovement()
    {
        // Re-enable player movement
        if (playerMovementScript != null)
        {
            // Try different common methods to enable movement
            playerMovementScript.SendMessage("EnableMovement", true, SendMessageOptions.DontRequireReceiver);
            playerMovementScript.SendMessage("SetMovementEnabled", true, SendMessageOptions.DontRequireReceiver);
            
            // Re-enable the script directly
            playerMovementScript.enabled = true;
        }
        
        // Re-enable player look
        if (playerLookScript != null)
        {
            playerLookScript.enabled = true;
        }
        
        // Re-enable Rigidbody
        if (playerTransform != null)
        {
            Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            
            // Re-enable CharacterController
            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = true;
            }
        }
        
        Debug.Log("Player movement unfrozen");
    }
    
    IEnumerator TypeDialogue(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
    void SetupChoiceButtons(DialogueChoice[] choices)
    {
        // Hide all buttons first
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }
        
        // Show and setup buttons for available choices
        for (int i = 0; i < choices.Length && i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].interactable = true;
            choiceTexts[i].text = choices[i].choiceText;
        }
    }
    
    void OnChoiceSelected(int choiceIndex)
    {
        if (currentScenario == null || choiceIndex >= currentScenario.choices.Length) return;
        
        DialogueChoice selectedChoice = currentScenario.choices[choiceIndex];
        
        // Award points using ScoreHelper
        ScoreHelper.AwardPoints(selectedChoice.pointsAwarded);
        
        // Show feedback based on choice
        string feedbackText = selectedChoice.isCorrect ? 
            "Correct choice! +" + selectedChoice.pointsAwarded + " points" : 
            "That wasn't the best choice. " + selectedChoice.pointsAwarded + " points";
            
        dialogueText.text = feedbackText + "\n\n" + selectedChoice.responseText;
        
        // Disable choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }
        
        // Show response and end dialogue after delay
        StartCoroutine(EndDialogueAfterDelay(3f));
    }
    
    IEnumerator EndDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndDialogue();
    }
    
    void EndDialogue()
    {
        // Hide dialogue panel
        dialoguePanel.SetActive(false);
        
        // Unfreeze player movement
        UnfreezePlayerMovement();
        
        // Reset cursor state
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Reset dialogue state
        isInDialogue = false;
        currentScenario = null;
        currentNPCTransform = null;
    }
}

// Classes to define dialogue scenarios
[System.Serializable]
public class DialogueScenario
{
    public string npcName;
    public string dialogueText;
    public DialogueChoice[] choices;
    
    public DialogueScenario(string npcName, string dialogueText, DialogueChoice[] choices)
    {
        this.npcName = npcName;
        this.dialogueText = dialogueText;
        this.choices = choices;
    }
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public string responseText;
    public int pointsAwarded;
    public bool isCorrect;
    
    public DialogueChoice(string choiceText, string responseText, int pointsAwarded, bool isCorrect)
    {
        this.choiceText = choiceText;
        this.responseText = responseText;
        this.pointsAwarded = pointsAwarded;
        this.isCorrect = isCorrect;
    }
} 