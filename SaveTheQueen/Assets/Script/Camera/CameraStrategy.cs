using Aniz.Cam.Info;
using Aniz.Cam.State;
using UnityEngine;

namespace Aniz.Cam
{
    public class SmoothStock
    {
        public float Value = 0;
        public float Power = 1;
        public float LifeTime = 0;
        public float PeriodTime = 1000;
    };

    public class StrategyStock
    {
        public SmoothStock Distance = new SmoothStock();
        public SmoothStock Direction = new SmoothStock();
        public SmoothStock Target = new SmoothStock();
    };

    public class CameraStrategy
    {
        public enum eState
        {
            Free,
            Target,
            Game,
        }

        private StrategyStock m_strategyStock = new StrategyStock();
        private ICameraState m_curState;
        private CameraTargetState m_targetState;
        private CameraFreeState m_freeState;
        private CameraGameState m_gameState;

        private CamAdjustStock m_oriStock = new CamAdjustStock();
        private CamAdjustStock m_preStock = new CamAdjustStock();

        public StrategyStock Stock
        {
            get { return m_strategyStock; }
        }

        public ICameraState CurState
        {
            get { return m_curState; }
        }

        public CameraStrategy()
        {
            m_strategyStock.Distance.Value = 20.0f;
            m_strategyStock.Distance.Power = 0.5f;
            m_strategyStock.Distance.PeriodTime = 1.3f;

            m_strategyStock.Direction.Value = LEMath.s_fPI * 0.9f;
            m_strategyStock.Direction.Power = 0.8f;
            m_strategyStock.Direction.PeriodTime = 0.6f;

            m_strategyStock.Target.Value = 0.3f;
            m_strategyStock.Target.Power = 0.5f;
            m_strategyStock.Target.PeriodTime = 0.8f;

            m_freeState = new CameraFreeState(this);
            m_targetState = new CameraTargetState(this);
            m_gameState = new CameraGameState(this);

            m_curState = m_gameState;
        }

        public void Change(eState i_eState)
        {
            if (i_eState == eState.Target)
            {
                m_curState = m_targetState;
            }
            else if (i_eState == eState.Free)
            {
                m_curState = m_freeState;
            }
            else
            {
                m_curState = null;
            }

        }
        public void Run(CameraStock stock, Vector3 targetPos, ref CameraUpdateInfo updateInfo, float elapsedTime)
        {
            m_oriStock.Stock.Copy(stock);
            m_oriStock.Target = targetPos;

            m_curState.Run(m_oriStock, m_preStock, ref updateInfo, elapsedTime);

            m_preStock.Stock.Copy(updateInfo);
            m_preStock.Target = updateInfo.Target;
        }
        public float GetAdjustDistance(CameraUpdateInfo updateInfo)
        {
            return m_curState.GetDistance(updateInfo);
        }

        public Vector3 GetEyePos(ref CameraUpdateInfo updateInfo)
        {
            return m_curState.GetEyePos(ref updateInfo);
        }
    }
}