using UnityEngine;

namespace Aniz.Cam.Info
{
    [System.Serializable]
    public class CameraStock
    {
        [SerializeField]
        private float m_distance;
        [SerializeField]
        private float m_vertical;
        [SerializeField]
        private float m_horizon;

        [SerializeField]
        private float m_fov;
        [SerializeField]
        private float m_heightOffset;

        public float Distance { get { return m_distance; } set { m_distance = value; } }
        public float Vertical { get { return m_vertical; } set { m_vertical = value; } }
        public float Horizon { get { return m_horizon; } set { m_horizon = value; } }
        public float Fov { get { return m_fov; } set { m_fov = value; } }
        public float HeightOffset { get { return m_heightOffset; } set { m_heightOffset = value; } }

        public CameraStock()
        {
            Distance = 28;
            Vertical = 10;
            Horizon = 0;
            Fov = 10;
            HeightOffset = 1.0f;
        }

        public void Copy(CameraStock stock)
        {
            m_distance = stock.Distance;
            m_vertical = stock.Vertical;
            m_horizon = stock.Horizon;
            m_fov = stock.Fov;
            m_heightOffset = stock.HeightOffset;
        }

        public void Copy(CameraUpdateInfo info)
        {
            m_distance = info.Distance;
            m_vertical = info.Vertical;
            m_horizon = info.Horizon;

            m_heightOffset = info.Height;
        }
    }
}