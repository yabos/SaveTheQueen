using Aniz.Cam.Info;
using UnityEngine;

namespace Aniz.Cam.State
{
    public class CameraFreeState : ICameraState
    {
        private CameraStrategy m_cameraStrategy;

        public CameraStrategy.eState State { get { return CameraStrategy.eState.Free; } }

        public CameraFreeState(CameraStrategy cameraStrategy)
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
            return updateInfo.At + updateInfo.LookatDir * updateInfo.Distance;
        }

        public void Run(CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, float deltaTime)
        {
            //rResult = crOrigin;

            CalcDistance(deltaTime, origin, prev, ref updateInfo, ref m_cameraStrategy.Stock.Distance);
            CalcDirection(deltaTime, origin, prev, ref updateInfo, ref m_cameraStrategy.Stock.Direction);
        }

        public static void CalcDistance(float deltaTime, CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, ref SmoothStock smooth)
        {
            if (prev.Stock.Distance == origin.Stock.Distance)
            {
                smooth.LifeTime = 0;
                return;
            }
            smooth.LifeTime += deltaTime;
            float elapsed = (float)smooth.LifeTime / smooth.PeriodTime;

            float difference = Mathf.Abs(prev.Stock.Distance - origin.Stock.Distance) / smooth.Value;
            float smoothDiff = GetSmoothValue(elapsed, difference, smooth.Power) * smooth.Value;
            if (smoothDiff == 0)
                return;

            if (prev.Stock.Distance > origin.Stock.Distance)
            {
                updateInfo.Distance = origin.Stock.Distance + smoothDiff;
            }
            else
            {
                updateInfo.Distance = origin.Stock.Distance - smoothDiff;
            }
        }

        public static void CalcDirection(float deltaTime, CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, ref SmoothStock smooth)
        {
            Vector2 v2Difference = Vector2.zero;
            v2Difference.x = LEMath.RadianDistance(origin.Stock.Horizon, prev.Stock.Horizon);
            v2Difference.y = LEMath.RadianDistance(origin.Stock.Vertical, prev.Stock.Vertical);

            float difference = v2Difference.magnitude;
            if (difference == 0)
            {
                smooth.LifeTime = 0;
                return;
            }
            smooth.LifeTime += deltaTime;
            float elapsed = (float)smooth.LifeTime / smooth.PeriodTime;

            difference = difference / smooth.Value;
            float smoothDiff = GetSmoothValue(elapsed, difference, smooth.Power) * smooth.Value;
            if (smoothDiff == 0)
                return;

            Vector2 v2Result = v2Difference.normalized;
            v2Result = v2Result * (smoothDiff);

            updateInfo.Horizon = origin.Stock.Horizon + v2Result.x;
            updateInfo.Vertical = origin.Stock.Vertical + v2Result.y;
        }

        public static void CalcTarget(float deltaTime, CamAdjustStock origin, CamAdjustStock prev, ref CameraUpdateInfo updateInfo, ref SmoothStock smooth)
        {
            float difference = Vector3.Distance(prev.Target, origin.Target);
            if (difference == 0.0f) return;

            if (difference - smooth.Value > 0)
            {
                smooth.LifeTime = 0;
            }

            smooth.LifeTime += deltaTime;

            float elapsed = 0.1f;
            float smoothDiff = GetSmoothValue(elapsed, difference, smooth.Power) * 2.0f;
            if (smoothDiff == 0) return;

            updateInfo.Distance += smoothDiff + 0.01f;

            Vector3 dir = prev.Target - origin.Target;
            dir.Normalize();
            updateInfo.Target = origin.Target + dir * smoothDiff;
        }

        private static float GetSmoothValue(float deltaTime, float origin, float power, bool lerp = false)
        {
            deltaTime = Mathf.Min(deltaTime, 1.0f);
            if (!lerp)
            {
                float resultPower = Mathf.Max(0.01f, Mathf.Min(power, 0.99f));
                float valueK = 0.5f - resultPower * 0.5f;
                float valueA = 2.0f - (4.0f * valueK);
                float valueB = 2.0f * valueK - 0.5f;

                float resultOrigin = Mathf.Max(0.0f, Mathf.Min(origin, 1.0f));

                // Y = a * X^2 + b * X 의 X값구하기
                float sqrtInside = (resultOrigin / valueA) + ((valueB * valueB) / (valueA * valueA));
                float originT = Mathf.Sqrt(sqrtInside) - (valueB / valueA);

                if (originT <= deltaTime)
                {
                    return 0.0f;
                }

                float resultT = originT - deltaTime;
                float result = valueA * resultT * resultT + 2.0f * valueB * resultT;

                return Mathf.Clamp(result, 0.0f, resultOrigin);
            }

            float easeInOut = LEMath.EaseFromTo(0, 1, deltaTime, LEMath.eEaseType.EaseOut);
            return easeInOut * origin;
        }

    }
}