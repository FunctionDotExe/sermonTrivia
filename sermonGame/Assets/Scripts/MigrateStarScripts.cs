using UnityEngine;

public class MigrateStarScripts : MonoBehaviour
{
    public void MigrateAllStars()
    {
        // Find all stars with the old script
        StarTriviaInteraction[] oldStars = FindObjectsOfType<StarTriviaInteraction>();
        
        Debug.Log($"Found {oldStars.Length} stars to migrate");
        
        foreach (StarTriviaInteraction oldStar in oldStars)
        {
            GameObject starObject = oldStar.gameObject;
            
            // Copy data from old script
            string question = oldStar.triviaQuestion;
            string[] answers = oldStar.answerOptions;
            int correctIndex = oldStar.correctAnswerIndex;
            int points = oldStar.pointsForCorrectAnswer;
            
            // Visual settings
            float rotSpeed = oldStar.rotationSpeed;
            Color color = oldStar.starColor;
            float pulseSpeed = oldStar.pulseSpeed;
            float pulseIntensity = oldStar.pulseIntensity;
            
            // Effect settings
            GameObject effectPrefab = oldStar.collectEffectPrefab;
            float duration = oldStar.effectDuration;
            bool addLight = oldStar.addLightToEffect;
            Color lightColor = oldStar.lightColor;
            float lightIntensity = oldStar.lightIntensity;
            
            // Remove old script
            DestroyImmediate(oldStar);
            
            // Add new script
            StarTriviaInteractionNew newStar = starObject.AddComponent<StarTriviaInteractionNew>();
            
            // Set properties
            newStar.triviaQuestion = question;
            newStar.answerOptions = answers;
            newStar.correctAnswerIndex = correctIndex;
            newStar.pointsForCorrectAnswer = points;
            
            // Visual settings
            newStar.rotationSpeed = rotSpeed;
            newStar.starColor = color;
            newStar.pulseSpeed = pulseSpeed;
            newStar.pulseIntensity = pulseIntensity;
            
            // Effect settings
            newStar.collectEffectPrefab = effectPrefab;
            newStar.effectDuration = duration;
            newStar.addLightToEffect = addLight;
            newStar.lightColor = lightColor;
            newStar.lightIntensity = lightIntensity;
            
            Debug.Log($"Migrated star: {starObject.name}");
        }
        
        Debug.Log("Migration complete!");
    }
} 