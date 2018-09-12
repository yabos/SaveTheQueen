namespace Aniz.Event
{
    public class UIEventNotify : EventNotify
    {

        public string Data = string.Empty;

        public UIEventNotify(eMessage subMessage) : this(subMessage, string.Empty)
        {
        }

        public UIEventNotify(eMessage subMessage, string data) : base(subMessage)
        {
            Data = data;
        }
    }
}