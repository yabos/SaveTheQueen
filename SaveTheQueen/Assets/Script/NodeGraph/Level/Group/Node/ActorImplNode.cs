using Aniz.Graph;
using Lib.Event;
using table.db;
using UnityEngine;

namespace Aniz.NodeGraph.Level.Group.Node
{
    public class ActorImplNode : EntityImplNode
    {
        public override eNodeType NodeType
        {
            get { return eNodeType.ActorImpl; }
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