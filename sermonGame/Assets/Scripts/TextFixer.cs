using UnityEngine;
using TMPro;

public class TextFixer : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    
    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        FixTextSettings();
    }
    
    public void FixTextSettings()
    {
        if (textComponent != null)
        {
            Debug.Log("Fixing text settings for: " + textComponent.name);
            
            // Fix text wrapping
            textComponent.enableWordWrapping = true;
            textComponent.overflowMode = TextOverflowModes.Overflow;
            
            // Fix spacing
            textComponent.lineSpacing = 0;
            textComponent.paragraphSpacing = 0;
            textComponent.characterSpacing = 0;
            textComponent.wordSpacing = 0;
            
            // Fix alignment
            textComponent.alignment = TextAlignmentOptions.TopLeft;
            
            // Force update
            textComponent.ForceMeshUpdate();
        }
    }
} 