using System;

namespace Lib.Event
{
    public interface IMessage : INotify, IDisposable
    {
        bool IsDeliveryMsg { get; }
        bool IsNetMsg { get; }
    }

    public abstract class MessageBase : IMessage
    {
        public abstract uint MsgCode { get; }

        public virtual bool IsDeliveryMsg
        {
            get { return false; }
        }

        public virtual bool IsNetMsg
        {
            get { return false; }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return string.Format("{0} DeliveryMsg {1} IsNetMsg {2}", this.GetType().Name, IsDeliveryMsg, IsNetMsg);
        }
    }

    public abstract class DeliveryMessage : MessageBase
    {
        public long receiverID { get; set; }

        public override bool IsDeliveryMsg
        {
            get { return true; }
        }
    }

    public abstract class DelayMessage : DeliveryMessage
    {
        public float DelayTime { get; set; }

        public bool DelayTimeCheck(float deltaTime)
        {
            DelayTime -= deltaTime;
            if (DelayTime <= 0.0f)
            {
                return true;
            }
            return false;
        }
    }

    public abstract class NetMessage : DeliveryMessage
    {
        public override bool IsNetMsg
        {
            get { return true; }
        }
    }


    public class SendMessage : MessageBase
    {
        private uint m_msgCode;
        public SendMessage(uint code)
        {
            m_msgCode = code;
        }

        public override uint MsgCode
        {
            get { return m_msgCode; }
        }
    }


    public class SendDeliveryMessage : DeliveryMessage
    {
        private uint m_msgCode;

        public SendDeliveryMessage(uint code)
        {
            m_msgCode = code;
        }

        public override uint MsgCode
        {
            get { return m_msgCode; }
        }
    }
}