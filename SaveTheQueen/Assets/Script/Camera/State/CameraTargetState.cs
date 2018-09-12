using System;
using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.State
{
    public class CameraTargetState : ICameraState
    {
        private CameraStrategy m_cameraStrategy;
        private float m_EyeHeight;
        private Vector3 m_AtPos;
        public CameraStrategy.eState State { get { return CameraStrategy.eState.Target; } }

        public CameraTargetState(CameraStrategy cameraStrategy)
        {
            m_cameraStrategy = cameraStrategy;
            m_AtPos = Vector3.zero;
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
            if (m_AtPos != updateInfo.At)
            {
                updateInfo.Distance = (m_EyeHeight - updateInfo.At.y) / (-updateInfo.LookatDir.y);

                Global.CameraMgr.Impl.Stock.Distance = updateInfo.Distance;
                //Global.CameraMgr.Impl.UpdateStock();
            }

            Vector3 eyePos = updateInfo.At + updateInfo.LookatDir * updateInfo.Distance;

            m_EyeHeight = eyePos.y;
            m_AtPos = updateInfo.At;

            return eyePos;
        }

        public void Run(CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, float deltaTime)
        {
            //rResult = crOrigin;

            CameraFreeState.CalcDistance(deltaTime, origin, prev, ref updateInfo, ref m_cameraStrategy.Stock.Distance);
            CameraFreeState.CalcDirection(deltaTime, origin, prev, ref updateInfo, ref m_cameraStrategy.Stock.Direction);
        }
    }
}