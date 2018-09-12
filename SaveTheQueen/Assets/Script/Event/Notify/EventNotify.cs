using Lib.Event;

namespace Aniz.Event
{


    public abstract class EventNotifyBase : INotify
    {
        public uint MsgCode
        {
            get { return (uint)MsgType; }
        }

        public abstract eMessage MsgType { get; }
    }

    public class EventNotify : EventNotifyBase
    {
        private eMessage m_message;
        public EventNotify(eMessage message)
        {
            m_message = message;
        }

        public override eMessage MsgType
        {
            get { return m_message; }
        }
    }
}