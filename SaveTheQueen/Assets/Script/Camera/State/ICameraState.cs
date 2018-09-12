using System;
using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.State
{
    public interface ICameraState
    {
        CameraStrategy.eState State { get; }
        CameraStrategy Strategy { get; }
        void Run(CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, float deltaTime);
        Vector3 GetEyePos(ref CameraUpdateInfo updateInfo);
        float GetDistance(CameraUpdateInfo updateInfo);
    }
}