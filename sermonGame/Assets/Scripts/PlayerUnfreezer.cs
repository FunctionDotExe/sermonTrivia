using UnityEngine;

public class PlayerUnfreezer : MonoBehaviour
{
    void Start()
    {
        // Enable all components on the player
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            component.enabled = true;
        }
        
        // Make sure the player isn't kinematic if it has a Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            Debug.Log("Unfroze Rigidbody");
        }
        
        // Enable character controller if present
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = true;
            Debug.Log("Enabled CharacterController");
        }
        
        Debug.Log("Player unfreezer ran");
    }
} 