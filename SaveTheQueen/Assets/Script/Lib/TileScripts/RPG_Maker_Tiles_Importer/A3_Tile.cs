using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class A3_Tile : A2_Tile
{
    /// <summary>
    /// force to show the right side border of the tile
    /// </summary>
    [SerializeField]
    public bool right_side = true;

    /// <summary>
    /// force to show the left side border of the tile
    /// </summary>
    [SerializeField]
    public bool left_side = true;
#if UNITY_EDITOR
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector3Int tmp_pos = position + new Vector3Int(x, y, 0);
                A3_Tile tmp_Tile = tilemap.GetTile(tmp_pos) as A3_Tile;
                if (tmp_Tile != null //&& tmp_Tile.id_tile_type == this.id_tile_type
                    )
                    tilemap.RefreshTile(tmp_pos);
            }
        }

        Vector3Int tmp_pos1 = position;
        while (true)
        {
            tmp_pos1 += Vector3Int.up;
            A3_Tile tmp_Tile = tilemap.GetTile(tmp_pos1) as A3_Tile;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
            {
                tilemap.RefreshTile(tmp_pos1 + Vector3Int.right);
                tilemap.RefreshTile(tmp_pos1 + Vector3Int.left);
                tilemap.RefreshTile(tmp_pos1);
            }
            else {
                break;
            }
        }
        tmp_pos1 = position;
        while (true)
        {
            tmp_pos1 += Vector3Int.down;
            A3_Tile tmp_Tile = tilemap.GetTile(tmp_pos1) as A3_Tile;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
            {
                tilemap.RefreshTile(tmp_pos1 + Vector3Int.right);
                tilemap.RefreshTile(tmp_pos1 + Vector3Int.left);
                tilemap.RefreshTile(tmp_pos1);
            }
            else {
                break;
            }
        }
    }
#endif
    protected override byte Compute_Neighbours(Vector3Int pos, ITilemap map)
    {
        int res = 0;

        Vector3Int tmp_pos = pos + Vector3Int.right + Vector3Int.up;
        A3_Tile tmp_Tile = map.GetTile(tmp_pos) as A3_Tile;
        //if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
        //    res |= (1 << 0);

        tmp_pos = pos;
        while(true)
        {
            tmp_pos = tmp_pos + Vector3Int.up; //top
            tmp_Tile = map.GetTile(tmp_pos) as A3_Tile;

            if(tmp_Tile == null || tmp_Tile.id_tile_type != this.id_tile_type)
            {
                tmp_pos += Vector3Int.down;
                //Debug.Log(tmp_pos);
                A3_Tile tmp_Tile_r = map.GetTile(tmp_pos + Vector3Int.right) as A3_Tile; //right
                right_side = !(tmp_Tile_r != null && tmp_Tile_r.id_tile_type == this.id_tile_type);

                //A3_Tile tmp_Tile_ru = map.GetTile(tmp_pos + Vector3Int.right + Vector3Int.up) as A3_Tile; //right-up
                //right_side = !((tmp_Tile_r != null && tmp_Tile_r.id_tile_type == this.id_tile_type) &&
                //    (tmp_Tile_ru != null && tmp_Tile_ru.id_tile_type == this.id_tile_type && !tmp_Tile_ru.left_side));

                A3_Tile tmp_Tile_l = map.GetTile(tmp_pos + Vector3Int.left) as A3_Tile; //left
                left_side = !(tmp_Tile_l != null && tmp_Tile_l.id_tile_type == this.id_tile_type);

                //A3_Tile tmp_Tile_lu = map.GetTile(tmp_pos + Vector3Int.left + Vector3Int.up) as A3_Tile; //left-up

                break;
                
            }               
        }

        tmp_pos = pos + Vector3Int.up; //down
        tmp_Tile = map.GetTile(tmp_pos) as A3_Tile;
        if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
        {
            res |= (1 << 1);
        }

        tmp_pos = pos + Vector3Int.down; //down
        tmp_Tile = map.GetTile(tmp_pos) as A3_Tile;
        if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
        {
            res |= (1 << 6);
        }

        tmp_pos = pos + Vector3Int.right; //right
        tmp_Tile = map.GetTile(tmp_pos) as A3_Tile;
        if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
        {
            if(!right_side)
                res |= (1 << 3);
        }

        tmp_pos = pos + Vector3Int.left; //left
        tmp_Tile = map.GetTile(tmp_pos) as A3_Tile;
        if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
        {
            if(!left_side)
                res |= (1 << 4);
        }

        return (byte)res;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(A3_Tile))]
public class A3_Editor : Editor
{
    private A3_Tile tile { get { return (target as A3_Tile); } }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 210;

        EditorGUI.BeginChangeCheck();
        tile.preview = (Sprite)EditorGUILayout.ObjectField("Preview", tile.preview, typeof(Sprite), false, null);

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(tile);

        EditorGUIUtility.labelWidth = oldLabelWidth;
    }
}
#endif