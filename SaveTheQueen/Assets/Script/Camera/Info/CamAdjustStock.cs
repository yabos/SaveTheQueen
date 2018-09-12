using UnityEngine;

namespace Aniz.Cam.Info
{
    public class CamAdjustStock
    {
        private CameraStock m_Stock = new CameraStock();
        private Vector3 m_Target;

        public Vector3 Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        public CameraStock Stock
        {
            get { return m_Stock; }
        }
    };
}