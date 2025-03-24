using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Star,    // Trivia
        NPC      // Dialogue
    }

    public InteractionType type;
    public string questionSceneName = "QuizScene";
    public string dialogueSceneName = "DialogueScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == InteractionType.Star)
            {
                SceneManager.LoadScene(questionSceneName);
            }
            else
            {
                SceneManager.LoadScene(dialogueSceneName);
            }
        }
    }
} 