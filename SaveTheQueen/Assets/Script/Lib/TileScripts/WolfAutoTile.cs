﻿using System;
using UnityEditor;

namespace UnityEngine.Tilemaps
{
    [Serializable]
    public class WolfAutoTile : TileBase
    {
        [SerializeField]
        public Sprite[] m_RawTilesSprites;

        public Sprite[] m_PatternedSprites;

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
            return;
        }

        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            if (m_PatternedSprites == null)
            {
                if (m_RawTilesSprites[0] && m_RawTilesSprites[1] && m_RawTilesSprites[2] && m_RawTilesSprites[3] && m_RawTilesSprites[4])
                {
                    GeneratePatterns();
                }
                else
                {
                    return;
                }
            }
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;

            int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;

            int index = GetIndex((byte)mask);
            if (index >= 0 && index < m_PatternedSprites.Length && TileValue(tileMap, location))
            {
                tileData.sprite = m_PatternedSprites[index];
                tileData.color = Color.white;
                tileData.flags = (TileFlags.LockTransform | TileFlags.LockColor);
                tileData.colliderType = Tile.ColliderType.Sprite;
            }
        }

        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }

        private int GetIndex(byte mask)
        {
            string[] patternTexts = {
                "x0x111x0",
                "x11111x0",
                "x111x0x0",
                "x10111x0",
                "x11101x0",
                "01111111",
                "11111101",
                "x0x1x0x0",
                "x0x11111",
                "11111111",
                "1111x0x1",
                "x0x10111",
                "1101x0x1",
                "11011111",
                "11110111",
                "x0x1x0x1",
                "x0x0x111",
                "11x0x111",
                "11x0x0x1",
                "x0x11101",
                "0111x0x1",
                "01110111",
                "11011101",
                "x0x0x0x1",
                "x0x101x0",
                "x10101x0",
                "x101x0x0",
                "01x0x111",
                "11x0x101",
                "11010101",
                "01010111",
                "11010111",
                "x0x10101",
                "01010101",
                "0101x0x1",
                "11110101",
                "01011111",
                "01110101",
                "01011101",
                "01111101",
                "x0x0x101",
                "01x0x101",
                "01x0x0x1",
                "x0x0x1x0",
                "x1x0x1x0",
                "x1x0x0x0",
                "x0x0x0x0"
            };
            int index = -1;
            for (int j = 0; j < patternTexts.Length; j++)
            {
                bool flag = true;
                for (int i = 0; i < 8; i++)
                {
                    if (patternTexts[j][i] != 'x')
                    {
                        char currentBitChar = ((mask & (byte)Mathf.Pow(2, 7 - i)) != 0) ? '1' : '0';
                        if (patternTexts[j][i] != currentBitChar)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    index = j;
                    break;
                }
            }
            return index;

        }



        Sprite[,] Segments = new Sprite[5, 4];
        int[][] Patterns = new int[][]
        {
            new int[] {0,2,1,4},
            new int[] {2,2,4,4},
            new int[] {2,0,4,1},
            new int[] {2,2,3,4},
            new int[] {2,2,4,3},
            new int[] {3,4,4,4},
            new int[] {4,3,4,4},
            new int[] {0,0,1,1},
            new int[] {1,4,1,4},
            new int[] {4,4,4,4},
            new int[] {4,1,4,1},
            new int[] {1,4,1,3},
            new int[] {4,1,3,1},
            new int[] {4,4,3,4},
            new int[] {4,4,4,3},
            new int[] {1,1,1,1},

            new int[] {1,4,0,2},
            new int[] {4,4,2,2},
            new int[] {4,1,2,0},
            new int[] {1,3,1,4},
            new int[] {3,1,4,1},
            new int[] {3,4,4,3},
            new int[] {4,3,3,4},
            new int[] {1,1,0,0},

            new int[] {0,2,1,3},
            new int[] {2,2,3,3},
            new int[] {2,0,3,1},
            new int[] {3,4,2,2},
            new int[] {4,3,2,2},
            new int[] {4,3,3,3},
            new int[] {3,4,3,3},
            new int[] {4,4,3,3},

            new int[] {1,3,1,3},

            new int[] {3,3,3,3},

            new int[] {3,1,3,1},
            new int[] {4,3,4,3},
            new int[] {3,4,3,4},
            new int[] {3,3,4,3},
            new int[] {3,3,3,4},
            new int[] {3,3,4,4},

            new int[] {1,3,0,2},
            new int[] {3,3,2,2},
            new int[] {3,1,2,0},
            new int[] {0,2,0,2},
            new int[] {2,2,2,2},
            new int[] {2,0,2,0},
            new int[] {0,0,0,0}

        };
        public void GeneratePatterns()
        {
            for (int i = 0; i < 5; i++)
            {
                Texture2D tex = m_RawTilesSprites[i].texture;
                int y = (int)m_RawTilesSprites[i].rect.y;
                int x = (int)m_RawTilesSprites[i].rect.x;
                int height = (int)m_RawTilesSprites[i].rect.height;
                int width = (int)m_RawTilesSprites[i].rect.width;
                int height_half = height / 2;
                int width_half = width / 2;
                Segments[i, 0] = Sprite.Create(tex, new Rect(x, y, width_half, height_half), Vector2.zero);
                Segments[i, 1] = Sprite.Create(tex, new Rect(x + width_half, y, width_half, height_half), Vector2.zero);
                Segments[i, 2] = Sprite.Create(tex, new Rect(x, y + height_half, width_half, height_half), Vector2.zero);
                Segments[i, 3] = Sprite.Create(tex, new Rect(x + width_half, y + height_half, width_half, height_half), Vector2.zero);

            }

            m_PatternedSprites = new Sprite[47];
            for (int i = 0; i < 47; i++)
            {
                m_PatternedSprites[i] = CombineTextures(Patterns[i]);
            }
        }

        private Sprite CombineTextures(int[] TypeIndex)
        {
            int[] fixedArray = new int[4];
            fixedArray[0] = TypeIndex[2];
            fixedArray[1] = TypeIndex[3];
            fixedArray[2] = TypeIndex[0];
            fixedArray[3] = TypeIndex[1];

            Color[][] texs = new Color[4][];
            for (int i = 0; i < 4; i++)
            {

                int x = (int)Segments[fixedArray[i], i].rect.x;
                int y = (int)Segments[fixedArray[i], i].rect.y;
                int w = (int)Segments[fixedArray[i], i].rect.width;
                int h = (int)Segments[fixedArray[i], i].rect.height;
                texs[i] = Segments[fixedArray[i], i].texture.GetPixels(x, y, w, h);
            }

            int width_half = (int)Segments[0, 0].rect.width;
            int height_half = (int)Segments[0, 0].rect.height;
            int width = width_half * 2;
            int height = height_half * 2;

            Color[] texArray = new Color[width * height];
            for (int i = 0; i < height_half; i++)
            {
                Array.Copy(texs[0], i * width_half, texArray, i * width, width_half);
            }

            for (int i = 0; i < height_half; i++)
            {
                Array.Copy(texs[1], i * width_half, texArray, i * width + width_half, width_half);
            }

            for (int i = 0; i < height_half; i++)
            {
                Array.Copy(texs[2], i * width_half, texArray, (i + height_half) * width, width_half);
            }

            for (int i = 0; i < height_half; i++)
            {
                Array.Copy(texs[3], i * width_half, texArray, (i + height_half) * width + width_half, width_half);
            }
            Texture2D ret = new Texture2D(width, height, TextureFormat.ARGB32, false);
            ret.filterMode = FilterMode.Point;
            ret.wrapMode = TextureWrapMode.Clamp;
            ret.SetPixels(texArray);
            return Sprite.Create(ret, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), width);
        }



