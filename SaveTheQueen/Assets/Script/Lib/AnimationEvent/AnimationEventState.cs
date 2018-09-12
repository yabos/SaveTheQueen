using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.AnimationEvent
{
    public class AnimationEventState
    {
        public string Name { get; private set; }
        public int NameHash { get; private set; }

        public bool Visible { get; set; }
        public float AnimationTime { get; set; }
        public bool isLooping { get; set; }
        public string clipName { get; set; }

        private List<AnimationEventInfo> m_eventList = new List<AnimationEventInfo>();
        private static AnimationEventInfo.Comparer animationEventComparer = new AnimationEventInfo.Comparer();

        public bool Initialized { get; private set; }

        public List<AnimationEventInfo> EventList
        {
            get { return m_eventList; }
        }

        public AnimationEventState Clone()
        {
            AnimationEventState clone = new AnimationEventState(Name);
            clone.Visible = Visible;
            clone.isLooping = isLooping;
            clone.AnimationTime = AnimationTime;
            clone.clipName = clipName;
            //clone.m_eventList = m_eventList;

            for (int i = 0; i < m_eventList.Count; i++)
            {
                AnimationEventInfo animationEventInfo = m_eventList[i];
                clone.AddAnimationEvent(animationEventInfo.Clone());
            }

            return clone;
        }

        public AnimationEventState(string name)
        {
            Name = name;
            NameHash = Animator.StringToHash(Name);
        }

        public AnimationEventInfo AddAnimationEvent(AnimationEventInfo animationEventInfo)
        {
            m_eventList.Add(animationEventInfo);
            return animationEventInfo;
        }

        public void ConvertAnimationEvents()
        {
            for (int i = 0; i < m_eventList.Count; ++i)
            {
                AnimationEventInfo animationEventInfo = m_eventList[i];
                AnimationEventUtil.Serialize(out animationEventInfo.param, animationEventInfo.attribute);
            }
        }

        public void SortAnimationEvent()
        {
            m_eventList.Sort(animationEventComparer);
        }

        public void RemoveAnimationEvent(AnimationEventInfo animationEventInfo)
        {
            if (m_eventList.Contains(animationEventInfo))
            {
                m_eventList.Remove(animationEventInfo);
            }
        }

#if UNITY_EDITOR

        public bool CheckEvent(int frame, string param)
        {
            for (int i = 0; i < m_eventList.Count; ++i)
            {
                AnimationEventInfo animationEventInfo = m_eventList[i];
                if (animationEventInfo.Frame == frame)
                {
                    if (animationEventInfo.param.Equals(param, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

#endif
    }
}