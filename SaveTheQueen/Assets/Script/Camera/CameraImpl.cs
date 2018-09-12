
using UnityEngine;
using System.Collections.Generic;
using Aniz.Cam.Info;
using Aniz.Cam.Player;
using Aniz.NodeGraph.Level.Group;
using Aniz.Graph;
using Lib.Event;

namespace Aniz.Cam
{

    [RequireComponent(typeof(Camera))]
    public class CameraImpl : GraphMonoNode
    {
        [SerializeField]
        public bool IsMain = false;

        private Camera m_camera;

        private Vector3 _targetsMidPoint;

        private Vector3 m_direction;
        private Vector3 m_rightDirection;

        private CameraTarget m_target;

        private CameraStrategy m_strategy;
        private CameraSetting m_cameraSetting;

        private CameraStock m_cameraStock = new CameraStock();
        private CameraUpdateInfo m_cameraUpdateInfo = new CameraUpdateInfo();
        private LimitStock m_limitStock;

        private CamAtQuake m_camAtQuake;
        private CamEyeQuake m_camEyeQuake;

        private List<ICameraPlayer> m_lstCameraPlayers = new List<ICameraPlayer>();

        private float m_intCheck = 0.0f;

        private bool m_cameraUpdate = true;
        private float m_cameraDelayUpdateTimer = 1.0f;

        public override eNodeType NodeType
        {
            get { return eNodeType.Camera; }
        }

        public Vector3 TargetsMidPoint
        {
            get { return _targetsMidPoint; }
        }

        public CameraTarget Target
        {
            get { return m_target; }
        }

        public Camera UCamera
        {
            get { return m_camera; }
        }

        public CameraSetting Setting
        {
            get { return m_cameraSetting; }
        }

        public CamAtQuake AtQuake
        {
            get { return m_camAtQuake; }
        }

        public CamEyeQuake EyeQuake
        {
            get { return m_camEyeQuake; }
        }

        public LimitStock Limit
        {
            get { return m_limitStock; }
        }

        public CameraUpdateInfo UpdateInfo
        {
            get { return m_cameraUpdateInfo; }
        }

        public CameraStock Stock
        {
            get { return m_cameraStock; }
        }

        public CameraStrategy Strategy
        {
            get { return m_strategy; }
        }

        protected override void BhvOnAwake()
        {
            m_camera = GetComponent<Camera>();
            m_strategy = new CameraStrategy();
            m_target = new CameraTarget(this);

            m_camAtQuake = new CamAtQuake();
            m_camEyeQuake = new CamEyeQuake();

            m_lstCameraPlayers.Add(m_camAtQuake);
            m_lstCameraPlayers.Add(m_camEyeQuake);


            if (IsMain)
            {
                Global.CameraMgr.RegistCameraImpl(this);

                Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
                for (int i = 0; i < cameras.Length; i++)
                {
                    if (cameras[i] == UCamera)
                        continue;

                    if (cameras[i].gameObject.layer == LayerMask.NameToLayer("UI") ||
                        cameras[i].gameObject.layer == LayerMask.NameToLayer("GameUI") ||
                        cameras[i].gameObject.layer == LayerMask.NameToLayer("MsgBox"))
                        continue;

                    cameras[i].gameObject.SetActive(false);
                }

            }
        }

        protected override void BhvOnStart() { }

        protected override void BhvOnDestroy()
        {

        }


        //
        // IGraphUpdatable
        //

        #region "IGraphUpdatable"

        public override void BhvOnEnter()
        {
        }

        public override void BhvOnLeave()
        {
            m_target.SetTarget(null);
            Global.CameraMgr.RegistCameraImpl(null);
        }

        public override void BhvFixedUpdate(float dt)
        {
        }

        public override void BhvLateFixedUpdate(float dt)
        {

        }

        public override void BhvUpdate(float dt)
        {
        }

