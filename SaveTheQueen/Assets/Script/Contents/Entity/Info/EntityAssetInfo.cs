using System.Collections.Generic;
using table.db;
using UnityEngine;

namespace Aniz.NodeGraph.Level.Group.Info
{
    public class EntityAssetInfo
    {
        private readonly int m_ID;
        private readonly eLayerMask m_layerMask;

        public Vector3 LocalMapPosition { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public byte direction { get; set; }
        public float Scale { get; set; }

        public string PrefabName { get; private set; }
        public string SpriteName { get; private set; }
        //public string DisplayName { get; set; }
        //public GameObject RootGameObject { get; set; }

        public E_TileMaterial TileMaterial { get; set; }

        private List<uint> m_lstZone = new List<uint>();

        public EntityAssetInfo(int ID, eLayerMask layerMask, string prefabName, string spriteName)
        {
            m_ID = ID;
            m_layerMask = layerMask;
            LocalMapPosition = Vector3.zero;
            Position = Vector2.zero;
            PrefabName = prefabName;
            SpriteName = spriteName;
        }

        public List<uint> LstZone
        {
            get { return m_lstZone; }
        }

        public int ID
        {
            get { return m_ID; }
        }

        public eLayerMask LayerMask
        {
            get { return m_layerMask; }
        }

        public void Copy(EntityAssetInfo assetInfo)
        {
            assetInfo.direction = direction;
            assetInfo.Position = Position;
            assetInfo.LocalMapPosition = LocalMapPosition;
            assetInfo.Rotation = Rotation;
            assetInfo.direction = direction;
        }
    };
}