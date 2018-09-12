
namespace Aniz.Cam.Info
{
    [System.Serializable]
    public struct EyeQuakeInfo : ICameraShakeStock
    {
        public enum Type
        {
            Increase = 0,
            Decrease,
            FullDuplex,
        };

        public string EffectName;
        public bool UserUsed;

        public Type eType;
        public int LoadCount;
        public uint BlendWidth;
        public int StepCount;
        public float TimeLength;
        public float MaxRange;
        public bool RandState;
        public float RandLength;
        public float LifeTime;

        public string Name { get { return EffectName; } }
        public bool User { get { return UserUsed; } }

    }
}