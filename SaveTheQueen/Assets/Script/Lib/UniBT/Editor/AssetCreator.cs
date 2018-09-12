﻿using UnityEngine;
using UnityEditor;

namespace Lib.UniBt.Editor
{
    public static class AssetCreator
    {
        [MenuItem("Assets/Create/UniBt/Behavior Trees")]
        public static void CreateBehaviorTrees()
        {
            // This code is borrowed from ICode(https://www.assetstore.unity3d.com/en/#!/content/13761)
            BehaviorTrees bt = AssetCreator.CreateAsset<BehaviorTrees>(false);
            if (bt != null)
            {
                bt.Name = bt.name;
                Root root = BehaviorTreesEditorUtility.AddNode<Root>(BehaviorTreesEditor.center, bt);
                bt.rootNode = root;
                root.Name = "Root";
                //AssetCreator.SaveAIAsset();
                SaveAIAsset();
            }
        }

        public static T CreateAsset<T>(bool displayFilePanel) where T : ScriptableObject
        {
            // This code is borrowed from ICode(https://www.assetstore.unity3d.com/en/#!/content/13761)
            if (displayFilePanel)
            {
                T asset = null;
                string path = EditorUtility.SaveFilePanelInProject(
                    "Create Asset of type " + typeof(T).Name,
                    "New " + typeof(T).Name + ".asset",
                    "asset", "");
                asset = CreateAsset<T>(path);
                return asset;
            }
            return CreateAsset<T>();
        }

        public static T CreateAsset<T>() where T : ScriptableObject
        {
            // This code is borrowed from ICode(https://www.assetstore.unity3d.com/en/#!/content/13761)
            T asset = null;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (path == "")
                path = "Assets";
            else if (System.IO.Path.GetExtension(path) != "")
                path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).Name + ".asset");
            asset = CreateAsset<T>(assetPathAndName);
            return asset;
        }

        public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            // This code is borrowed from ICode(https://www.assetstore.unity3d.com/en/#!/content/13761)
            if (string.IsNullOrEmpty(path))
                return null;

            T data = null;
            data = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(data, path);
            //AssetCreator.SaveAIAsset();
            SaveAIAsset();
            return data;
        }

        public static void SaveAIAsset()
        {
            //
            EditorUtility.SetDirty(BehaviorTreesEditor.active);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        public static void Refresh()
        {
            //AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
}