        public override void BhvLateUpdate(float dt)
        {
            if (m_cameraUpdate == false)
                return;

            m_cameraUpdateInfo.Distance = m_cameraStock.Distance;
            m_cameraUpdateInfo.Horizon = m_cameraStock.Horizon * Mathf.Deg2Rad;
            m_cameraUpdateInfo.Vertical = m_cameraStock.Vertical * Mathf.Deg2Rad;
            m_cameraUpdateInfo.Height = m_cameraStock.HeightOffset;

            m_strategy.Run(m_cameraStock, m_target.TargetPos, ref m_cameraUpdateInfo, dt);

            Calculate(m_cameraUpdateInfo);

            float fDistance = Mathf.Max(m_strategy.GetAdjustDistance(m_cameraUpdateInfo), m_limitStock.MinDistance);
            if (Mathf.Abs(fDistance - m_cameraUpdateInfo.Distance) > LEMath.s_fEpsilon)
            {
                Calculate(m_cameraUpdateInfo);
            }

            ApplyEffect(ref m_cameraUpdateInfo, dt);

            m_camera.transform.position = m_cameraUpdateInfo.Eye; // + m_cameraInfo.getOffset();
            m_camera.transform.LookAt(m_cameraUpdateInfo.At);
        }

        public override bool OnMessage(IMessage message)
        {
            return true;
        }

        #endregion // "IGraphUpdatable"

        private void Calculate(CameraUpdateInfo updateInfo)
        {

            updateInfo.At = updateInfo.Target + new Vector3(0.0f, updateInfo.Height, 0.0f);

            Vector3 lookatDir = LEMath.AngleToDirection(updateInfo.Vertical, updateInfo.Horizon);
            updateInfo.LookatDir = lookatDir;
            updateInfo.Eye = m_strategy.GetEyePos(ref updateInfo);

            //Vector2 v2SideDir = (LEMath.RadianToDir(updateInfo.Horizon + LEMath.s_fHalfPI));

            m_direction = new Vector3(lookatDir.x, 0.0f, lookatDir.z);
            //m_rightDirection = -new Vector3(v2SideDir.x, 0.0f, v2SideDir.y);
            m_rightDirection = new Vector3(-lookatDir.z, 0.0f, lookatDir.x);
            m_direction.Normalize();
            m_rightDirection.Normalize();

        }

        public void Rotate(Vector2 f2Offset)
        {
            m_cameraStock.Horizon = m_cameraStock.Horizon + f2Offset.x;
            m_cameraStock.Vertical = m_cameraStock.Vertical + f2Offset.y;
        }

        public void Zoom(float fDelta)
        {
            m_cameraStock.Distance = m_cameraStock.Distance * Mathf.Pow(2.0f, fDelta);
        }

        public void SetCameraViewStock(CameraSetting option)
        {
            m_cameraUpdate = true;

            m_cameraSetting = option;

            if (m_cameraSetting != null)
            {
                UpdateLimit(m_cameraSetting.LimitStock);
                UpdateStock(m_cameraSetting.CameraStock);
                //m_camera.transform.position = option.transform.position;
                //m_camera.transform.rotation = option.transform.rotation;
                m_camera.fieldOfView = m_cameraStock.Fov;

#if UNITY_EDITOR
                InitEditorStock(m_cameraSetting.CameraStock);
#endif
            }

        }


        public void UpdateStock(CameraStock stock)
        {
            if (stock != null)
            {
                if (m_limitStock != null && m_cameraStock != null)
                {
                    m_cameraStock.Distance = Mathf.Max(m_limitStock.MinDistance, Mathf.Min(stock.Distance, m_limitStock.MaxDistance));
                    m_cameraStock.Vertical = Mathf.Max(m_limitStock.VerMinRadian, Mathf.Min(stock.Vertical, m_limitStock.VerMaxRadian));
                    m_cameraStock.Horizon = stock.Horizon;

                    m_cameraStock.Fov = stock.Fov;
                    m_camera.fieldOfView = m_cameraStock.Fov;
                    m_cameraStock.HeightOffset = stock.HeightOffset;
                }

                if (IsVerticalEqual())
                    m_cameraStock.Vertical = stock.Vertical;
            }

        }

