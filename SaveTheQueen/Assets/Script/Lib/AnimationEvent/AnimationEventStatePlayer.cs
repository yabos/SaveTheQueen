
namespace Lib.AnimationEvent
{
    public class AnimationEventStatePlayer
    {
        private readonly IAnimationEventHandler m_animationEventHandler;

        private AnimationEventState m_currentState;
        private int m_playtimes = 0;
        private bool[] m_sentactorEvents;


        public AnimationEventStatePlayer(IAnimationEventHandler handler)
        {
            m_animationEventHandler = handler;
        }

        public void OnEnter(AnimationEventState state)
        {
            m_currentState = state;
            m_animationEventHandler.OnAnimationEvent(AnimationEventUtil.Enter, null);
            ResetAniTime();
        }

        public void OnLeave()
        {
            if (m_currentState == null)
                return;

            if (m_sentactorEvents == null)
                return;

            for (int i = 0; i < m_sentactorEvents.Length; i++)
            {
                if (m_sentactorEvents[i] == false)
                {
                    if (m_currentState.EventList.Count > i && m_currentState.EventList[i] != null)
                    {
                        AnimationEventInfo animationEventInfo = m_currentState.EventList[i];
                        if (animationEventInfo.EventOnExit)
                        {
                            m_sentactorEvents[i] = true;
                        }
                        m_animationEventHandler.OnAnimationEvent(animationEventInfo.FunctionName,
                            animationEventInfo.attribute);
                        EventLogger.Log(EventLogType.AnimEvent_State,
                            string.Format("OnLeave ActorName : {0} Event : {1}_{2}, NorTime {3} ",
                                m_animationEventHandler.HandlerName, animationEventInfo.FunctionName,
                                animationEventInfo.NomalizeTime, animationEventInfo.attribute));
                    }
                }
            }
            m_currentState = null;
            m_animationEventHandler.OnAnimationEvent(AnimationEventUtil.Exit, null);
        }

        public void UpdateEvents(float animNormalizedTime, bool looping /*, AActor actor, AController controller*/)
        {
            if (null == m_currentState)
                return;

            if (null == m_currentState.EventList)
                return;

            if (m_currentState.EventList.Count <= 0)
                return;

            int currenttimes = (int)(animNormalizedTime / 1f);
            float currentNormalizedTime = animNormalizedTime % 1.0f;

            if (m_playtimes != currenttimes && looping)
            {
                m_playtimes = currenttimes;
                ResetAniTime();
            }

            // TODO: do more optimization with last fired event index
            for (int i = 0; i < m_currentState.EventList.Count; ++i)
            {
                AnimationEventInfo animationEventInfo = m_currentState.EventList[i];

                if (animationEventInfo.NomalizeTime < currentNormalizedTime && !m_sentactorEvents[i])
                {
                    m_sentactorEvents[i] = true;

                    m_animationEventHandler.OnAnimationEvent(animationEventInfo.FunctionName,
                        animationEventInfo.attribute);

                    eAnimationEventTypeMask mask =
                        AnimationEventUtil.GetAnimationEventTypeMask(animationEventInfo.FunctionName);
                    EventLogType eventLogType = EventLogType.AnimEvent_Combat;
                    if (mask == eAnimationEventTypeMask.SFX)
                    {
                        eventLogType = EventLogType.AnimEvent_SFX;
                    }
                    else if (mask == eAnimationEventTypeMask.VFX)
                    {
                        eventLogType = EventLogType.AnimEvent_VFX;
                    }
                    EventLogger.Log(eventLogType,
                        string.Format(
                            "State[{8}] UpdateEvents[{6}][{7}] ActorName : {0} Event : {1}_{4}, Time1 : {2} NorTime {3} currentNormalizedTime : {5}",
                            m_animationEventHandler.HandlerName, animationEventInfo.FunctionName, animNormalizedTime,
                            animationEventInfo.NomalizeTime, animationEventInfo.attribute, currentNormalizedTime, i,
                            m_sentactorEvents[i], m_currentState.Name));
                }
            }
        }

        private void ResetAniTime()
        {
            if (m_currentState.EventList.Count <= 0)
                return;

            if (m_sentactorEvents == null)
            {
                m_sentactorEvents = new bool[m_currentState.EventList.Count];
            }
            else
            {
                if (m_currentState.EventList.Count != m_sentactorEvents.Length)
                {
                    m_sentactorEvents = new bool[m_currentState.EventList.Count];
                }
            }

            for (int i = 0; i < m_sentactorEvents.Length; i++)
            {
                m_sentactorEvents[i] = false;
            }

        }
    }
}