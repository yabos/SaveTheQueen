using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
//using UnityEditor.BB10;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpriteImportProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets, string[] movedFromAssetPaths
        )
    {
        //         foreach (string asset in importedAssets)
        //         {
        //             Debug.Log("추가되거나 수정된 애셋: " + asset);
        //         }

        //         foreach (string asset in deletedAssets)
        //         {
        //             Debug.Log("삭제된 애셋: " + asset);
        //         }

        //         for (int i = 0; i < movedAssets.Length; i++)
        //         {
        //             string asset = movedAssets[i];
        //             string from = movedFromAssetPaths[i];
        // 
        //             Debug.Log("이동된 애셋: " + asset + " from " + from);
        //         }


        List<string> lstMeshPath = new List<string>();
        List<string> lstTexPath = new List<string>();

        foreach (string assetPath in importedAssets)
        {
            if (assetPath.Contains("/MapObject/") && assetPath.Contains(".fbx"))
            {
                lstMeshPath.Add(assetPath);
            }
            if (assetPath.Contains("/Tile/") && assetPath.Contains(".png"))
            {
                lstTexPath.Add(assetPath);
            }
        }

        //         foreach (string assetPath in lstMeshPath)
        //         {
        //             string modelName = Path.GetFileName(assetPath.Substring(0, assetPath.LastIndexOf(".fbx")));
        //             string basePath = Path.GetDirectoryName(assetPath.Substring(0, assetPath.LastIndexOf(".fbx")));
        //             string prefabspath = basePath + "/prefabs";
        //             string filecheck = prefabspath + "/" + modelName + ".prefab";
        //             if (File.Exists(filecheck))
        //             {
        //                 return;
        //             }
        // 
        //             Object[] meshs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        //             List<Mesh> lstMesh = new List<Mesh>();
        //             List<MeshRenderer> lstMeshRenderers = new List<MeshRenderer>();
        //             foreach (var obj in meshs)
        //             {
        //                 if (obj is Mesh)
        //                 {
        //                     lstMesh.Add((Mesh)obj);
        //                 }
        //                 else if (obj is MeshRenderer)
        //                 {
        //                     lstMeshRenderers.Add((MeshRenderer)obj);
        //                 }
        // 
        //             }
        //             MakeMesh(assetPath, lstMesh, lstMeshRenderers);
        //         }

        AutoTilePrefabs(lstTexPath);
    }

    private static Material MakeMat(string matpath)
    {

        string assetPath = matpath.Replace(".mat", ".dds");

        Material material = (Material)(AssetDatabase.LoadAssetAtPath(matpath, typeof(Material)));
        if (material)
        {
            return material;
        }


        Material myNewMat = new Material(Shader.Find("Diffuse"));
        AssetDatabase.CreateAsset(myNewMat, matpath);

        material = (Material)(AssetDatabase.LoadAssetAtPath(matpath, typeof(Material)));
        if (material)
        {
            Texture2D tex = (Texture2D)(AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)));
            if (tex)
                material.mainTexture = tex;
            else
            {
                AssetDatabase.Refresh();
                tex = (Texture2D)(AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)));
                if (tex)
                    material.mainTexture = tex;
            }
        }
        else
            Debug.Log("Error: material not found for " + assetPath);

        return material;
    }

    private static void MakeMesh(string assetPath, List<Mesh> lstMesh, List<MeshRenderer> lstMeshRenderers)
    {
        string modelName = Path.GetFileName(assetPath.Substring(0, assetPath.LastIndexOf(".fbx")));
        string basePath = Path.GetDirectoryName(assetPath.Substring(0, assetPath.LastIndexOf(".fbx")));
        string tpackpath = basePath + "/" + modelName + ".tpack";

        List<string> lsttex = new List<string>();
        LoadTPack(tpackpath, ref lsttex);

        string prefabspath = basePath + "/prefabs";
        MakeDirectory(prefabspath);


        //Object prefab = PrefabUtility.CreateEmptyPrefab(prefabspath + "/" + modelName + ".prefab");
        //             if (prefab == null)
        //                 return;
        //             // 
        //             GameObject gcopy = (GameObject)Object.Instantiate(g);
        //             // 
        //             GameObject tttt = PrefabUtility.ReplacePrefab(gcopy, prefab);


        GameObject gocopy = new GameObject(modelName);


        if (lstMesh.Count <= 1)
        {
            MeshFilter meshFilter = gocopy.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gocopy.AddComponent<MeshRenderer>();
            meshFilter.sharedMesh = lstMesh[0];

            if (lsttex.Count > 0)
            {

                //ttt.renderer.sharedMaterials = new Material[lsttex.Count];

                for (int i = 0; i < lsttex.Count; i++)
                {
                    if (lsttex[i] == "" || string.IsNullOrEmpty(lsttex[i]))
                    {
                        continue;
                    }

                    if (lsttex[i].Contains(".dds") == false && lsttex[i].Contains(".DDS") == false)
                    {
                        continue;
                    }

                    string matpath = basePath + "/texture/";
                    try
                    {
                        string matname = lsttex[i].Substring(0, lsttex[i].LastIndexOf(".dds", StringComparison.CurrentCultureIgnoreCase)) + ".mat";
                        Material material = MakeMat(matpath + matname);
                        if (material)
                        {
                            meshRenderer.sharedMaterial = material;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(string.Format("{0}_{1}", lsttex[i], e.Message));
                    }

                }

            }
        }
        else
        {



            //MeshFilter[] copymeshFilters = gocopy.GetComponentsInChildren<MeshFilter>(true);
            for (int i = 0; i < lstMesh.Count; i++)
            {
                GameObject childGameObject = new GameObject(modelName + "_" + i.ToString());
                childGameObject.transform.parent = gocopy.transform;

                MeshFilter meshFilter = childGameObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = lstMesh[i];

                MeshRenderer meshRenderer = childGameObject.AddComponent<MeshRenderer>();

                if (lsttex.Count <= i)
                {
                    continue;
                }

                if (lsttex[i] == "" || string.IsNullOrEmpty(lsttex[i]))
                {
                    continue;
                }

                if (lsttex[i].Contains(".dds") == false && lsttex[i].Contains(".DDS") == false)
                {
                    continue;
                }

                string matpath = basePath + "/texture/";
                string matname = lsttex[i].Substring(0, lsttex[i].LastIndexOf(".dds", StringComparison.CurrentCultureIgnoreCase)) + ".mat";
                Material material = MakeMat(matpath + matname);
                if (material)
                {
                    meshRenderer.sharedMaterial = material;
                }
            }
        }


        PrefabUtility.CreatePrefab(prefabspath + "/" + modelName + ".prefab", gocopy, ReplacePrefabOptions.ConnectToPrefab);
        GameObject.DestroyImmediate(gocopy);

        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh(ImportAssetOptions.TryFastReimportFromMetaData);
        //AssetDatabase.Refresh();
        //Object.DestroyImmediate(gcopy);

    }

    private void OnPostprocessModel(GameObject g)
    {
        Debug.Log("OnPostprocessModel");
    }

    private void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] propNames, System.Object[] values)
    {
        Debug.Log("OnPostprocessGameObjectWithUserProperties");

        for (int i = 0; i < propNames.Length; i++)
        {
            string propName = propNames[i];
            System.Object value1 = values[i];

            Debug.Log("GO: " + go.name + "--Propname: " + propName + "--value1: " + value1.GetType());

            if (value1 is string)
                Debug.Log("string: " + (string)value1);

            if (value1 is Vector4)
                Debug.Log("Vector4: " + (Vector4)value1);

            if (value1 is Color)
                Debug.Log("Color: " + (Color)value1);

            if (value1 is bool)
                Debug.Log("bool: " + (bool)value1);

            if (value1 is int)
                Debug.Log("int: " + (int)value1);

            if (value1 is float)
                Debug.Log("float: " + (float)value1);
        }
    }

    private void OnPreprocessModel()
    {
        if (assetPath.Contains("/MapObject/"))
        {
            UnityEditor.ModelImporter _mI = (UnityEditor.ModelImporter)assetImporter;
            //_mI.animationType = ModelImporterAnimationType.Legacy;
            _mI.tangentImportMode = ModelImporterTangentSpaceMode.Import;
            _mI.globalScale = 2.5f;
            _mI.importMaterials = false;
            //_mI.generateAnimations = ModelImporterGenerateAnimations.None;
            _mI.animationType = ModelImporterAnimationType.None;
        }
    }


    public static bool LoadTPack(string path, ref List<string> lsttex)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            string line = "";

            int nCount = 1;
            while ((line = sr.ReadLine()) != null)
            {
                char[] Separators = new char[] { ' ', '\t' };
                string[] split = line.Split(Separators, System.StringSplitOptions.RemoveEmptyEntries);
                if (split == null) continue;
                if (split.Length == 0) continue;
                string strKey = split[0].TrimEnd(null);
                if (strKey == "texpath")
                {
                    ++nCount;
                    continue;
                }


                ReadLine(split, ref lsttex);
                ++nCount;


            }
            sr.Close();
        }

        return true;
    }

    private static void ReadLine(string[] split, ref List<string> lsttex)
    {
        //string strKey = split[0].TrimEnd(null);

        //int texInfo = Convert.ToInt32(strKey);
        if (split == null)
            return;

        if (split.Length >= 2)
        {
            string Name = split[1].Trim('\"').ToLower();
            lsttex.Add(Name);
        }
    }

    private void Apply(Transform transform)
    {
        foreach (Transform child in transform)
        {
            Apply(child);
        }
    }

    //     private void OnPreprocessTexture()
    //     {
    //         if (assetPath.Contains("/Tile/"))
    //         {
    //             UnityEditor.TextureImporter _mI = (UnityEditor.TextureImporter) assetImporter;
    //             _mI.filterMode = FilterMode.Point;
    //             _mI.mipmapEnabled = false;
    //             _mI.textureType = TextureImporterType.Sprite;
    //             _mI.spriteImportMode = SpriteImportMode.Multiple;
    //             _mI.spritePixelsPerUnit = 32;
    //             _mI.spritePackingTag = "map";
    //             _mI.textureFormat = TextureImporterFormat.RGBA32;
    //             
    //         }
    //     }



    private static void AutoTilePrefabs(List<string> lstTexPath)
    {
        for (int i = 0; i < lstTexPath.Count; i++)
        {
            string assetPath = lstTexPath[i];
            if (assetPath.Contains("/Tile/AutoTileMaker/"))
            {
                CreateAutoTileMakerPrefabs(assetPath);
            }
            else if (assetPath.Contains("/Tile/AutoTile16/"))
            {
                CreateAutoTile16Prefabs(assetPath);
            }
            else if (assetPath.Contains("/Tile/Character/"))
            {
                CreateCharacterPrefabs(assetPath);
            }
            else if (assetPath.Contains("/Tile/Object/"))
            {
                CreateSpritePacker(assetPath, "Object");
            }
            else if (assetPath.Contains("/Tile/Floor/Auto/"))
            {
                CreateAutoTileFloorPrefabs(assetPath);
            }
            else if (assetPath.Contains("/Tile/Floor/"))
            {
                CreateSpritePacker(assetPath, "Floor");
            }
        }
    }


    private static void CreateSpritePacker(string assetPath, string outpath)
    {
        string[] split = assetPath.Split('/');
        string name = split[split.Length - 1];
        name = name.Replace(".png", "");

        //EditorUtility.SetDirty(texture);

        //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        Object[] SpriteAssetArr = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
        if (SpriteAssetArr.Length <= 0)
        {
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }
        else
        {
            //GameObject gocopy = new GameObject(name);


            //GameObject gocopy = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);
            //AutoTileSprite autoTileResource = gocopy.AddComponent<AutoTileSprite>();

            string prefabspath = "Assets/Resources/Map/Prefabs/" + outpath;
            if (Directory.Exists(prefabspath) == false)
            {
                MakeDirectory(prefabspath);
            }
            string filecheck = prefabspath + "/" + name + ".asset";
            if (File.Exists(filecheck))
            {
                File.Delete(filecheck);
            }
            ScriptCsv scriptCsv = new ScriptCsv(prefabspath + "/" + name + ".csv");
            Texture2D texture2D = (Texture2D)SpriteAssetArr[0];
            Sprite sprite = (Sprite)SpriteAssetArr[1];

            int offsetX = (int)(texture2D.width / sprite.rect.width);
            int offsetY = (int)(texture2D.height / sprite.rect.height);

            SpritePackerAsset objectPackerSprite = ScriptableObject.CreateInstance<SpritePackerAsset>();
            objectPackerSprite.AllSprites = new Sprite[SpriteAssetArr.Length - 1];
            for (int i = 1; i < SpriteAssetArr.Length; i++)
            {
                objectPackerSprite.AllSprites[i - 1] = (Sprite)SpriteAssetArr[i];
                SpriteSheetCsv scSheetCsv = new SpriteSheetCsv();
                scSheetCsv.assetName = name;
                scSheetCsv.id = i - 1;
                scSheetCsv.spriteName = objectPackerSprite.AllSprites[i - 1].name;
                scSheetCsv.numIndex = scSheetCsv.id;
                scSheetCsv.numX = scSheetCsv.id % offsetX;
                scSheetCsv.numY = scSheetCsv.id / offsetX;
                scriptCsv.LstValue.Add(scSheetCsv);
            }
            objectPackerSprite.AssetName = name;
            scriptCsv.Save();

            AssetDatabase.CreateAsset(objectPackerSprite, filecheck);
        }

    }

    private static void CreateCharacterPrefabs(string assetPath)
    {


        //EditorUtility.SetDirty(texture);

        //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        string[] split = assetPath.Split('/');
        string name = split[split.Length - 1];
        name = name.Replace(".png", "");

        Object[] SpriteAssetArr = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
        if (SpriteAssetArr.Length <= 0)
        {
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }
        else
        {
            //GameObject gocopy = new GameObject(name);
            //GameObject gocopy = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);
            //AutoTileSprite autoTileResource = gocopy.AddComponent<AutoTileSprite>();



            //             CharacterSpritePackerAsset characterSprite = ScriptableObject.CreateInstance<CharacterSpritePackerAsset>();
            //             characterSprite.AssetName = name;
            //             characterSprite.ListPack = new CharacterSpritePackerAsset.CharacterSpritePacker[11 * 18];

            string prefabspath = "Assets/Resources/Map/Prefabs/Character";
            if (Directory.Exists(prefabspath) == false)
            {
                MakeDirectory(prefabspath);
            }


            ScriptCsv scriptCsv = new ScriptCsv(prefabspath + "/" + name + ".csv");
            Texture2D texture2D = (Texture2D)SpriteAssetArr[0];
            Sprite sprite = (Sprite)SpriteAssetArr[1];
            int offsetX = (int)(texture2D.width / sprite.rect.width);

            int index = 0;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    int offset = i * 36 + 1;
                    SpritePackerAsset spritePackerAsset = ScriptableObject.CreateInstance<SpritePackerAsset>();
                    spritePackerAsset.AllSprites = new Sprite[2];
                    spritePackerAsset.AllSprites[0] = (Sprite)SpriteAssetArr[j + offset];
                    spritePackerAsset.AllSprites[1] = (Sprite)SpriteAssetArr[j + offset + 18];
                    spritePackerAsset.AssetName = spritePackerAsset.AllSprites[0].name;

                    string filecheck = prefabspath + "/" + spritePackerAsset.AssetName + ".asset";
                    if (File.Exists(filecheck))
                    {
                        File.Delete(filecheck);
                    }

                    AssetDatabase.CreateAsset(spritePackerAsset, filecheck);

                    SpriteSheetCsv scSheetCsv = new SpriteSheetCsv();
                    scSheetCsv.assetName = name;
                    scSheetCsv.id = index;
                    scSheetCsv.spriteName = spritePackerAsset.AllSprites[0].name;
                    scSheetCsv.numIndex = j + offset - 1;
                    scSheetCsv.numX = (scSheetCsv.numIndex) % offsetX;
                    scSheetCsv.numY = (scSheetCsv.numIndex) / offsetX;
                    scriptCsv.LstValue.Add(scSheetCsv);



                    index++;
                }
            }
            scriptCsv.Save();
            SpritePackerAsset spriteshadowPackerAsset = ScriptableObject.CreateInstance<SpritePackerAsset>();
            spriteshadowPackerAsset.AllSprites = new Sprite[6];
            index = 0;
            for (int i = 11 * 36 + 1; i < SpriteAssetArr.Length; i++)
            {
                spriteshadowPackerAsset.AllSprites[index++] = (Sprite)SpriteAssetArr[i];
            }
            spriteshadowPackerAsset.AssetName = name;

            string fileshadowcheck = prefabspath + "/" + name + "_shadow.asset";
            if (File.Exists(fileshadowcheck))
            {
                File.Delete(fileshadowcheck);
            }

            AssetDatabase.CreateAsset(spriteshadowPackerAsset, fileshadowcheck);

            //string basePath = Path.GetDirectoryName(assetPath.Substring(0, assetPath.LastIndexOf(".png")));

            //Object tempPrefab = PrefabUtility.CreateEmptyPrefab(prefabspath + "/" + name + ".prefab");
            //PrefabUtility.ReplacePrefab(gocopy, tempPrefab, ReplacePrefabOptions.ConnectToPrefab);
            //GameObject.DestroyImmediate(gocopy, true);
        }

    }

    private static void CreateAutoTileMakerPrefabs(string assetPath)
    {
        string[] split = assetPath.Split('/');
        string name = split[split.Length - 1];
        name = name.Replace(".png", "");

        //EditorUtility.SetDirty(texture);

        //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        Object[] SpriteAssetArr = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
        if (SpriteAssetArr.Length <= 0)
        {
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }
        else
        {
            //GameObject gocopy = new GameObject(name);


            //GameObject gocopy = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);
            //AutoTileSprite autoTileResource = gocopy.AddComponent<AutoTileSprite>();
            SpritePackerAsset autoTileResource = ScriptableObject.CreateInstance<SpritePackerAsset>();
            autoTileResource.AllSprites = new Sprite[SpriteAssetArr.Length - 1];
            for (int i = 1; i < SpriteAssetArr.Length; i++)
            {
                autoTileResource.AllSprites[i - 1] = (Sprite)SpriteAssetArr[i];
            }
            autoTileResource.AssetName = name;

            //string basePath = Path.GetDirectoryName(assetPath.Substring(0, assetPath.LastIndexOf(".png")));
            string prefabspath = "Assets/Resources/Map/Prefabs/AutoTile";
            if (Directory.Exists(prefabspath) == false)
            {
                MakeDirectory(prefabspath);
            }
            string filecheck = prefabspath + "/" + name + ".asset";
            if (File.Exists(filecheck))
            {
                return;
            }
            AssetDatabase.CreateAsset(autoTileResource, filecheck);
            //Object tempPrefab = PrefabUtility.CreateEmptyPrefab(prefabspath + "/" + name + ".prefab");
            //PrefabUtility.ReplacePrefab(gocopy, tempPrefab, ReplacePrefabOptions.ConnectToPrefab);
            //GameObject.DestroyImmediate(gocopy, true);
        }

    }

    private static void CreateAutoTileFloorPrefabs(string assetPath)
    {
        string[] split = assetPath.Split('/');
        string name = split[split.Length - 1];
        name = name.Replace(".png", "");

        string[] split2 = name.Split('_');
        string subname = split2[split2.Length - 1];
        //EditorUtility.SetDirty(texture);

        //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        Object[] SpriteAssetArr = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
        if (SpriteAssetArr.Length <= 0)
        {
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }
        else
        {
            string prefabspath = "Assets/Resources/Map/Prefabs/AutoTile";
            if (Directory.Exists(prefabspath) == false)
            {
                MakeDirectory(prefabspath);
            }


            ScriptCsv scriptCsv = new ScriptCsv(prefabspath + "/" + name + ".csv");
            Texture2D texture2D = (Texture2D)SpriteAssetArr[0];
            int offset = texture2D.width / 24;

            int count = (SpriteAssetArr.Length - 2) / offset;
            //autoTileResource.ListPack = new TileSpritePackerAsset.TileSpritePacker[count];
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                int spriteoffset = i * offset + 1;
                SpritePackerAsset spritePackerAsset = ScriptableObject.CreateInstance<SpritePackerAsset>();
                //TileSpritePackerAsset.TileSpritePacker spritePackerAsset = new TileSpritePackerAsset.TileSpritePacker();
                spritePackerAsset.AssetName = subname + i.ToString();
                spritePackerAsset.AllSprites = new Sprite[offset];
                for (int j = 0; j < offset; j++)
                {
                    spritePackerAsset.AllSprites[j] = (Sprite)SpriteAssetArr[j + spriteoffset];

                    SpriteSheetCsv scSheetCsv = new SpriteSheetCsv();
                    scSheetCsv.assetName = spritePackerAsset.AssetName;
                    scSheetCsv.id = index;
                    scSheetCsv.spriteName = spritePackerAsset.AllSprites[j].name;
                    scSheetCsv.numIndex = j;
                    scSheetCsv.numX = j;
                    scSheetCsv.numY = i;
                    scriptCsv.LstValue.Add(scSheetCsv);
                    index++;
                }
                //autoTileResource.ListPack[i] = spritePackerAsset;
                string filecheck = prefabspath + "/" + spritePackerAsset.AssetName + ".asset";
                if (File.Exists(filecheck))
                {
                    File.Delete(filecheck);
                }
                AssetDatabase.CreateAsset(spritePackerAsset, filecheck);


            }

            scriptCsv.Save();
            //autoTileResource.AssetName = name;


        }

    }

    private static void CreateAutoTile16Prefabs(string assetPath)
    {
        string[] split = assetPath.Split('/');
        string name = split[split.Length - 1];
        name = name.Replace(".png", "");

        string[] split2 = name.Split('_');
        string subname = split2[split2.Length - 1];
        //EditorUtility.SetDirty(texture);

        //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        Object[] SpriteAssetArr = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
        if (SpriteAssetArr.Length <= 0)
        {
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }
        else
        {
            string prefabspath = "Assets/Resources/Map/Prefabs/AutoTile";
            if (Directory.Exists(prefabspath) == false)
            {
                MakeDirectory(prefabspath);
            }


            ScriptCsv scriptCsv = new ScriptCsv(prefabspath + "/" + name + ".csv");
            Texture2D texture2D = (Texture2D)SpriteAssetArr[0];
            int offset = texture2D.width / 24;

            int count = (SpriteAssetArr.Length - 2) / offset;
            //autoTileResource.ListPack = new TileSpritePackerAsset.TileSpritePacker[count];
            for (int i = 0; i < count; i++)
            {
                int spriteoffset = i * offset + 1;
                SpritePackerAsset spritePackerAsset = ScriptableObject.CreateInstance<SpritePackerAsset>();
                //TileSpritePackerAsset.TileSpritePacker spritePackerAsset = new TileSpritePackerAsset.TileSpritePacker();
                spritePackerAsset.AssetName = subname + i.ToString();
                spritePackerAsset.AllSprites = new Sprite[offset];
                for (int j = 0; j < offset; j++)
                {
                    spritePackerAsset.AllSprites[j] = (Sprite)SpriteAssetArr[j + spriteoffset];
                }
                //autoTileResource.ListPack[i] = spritePackerAsset;
                string filecheck = prefabspath + "/" + spritePackerAsset.AssetName + ".asset";
                if (File.Exists(filecheck))
                {
                    File.Delete(filecheck);
                }
                AssetDatabase.CreateAsset(spritePackerAsset, filecheck);

                SpriteSheetCsv scSheetCsv = new SpriteSheetCsv();
                scSheetCsv.assetName = name;
                scSheetCsv.id = i;
                scSheetCsv.spriteName = spritePackerAsset.AssetName;
                scSheetCsv.numIndex = i;
                scSheetCsv.numX = (scSheetCsv.numIndex) % spriteoffset;
                scSheetCsv.numY = (scSheetCsv.numIndex) / spriteoffset;
                scriptCsv.LstValue.Add(scSheetCsv);
            }

            scriptCsv.Save();
            //autoTileResource.AssetName = name;


        }

    }

    private void OnPostprocessTexture(Texture2D texture)
    {
        Debug.Log("OnPostprocessTexture");

        if (assetPath.Contains("/Tile/AutoTile/"))
        {
            UnityEditor.TextureImporter _mI = (UnityEditor.TextureImporter)assetImporter;
            _mI.filterMode = FilterMode.Point;
            _mI.mipmapEnabled = false;
            _mI.textureType = TextureImporterType.Sprite;
            _mI.spriteImportMode = SpriteImportMode.Multiple;
            _mI.spritePixelsPerUnit = 32;
            _mI.spritePackingTag = "map";
            _mI.textureFormat = TextureImporterFormat.RGB24;
            string[] split = assetPath.Split('/');
            string name = split[split.Length - 1];
            name = name.Replace(".png", "");
            _mI.spritesheet = CreateSpriteMetaDataArray(name, texture, texture.width / 16, texture.height / 16);
        }
    }

    static SpriteMetaData[] CreateSpriteMetaDataArray(string name, Texture texture, int horizontalCount, int verticalCount)
    {
        float spriteWidth = texture.width / horizontalCount;
        float spriteHeight = texture.height / verticalCount;

        return Enumerable
            .Range(0, horizontalCount * verticalCount)
            .Select(index =>
            {
                int x = index % horizontalCount;
                int y = index / horizontalCount;

                return new SpriteMetaData
                {
                    name = string.Format("{0}_{1}", name, index),
                    rect = new Rect(spriteWidth * x, texture.height - spriteHeight * (y + 1), spriteWidth, spriteHeight)
                };
            })
            .ToArray();
    }


    private static string MakeDirectory(string path)
    {
        string directory = path;
        DirectoryInfo dirinfo = new DirectoryInfo(directory);
        if (dirinfo.Exists == false)
        {
            dirinfo.Create();
        }

        return directory;
    }

}