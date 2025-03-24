using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    private SimpleMultiplayerManager gameManager;
    
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ScoreManager");
                    _instance = obj.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        gameManager = FindObjectOfType<SimpleMultiplayerManager>();
    }
    
    public void AddScore(int points)
    {
        if (gameManager != null)
        {
            gameManager.AddPoints(points);
        }
        else
        {
            Debug.LogWarning("ScoreManager: SimpleMultiplayerManager not found!");
        }
    }
} 