using UnityEngine;
using UnityEngine.SceneManagement;

public class StarInteraction : MonoBehaviour
{
    // This will be called when the player interacts with the star
    public void Interact()
    {
        Debug.Log("Star interaction detected - loading QuizScene");
        
        // Simply load the QuizScene directly
        SceneManager.LoadScene("QuizScene");
    }
}
