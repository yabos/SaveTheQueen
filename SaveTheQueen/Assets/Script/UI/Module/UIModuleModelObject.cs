using UnityEngine;
using System.Collections;

namespace Lib.uGui
{

    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent, ExecuteInEditMode]
    [AddComponentMenu("UI/ModelObject")]
    public class UIModuleModelObject : MonoBehaviour
    {
        [Header("Target"), SerializeField]
        private Transform m_objectPrefab = null;
        public Transform ObjectPrefab
        {
            get { return m_objectPrefab; }
            set
            {
                m_objectPrefab = value;
                HardUpdateDisplay();
            }
        }

        [SerializeField]
        private Vector3 m_targetRotation = Vector3.zero;
        public Vector3 TargetRotation
        {
            get { return m_targetRotation; }
            set
            {
                m_targetRotation = UIModuleModelUtils.NormalizeRotation(value);

                UpdateDisplay();
            }
        }

        [SerializeField, Range(-1, 1)]
        private float m_targetOffsetX = 0f;
        [SerializeField, Range(-1, 1)]
        private float m_targetOffsetY = 0f;

        [SerializeField]
        public Vector2 TargetOffset
        {
            get { return new Vector2(m_targetOffsetX, m_targetOffsetY); }
            set
            {
                m_targetOffsetX = value.x;
                m_targetOffsetY = value.y;
                UpdateDisplay();
            }
        }

        [Header("Camera Settings"), SerializeField, Range(20, 100)]
        private float m_cameraFOV = 35f;
        public float CameraFOV
        {
            get { return m_cameraFOV; }
            set
            {
                m_cameraFOV = value;
                UpdateDisplay();
            }
        }

        [SerializeField, Range(-10, -1)]
        private float m_cameraDistance = -3.5f;
        public float CameraDistance
        {
            get { return m_cameraDistance; }
            set
            {
                m_cameraDistance = value;
                UpdateDisplay();
            }
        }

        public Vector2 TextureSize
        {
            get
            {
                if (Target != null)
                {
                    Vector2 size = new Vector2(Mathf.Abs(Mathf.Floor(rectTransform.rect.width)), Mathf.Abs(Mathf.Floor(rectTransform.rect.height)));

                    if (size.x == 0 || size.y == 0) return new Vector2(256, 256);

                    return size;
                }

                return Vector2.one;
            }
        }

        [SerializeField]
        private Color m_backgroundColor = Color.clear;
        public Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set
            {
                m_backgroundColor = value;

                UpdateDisplay();
            }
        }

        [Header("Performance Setting"), SerializeField]
        public bool LimitFrameRate = false;

        [SerializeField]
        public float FrameRateLimit = 30f;

        public bool RenderConstantly = false;

        private float timeBetweenFrames
        {
            get { return 1f / FrameRateLimit; }
        }

        private float timeSinceLastRender = 0f;


        [Header("Lighting Setting"), SerializeField]
        private bool m_enableCameraLight = false;
        public bool EnableCameraLight
        {
            get { return m_enableCameraLight; }
            set
            {
                m_enableCameraLight = value;

                UpdateDisplay();
            }
        }

        [SerializeField]
        private Color m_lightColor = Color.white;
        public Color LightColor
        {
            get { return m_lightColor; }
            set
            {
                m_lightColor = value;
                UpdateDisplay();
            }
        }

        [SerializeField, Range(0, 8)]
        private float m_lightIntensity = 1f;
        public float LightIntensity
        {
            get { return m_lightIntensity; }
            set
            {
                m_lightIntensity = value;
                UpdateDisplay();
            }
        }

        private bool m_started = false;
        private bool m_hardUpdateQueued = false;
        private bool m_renderQueued = false;
        private Bounds m_targetBounds;

        private bool m_enabled = false;

        public void HardUpdateDisplay()
        {
            if (m_targetCamera != null) m_targetCamera.targetTexture = null;
            if (m_texture2D != null) DestroyImmediate(m_texture2D);
            if (m_renderTexture != null) DestroyImmediate(m_renderTexture);

            Cleanup();

            UpdateDisplay();
        }

        void Start()
        {

            UIModuleModelTimer.AtEndOfFrame(() =>
            {
                m_started = true;
                OnEnable();
            }, this);

            UIModuleModelTimer.DelayedCall(0.01f, () =>
            {
                Cleanup();
                UpdateDisplay();
            }, this);
        }

        public void UpdateDisplay(bool instantRender = false)
        {
            Prepare();

            UpdateTargetPositioningAndScale();
            UpdateTargetCameraPositioningEtc();

            Render(instantRender);
        }

        void OnEnable()
        {
            if (!m_started) return;

            m_enabled = true;

            if (ObjectLayer != -1)
            {
                ClearObjectLayerFromCameras();
                ClearObjectLayerFromLights();
            }

            // This is now called by Start()
            //UIModuleModelTimer.AtEndOfFrame(() => UpdateDisplay(true), this);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playmodeStateChanged += InEditorCleanup;
#endif
        }

        private void ClearObjectLayerFromCameras()
        {
            var otherCameras = GameObject.FindObjectsOfType<Camera>();
            foreach (var c in otherCameras)
            {
                if (c.GetComponent<UIModuleModelCamera>() != null) continue;

                c.cullingMask &= ~(1 << ObjectLayer);
            }
        }

        private void ClearObjectLayerFromLights()
        {
            var otherLights = GameObject.FindObjectsOfType<Light>();
            foreach (var l in otherLights)
            {
                if (l.type == LightType.Directional) continue;
                if (l.GetComponent<UIModuleModelCamera>() != null) continue;

                l.cullingMask &= ~(1 << ObjectLayer);
            }
        }

        void OnDisable()
        {
            m_enabled = false;

            Cleanup();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playmodeStateChanged -= InEditorCleanup;
#endif
        }

