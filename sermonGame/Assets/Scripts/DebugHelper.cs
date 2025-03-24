using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Game started. Player position: " + transform.position);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed at position: " + transform.position);
            
            // Cast a ray forward to see what's in front
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " at distance " + hit.distance);
            }
            else
            {
                Debug.Log("Raycast hit nothing");
            }
        }
    }
} 