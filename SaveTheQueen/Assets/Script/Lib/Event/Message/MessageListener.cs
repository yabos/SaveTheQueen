using System.Collections.Generic;

namespace Lib.Event
{
    public interface IMessageHandler
    {
        bool OnMessage(IMessage message);
    }

    public interface IMessageHandler<T> where T : IMessage
    {
        bool OnMessage(T message);
    }

    public delegate bool MessageHandlerFunc(IMessage orcaMessage);

    public abstract class MessageListener : IMessageHandler
    {
        protected Dictionary<uint, MessageHandlerFunc> m_dicMessageFunc = new Dictionary<uint, MessageHandlerFunc>();

        public bool OnMessage(IMessage message)
        {
            MessageHandlerFunc func = null;
            if (m_dicMessageFunc.TryGetValue(message.MsgCode, out func))
            {
                return func(message);
            }

            return false;
        }

        protected void AddHandler(uint msgCode, MessageHandlerFunc func)
        {
            if (m_dicMessageFunc.ContainsKey(msgCode) == false)
            {
                m_dicMessageFunc.Add(msgCode, func);
            }
        }
    }

    public abstract class MessageListener<T> where T : IMessage, IMessageHandler<T>
    {
        protected Dictionary<uint, MessageHandlerFunc> m_dicMessageFunc = new Dictionary<uint, MessageHandlerFunc>();

        public bool OnMessage(T message)
        {
            MessageHandlerFunc func = null;
            if (m_dicMessageFunc.TryGetValue(message.MsgCode, out func))
            {
                return func(message);
            }

            return false;
        }

        protected void AddHandler(uint msgCode, MessageHandlerFunc func)
        {
            if (m_dicMessageFunc.ContainsKey(msgCode) == false)
            {
                m_dicMessageFunc.Add(msgCode, func);
            }
        }
    }

}