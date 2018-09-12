using Aniz.Graph;
using Lib.Event;
using UnityEngine;

namespace Aniz.NodeGraph.Level.Group.Node
{
    public class EntityImplNode : GraphMonoPoolNode
    {
        public override eNodeType NodeType
        {
            get { return eNodeType.EntityImpl; }
        }

        public virtual bool IsAnim
        {
            get { return false; }
        }

        public virtual Transform GetBoneObject(string boneName)
        {
            return null;
        }

        public Transform GetRootObject()
        {
            return transform;
        }
        //
        // BhvMono
        //

        #region "BhvMono"

        protected override void BhvOnAwake()
        {
        }


        //protected override void BhvOnStart()
        //{
        //}

        //protected override void BhvOnDestroy()
        //{
        //}
        #endregion "BhvMono"

        //
        // IBhvUpdatable
        //

        #region "IBhvUpdatable"
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

        #endregion "IBhvUpdatable"

    }
}