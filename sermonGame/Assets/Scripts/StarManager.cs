using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StarManager : MonoBehaviour
{
    public List<StarObject> stars = new List<StarObject>();
    
    void Start()
    {
        // Find all stars in the scene if not manually assigned
        if (stars.Count == 0)
        {
            StarObject[] foundStars = FindObjectsOfType<StarObject>();
            stars.AddRange(foundStars);
        }
        
        // Initialize stars
        foreach (StarObject star in stars)
        {
            if (star != null)
            {
                // Make sure the star has a valid scene to load
                if (string.IsNullOrEmpty(star.sceneToLoad))
                {
                    star.sceneToLoad = "QuizScene";
                }
            }
        }
    }
    
    // Method to activate/deactivate stars
    public void SetStarsActive(bool active)
    {
        foreach (StarObject star in stars)
        {
            if (star != null)
            {
                star.gameObject.SetActive(active);
            }
        }
    }
    
    // Method to get a random star
    public StarObject GetRandomStar()
    {
        if (stars.Count == 0) return null;
        
        int randomIndex = Random.Range(0, stars.Count);
        return stars[randomIndex];
    }
} 