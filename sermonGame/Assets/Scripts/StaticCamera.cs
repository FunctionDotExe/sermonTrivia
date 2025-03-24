using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    // This script does nothing but exist to mark this as a static camera
    // The camera will stay exactly where you place it in the scene
    
    void Start()
    {
        Debug.Log("Static camera initialized - will not move");
    }
} 