using UnityEngine;

namespace Aniz.Cam.Info
{
    [System.Serializable]
    public struct AtQuakeInfo : ICameraShakeStock
    {
        public string EffectName;
        public bool UserUsed;
        public float FadeTime;
        public float Power;
        public float MaxRange;
        public Vector3 Direction;
        public float LifeTime;
        public int LoopCount;

        public string Name { get { return EffectName; } }
        public bool User { get { return UserUsed; } }

    }
}