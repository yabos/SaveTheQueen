using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetPostProcessor : AssetPostprocessor
{
	private static void OnPostprocessAllAssets(
		   string[] importedAssets,
		   string[] deletedAssets,
		   string[] movedAssets, string[] movedFromAssetPaths
		   )
	{
		/*
		List<string> lstMeshPath = new List<string>();

		foreach (string importedAsset in importedAssets)
		{
			if (importedAsset.IndexOf(".fbx", StringComparison.CurrentCultureIgnoreCase) < 0)
			{
				continue;
			}
			lstMeshPath.Add(importedAsset);

		}

		foreach (string s in lstMeshPath)
		{
			if (s.Contains("Assets/GameAssets/Environments/FBX/theme"))
			{
				GameObject importGameObject = (GameObject)AssetDatabase.LoadAssetAtPath(s, typeof(GameObject));
				FbxPostProcessor.FbxToCreatePrefabs(s, importGameObject);
			}
		}
		*/
	}

	private void OnPostprocessModel(GameObject g)
	{
		if (assetPath.Contains("Assets/GameAssets/Environments/FBX/theme") == false)
		{
			return;
		}

		Debug.Log("OnPostprocessModel Name" + g);


	}

	private void OnPreprocessModel()
	{
		if (assetPath.Contains("Assets/GameAssets/Environments/FBX/theme") == false)
		{
			return;
		}

		ModelImporter importer = assetImporter as ModelImporter;
		importer.importAnimation = false;
		importer.meshCompression = ModelImporterMeshCompression.Off;
		//importer.meshCompression = ModelImporterMeshCompression.Medium;
		importer.generateSecondaryUV = true;
		importer.importNormals = ModelImporterNormals.Import;
		importer.importTangents = ModelImporterTangents.Import;


		//Debug.Log("OnPreprocessModel Name" + importer);
	}

	//private void OnPostprocessTexture(Texture2D texture)
	//{
	//	Debug.Log("OnPostprocessTexture");
	//}

	//private void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] propNames, System.Object[] values)
	//{
	//	Debug.Log("OnPostprocessGameObjectWithUserProperties");

	//	for (int i = 0; i < propNames.Length; i++)
	//	{
	//		string propName = propNames[i];
	//		System.Object value1 = values[i];

	//		Debug.Log("GO: " + go.name + "--Propname: " + propName + "--value1: " + value1.GetType());

	//		if (value1 is string)
	//			Debug.Log("string: " + (string)value1);

	//		if (value1 is Vector4)
	//			Debug.Log("Vector4: " + (Vector4)value1);

	//		if (value1 is Color)
	//			Debug.Log("Color: " + (Color)value1);

	//		if (value1 is bool)
	//			Debug.Log("bool: " + (bool)value1);

	//		if (value1 is int)
	//			Debug.Log("int: " + (int)value1);

	//		if (value1 is float)
	//			Debug.Log("float: " + (float)value1);
	//	}
	//}
}
