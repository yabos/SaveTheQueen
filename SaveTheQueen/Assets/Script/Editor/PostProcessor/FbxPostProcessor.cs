using UnityEditor;
using UnityEngine;

public static class FbxPostProcessor
{
    [MenuItem("Assets/Joker/FBX to Prefabs")]
    public static void FbxToCreatePrefabs()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
            return;

        foreach (var fbxObject in Selection.gameObjects)
        {
            if (PrefabUtility.GetPrefabType(fbxObject) != PrefabType.ModelPrefab)
            {
                UnityEngine.Debug.LogError("FbxToCreatePrefabs Not FBX Name : " + fbxObject.name);
                continue;
            }

            string path = AssetDatabase.GetAssetPath(fbxObject);
        }
    }

}