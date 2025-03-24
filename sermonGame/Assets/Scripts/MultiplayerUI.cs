using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiplayerUI : MonoBehaviour
{
    [Header("Player Indicators")]
    public Color[] playerColors = new Color[4] {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };
    public GameObject playerIndicatorPrefab;
    public Transform playerIndicatorParent;
    
    [Header("UI References")]
    public GameObject buttonPanel;
    public GameObject gameHUD;
    
    void Start()
    {
        // Create player indicators
        CreatePlayerIndicators();
        
        // Set up initial UI state
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(true);
        }
        
        if (gameHUD != null)
        {
            gameHUD.SetActive(true);
        }
    }
    
    void CreatePlayerIndicators()
    {
        if (playerIndicatorPrefab == null || playerIndicatorParent == null)
            return;
            
        MultiplayerGameManager gameManager = MultiplayerGameManager.Instance;
        if (gameManager == null)
            return;
            
        // Create indicators for each player
        for (int i = 0; i < gameManager.numberOfPlayers; i++)
        {
            GameObject indicator = Instantiate(playerIndicatorPrefab, playerIndicatorParent);
            
            // Set color
            Image indicatorImage = indicator.GetComponent<Image>();
            if (indicatorImage != null && i < playerColors.Length)
            {
                indicatorImage.color = playerColors[i];
            }
            
            // Set text
            TextMeshProUGUI indicatorText = indicator.GetComponentInChildren<TextMeshProUGUI>();
            if (indicatorText != null)
            {
                indicatorText.text = "P" + (i + 1);
            }
            
            // Hide initially
            indicator.SetActive(false);
            
            // Add to game manager
            if (i < gameManager.playerIndicators.Length)
            {
                gameManager.playerIndicators[i] = indicator;
            }
        }
    }
} 