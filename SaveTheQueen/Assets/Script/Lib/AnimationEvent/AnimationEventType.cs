
namespace Lib.AnimationEvent
{
    [System.Flags]
    public enum eAnimationEventTypeMask
    {
        None = 0x00,
        Combat = 0x01,
        SFX = 0x02,
        VFX = 0x04,
    }

    public enum eAnimationEventType
    {
        SendAniEvent,

        PlayVFX, // ParticleSystem;
        StopVFX,

        PlaySFX,

        BGMAudioSwitcher,
        SetCombatHelper,

        PlayShaderFX,

        PlayPostEffect,

        Nothing
    }
}