using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class StarObject : MonoBehaviour
{
    public float glowIntensity = 1.5f;
    public float glowSpeed = 2f;
    public Color glowColor = Color.yellow;
    public string sceneToLoad = "QuizScene";
    public float timeBonus = 30f;
    public float timePenalty = -15f;
    public int pointValue = 10;
    
    private Material material;
    private ParticleSystem flameParticles;
    private ParticleSystem beaconParticles;
    private GameObject gameManagerObject;

    void Start()
    {
        // Setup material
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            material = new Material(rend.material);
            rend.material = material;
            
            // Make sure material can glow
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogError("No Renderer found on star object!");
        }

        // Find the GameManager GameObject in the scene
        gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject == null)
        {
            Debug.LogError("GameManager GameObject not found in scene!");
        }
    }

    void Update()
    {
        if (material != null)
        {
            // Create pulsing glow effect
            float glow = Mathf.Sin(Time.time * glowSpeed) * 0.5f + 0.5f;
            Color emissionColor = glowColor * glow * glowIntensity;
            material.SetColor("_EmissionColor", emissionColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManagerObject != null)
            {
                // Use SendMessage to call the AddPoints method
                gameManagerObject.SendMessage("AddPoints", pointValue, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                // Try to find it again in case it was added after Start
                gameManagerObject = GameObject.Find("GameManager");
                if (gameManagerObject != null)
                {
                    gameManagerObject.SendMessage("AddPoints", pointValue, SendMessageOptions.DontRequireReceiver);
                }
            }
            Destroy(gameObject);
        }
    }

    void UpdateScore(int points)
    {
        // Use the ScoreManager instead of directly accessing MultiplayerGameManager
        ScoreManager.Instance.AddPoints(points);
    }
}