using System;
using System.Collections.Generic;
using Aniz.Cam.Info;
using Aniz.Cam.Quake;
using UnityEngine;

namespace Aniz.Cam.Player
{
    public class CamEyeQuake : ICameraPlayer
    {
        private List<Vector3> m_lstPoints = new List<Vector3>();
        private List<Vector3> m_lstCamera = new List<Vector3>();
        private CameraUpdateInfo m_cameraUpdateInfo;

        private int m_EyeCamPost;
        private float m_tickTmCount;
        private bool m_CamEyePlay;
        private bool m_CamEyeSwap;
        private List<Vector3> m_lstRoundPos = new List<Vector3>();

        private EyeQuakeUnit.Stock m_effectQuake;

        public E_CameraPlayer CameraPlayer
        {
            get { return E_CameraPlayer.AtQuakeEffect; }
        }

        public void Update(ref CameraUpdateInfo cameraUpdateInfo, float deltaTime)
        {
            m_cameraUpdateInfo = cameraUpdateInfo;
            m_tickTmCount += deltaTime;

            if (m_effectQuake == null || m_CamEyePlay == false) return;

            switch (m_effectQuake.eType)
            {
                case EyeQuakeInfo.Type.Increase:
                    {
                        if (UpdateIncrease(ref cameraUpdateInfo))
                        {
                            m_CamEyePlay = false;
                        }
                    }
                    break;
                case EyeQuakeInfo.Type.Decrease:
                    {
                        if (UpdateDecrease(ref cameraUpdateInfo))
                        {
                            m_CamEyePlay = false;
                        }
                    }
                    break;
                case EyeQuakeInfo.Type.FullDuplex:
                    {
                        if (UpdateFullDuplex(ref cameraUpdateInfo))
                        {
                            m_CamEyePlay = false;
                        }
                    }
                    break;
            }

        }

        public void SetQuakeUnit(EyeQuakeUnit.Stock effectQuake)
        {
            if (m_effectQuake != null)
                return;

            m_effectQuake = effectQuake;
            m_effectQuake.BlendWidth = System.Math.Max(m_effectQuake.BlendWidth, 3);

            SetCameraEyeEvent();
        }

        public void RemoveQuakeUnit()
        {
            m_effectQuake = null;
            m_CamEyePlay = false;
            m_CamEyeSwap = false;
        }

        private void SetCameraEyeEvent()
        {

            m_tickTmCount = 0.0f;

            SetEyePosition(m_cameraUpdateInfo);

            switch (m_effectQuake.eType)
            {
                case EyeQuakeInfo.Type.Increase:
                case EyeQuakeInfo.Type.FullDuplex:
                    {
                        m_EyeCamPost = 0;
                    }
                    break;
                case EyeQuakeInfo.Type.Decrease:
                    {
                        m_EyeCamPost = m_effectQuake.LoadCount - 1;
                    }
                    break;
            }

            m_CamEyePlay = true;
            m_CamEyeSwap = true;
        }

        private void SetEyePosition(CameraUpdateInfo iRUpdateInfo)
        {
            m_lstPoints.Clear();
            m_lstCamera.Clear();

            LimitStock crLimit = Global.CameraMgr.Impl.Limit;

            m_lstPoints.Add((iRUpdateInfo.At + iRUpdateInfo.LookatDir * crLimit.MinDistance));   // Last point

            float fDistance = iRUpdateInfo.Distance / 4.0f;

            float nXDir = m_effectQuake.RandLength;
            float nYDir = m_effectQuake.RandLength;

            int ran = LEMath.RandInt(2);
            if (m_effectQuake.RandState && ran > 0)
            {
                nXDir *= -1.0f;
                nYDir *= -1.0f;
            }

            for (int i = 0; i < 2; i++)
            {
                if (i % 2 > 0)
                {
                    nXDir *= -1.0f;
                    nYDir *= -1.0f;
                }
                if (m_effectQuake.RandState)
                {
                    nXDir = LEMath.RandRange(0.0f, nXDir);
                    nYDir = LEMath.RandRange(0.0f, nYDir);
                }

                float valueDst = (i + 2) * fDistance;
                float horAngle = (iRUpdateInfo.Horizon) + nXDir;
                float verAngle = (iRUpdateInfo.Vertical) + nYDir;

                Vector3 lookatDir = (LEMath.AngleToDirection(verAngle * LEMath.s_fDegreeToRadian, horAngle * LEMath.s_fDegreeToRadian));

                m_lstPoints.Add((iRUpdateInfo.At + lookatDir * (valueDst)));
            }

            m_lstPoints.Add((iRUpdateInfo.At + iRUpdateInfo.LookatDir * iRUpdateInfo.Distance));  // Start point

            SetRespectiveBlending();    // Camera path setting
        }

