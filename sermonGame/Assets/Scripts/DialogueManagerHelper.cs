using UnityEngine;
using System.Reflection;

public class DialogueManagerHelper : MonoBehaviour
{
    void Start()
    {
        // Find the DialogueManager
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        
        if (dialogueManager != null)
        {
            // Get all methods named StartDialogue
            MethodInfo[] methods = dialogueManager.GetType().GetMethods();
            
            foreach (MethodInfo method in methods)
            {
                if (method.Name == "StartDialogue")
                {
                    Debug.Log("Found StartDialogue method:");
                    
                    // Get parameters
                    ParameterInfo[] parameters = method.GetParameters();
                    string paramInfo = "Parameters: ";
                    
                    foreach (ParameterInfo param in parameters)
                    {
                        paramInfo += param.ParameterType.Name + " " + param.Name + ", ";
                    }
                    
                    Debug.Log(paramInfo);
                }
            }
        }
    }
} 