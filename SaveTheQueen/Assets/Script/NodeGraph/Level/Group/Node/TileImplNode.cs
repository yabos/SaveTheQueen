using Aniz.Basis;
using Aniz.Factory;
using Aniz.Graph;
using Lib.Event;
using table.db;
using UnityEngine;

namespace Aniz.NodeGraph.Level.Group.Node
{
    public class TileImplNode : GraphMonoPoolNode
    {
        //private SpriteResource m_SpriteResource;
        private bool m_autoTile = false;
        private DB_TileSet m_tileSet;
        private DB_SpriteData m_dbSpriteData;

        protected SpriteRenderer m_SpriteRenderer;

        public override eNodeType NodeType
        {
            get { return eNodeType.TileImpl; }
        }

        protected override void BhvOnAwake()
        {
        }

        protected override void BhvOnStart()
        {
        }

        protected override void BhvOnDestroy()
        {
        }

        public override void BhvOnEnter()
        {

        }

        public override void BhvOnLeave()
        {
            SetParentNode(null);
            Global.FactoryMgr.FastDestory(this);
            m_SpriteRenderer = null;
        }

        public override void BhvFixedUpdate(float dt)
        {
        }

        public override void BhvLateFixedUpdate(float dt)
        {
        }

        public override void BhvUpdate(float dt)
        {
        }

        public override void BhvLateUpdate(float dt)
        {
        }

        public override bool OnMessage(IMessage message)
        {
            return false;
        }

        
    }
}