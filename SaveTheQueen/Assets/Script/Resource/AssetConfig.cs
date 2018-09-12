using UnityEngine;


public enum E_AssetType
{
    None,
    AssetBundle,
    AnimationGraph,
    Database,
    VFX,
    AudioClip,
    Prefab,
    Skeleton,
    SkinnedMesh,
    Texture,
    Text,
    Binary,
}

public static class AssetPathSettings
{

    // VFX
    public static string VFXRootPath = "Assets/AssetBundle/Fx/";
    public static string DataTableRootPath = "Assets/Resources/ExcelTable/";
    //public static string VFXResourcesRootPath = "Assets/Resources/Fx/";
    public static string VFXExtension = "prefab";

    // Audio(BGM and SFX)
    public static string AudioRootPath = "Assets/AssetBundle/";
    public static string AudioExtension = "mp3";
    public static string BGMRootPath = "Assets/AssetBundle/Sounds/BGM/";

    public static string BGMExtension = "wav";   // <- (.meta files refer to mp3 encoding)

    public static string SFXRootPath = "Assets/AssetBundle/Sounds/";
    public static string SFXAssetRootPath = "Assets/AssetBundle/Sounds/Asset/";
    public static string SFXMIXAssetRootPath = "Assets/AssetBundle/Sounds/Asset/AudioSource";
    public static string SFXExtension = "wav";
    public static string SFXExtension2 = ".wav";
    //public static string SFXPackageResourceRootPath = "Assets/Resources/Sounds/";
    public static string SFXPackageExtension = "asset";

    public static string videoExtension = ".mp4"; // <- used for all platforms

    public static string DataTableExtension = "bytes";

    public static string WndPath = "Assets/Resources/UI/Prefabs";

}

public static class AssetConfig
{
    public static readonly string WebPlayerAssetPath = "Assetbundles/WebPlayer/";
    public static readonly string StandAloneAssetPath = "Assetbundles/StandAlone/";
    public static readonly string AndroidAssetPath = "Assetbundles/Android/";
    public static readonly string AndroidDstAssetPath = "Assetbundles/Android/";        // <- build to here
    public static readonly string AndroidSrcAssetPath = "Assets/StreamingAssets/";      // <- load from here
    public static readonly string iOSAssetPath = "Assetbundles/iOS/";
    public static readonly string iOSDstAssetPath = "Assetbundles/iOS/";            // <- build to here
    public static readonly string iOSSrcAssetPath = "Assets/StreamingAssets/";      // <- load from here

    public static readonly string WebPlayerPlayerExtension = "unity3d";
    public static readonly string StandAlonePlayerExtension = "exe";
    public static readonly string AndroidPlayerExtension = "apk";
    public static readonly string iOSPlayerExtension = "ipa";

    public static readonly string AssetExtension = ".sasset";

    public static string rootPath = string.Empty;
    public static bool useWWW = false;

