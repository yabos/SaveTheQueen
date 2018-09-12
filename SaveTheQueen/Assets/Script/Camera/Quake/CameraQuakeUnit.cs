using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.Quake
{
    public abstract class CameraQuakeUnit
    {
        protected float m_Radius;
        protected bool m_UserUsed;

        protected float m_lifeTime;             ///< 지금까지 흐른 시간
		protected bool m_Played;
        protected bool m_Finish = true;

        protected Vector3 m_centerPos;

        protected Transform m_parent;

        public bool IsFinish { get { return m_Finish; } }
        public bool IsUserUsed { get { return m_UserUsed; } }

        public void SetParent(Transform parent)
        {
            m_parent = parent;
        }

        public abstract void Capture();
        public abstract void UnitRelease();
        public abstract bool Process(float deltaTime);
        public abstract void Initialize(ICameraShakeStock cameraShakeStock);
        public abstract void ReStart();
        public abstract void Terminate();
        public abstract void Play(Vector3 offset);


        protected void PlayInit(float fLifeTime)
        {
            ReStart();
            m_centerPos = Vector3.zero;

            m_Finish = false;
            m_lifeTime = fLifeTime;
        }

        protected void ParentPosition(Vector3 offset)
        {
            m_centerPos = offset;
            if (m_parent != null)
            {
                m_centerPos = m_parent.position + offset;
            }
        }

        protected bool ProcessTime(ref float lifeTime, float elapsedTime)
        {
            if (elapsedTime >= lifeTime)
            {
                lifeTime = 0;
                return false;
            }
            else
            {
                lifeTime -= elapsedTime;
            }
            return true;
        }
    }




}