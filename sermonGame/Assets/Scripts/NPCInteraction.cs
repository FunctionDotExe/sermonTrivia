using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "NPC";
    [TextArea(3, 10)]
    public string dialogueText = "Hello there!";
    
    [Header("Dialogue Choices")]
    public string[] choiceTexts = new string[4];
    [TextArea(2, 5)]
    public string[] responseTexts = new string[4];
    public int[] pointsAwarded = new int[4];
    public int correctChoiceIndex = 3; // Default to the last choice being correct
    
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public bool canInteractMultipleTimes = false;
    public bool useColliderTrigger = true; // Use trigger collider for interaction
    
    private bool hasInteracted = false;
    private DialogueScenario scenario;
    
    // Change to protected so derived classes can access it
    protected virtual void Start()
    {
        // Create dialogue scenario
        CreateDialogueScenario();
        
        // Add a collider if needed
        if (useColliderTrigger && GetComponent<Collider>() == null)
        {
            SphereCollider col = gameObject.AddComponent<SphereCollider>();
            col.radius = interactionDistance;
            col.isTrigger = true;
        }
        
        // Make sure collider is a trigger
        Collider col2 = GetComponent<Collider>();
        if (col2 != null && useColliderTrigger)
        {
            col2.isTrigger = true;
        }
    }
    
    void CreateDialogueScenario()
    {
        DialogueChoice[] choices = new DialogueChoice[choiceTexts.Length];
        
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            bool isCorrect = (i == correctChoiceIndex);
            choices[i] = new DialogueChoice(
                choiceTexts[i],
                responseTexts[i],
                pointsAwarded[i],
                isCorrect
            );
        }
        
        scenario = new DialogueScenario(npcName, dialogueText, choices);
    }
    
    // This will be called by the PlayerController
    public void TriggerInteraction()
    {
        if (hasInteracted && !canInteractMultipleTimes)
            return;
            
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(scenario, transform);
            
            // Mark as interacted if can't interact multiple times
            if (!canInteractMultipleTimes)
            {
                hasInteracted = true;
            }
        }
    }
    
    // Trigger interaction when player enters the collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && useColliderTrigger)
        {
            Debug.Log("Player entered NPC trigger zone: " + gameObject.name);
            TriggerInteraction();
        }
    }
    
    // Optional: Add a visual indicator for interaction range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
    
    // Add this method to handle choice selection
    public void HandleChoiceSelected(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= pointsAwarded.Length) return;
        
        // Award points through our ScoreHelper
        ScoreHelper.AwardPoints(pointsAwarded[choiceIndex]);
        
        Debug.Log($"NPCInteraction: Choice {choiceIndex} selected, awarded {pointsAwarded[choiceIndex]} points");
    }
}