using UnityEngine;
using UnityEditor;

public class LevelData
{
    [MenuItem("Assets/Create/Level")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Level>();
    }
}