    public static string GetAssetRootPath(RuntimePlatform platform)
    {
        if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXPlayer)
        {
            return AssetConfig.WebPlayerAssetPath;
        }
        else if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXPlayer)
        {
            return AssetConfig.StandAloneAssetPath;
        }
        else if (platform == RuntimePlatform.Android)
        {
            //#if USE_PATCH_SYSTEM
            return AssetConfig.AndroidAssetPath;
            //#else
            //            return AssetConfig.AndroidSrcAssetPath;
            //#ensif //AndroidAssetPath
        }
        else if (platform == RuntimePlatform.IPhonePlayer)
        {
            return AssetConfig.iOSAssetPath;
        }
        else
        {
            return AssetConfig.StandAloneAssetPath;
        }
    }

    public static string AssetbundleExtension
    {
        get
        {
            return AssetConfig.AssetExtension;
        }
    }


    public static string VideoFileExtension
    {
        get
        {
            return AssetPathSettings.videoExtension;
        }
    }

    public static string GetStreamingAssetUrl(RuntimePlatform platform, string assetURL)
    {
        string absoluteURL = assetURL;
        if (absoluteURL.IndexOf("file://") == 0 || absoluteURL.IndexOf("http://") == 0)
        {
            absoluteURL = assetURL;
        }
        else
        {
            if (rootPath != string.Empty)
            {
                absoluteURL = rootPath + assetURL;
            }
            else
            {
                //Debug.Log("!!!! platform = " + platform.ToString());

                string assetRootPath = GetAssetRootPath(platform);
                if (platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.WindowsPlayer)
                {
                    // "file://" was twice in the URL string
                    //absoluteURL = "file://" + Application.dataPath + "/../" + assetRootPath + assetURL;
                    absoluteURL = "" + Application.dataPath + "/" + assetRootPath + assetURL;
                }
                else if (platform == RuntimePlatform.Android)
                {
                    absoluteURL = Application.streamingAssetsPath + "/" + assetURL;
                }
                else if (platform == RuntimePlatform.IPhonePlayer)
                {
                    absoluteURL = "file://" + Application.streamingAssetsPath + "/" + assetURL;
                }
                else if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXPlayer)
                {
                    absoluteURL = Application.streamingAssetsPath + "/" + assetURL;
                    //absoluteURL = assetRootPath + assetURL.ToLower();
                }
                else if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.WindowsEditor)
                {
                    if (useWWW)
                    {
                        absoluteURL = "file://" + Application.streamingAssetsPath + "/" + assetURL;
                    }
                    else
                    {
                        absoluteURL = Application.streamingAssetsPath + "/" + assetURL;
                    }
                    //absoluteURL = assetRootPath + assetURL.ToLower();
                }
                else
                {
                    absoluteURL = assetURL;
                }
            }
        }
        return absoluteURL;
    }

    public static string GetAbsoluteURL(RuntimePlatform platform, string assetURL)
    {
        string absoluteURL = assetURL;
        if (absoluteURL.IndexOf("file://") == 0 || absoluteURL.IndexOf("http://") == 0)
        {
            absoluteURL = assetURL;
        }
        else
        {
            if (rootPath != string.Empty)
            {
                absoluteURL = rootPath + assetURL;
            }
            else
            {
                //Debug.Log("!!!! platform = " + platform.ToString());

                string assetRootPath = GetAssetRootPath(platform);
                if (platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.WindowsPlayer)
                {
                    // "file://" was twice in the URL string
                    //absoluteURL = "file://" + Application.dataPath + "/../" + assetRootPath + assetURL;
                    absoluteURL = Application.dataPath + "/" + assetRootPath + assetURL;
                }
                else if (platform == RuntimePlatform.Android)
                {
                    absoluteURL = "file://" + GetPatchAbsoluteAssetBundlePath(assetURL);
                }
                else if (platform == RuntimePlatform.IPhonePlayer)
                {
                    absoluteURL = "file://" + GetPatchAbsoluteAssetBundlePath(assetURL);
                }
                else if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXPlayer)
                {
                    absoluteURL = Application.streamingAssetsPath + "/" + assetURL;
                    //absoluteURL = assetRootPath + assetURL.ToLower();
                }
                else if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.WindowsEditor)
                {
                    if (useWWW)
                    {
                        absoluteURL = "file://" + Application.dataPath + "/../" + assetRootPath + assetURL;
                    }
                    else
                    {
                        absoluteURL = Application.dataPath + "/../" + assetRootPath + assetURL;
                    }
                    //absoluteURL = assetRootPath + assetURL.ToLower();
                }
                else
                {
                    absoluteURL = assetURL;
                }
            }
        }
        return absoluteURL;
    }


    public static string GetPatchAbsoluteAssetBundlePath(string filePath)
    {
        string root;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            root = Application.temporaryCachePath;
            filePath = filePath.ToLower();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            root = Application.persistentDataPath;
            filePath = filePath.ToLower();
        }
        else
        {
            root = Application.dataPath;
            root = root.Substring(0, root.LastIndexOf('/'));
        }

        root = root + "/" + AssetConfig.GetAssetRootPath(Application.platform);
        root += filePath;
        return root;
    }

}