using UnityEngine;
using UnityEngine.SceneManagement;

public class StarObject : MonoBehaviour
{
    public float glowIntensity = 1.5f;
    public float glowSpeed = 2f;
    public Color glowColor = Color.yellow;
    public string sceneToLoad = "QuizScene";
    public float timeBonus = 30f;
    public float timePenalty = -15f;
    
    private Material material;
    private ParticleSystem flameParticles;
    private ParticleSystem beaconParticles;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit star!");
            
            // Prepare GameManager for scene transition
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PrepareForQuizScene(gameObject, other.gameObject);
            }
            
            // Load quiz scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}