#if UNITY_EDITOR
        void InEditorCleanup()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
            {
                Cleanup();
            }
        }
#endif

        void OnDestroy()
        {
            UIModuleModelUtils.UnRegisterTargetContainer(this);
        }

        void Prepare()
        {
            if (ImageComponent.sprite != Sprite) ImageComponent.sprite = Sprite;

            SetupTargetCamera();
        }

        public void Cleanup()
        {
            m_texture2D = null;
            m_sprite = null;
            m_renderTexture = null;
            m_targetBounds = default(Bounds);

            if (m_container != null)
            {
                UIModuleModelUtils.UnRegisterTargetContainer(this);

                DestroyImmediate(m_container.gameObject);
                m_container = null;
            }
        }

        public Transform GetTargetInstance()
        {
            return Target;
        }

        void Render(bool instant = false)
        {
            if (Application.isPlaying && !instant)
            {
                m_renderQueued = true;
                return;
            }

            if (TargetCamera == null) return;

            var rect = new Rect(0, 0, (int)TextureSize.x, (int)TextureSize.y);

            RenderTexture.active = this.RenderTexture;

            GL.Clear(false, true, BackgroundColor);
            TargetCamera.Render();

            this.Texture2D.ReadPixels(rect, 0, 0);
            this.Texture2D.Apply();

            RenderTexture.active = null;
            m_renderQueued = false;
        }

        void OnRectTransformDimensionsChange()
        {
            if (Application.isPlaying)
            {
                m_hardUpdateQueued = true;
            }
            else
            {
                UIModuleModelTimer.AtEndOfFrame(() => { if (m_enabled) HardUpdateDisplay(); }, this);
            }
        }

        void Update()
        {
            if (!Application.isPlaying) return;
            if (!m_started) return;

            timeSinceLastRender += Time.unscaledDeltaTime;

            if (m_hardUpdateQueued)
            {
                m_hardUpdateQueued = false;

                HardUpdateDisplay();
                return;
            }

            if (LimitFrameRate)
            {
                if (timeSinceLastRender < timeBetweenFrames) return;
            }

            if (m_renderQueued || RenderConstantly)
            {
                Render(true);
                timeSinceLastRender = 0f;
            }
        }

        #region Internal Components
        private RectTransform _rectTransform;
        protected RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = this.GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        [SerializeField, HideInInspector]
        private UIModuleModelImage m_imageComponent;
        public UIModuleModelImage ImageComponent
        {
            get
            {
                bool setProperties = false;
                if (m_imageComponent == null)
                {
                    m_imageComponent = this.GetComponent<UIModuleModelImage>();
                    setProperties = true;
                }

                if (m_imageComponent == null)
                {
                    m_imageComponent = this.gameObject.AddComponent<UIModuleModelImage>();
                    setProperties = true;
                }

                if (setProperties)
                {
                    m_imageComponent.type = UnityEngine.UI.Image.Type.Simple;
                    m_imageComponent.preserveAspect = true;
                }

                return m_imageComponent;
            }
        }

        private Texture2D m_texture2D;
        protected Texture2D Texture2D
        {
            get
            {
                if (m_texture2D == null) m_texture2D = new Texture2D((int)TextureSize.x, (int)TextureSize.y, TextureFormat.ARGB32, false, false);

                return m_texture2D;
            }
        }

        private Sprite m_sprite;
        protected Sprite Sprite
        {
            get
            {
                if (m_sprite == null)
                {
                    m_sprite = Sprite.Create(Texture2D, new Rect(0, 0, (int)TextureSize.x, (int)TextureSize.y), new Vector2(0.5f, 0.5f));
                }

                return m_sprite;
            }
        }

        private RenderTexture m_renderTexture;
        protected RenderTexture RenderTexture
        {
            get
            {
                if (m_renderTexture == null)
                {
                    m_renderTexture = new RenderTexture((int)TextureSize.x, (int)TextureSize.y, 16, RenderTextureFormat.ARGB32);
                    if (QualitySettings.antiAliasing > 0) m_renderTexture.antiAliasing = QualitySettings.antiAliasing;
                }

                return m_renderTexture;
            }
        }

        private static Transform m_parentContainer;
        private static Transform ParentContainer
        {
            get
            {
                if (m_parentContainer == null)
                {
                    var go = GameObject.Find("UIModuleModelObject Scenes");

                    if (go != null)
                    {
                        m_parentContainer = go.transform;
                    }
                    else
                    {
                        m_parentContainer = new GameObject().transform;
                        m_parentContainer.name = "UIModuleModelObject Scenes";
                    }
                }

                return m_parentContainer;
            }
        }

        private Transform m_container;
        protected Transform Container
        {
            get
            {
                if (m_container == null)
                {
                    if (ObjectPrefab == null) return null;

                    m_container = new GameObject().transform;
                    m_container.SetParent(ParentContainer);
                    m_container.position = Vector3.zero;
                    m_container.localScale = Vector3.one;
                    m_container.localRotation = Quaternion.identity;
                    m_container.gameObject.layer = ObjectLayer;
                    m_container.name = "_UIModuleModelObject_" + ObjectPrefab.name;

                    m_container.localPosition = UIModuleModelUtils.GetTargetContainerPosition(this);
                    UIModuleModelUtils.RegisterTargetContainerPosition(this, m_container.localPosition);
                }

                return m_container;
            }
        }

        private Transform m_targetContainer;
        protected Transform TargetContainer
        {
            get
            {
                if (m_targetContainer == null)
                {
                    if (Container == null) return null;

                    m_targetContainer = new GameObject().transform;
                    m_targetContainer.SetParent(Container);

                    m_targetContainer.localPosition = Vector3.zero;
                    m_targetContainer.localScale = Vector3.one;
                    m_targetContainer.localRotation = Quaternion.identity;
                    m_targetContainer.name = "Target Container";
                    m_targetContainer.gameObject.layer = ObjectLayer;
                }

                return m_targetContainer;
            }
        }

        private Transform m_target;
        protected Transform Target
        {
            get
            {
                if (m_target == null && m_started) SetupTarget();

                return m_target;
            }
        }

        private void SetupTarget()
        {
            if (m_target == null)
            {
                if (ObjectPrefab == null)
                {
                    if (Application.isPlaying) Debug.LogWarning("No prefab set.");
                    return;
                }

                m_target = GameObject.Instantiate(ObjectPrefab);
            }

            UpdateTargetPositioningAndScale();
        }

        private void UpdateTargetPositioningAndScale()
        {
            if (m_target == null) return;
            var renderer = m_target.GetComponentInChildren<Renderer>();

            m_target.name = "Target";

            bool initial = m_targetBounds == default(Bounds);

            if (initial)
            {
                var prefabIsModel = false;

                m_target.transform.SetParent(TargetContainer);

                if (prefabIsModel)
                {
                    m_target.transform.localPosition = Vector3.zero;
                    m_target.transform.localScale = Vector3.one;
                    m_target.localRotation = Quaternion.identity;
                }
                else
                {
                    m_target.transform.localPosition = ObjectPrefab.localPosition;
                    m_target.transform.localScale = ObjectPrefab.localScale;
                    m_target.transform.localRotation = ObjectPrefab.localRotation;
                }

                SetLayerRecursively(m_target.transform, ObjectLayer);
            }


            if (renderer != null)
            {
                if (initial)
                {
                    var storedPosition = m_target.transform.localPosition;
                    m_target.transform.position = Vector3.zero;
                    m_targetBounds = new Bounds(renderer.bounds.center, renderer.bounds.size);
                    m_target.transform.localPosition = storedPosition;
                    m_target.transform.localPosition -= m_targetBounds.center;
                }

                var frustumHeight = 2 * 2 * System.Math.Tan(TargetCamera.fieldOfView * 0.5 * Mathf.Deg2Rad);
                var frustumWidth = frustumHeight * TargetCamera.aspect;

                double scale = 1f / System.Math.Max(m_targetBounds.size.x, m_targetBounds.size.y);

                var wideObject = m_targetBounds.size.x > m_targetBounds.size.y;
                var tallObject = m_targetBounds.size.y > m_targetBounds.size.x;

                if (wideObject)
                {
                    scale = frustumWidth / m_targetBounds.size.x;
                }
                else if (tallObject)
                {
                    scale = frustumHeight / m_targetBounds.size.y;
                }

                var newHeight = m_targetBounds.size.y * scale;
                var newWidth = m_targetBounds.size.x * scale;

                var newWidthIsHigher = newWidth > frustumWidth;
                var newHeightIsHigher = newHeight > frustumHeight;

                if (newWidthIsHigher)
                {
                    scale = frustumWidth / m_targetBounds.size.x;
                }

                if (newHeightIsHigher)
                {
                    scale = frustumHeight / m_targetBounds.size.y;
                }

                TargetContainer.transform.localScale = Vector3.one * (float)scale;
            }

            TargetContainer.transform.localPosition = new Vector3(TargetOffset.x, TargetOffset.y, 0);
            TargetContainer.transform.localEulerAngles = TargetRotation;
        }

        private void SetLayerRecursively(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;

            foreach (Transform t in transform)
            {
                SetLayerRecursively(t, layer);
            }
        }

        private Camera m_targetCamera;
        protected Camera TargetCamera
        {
            get
            {
                if (m_targetCamera == null) SetupTargetCamera();

                return m_targetCamera;
            }
        }

        private void SetupTargetCamera()
        {
            if (m_targetCamera == null)
            {
                if (ObjectPrefab == null) return;

                var cameraGO = new GameObject();
                cameraGO.transform.SetParent(Container);
                m_targetCamera = cameraGO.AddComponent<Camera>();
                m_targetCamera.enabled = false;

                cameraGO.AddComponent<UIModuleModelCamera>();
            }

            UpdateTargetCameraPositioningEtc();
        }

        private Light m_cameraLight;
        protected Light CameraLight
        {
            get
            {
                if (m_cameraLight == null) SetupCameraLight();

                return m_cameraLight;
            }
        }

        private void SetupCameraLight()
        {
            if (TargetCamera == null) return;

            if (m_cameraLight == null) m_cameraLight = TargetCamera.gameObject.AddComponent<Light>();

            m_cameraLight.enabled = EnableCameraLight;

            if (EnableCameraLight)
            {
                m_cameraLight.gameObject.layer = ObjectLayer;
                m_cameraLight.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(ObjectLayer));
                m_cameraLight.type = LightType.Point;
                m_cameraLight.intensity = LightIntensity;
                m_cameraLight.range = 200;
                m_cameraLight.color = LightColor;
            }
        }

        private void UpdateTargetCameraPositioningEtc()
        {
            if (m_targetCamera == null) return;

            m_targetCamera.transform.localPosition = Vector3.zero + new Vector3(0, 0, CameraDistance);
            m_targetCamera.name = "Camera";

            m_targetCamera.targetTexture = this.RenderTexture;
            m_targetCamera.clearFlags = CameraClearFlags.SolidColor;
            m_targetCamera.backgroundColor = Color.clear;
            m_targetCamera.nearClipPlane = 0.1f;
            m_targetCamera.farClipPlane = 50f;

            m_targetCamera.fieldOfView = CameraFOV;
            m_targetCamera.gameObject.layer = ObjectLayer;
            m_targetCamera.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(ObjectLayer));
            m_targetCamera.backgroundColor = BackgroundColor;

            SetupCameraLight();
        }

        private int m_objectLayer = -1;
        protected int ObjectLayer
        {
            get
            {
                if (m_objectLayer == -1)
                {
                    m_objectLayer = LayerMask.NameToLayer("UIModuleModelObject");
#if UNITY_EDITOR
                    if (m_objectLayer == -1)
                    {
                        UIModuleModelUtils.ManageLayer();
                        m_objectLayer = LayerMask.NameToLayer("UIModuleModelObject");
                    }
#endif
                }

                return m_objectLayer;
            }
        }
        #endregion
    }
}