#if UNITY_EDITOR
        [MenuItem("Assets/Create/WolfAuto Tile")]
        public static void CreateTerrainTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save WolfAuto Tile", "New WolfAuto Tile", "asset", "Save WolfAuto Tile", "Assets");

            if (path == "")
                return;

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WolfAutoTile>(), path);
        }

#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WolfAutoTile))]
    public class WolfAutoTileEditor : Editor
    {
        private WolfAutoTile tile { get { return (target as WolfAutoTile); } }

        public void OnEnable()
        {
            if (tile.m_RawTilesSprites == null || tile.m_RawTilesSprites.Length != 15)
            {
                tile.m_RawTilesSprites = new Sprite[15];
                EditorUtility.SetDirty(tile);
            }
            if (tile.m_RawTilesSprites[0] && tile.m_RawTilesSprites[1] && tile.m_RawTilesSprites[2] && tile.m_RawTilesSprites[3] && tile.m_RawTilesSprites[4])
            {
                tile.GeneratePatterns();
            }
        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("우디타 규격의 오토 타일 칩을 위에서부터 차례로 슬롯하세요.(애니메이션 비대응)");
            EditorGUILayout.Space();

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 210;

            EditorGUI.BeginChangeCheck();
            tile.m_RawTilesSprites[0] = (Sprite)EditorGUILayout.ObjectField("상하 좌우 어디에도 이 오토 타일이 없다(직각으로 배치했을 때의 바깥쪽 모퉁이에 나타나는)", tile.m_RawTilesSprites[0], typeof(Sprite), false, null);
            tile.m_RawTilesSprites[1] = (Sprite)EditorGUILayout.ObjectField("상하로 이 오토 타일이 있다", tile.m_RawTilesSprites[1], typeof(Sprite), false, null);
            tile.m_RawTilesSprites[2] = (Sprite)EditorGUILayout.ObjectField("좌우에 이 오토 타일이 있다", tile.m_RawTilesSprites[2], typeof(Sprite), false, null);
            tile.m_RawTilesSprites[3] = (Sprite)EditorGUILayout.ObjectField("직각으로 이 오토 타일을 배치했을 때 안쪽 모퉁이에 나타났다", tile.m_RawTilesSprites[3], typeof(Sprite), false, null);
            tile.m_RawTilesSprites[4] = (Sprite)EditorGUILayout.ObjectField("이 오토 타일로 주위를 모두 둘러싸인", tile.m_RawTilesSprites[4], typeof(Sprite), false, null);
            if (EditorGUI.EndChangeCheck())
            {
                if (tile.m_RawTilesSprites[0] && tile.m_RawTilesSprites[1] && tile.m_RawTilesSprites[2] && tile.m_RawTilesSprites[3] && tile.m_RawTilesSprites[4])
                {
                    tile.GeneratePatterns();
                }

                EditorUtility.SetDirty(tile);
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;
        }
    }
#endif
}