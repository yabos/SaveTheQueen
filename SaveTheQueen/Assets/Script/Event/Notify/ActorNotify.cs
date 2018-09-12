namespace Aniz.Event
{
    public abstract class ActorNotifyBase : EventNotifyBase
    {
        public virtual bool IsNetMsg
        {
            get
            {
                return true;
            }
        }

        public int ReceiverID { get; set; }

        public virtual bool IsDeliveryMsg
        {
            get { return true; }
        }
    }

    public class SpawnerNotify : EventNotify
    {
        public int ActorNetID { get; set; }
        public int SpawnerID { get; set; }

        public SpawnerNotify(eMessage message) : base(message)
        {

        }
    }
}