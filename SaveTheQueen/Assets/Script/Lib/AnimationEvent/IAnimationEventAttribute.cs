using Lib.Parse;

namespace Lib.AnimationEvent
{
    [System.Flags]
    public enum eAnimationGameEvent
    {
        NONE,

        ENABLE_ATTACK,
        DISABLE_ATTACK,
    }

    [System.Flags]
    public enum eShaderAnimEventType
    {
        SPAWN_ALPHA,
        DISSOLVE_BEGIN,
        DISSOLVE,
        DAMAGE_RIM,         //Decrescendo
        LHAND_RIM,
        RHAND_RIM,
        DAMAGE_RIM_CRES,    //Crescendo
        LHAND_RIM_CRES,
        RHAND_RIM_CRES,
        NONE,
    }

    public enum ePostEffectType
    {
        COLOR_CORRECTION,       //Decrescendo
        COLOR_CORRECTION_CRES,  //Crescendo
        NONE,
    }

    public interface IAnimationEventAttribute
    {
        void OnSerialize(KeyValueSerializer serializer);

        bool InitEventOnExit { get; }
    }

    public interface IAnimationEventHandler
    {
        string HandlerName { get; }
        void OnAnimationEvent(string eventName, IAnimationEventAttribute arg);
    }
}