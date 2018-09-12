using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.Quake
{
    public class EyeQuakeUnit : CameraQuakeUnit
    {
        public class Stock
        {
            public EyeQuakeInfo.Type eType { get; set; }
            public float MaxRange { get; set; }
            public uint StartTime { get; set; }

            public int LoadCount { get; set; }
            public uint BlendWidth { get; set; }
            public int StepCount { get; set; }
            public float TimeLength { get; set; }
            public bool RandState { get; set; }
            public float RandLength { get; set; }

            public Stock()
            {
                eType = EyeQuakeInfo.Type.Increase;

                MaxRange = 1;
                StartTime = 0;
                LoadCount = 30;
                BlendWidth = 3;
                StepCount = 2;
                TimeLength = 50.0f;
                RandState = false;
                RandLength = 30.0f;
            }

            public Stock(EyeQuakeInfo eyeQuakeData)
            {
                eType = eyeQuakeData.eType;

                MaxRange = eyeQuakeData.MaxRange;
                LoadCount = eyeQuakeData.LoadCount;
                BlendWidth = eyeQuakeData.BlendWidth;
                StepCount = eyeQuakeData.StepCount;
                TimeLength = eyeQuakeData.TimeLength;
                RandState = eyeQuakeData.RandState;
                RandLength = eyeQuakeData.RandLength;
            }
        };

        private EyeQuakeInfo m_eyeQuakeData;
        private Vector3 m_prePos;

        private void SendEffectQuake()
        {
            if (m_Played)
                return;

            Stock stock = new Stock(m_eyeQuakeData);

            Global.CameraMgr.Impl.EyeQuake.SetQuakeUnit(stock);
            m_Played = true;
        }

        public override void Capture()
        {
            m_Played = false;
            m_Finish = false;
        }

        public override void UnitRelease()
        {
            Terminate();
        }

        public override void Initialize(ICameraShakeStock cameraEffectData)
        {
            m_eyeQuakeData = (EyeQuakeInfo)cameraEffectData;
            m_UserUsed = cameraEffectData.User;
            m_Radius = m_eyeQuakeData.MaxRange;
            m_Played = false;
        }

        public override void Play(Vector3 offset)
        {
            m_Finish = false;
            m_Played = false;

            PlayInit(m_eyeQuakeData.LifeTime);

            ParentPosition(offset);
        }

        public override bool Process(float deltaTime)
        {
            if (ProcessTime(ref m_lifeTime, deltaTime) == false)
            {
                Terminate();
                return false;
            }

            if (m_Played == false)
            {
                SendEffectQuake();
            }
            return !m_Finish;
        }

        public override void ReStart()
        {
            Terminate();

            m_Finish = false;
            m_Played = false;
        }

        public override void Terminate()
        {
            if (m_Finish)
            {
                return;
            }

            m_Finish = true;
            m_Played = false;

            Global.CameraMgr.Impl.EyeQuake.RemoveQuakeUnit();
        }
    }
}