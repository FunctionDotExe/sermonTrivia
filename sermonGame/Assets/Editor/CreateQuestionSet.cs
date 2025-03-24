using UnityEngine;
using UnityEditor;

public class CreateQuestionSet
{
    [MenuItem("Assets/Create/Quiz/Sample Question Set")]
    public static void CreateSampleQuestionSet()
    {
        // Create a simple ScriptableObject instead
        ScriptableObject sampleAsset = ScriptableObject.CreateInstance<ScriptableObject>();
        
        // Save it to the project
        AssetDatabase.CreateAsset(sampleAsset, "Assets/QuestionSets/SampleAsset.asset");
        AssetDatabase.SaveAssets();
        
        // Log success message
        Debug.Log("Created sample asset at Assets/QuestionSets/SampleAsset.asset");
        
        // Select the created asset
        Selection.activeObject = sampleAsset;
    }
}