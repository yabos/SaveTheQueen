using Aniz;
using Lib.Event;

namespace Aniz.Graph
{
    //************************************************************************
    // GraphMonoNode
    //------------------------------------------------------------------------
    public abstract class GraphMonoNode : GraphMono, IGraphNode
    {
        int uid = -1;


        IGraphNode parentNode;

        //
        // IGraphNode
        //

        #region "IGraphNode"

        public abstract eNodeType NodeType { get; }

        public eNodeCategory NodeCategory
        {
            get
            {
                eNodeType nodeType = NodeType;
                uint category = (uint)nodeType >> 16;
                return (eNodeCategory)category;
            }
        }

        public int Uid { get { return this.uid; } private set { this.uid = value; } }


        public string Name
        {
            get { return gameObject.name; }
            set { gameObject.name = value; }
        }

        public IGraphNode Parent { get { return this.parentNode; } }

        public virtual void SetParentNode(IGraphNode parent)
        {
            this.parentNode = parent;
        }

        #endregion // "IGraphNode"

        //
        // IGraphUpdatable
        //

        #region "IGraphUpdatable"

        public abstract void BhvOnEnter();

        public abstract void BhvOnLeave();

        public abstract void BhvFixedUpdate(float dt);

        public abstract void BhvLateFixedUpdate(float dt);

        public abstract void BhvUpdate(float dt);

        public abstract void BhvLateUpdate(float dt);

        public abstract bool OnMessage(IMessage message);

        #endregion // "IGraphUpdatable"
    }
}
