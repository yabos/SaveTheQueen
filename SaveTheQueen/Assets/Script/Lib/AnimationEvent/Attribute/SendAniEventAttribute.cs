using Lib.Parse;

namespace Lib.AnimationEvent.Attribute
{
    public class SendAniEventAttribute : IAnimationEventAttribute
    {
        public eAnimationGameEvent animationGameEvent = eAnimationGameEvent.NONE;
        public string Argument;

        public bool InitEventOnExit
        {
            get { return false; }
        }

        public static IAnimationEventAttribute OnCreate(string text)
        {
            SendAniEventAttribute attribute = new SendAniEventAttribute();

            AnimationEventUtil.Deserialize(text, ref attribute);

            return attribute;
        }

        public void OnSerialize(KeyValueSerializer serializer)
        {
            serializer.Serialize("EventName", ref animationGameEvent);
            serializer.Serialize("Argument", ref Argument);
            //	serializer.Serialize("ENABLEATTACK_EVENTNUM", ref enableAttackEventNum);

        }

        public override string ToString()
        {
            return string.Format("SendAniEventAttribute_{0}", animationGameEvent);
        }
    }
}