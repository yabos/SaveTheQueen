using System;
using System.Collections;
using System.Collections.Generic;
using Aniz.Cam.State;
using Aniz.Graph;
using Aniz.Cam.Info;
using Aniz.NodeGraph.Level.Group;
using Lib.Event;
using Lib.Pattern;
using UnityEngine;

namespace Aniz.Cam
{
    public class CameraManager : GlobalManagerBase<CameraSetting>
    {
        private CameraImpl m_cameraImpl = null;
        private bool m_initLook = false;
        private bool m_initialized = false;
        private CameraQuakeUnitManager m_cameraQuakeUnitManager;

        #region Events
        public override void OnAppStart(ManagerSettingBase managerSetting)
        {
            m_name = typeof(CameraManager).ToString();

            if (string.IsNullOrEmpty(m_name))
            {
                throw new System.Exception("manager name is empty");
            }

            m_setting = managerSetting as CameraSetting;

            if (null == m_setting)
            {
                throw new System.Exception("manager setting is null");
            }

            m_cameraQuakeUnitManager = new CameraQuakeUnitManager();
        }

        public override void OnAppEnd()
        {
            if (m_setting != null)
            {
                GameObjectFactory.DestroyComponent(m_setting);
                m_setting = null;
            }
        }

        public override void OnAppFocus(bool focused)
        {

        }

        public override void OnAppPause(bool paused)
        {

        }

        public override void OnPageEnter(string pageName)
        {
        }

        public override IEnumerator OnPageExit()
        {
            yield return new WaitForEndOfFrame();
        }

        #endregion Events


        #region IGraphUpdatable

        public override void BhvOnEnter()
        {
            if (m_cameraImpl != null)
            {
                m_cameraImpl.BhvOnEnter();
            }

            if (m_cameraQuakeUnitManager != null)
            {
                m_cameraQuakeUnitManager.Initialize();
            }
        }

        public override void BhvOnLeave()
        {
            if (m_cameraImpl != null)
            {
                m_cameraImpl.BhvOnLeave();
            }

            if (m_cameraQuakeUnitManager != null)
            {
                m_cameraQuakeUnitManager.Terminate();
            }
        }

        public override void BhvFixedUpdate(float dt)
        {
            m_cameraQuakeUnitManager.OnUpdate(dt);
            if (m_initialized)
            {
                m_cameraImpl.BhvFixedUpdate(dt);
            }
        }

        public override void BhvLateFixedUpdate(float dt)
        {
            if (m_initialized)
            {
                m_cameraImpl.BhvLateFixedUpdate(dt);
            }
        }

        public override void BhvUpdate(float dt)
        {
            if (m_initialized)
            {
                m_cameraImpl.BhvUpdate(dt);
            }
        }

        public override void BhvLateUpdate(float dt)
        {
            if (m_initialized)
            {
                m_cameraImpl.BhvLateUpdate(dt);
            }
        }

        public override bool OnMessage(IMessage message)
        {
            return false;
        }

        #endregion IGraphUpdatable

        public Camera MainCamera
        {
            get
            {
                if (m_cameraImpl != null)
                {
                    return m_cameraImpl.UCamera;
                }
                return null;

            }
        }

        public CameraImpl Impl
        {
            get { return m_cameraImpl; }
        }

        public void InitLook()
        {
            if (m_initLook)
                return;

            GameObject gameObject = GameObject.Find("CameraImpl");
            if (gameObject != null)
            {
                m_cameraImpl = gameObject.GetComponent<CameraImpl>();
                CameraSetting cameraSetting = gameObject.transform.FindChildComponent<CameraSetting>("CameraOption");
                if (cameraSetting != null)
                {
                    m_cameraImpl.SetCameraViewStock(cameraSetting);
                }

                m_initLook = true;
            }
        }

        public void InitCameraStock(CameraSetting option)
        {
            if (option != null)
            {
                m_cameraImpl.SetCameraViewStock(option);
            }
            m_initialized = true;
        }

        public void RegistCameraImpl(CameraImpl cameraImpl)
        {
            m_cameraImpl = cameraImpl;
            if (m_cameraImpl == null)
            {
                m_initialized = false;
            }
            else
            {
                if (m_setting != null)
                {
                    InitCameraStock(m_setting);
                }
            }
        }

        public void SetTargetActor(ActorRoot actorRoot)
        {
            if (actorRoot == null)
                return;

            m_cameraImpl.SetTarget(actorRoot);
        }

