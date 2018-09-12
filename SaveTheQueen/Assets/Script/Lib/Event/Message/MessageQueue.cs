using System;
using System.Collections.Generic;
using Lib.Pattern;

namespace Lib.Event
{
    public class MessageQueue : IBaseClass
    {
        private Queue<IMessage> m_que_Message = new Queue<IMessage>();
        private List<DelayMessage> m_delayMsgList = new List<DelayMessage>();


        public void Initialize()
        {
        }

        public void Terminate()
        {
            m_que_Message.Clear();
        }

        public IMessage Pop()
        {
            if (m_que_Message.Count > 0)
            {
                return m_que_Message.Dequeue();
            }

            return null;
        }

        public void DelayPostMessage(DelayMessage i_pMessage)
        {
            m_delayMsgList.Add(i_pMessage);
        }

        public void PostMessage(IMessage message)
        {
            m_que_Message.Enqueue(message);
        }

        public void OnUpdate(float dt)
        {
            for (int i = m_delayMsgList.Count - 1; i >= 0; i--)
            {
                DelayMessage delayMessage = m_delayMsgList[i];
                if (delayMessage.DelayTimeCheck(dt))
                {
                    m_delayMsgList.Remove(delayMessage);
                    PostMessage(delayMessage);
                }
            }
        }
    }

}