        private void SetRoundEllipse(int iStart, int iEnd, Vector2 fSize, Vector3 fPos)
        {
            m_lstRoundPos.Clear();

            for (int i = iStart; i < (iEnd + iStart); ++i)
            {
                float degInRad = i * LEMath.s_fDegreeToRadian;

                m_lstRoundPos.Add(new Vector3(fPos.x + Mathf.Cos(degInRad) * fSize.x,
                                                fPos.y,
                                                fPos.z + Mathf.Sin(degInRad) * fSize.y));
            }
        }

        private bool UpdateIncrease(ref CameraUpdateInfo iRUpdateInfo)
        {
            iRUpdateInfo.Eye = GetRespectiveBlending(m_EyeCamPost);

            if (m_tickTmCount >= m_effectQuake.TimeLength)
            {
                m_tickTmCount = m_tickTmCount - m_effectQuake.TimeLength;
                m_EyeCamPost += m_effectQuake.StepCount;
                //return false;
            }
            if (m_EyeCamPost >= m_effectQuake.LoadCount)
            {
                m_EyeCamPost = m_effectQuake.LoadCount - 1;
                return true;
            }
            return false;
        }

        private bool UpdateDecrease(ref CameraUpdateInfo iRUpdateInfo)
        {
            iRUpdateInfo.Eye = GetRespectiveBlending(m_EyeCamPost);

            if ((m_tickTmCount) >= m_effectQuake.TimeLength)
            {
                m_tickTmCount = m_tickTmCount - m_effectQuake.TimeLength;
                m_EyeCamPost -= m_effectQuake.StepCount;
                //return false;
            }
            if (m_EyeCamPost <= 0)
            {
                m_EyeCamPost = 0;
                return true;
            }
            return false;
        }

        private bool UpdateFullDuplex(ref CameraUpdateInfo iRUpdateInfo)
        {
            if (m_CamEyeSwap)
            {
                if (UpdateIncrease(ref iRUpdateInfo))
                {
                    m_CamEyeSwap = false;
                }
            }
            else
            {
                if (UpdateDecrease(ref iRUpdateInfo))
                {
                    return true;
                }
            }
            return false;
        }

        private void SetRespectiveBlending()
        {
            for (int nIndex = 0; nIndex != m_effectQuake.LoadCount; ++nIndex)
            {
                float b0 = 0.0f;
                float b1 = 0.0f;
                float b2 = 0.0f;
                float b3 = 0.0f;

                BlendingWidth(nIndex, out b0, out b1, out b2, out b3);

                float x = b0 * m_lstPoints[0].x + b1 * m_lstPoints[1].x + b2 * m_lstPoints[2].x + b3 * m_lstPoints[3].x;

                float y = b0 * m_lstPoints[0].y + b1 * m_lstPoints[1].y + b2 * m_lstPoints[2].y + b3 * m_lstPoints[3].y;

                float z = b0 * m_lstPoints[0].z + b1 * m_lstPoints[1].z + b2 * m_lstPoints[2].z + b3 * m_lstPoints[3].z;

                m_lstCamera.Add(new Vector3(x, y, z));
            }
        }

        private void BlendingWidth(int nIndex, out float b0, out float b1, out float b2, out float b3)
        {
            float t = (float)nIndex / (m_effectQuake.LoadCount - 1);

            float it = 1.0f - t;

            b0 = t * t * t;
            b1 = t * t * it * m_effectQuake.BlendWidth;
            b2 = t * it * it * m_effectQuake.BlendWidth;
            b3 = it * it * it;
        }

        private Vector3 GetRespectiveBlending(int nIndex)
        {
            float b0 = 0.0f;
            float b1 = 0.0f;
            float b2 = 0.0f;
            float b3 = 0.0f;

            BlendingWidth(nIndex, out b0, out b1, out b2, out b3);

            float x = b0 * m_lstPoints[0].x + b1 * m_lstPoints[1].x + b2 * m_lstPoints[2].x + b3 * m_lstPoints[3].x;

            float y = b0 * m_lstPoints[0].y + b1 * m_lstPoints[1].y + b2 * m_lstPoints[2].y + b3 * m_lstPoints[3].y;

            float z = b0 * m_lstPoints[0].z + b1 * m_lstPoints[1].z + b2 * m_lstPoints[2].z + b3 * m_lstPoints[3].z;

            return new Vector3(x, y, z);
        }
    }
}