using System;
using System.Collections.Generic;
using Aniz;
using Aniz.Event;
using Lib.Event;
using UnityEngine;

namespace Aniz.Graph
{
    //************************************************************************
    // IGraphNodeGroup
    //------------------------------------------------------------------------
    public interface IGraphNodeGroup : IGraphNode
    {
        int GetNumChildren();

        int AttachChild(IGraphNode child);

        int DetachChild(IGraphNode child);

        IGraphNode DetachChildAt(int idx);

        void DetachAllChildren();

        IGraphNode SetChild(int idx, IGraphNode child);

        IGraphNode GetChild(int idx);

        T GetChild<T>() where T : class, IGraphNode;

        List<T> GetChilds<T>() where T : class, IGraphNode;
    }

    //************************************************************************
    // GraphNodeGroup
    //------------------------------------------------------------------------
    public abstract class GraphNodeGroup : IGraphNodeGroup
    {
        //
        // GraphNodeGroup
        //

        private int m_uid = -1;

        protected List<IGraphNode> children = new List<IGraphNode>();

        private IGraphNode parentNode;

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

        public int Uid { get { return this.m_uid; } private set { this.m_uid = value; } }


        public string Name { get; set; }

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

        public virtual void BhvOnEnter()
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] != null)
                    children[idx].BhvOnEnter();
            }
        }

        public virtual void BhvOnLeave()
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] != null)
                    children[idx].BhvOnLeave();
            }
        }

        public virtual void BhvFixedUpdate(float dt)
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] != null)
                    children[idx].BhvFixedUpdate(dt);
            }
        }

        public virtual void BhvLateFixedUpdate(float dt)
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] != null)
                    children[idx].BhvLateFixedUpdate(dt);
            }
        }

        public virtual void BhvUpdate(float dt)
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] != null)
                    children[idx].BhvUpdate(dt);
            }
        }

        public virtual void BhvLateUpdate(float dt)
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] != null)
                    children[idx].BhvLateUpdate(dt);
            }
        }

        public virtual bool OnMessage(IMessage message)
        {
            for (int idx = 0; idx < children.Count; ++idx)
            {
                if (children[idx] == null)
                    continue;

                if (children[idx] is IGraphNodeGroup)
                {
                    IGraphNodeGroup nodeGroup = children[idx] as IGraphNodeGroup;
                    if (nodeGroup.OnMessage(message))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion // "IGraphUpdatable"

        //
        // IGraphNodeGroup
        //

        #region "IGraphNodeGroup"

        public int GetNumChildren()
        {
            return this.children.Count;
        }

        public int AttachChild(IGraphNode child)
        {
            if (child == null)
            {
                Debug.LogError("You cannot attach null child to a GraphNodeGroup.");
                return -1;
            }

            if (child.Parent != null)
            {
                Debug.LogError("The child already has a parent.");
                return -1;
            }

            child.SetParentNode(this);

            for (int idx = 0; idx < this.children.Count; ++idx)
            {
                if (this.children[idx] == null)
                {
                    this.children[idx] = child;
                    return idx;
                }
            }

            this.children.Add(child);
            return this.children.Count;
        }

        public int DetachChild(IGraphNode child)
        {
            if (child != null)
            {
                for (int idx = 0; idx < this.children.Count; ++idx)
                {
                    if (this.children[idx] == child)
                    {
                        this.children[idx].SetParentNode(null);
                        this.children[idx] = null;
                        return idx;
                    }
                }
            }

            return -1;
        }

        public IGraphNode DetachChildAt(int idx)
        {
            if (0 <= idx && idx < this.children.Count)
            {
                IGraphNode child = this.children[idx];
                if (child != null)
                {
                    child.SetParentNode(null);
                    this.children[idx] = null;
                }

                return child;
            }

            return null;
        }

        public void DetachAllChildren()
        {
            for (int idx = 0; idx < this.children.Count; ++idx)
            {
                if (children[idx] is IGraphNodeGroup)
                {
                    IGraphNodeGroup nodeGroup = children[idx] as IGraphNodeGroup;
                    nodeGroup.DetachAllChildren();
                }

                DetachChild(this.children[idx]);
            }
        }

        public IGraphNode SetChild(int idx, IGraphNode child)
        {
            if (child != null)
                Debug.Assert(child.Parent == null);

            if (0 <= idx && idx < this.children.Count)
            {
                IGraphNode prevChild = this.children[idx];
                if (prevChild != null)
                    prevChild.SetParentNode(null);

                if (child != null)
                    child.SetParentNode(this);

                this.children[idx] = child;
                return prevChild;
            }

            if (child != null)
                child.SetParentNode(this);

            this.children.Add(child);
            return null;
        }

        public IGraphNode GetChild(int idx)
        {
            if (0 <= idx && idx < this.children.Count)
            {
                return this.children[idx];
            }

            return null;
        }

        public T GetChild<T>() where T : class, IGraphNode
        {
            for (int idx = 0; idx < this.children.Count; ++idx)
            {
                if (this.children[idx] is T)
                {
                    return this.children[idx] as T;
                }
            }

            return null;
        }

        public List<T> GetChilds<T>() where T : class, IGraphNode
        {
            List<T> childs = new List<T>();
            for (int idx = 0; idx < this.children.Count; ++idx)
            {
                if (this.children[idx] is T)
                {
                    childs.Add(this.children[idx] as T);
                }
            }

            return childs;
        }

        #endregion // "IGraphNodeGroup"

        #region "IMessageHandler"

        #region "IHandler"
        #endregion // "IHandler"

        protected bool m_isConnected = false;

        public string HandlerName
        {
            get { return Name; }
        }

        public bool IsConnected
        {
            get { return m_isConnected; }
        }

        public eNotifyHandler GetHandlerType()
        {
            return eNotifyHandler.Node;
        }

        public int GetOrder()
        {
            return (int)GetHandlerType();
        }

        public bool IsActiveAndEnabled()
        {
            if (m_isConnected == false)
                return false;

            return true;
        }

        public void OnConnectHandler()
        {
            m_isConnected = true;
        }

        public void OnDisconnectHandler()
        {
            m_isConnected = false;
        }

        #endregion // "IMessageHandler"
    }
}
