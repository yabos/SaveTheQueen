using Aniz;
using Aniz.Event;
using Lib.Event;
using UnityEngine;

namespace Aniz.Graph
{
    //************************************************************************
    // IGraphUpdatable
    //------------------------------------------------------------------------
    public interface IGraphUpdatable : IMessageHandler
    {
        void BhvOnEnter();

        void BhvOnLeave();

        void BhvFixedUpdate(float dt);

        void BhvLateFixedUpdate(float dt);

        void BhvUpdate(float dt);

        void BhvLateUpdate(float dt);
    }

    //************************************************************************
    // IGraphNode
    //------------------------------------------------------------------------
    public interface IGraphNode : IGraphUpdatable
    {
        eNodeType NodeType { get; }

        eNodeCategory NodeCategory { get; }

        int Uid { get; }

        string Name { get; set; }

        IGraphNode Parent { get; }

        void SetParentNode(IGraphNode parent);
    }
}
