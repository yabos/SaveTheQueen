using System.Collections.Generic;
using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.Quake
{
    public class AtQuakeUnit : CameraQuakeUnit
    {
        enum E_ID
        {
            Effect = 0x0100,
        }

        public class Stock
        {
            public string Name { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Direction { get; set; }
            public float Power { get; set; }
            public float Dist { get; set; }

            public class Sort : IComparer<Stock>
            {
                public int Compare(Stock x, Stock y)
                {
                    if (x.Power > y.Power) return 1;
                    if (x.Power < y.Power) return -1;

                    if (x.Dist < y.Dist) return 1;
                    return -1;
                }
            }
        }

        private AtQuakeInfo m_atCameraData;
        private Stock m_AtQuakeStock = new Stock();

        private bool m_DirChecked;
        private Vector3 m_offsetPos;

        private int m_QuakeLoopCount;

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
            m_atCameraData = (AtQuakeInfo)cameraEffectData;
            m_UserUsed = cameraEffectData.User;

            m_Radius = m_atCameraData.MaxRange;
            m_Played = false;
            m_Finish = true;

            m_AtQuakeStock.Name = m_atCameraData.Name;
            m_AtQuakeStock.Power = m_atCameraData.Power;
            if (m_atCameraData.Direction == Vector3.zero)
            {
                m_AtQuakeStock.Direction = (LEMath.RandomDirection());
            }
            else
            {
                m_AtQuakeStock.Direction = new Vector3(m_atCameraData.Direction.x * LEMath.s_fDegreeToRadian,
                                                        m_atCameraData.Direction.y * LEMath.s_fDegreeToRadian,
                                                        m_atCameraData.Direction.z * LEMath.s_fDegreeToRadian);
            }
        }

        public override void Play(Vector3 offset)
        {
            if (m_atCameraData.Direction == Vector3.zero)
            {
                m_AtQuakeStock.Direction = (LEMath.RandomDirection());
            }

            m_QuakeLoopCount = m_atCameraData.LoopCount;
            if (m_QuakeLoopCount == 0)
                m_QuakeLoopCount = 10000;

            m_Finish = false;
            m_Played = false;

            PlayInit(m_atCameraData.LifeTime);

            m_offsetPos = offset;
            ParentPosition(m_offsetPos);

            EventLogger.Log(EventLogType.CameraShake, "Play");
        }

        public override bool Process(float deltaTime)
        {

            if (ProcessTime(ref m_lifeTime, deltaTime) == false)
            {
                Terminate();
                return false;
            }
            ParentPosition(m_offsetPos);
            ProcessElement(deltaTime);

            SendEffectQuake();

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
                return;

            m_Finish = true;
            m_Played = false;

            RemoveEffectQuake();

            EventLogger.Log(EventLogType.CameraShake, "Terminate");
        }

        private void ProcessElement(float elapsedTime)
        {
            float time = m_lifeTime;
            m_AtQuakeStock.Position = m_centerPos;
            Vector3 at = (Global.CameraMgr.Impl.UpdateInfo.At);
            m_AtQuakeStock.Dist = Vector3.Distance(m_centerPos, at);
            float FadeTime = m_atCameraData.FadeTime;
            float Count = time / FadeTime;
            if (m_AtQuakeStock.Dist > m_atCameraData.MaxRange || Count > m_QuakeLoopCount)
            {
                m_AtQuakeStock.Power = 0.0f;
                return;
            }

            float FadePower = (LEMath.FMOD(time, FadeTime)) / FadeTime;
            if (LEMath.FMOD(Count, 2) == 0)
            {
                FadePower = FadeTime;
                m_DirChecked = true;
            }
            else
            {
                FadePower = (1.0f - FadePower);
            }
            if (m_DirChecked && m_atCameraData.Direction == Vector3.zero)
            {
                m_AtQuakeStock.Direction = (LEMath.RandomDirection());
            }


            float fDistPower = m_atCameraData.Power * (1.0f - (m_AtQuakeStock.Dist / m_atCameraData.MaxRange));
            m_AtQuakeStock.Power = fDistPower * FadePower;
        }

        private void SendEffectQuake()
        {
            if (m_Played)
                return;

            m_AtQuakeStock.Position = m_centerPos;
            Global.CameraMgr.Impl.AtQuake.SetQuakeUnit(m_AtQuakeStock);

            m_Played = true;
        }

        private void RemoveEffectQuake()
        {
            Global.CameraMgr.Impl.AtQuake.RemoveQuakeUnit(m_AtQuakeStock);
        }
    }
}