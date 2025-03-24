using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private int pointsValue = 10;
    [SerializeField] private bool canInteractMultipleTimes = false;
    private bool hasBeenInteracted = false;
    private SimpleMultiplayerManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<SimpleMultiplayerManager>();
    }
    
    public void Interact()
    {
        if (hasBeenInteracted && !canInteractMultipleTimes) return;
        
        if (gameManager != null)
        {
            gameManager.AddScore(pointsValue);
            Debug.Log($"Interacted with {gameObject.name} - Added {pointsValue} points");
        }
        
        hasBeenInteracted = true;
    }
} 