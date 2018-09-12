using UnityEngine;

namespace Aniz.Cam.Info
{
    [System.Serializable]
    public class LimitStock
    {
        [SerializeField]
        private float m_MinDistance = 1.0f;
        [SerializeField]
        private float m_MaxDistance = 100.0f;
        [SerializeField]
        private float m_HorMinRadian = -LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;
        [SerializeField]
        private float m_HorMaxRadian = LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;
        [SerializeField]
        private float m_VerMinRadian = -LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;
        [SerializeField]
        private float m_VerMaxRadian = LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;

        public float MinDistance { get { return m_MinDistance; } set { m_MinDistance = value; } }
        public float MaxDistance { get { return m_MaxDistance; } set { m_MaxDistance = value; } }
        public float HorMinRadian { get { return m_HorMinRadian; } set { m_HorMinRadian = value; } }
        public float HorMaxRadian { get { return m_HorMaxRadian; } set { m_HorMaxRadian = value; } }
        public float VerMinRadian { get { return m_VerMinRadian; } set { m_VerMinRadian = value; } }
        public float VerMaxRadian { get { return m_VerMaxRadian; } set { m_VerMaxRadian = value; } }

        public LimitStock()
        {
            MinDistance = 1.0f;
            MaxDistance = 1000.0f;

            VerMinRadian = -LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;
            VerMaxRadian = LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;

            HorMinRadian = -LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;
            HorMaxRadian = LEMath.s_fHalfPI * 0.99f * Mathf.Rad2Deg;

        }
    };
}