        public void UpdateLimit(LimitStock stock)
        {
            m_limitStock = stock;
        }

        public void SetTarget(ActorRoot actorRoot)
        {
            m_target.SetTarget(actorRoot);

            Vector3 targetCenter = (m_target.TargetPos) + new Vector3(0, m_cameraUpdateInfo.Height);
            m_camera.transform.position = targetCenter + m_cameraUpdateInfo.getOffset();
            m_camera.transform.LookAt(targetCenter);
        }


        public ICameraPlayer GetPlayer(E_CameraPlayer eCameraPlayer)
        {
            for (int i = 0; i < m_lstCameraPlayers.Count; i++)
            {
                ICameraPlayer player = m_lstCameraPlayers[i];
                if (player.CameraPlayer == eCameraPlayer)
                {
                    return player;
                }
            }
            return null;
        }

        private void ApplyEffect(ref CameraUpdateInfo rUpdateInfo, float deltaTime)
        {
            if (m_lstCameraPlayers.Count <= 0)
                return;

            for (int i = 0; i < m_lstCameraPlayers.Count; i++)
            {
                ICameraPlayer player = m_lstCameraPlayers[i];
                player.Update(ref rUpdateInfo, deltaTime);
            }
        }

        public bool IsVerticalEqual()
        {
            if (m_limitStock != null)
            {
                if (m_limitStock.VerMaxRadian + m_limitStock.VerMinRadian > 0.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsHorizonEqual()
        {
            if (m_limitStock.HorMaxRadian + m_limitStock.HorMinRadian > 0.0f)
            {
                return true;
            }
            return false;
        }

        public Vector3 GetMoveDirection(uint moveDirection, Vector3 dir)
        {
            Vector3 targetDirection = dir;
            //dir = Vector3.zero;
            //if ((moveDirection & (uint)eMoveDirection.Up) > 0) dir -= m_direction;
            //if ((moveDirection & (uint)eMoveDirection.Down) > 0) dir += m_direction;
            //if ((moveDirection & (uint)eMoveDirection.Right) > 0) dir += m_rightDirection;
            //if ((moveDirection & (uint)eMoveDirection.Left) > 0) dir -= m_rightDirection;

            targetDirection = dir.x * m_rightDirection + dir.z * -m_direction;

            targetDirection.Normalize();
            return targetDirection;
        }

        #region Editor

#if UNITY_EDITOR
        public float EditorDistance = 1.0f;
        public float EditorVertical;
        public float EditorHorizon;
        public float EditorFov;
        public float EditorHeightOffset;

        public bool DrawGizmos = false;

        private CameraStock m_editStock = new CameraStock();

        private void InitEditorStock(CameraStock stock)
        {
            m_editStock.Copy(stock);
            EditorDistance = m_editStock.Distance;
            EditorVertical = m_editStock.Vertical;
            EditorHorizon = m_editStock.Horizon;
            EditorFov = m_editStock.Fov;
            EditorHeightOffset = m_editStock.HeightOffset;
        }

        public void EditorUpdateStock()
        {
            if (Application.isPlaying == false)
                return;

            bool update = false;
            if (m_cameraStock.Distance != EditorDistance)
            {
                update = true;
            }
            if (m_cameraStock.Vertical != EditorVertical)
            {
                update = true;
            }
            if (m_cameraStock.HeightOffset != EditorHeightOffset)
            {
                update = true;
            }
            if (m_cameraStock.Horizon != EditorHorizon)
            {
                update = true;
            }
            if (m_cameraStock.Fov != EditorFov)
            {
                update = true;
            }

            if (update)
            {
                m_editStock.Distance = EditorDistance;
                m_editStock.Vertical = EditorVertical;
                m_editStock.Horizon = EditorHorizon;
                m_editStock.Fov = EditorFov;
                m_editStock.HeightOffset = EditorHeightOffset;
                UpdateStock(m_editStock);
            }
        }


#endif

        #endregion
    }

}