using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UISetup : MonoBehaviour
{
    void Start()
    {
        // Check if UI elements exist, create them if they don't
        if (GameObject.Find("ScoreText") == null)
        {
            CreateScoreText();
        }
        
        if (GameObject.Find("TimerText") == null)
        {
            CreateTimerText();
        }
    }
    
    void CreateScoreText()
    {
        // Create Canvas if it doesn't exist
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create Score Text
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(canvas.transform, false);
        TextMeshProUGUI scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "Score: 0";
        scoreText.fontSize = 24;
        scoreText.color = Color.white;
        
        // Position it in the top-left corner
        RectTransform rectTransform = scoreTextObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(20, -20);
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        Debug.Log("Created ScoreText UI element");
    }
    
    void CreateTimerText()
    {
        // Create Canvas if it doesn't exist
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create Timer Text
        GameObject timerTextObj = new GameObject("TimerText");
        timerTextObj.transform.SetParent(canvas.transform, false);
        TextMeshProUGUI timerText = timerTextObj.AddComponent<TextMeshProUGUI>();
        timerText.text = "Time: 05:00";
        timerText.fontSize = 24;
        timerText.color = Color.white;
        
        // Position it in the top-right corner
        RectTransform rectTransform = timerTextObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = new Vector2(-20, -20);
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        Debug.Log("Created TimerText UI element");
    }
} 