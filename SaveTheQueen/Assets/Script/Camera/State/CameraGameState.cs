using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.State
{
    public class CameraGameState : ICameraState
    {
        private CameraStrategy m_cameraStrategy;
        private float m_prevTargetDiff;
        private Vector3 m_prevEye = Vector3.zero;
        private Vector3 m_prevAt = Vector3.zero;

        public CameraStrategy.eState State { get { return CameraStrategy.eState.Game; } }

        public CameraGameState(CameraStrategy cameraStrategy)
        {
            m_cameraStrategy = cameraStrategy;
        }
        public CameraStrategy Strategy
        {
            get { return m_cameraStrategy; }
        }

        public float GetDistance(CameraUpdateInfo updateInfo)
        {
            return updateInfo.Distance;
        }

        public Vector3 GetEyePos(ref CameraUpdateInfo updateInfo)
        {
            return GetEyeArcPos(ref updateInfo);
        }

        public void Run(CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, float deltaTime)
        {
            updateInfo.Target = origin.Target;

            CameraFreeState.CalcDistance(deltaTime, origin, prev, ref updateInfo, ref m_cameraStrategy.Stock.Distance);
            CameraFreeState.CalcDirection(deltaTime, origin, prev, ref updateInfo, ref m_cameraStrategy.Stock.Direction);
        }


        public Vector3 GetEyeArcPos(ref CameraUpdateInfo updateInfo)
        {
            Vector3 fEyePos = updateInfo.At + updateInfo.LookatDir * updateInfo.Distance;

            return fEyePos;
        }
    }
}