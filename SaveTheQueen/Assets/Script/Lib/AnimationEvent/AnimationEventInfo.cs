using System.Collections.Generic;

namespace Lib.AnimationEvent
{
    public class AnimationEventInfo
    {
        public string param;

        private string m_functionName;
        private int m_frame;
        private float m_time;
        private float m_nomalizeTime;
        private bool m_eventOnExit = false;
        public IAnimationEventAttribute attribute = null;

        public string FunctionName
        {
            get { return m_functionName; }
            set { m_functionName = value; }
        }

        public float Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public int Frame
        {
            get { return m_frame; }
            set { m_frame = value; }
        }

        public float NomalizeTime
        {
            get { return m_nomalizeTime; }
            set { m_nomalizeTime = value; }
        }

        public bool EventOnExit
        {
            get { return m_eventOnExit; }
            set { m_eventOnExit = value; }
        }

        public AnimationEventInfo Clone()
        {
            AnimationEventInfo animationEventInfo = new AnimationEventInfo();
            animationEventInfo.Frame = m_frame;
            animationEventInfo.FunctionName = m_functionName;
            animationEventInfo.param = param;
            animationEventInfo.NomalizeTime = m_nomalizeTime;
            animationEventInfo.Time = m_time;
            animationEventInfo.EventOnExit = m_eventOnExit;

            animationEventInfo.attribute = attribute; //AnimationEventUtil.CreateAttribute(FunctionName, param);
            return animationEventInfo;
        }

        public AnimationEventInfo EditorClone()
        {
            AnimationEventInfo animationEventInfo = new AnimationEventInfo();
            animationEventInfo.Frame = m_frame;
            animationEventInfo.FunctionName = m_functionName;
            animationEventInfo.param = param;
            animationEventInfo.NomalizeTime = m_nomalizeTime;
            animationEventInfo.Time = m_time;
            animationEventInfo.EventOnExit = m_eventOnExit;

            animationEventInfo.attribute = AnimationEventUtil.CreateAttribute(FunctionName, param);
            return animationEventInfo;
        }

        public class Comparer : IComparer<AnimationEventInfo>
        {
            public int Compare(AnimationEventInfo lhs, AnimationEventInfo rhs)
            {
                if (lhs.m_frame == rhs.m_frame)
                {
                    int compare = string.Compare(lhs.FunctionName, rhs.FunctionName);
                    if (compare == 0)
                    {
                        return string.Compare(lhs.param, rhs.param);
                    }
                    else
                    {
                        return compare;
                    }
                }
                return lhs.m_frame - rhs.m_frame;
            }
        }
    }

}