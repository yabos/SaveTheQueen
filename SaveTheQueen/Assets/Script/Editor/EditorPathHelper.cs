using UnityEditor;

public static class EditorPathHelper
{
    public const string FbxAssetPath = "FBXAssets/";
    public const string AssetBundlePath = "AssetBundle/";

    public static string ConvertToFbxAssetPath(string path)
    {
        if (path.Contains("Actor/"))
            return path.Replace(AssetBundlePath, FbxAssetPath);

        return path;
    }

    public static string ConvertToAssetBundlePath(string path)
    {
        if (path.Contains("Actor/"))
            return path.Replace(FbxAssetPath, AssetBundlePath);

        return path;
    }
}