using UnityEngine;
using UnityEngine.UI;

public class ButtonClickTest : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => {
                Debug.Log("Button clicked!");
            });
        }
    }
} 