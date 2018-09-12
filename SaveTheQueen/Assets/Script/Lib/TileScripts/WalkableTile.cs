using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WalkableTile : Tile
{
    public int m_MoveCost = 1;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Walkable Tile")]
    public static void CreateWalkableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Walkable Tile", "New Walkable Tile", "asset", "Save Walkable Tile", "Assets");

        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WalkableTile>(), path);
    }
#endif
}