        public void DestoryTarget()
        {
            if (m_cameraImpl != null)
                m_cameraImpl.SetTarget(null);
        }

        public void SetMapSize(int x, int y)
        {

        }

        //public void OnHandleEvent(IGameNormalEventSender sender, E_GameNormalEvent e, params object[] args)
        //{
        //    if (E_GameNormalEvent.Trigger_Camera_Stock == e)
        //    {
        //        CameraStock stock = (CameraStock)args[0];
        //        m_cameraImpl.UpdateStock(stock);
        //    }
        //    else if (E_GameNormalEvent.Trigger_Camera_Zoom == e)
        //    {
        //        float Distance = (float)args[0];
        //        float Time = (float)args[1];
        //        m_cameraImpl.StartCoroutine(ZoomBlendStart(Distance, Time));
        //    }
        //    else if (E_GameNormalEvent.StartDarkChange == e)
        //        StartDarkChange();
        //    else if (E_GameNormalEvent.EndDarkChange == e)
        //        EndDarkChange();
        //}


        IEnumerator ZoomBlendStart(float distance, float time)
        {
            CameraSetting setting = m_cameraImpl.Setting;

            float deltatime = 0.0f;
            float olddis = setting.CameraStock.Distance;
            bool outcheck = distance > 0.0f ? true : false;
            float newdis = Mathf.Abs(distance);
            var ease = Interpolate.Ease(Interpolate.EaseType.Linear);
            while (deltatime <= time)
            {
                deltatime += TimeManager.deltaTime;
                float deltadis = ease(0.0f, newdis, deltatime, time);
                if (outcheck)
                {
                    setting.CameraStock.Distance = olddis + deltadis;
                }
                else
                {
                    setting.CameraStock.Distance = olddis - deltadis;
                }
                m_cameraImpl.UpdateStock(setting.CameraStock);
                yield return new WaitForEndOfFrame();
            }
            setting.CameraStock.Distance = distance;
        }


        public Vector3 GetMoveDirection(uint moveDirection, Vector3 dir)
        {
            return m_cameraImpl.GetMoveDirection(moveDirection, dir);
        }

#if UNITY_EDITOR
        public void Slide(Vector3 f3Offset)
        {
            if (m_cameraImpl == null)
                return;

            Vector3 f3Prev = m_cameraImpl.Target.TargetPos;
            Vector2 f2New = new Vector2(f3Prev.x + f3Offset.x, f3Prev.z + f3Offset.z);
            float fHeight = f3Prev.y;

            m_cameraImpl.Target.SetTarget(new Vector3(f2New.x, fHeight, f2New.y));
        }

        public void Rotate(Vector2 f2Offset)
        {
            if (m_cameraImpl == null)
                return;

            if (m_cameraImpl.IsHorizonEqual() || m_cameraImpl.IsVerticalEqual()) return;

            CameraFreeState freeState = m_cameraImpl.Strategy.CurState as CameraFreeState;
            if (freeState != null)
            {
                m_cameraImpl.Rotate(f2Offset);
            }
            else
            {
                m_cameraImpl.EditorHorizon = m_cameraImpl.EditorHorizon + f2Offset.x;
                m_cameraImpl.EditorVertical = m_cameraImpl.EditorVertical + f2Offset.y;
                m_cameraImpl.EditorUpdateStock();
            }
        }

        public void Zoom(float fDelta)
        {
            if (m_cameraImpl != null)
            {
                CameraFreeState freeState = m_cameraImpl.Strategy.CurState as CameraFreeState;
                if (freeState != null)
                {
                    m_cameraImpl.Zoom(fDelta);
                }
                else
                {
                    m_cameraImpl.EditorDistance = m_cameraImpl.EditorDistance * Mathf.Pow(2.0f, fDelta);
                    m_cameraImpl.EditorUpdateStock();
                }
            }
        }

        public void Move(Vector3 f3Offset)
        {
            if (m_cameraImpl == null)
                return;

            Vector3 f3Prev = m_cameraImpl.Target.TargetPos;
            Vector2 f2New = new Vector2(f3Prev.x + f3Offset.x, f3Prev.z);
            float fHeight = f3Prev.y + f3Offset.z;

            m_cameraImpl.Target.SetTarget(new Vector3(f2New.x, fHeight, f2New.y));
        }
#endif

    }
}