using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace UnityEngine.Tilemaps
{
	[Serializable]
	public class IsometricPipelineTile : WalkableTile
	{
		[SerializeField]
		public Sprite[] m_Sprites;

		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			for (int yd = -1; yd <= 1; yd++)
				for (int xd = -1; xd <= 1; xd++)
				{
					Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
					if (TileValue(tileMap, position))
						tileMap.RefreshTile(position);
				}
		}

		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			UpdateTile(location, tileMap, ref tileData);
		}

		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;

			int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;

			int index = GetIndex((byte)mask);
			if (index >= 0 && index < m_Sprites.Length && TileValue(tileMap, location))
			{
				tileData.sprite = m_Sprites[index];
				tileData.transform = GetTransform((byte)mask);
				tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
			}
		}

		private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return (tile != null && tile == this);
		}

		private int GetIndex(byte mask)
		{
			switch (mask)
			{
				case 0: return 0;
				case 1: 
				case 2: return 2;
				case 4:
				case 8: return 1;
				case 3: return 3;
				case 5:
				case 10: return 4;
				case 6:
				case 9: return 5;
				case 12: return 6;
				case 7:
				case 11: return 7;
				case 13:
				case 14: return 8;
				case 15: return 9;
			}
			Debug.Log(mask);
			return -1;
		}

		private Matrix4x4 GetTransform(byte mask)
		{
			switch (mask)
			{
				case 1:
				case 5:
				case 7:
				case 8:
				case 9:
				case 14:
					return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
			}
			return Matrix4x4.identity;
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/Isometric Pipeline Tile")]
		public static void CreateIsometricPipelineTile()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Isometric Pipeline Tile", "New Isometric Pipeline Tile", "asset", "Save Isometric Pipeline Tile", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<IsometricPipelineTile>(), path);
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(IsometricPipelineTile))]
	public class IsometricPipelineTileEditor : Editor
	{
		private IsometricPipelineTile tile { get { return (target as IsometricPipelineTile); } }

		public void OnEnable()
		{
			if (tile.m_Sprites == null || tile.m_Sprites.Length != 10)
				tile.m_Sprites = new Sprite[10];
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Place sprites shown based on the number of tiles bordering it.");
			EditorGUILayout.Space();
			
			EditorGUI.BeginChangeCheck();
			tile.m_Sprites[0] = (Sprite) EditorGUILayout.ObjectField("None", tile.m_Sprites[0], typeof(Sprite), false, null);
			tile.m_Sprites[1] = (Sprite) EditorGUILayout.ObjectField("One", tile.m_Sprites[1], typeof(Sprite), false, null);
			tile.m_Sprites[2] = (Sprite) EditorGUILayout.ObjectField("One Alt", tile.m_Sprites[2], typeof(Sprite), false, null);
			tile.m_Sprites[3] = (Sprite) EditorGUILayout.ObjectField("Two Top", tile.m_Sprites[3], typeof(Sprite), false, null);
			tile.m_Sprites[4] = (Sprite) EditorGUILayout.ObjectField("Two Straight", tile.m_Sprites[4], typeof(Sprite), false, null);
			tile.m_Sprites[5] = (Sprite) EditorGUILayout.ObjectField("Two Left", tile.m_Sprites[5], typeof(Sprite), false, null);
			tile.m_Sprites[6] = (Sprite) EditorGUILayout.ObjectField("Two Bottom", tile.m_Sprites[6], typeof(Sprite), false, null);
			tile.m_Sprites[7] = (Sprite) EditorGUILayout.ObjectField("Three", tile.m_Sprites[7], typeof(Sprite), false, null);
			tile.m_Sprites[8] = (Sprite) EditorGUILayout.ObjectField("Three Alt", tile.m_Sprites[8], typeof(Sprite), false, null);
			tile.m_Sprites[9] = (Sprite) EditorGUILayout.ObjectField("Four", tile.m_Sprites[9], typeof(Sprite), false, null);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Set a movement cost for walking on this tile.");
			tile.m_MoveCost = EditorGUILayout.IntField("Move Cost", tile.m_MoveCost);
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);
		}
	}
#endif
}
