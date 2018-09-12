using System;
using System.Collections.Generic;

namespace Lib.AnimationEvent
{

    public class AnimationEventStates
    {

        private static StateComparer stateComparer = new StateComparer();

        public class StateComparer : IComparer<AnimationEventState>
        {
            public int Compare(AnimationEventState lhs, AnimationEventState rhs)
            {
                return string.Compare(lhs.Name, rhs.Name);
            }
        }

        public delegate void ReleaseStates(AnimationEventStates eventState);

        private string m_graphName;
        private List<AnimationEventState> m_stateList = new List<AnimationEventState>();
        private string[] m_boneList;
        private List<string> m_vfxboneList = new List<string>();
        private ReleaseStates m_releaseStates;


        public List<AnimationEventState> StateList
        {
            get { return m_stateList; }
        }

        public string GraphName
        {
            get { return m_graphName; }
            set { m_graphName = value; }
        }

        public string[] BoneList
        {
            get { return m_boneList; }
            set { m_boneList = value; }
        }

        public List<string> VfxboneList
        {
            get { return m_vfxboneList; }
        }

        public void Init(ReleaseStates releaseStates)
        {
            m_releaseStates = releaseStates;
        }


        public void Release()
        {
            if (m_releaseStates != null)
            {
                m_releaseStates(this);
            }
        }

        public AnimationEventState GetState(string stateName)
        {
            for (int i = 0; i < m_stateList.Count; ++i)
            {
                AnimationEventState eventState = m_stateList[i];
                if (eventState.Name.Equals(stateName, StringComparison.CurrentCultureIgnoreCase))
                    return eventState;
            }

            return null;
        }
        public AnimationEventState GetState(int stateNameHash)
        {
            for (int i = 0; i < m_stateList.Count; ++i)
            {
                AnimationEventState eventState = m_stateList[i];
                if (eventState.NameHash == stateNameHash)
                    return eventState;
            }

            return null;
        }


        public AnimationEventState AddState(AnimationEventState eventState)
        {
            m_stateList.Add(eventState);
            return eventState;
        }

        public void StateSort()
        {
            m_stateList.Sort(stateComparer);
        }

        public bool RemoveState(AnimationEventState eventState)
        {
            if (m_stateList.Contains(eventState))
            {
                m_stateList.Remove(eventState);
                return true;
            }
            return false;
        }

        public bool RemoveState(string name)
        {
            for (int i = m_stateList.Count - 1; i >= 0; --i)
            {
                if (m_stateList[i].Name.Equals(name))
                {
                    m_stateList.Remove(m_stateList[i]);
                    return true;
                }
            }
            return false;
        }

        public AnimationEventStates Clone( /*ReleaseStates releaseStates*/)
        {
            AnimationEventStates cloneAnimationEventStates = new AnimationEventStates();
            cloneAnimationEventStates.Init(m_releaseStates);
            for (int i = 0; i < m_stateList.Count; i++)
            {
                cloneAnimationEventStates.AddState(m_stateList[i].Clone());
            }
            cloneAnimationEventStates.StateSort();
            cloneAnimationEventStates.GraphName = m_graphName;

            //m_releaseStates = releaseStates;

            return cloneAnimationEventStates;
        }
    }
}