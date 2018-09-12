using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lib.Pattern;

namespace Lib.uGui
{
    public static partial class UnityTransformExtension
    {
        public static Transform GetCustomRootBone(this Transform self)
        {
            Transform[] transforms =
                ComponentFactory.GetChildComponents<Transform>(self.gameObject, IfNotExist.ReturnNull);
            for (int i = 0; i < transforms.Length; ++i)
            {
                if (ComponentFactory.GetComponent<Animator>(transforms[i].gameObject, IfNotExist.ReturnNull))
                    return transforms[i];
            }
            return null;
        }
    }

    public class UIModuleModelView : MonoBehaviour, IUIModule, IPointerClickHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        public Camera UICamera = null;
        public UIModuleModelData UIModuleModelData;

        public bool PeaceIdleState = true;
        public int PeaceIdleIndex = 0;
        public string StartAnimationState = string.Empty;

        public bool UseRolling = false;
        public bool UsePitchZooming = false;
        public bool IsAxisX = true;

        // Character Rolling
        protected Vector2 m_prevMousePosition = Vector2.zero;

        protected Vector3 m_prevTouchPosition = Vector3.zero;
        public float RollingTime = 250.0f;

        protected Quaternion m_initializeCameraQuaternion = Quaternion.identity;
        protected Vector3 m_initializeCameraPosition = Vector3.zero;

        // PitchZooming
        public float ZoomingCameraDistance = 1.0f;

        protected float m_zoomDistance = 0.0f;
        public float FocusingSpeed = 3f;
        public float PerspectiveZoomSpeed = 0.1f;

        protected Transform m_pivotHeadTransform = null;

        public Canvas Canvas = null;

        public bool IsForceLookAtDisableMode = false;
        public bool UseTouchReaction = true;

        protected bool m_isDragging = false;

        void Awake()
        {
            if (UICamera != null)
            {
                m_initializeCameraQuaternion = UICamera.transform.rotation;
                m_initializeCameraPosition = UICamera.transform.position;
            }
        }

        void Start()
        {
        }

        void Update()
        {
#if UNITY_EDITOR
            if (UsePitchZooming == true && UICamera != null)
            {
                float pinchZoom = Input.GetAxis("Mouse ScrollWheel");
                if (pinchZoom > 0 || pinchZoom < 0)
                {
                    DisableCanvas();

                    m_zoomDistance += pinchZoom * FocusingSpeed;
                    m_zoomDistance = Mathf.Clamp(m_zoomDistance, 0f, ZoomingCameraDistance);
                    UICamera.transform.position = new Vector3(UICamera.transform.position.x,
                        UICamera.transform.position.y, m_zoomDistance);
                }
            }
#endif
        }

        #region IUIModule

        public virtual void OnEnterModule()
        {
            if (UIModuleModelData != null)
            {
                UIModuleModelData.OnEnterModule();
            }
        }

        public virtual void OnExitModule()
        {
            if (UIModuleModelData != null)
            {
                UIModuleModelData.OnExitModule();
            }
        }

        public virtual void OnRefreshModule()
        {

        }

        public virtual void OnDestroyModule()
        {

        }

        #endregion IUIModule

        #region Unity EventSystems

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_isDragging == true)
            {
                return;
            }

            if (UseTouchReaction == false)
            {
                return;
            }

            if (UICamera == null)
            {
                return;
            }

            if (eventData.pointerEnter == null || eventData.pointerEnter.layer != LayerMask.NameToLayer("Background"))
            {
                return;
            }

            if (UIModuleModelData != null && UIModuleModelData.BoxColliders.Any())
            {
                List<RaycastHit> hits = new List<RaycastHit>();
                for (int i = 0; i < UIModuleModelData.BoxColliders.Count; ++i)
                {
                    RaycastHit hit;
                    if (UIModuleModelData.BoxColliders[i]
                        .Raycast(UICamera.ScreenPointToRay(eventData.pressPosition), out hit, 100))
                    {
                        hits.Add(hit);
                    }
                }

                if (hits != null && hits.Any())
                {
                    hits.Sort((c1, c2) =>
                    {
                        return c1.distance.CompareTo(c2.distance);
                    });

                    // change animation clip
                }
            }
        }

        public void DisableCanvas()
        {
            if (Canvas != null)
            {
                Canvas.enabled = false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_isDragging = true;

            m_prevMousePosition = eventData.pressPosition;
            m_prevTouchPosition = eventData.position;

            DisableCanvas();
        }

        private Transform GetPivotHead()
        {
            if (m_pivotHeadTransform != null)
            {
                return m_pivotHeadTransform;
            }

            if (UIModuleModelData.ModelObject != null)
            {
                Transform rootBone = UIModuleModelData.ModelObject.transform.GetCustomRootBone();

                if (rootBone == null)
                {
                    return null;
                }

                string headName = "bip001 head";
                Transform boneTransform = rootBone.GetComponentsInChildren<Transform>()
                    .FirstOrDefault(c => (c.name.IndexOf(headName, System.StringComparison.OrdinalIgnoreCase) >= 0));

                if (boneTransform != null)
                {
                    m_pivotHeadTransform = boneTransform;
                    return m_pivotHeadTransform;
                }
            }
            return null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1)
            {
                if (false == UsePitchZooming)
                {
                    return;
                }

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 deltatouchZeroPrevPos = new Vector2(touchZero.position.x / Screen.width,
                    touchZero.position.y / Screen.height);
                Vector2 deltatouchOnePrevPos = new Vector2(touchOne.position.x / Screen.width,
                    touchOne.position.y / Screen.height);

                Vector2 touchZeroPrevPos = deltatouchZeroPrevPos -
                                           new Vector2(touchZero.deltaPosition.x / Screen.width,
                                               touchZero.deltaPosition.y / Screen.height);
                Vector2 touchOnePrevPos = deltatouchOnePrevPos -
                                          new Vector2(touchOne.deltaPosition.x / Screen.width,
                                              touchOne.deltaPosition.y / Screen.height);

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (deltatouchZeroPrevPos - deltatouchOnePrevPos).magnitude;
                float deltaMagnitudeDiff = touchDeltaMag - (prevTouchDeltaMag);

                m_zoomDistance += deltaMagnitudeDiff * FocusingSpeed;
                m_zoomDistance = Mathf.Clamp(m_zoomDistance, 0f, ZoomingCameraDistance);

                if (UICamera != null)
                {
                    UICamera.transform.position = new Vector3(UICamera.transform.position.x,
                        UICamera.transform.position.y, m_zoomDistance);
                }

                return;
            }

            Vector2 deltaMousePosition =
                new Vector2(eventData.position.x / Screen.width, eventData.position.y / Screen.height) -
                new Vector2(m_prevMousePosition.x / Screen.width, m_prevMousePosition.y / Screen.height);

            if (UseRolling == true && UIModuleModelData != null)
            {
                float fangle = IsAxisX == true
                    ? -deltaMousePosition.x * RollingTime
                    : -deltaMousePosition.y * RollingTime;
                UIModuleModelData.transform.Rotate(Vector3.up, fangle);
            }

            if (UsePitchZooming == true && UICamera != null && UIModuleModelData != null)
            {
                float distanceToModel = Mathf.Max(6.0f - m_zoomDistance, 0.5f);
                Vector3 touchPosition = eventData.position;
                touchPosition.z = distanceToModel;
                m_prevTouchPosition.z = distanceToModel;

                Vector3 worldPosition = UICamera.ScreenToWorldPoint(touchPosition);
                Vector3 worldPrevPosition = UICamera.ScreenToWorldPoint(m_prevTouchPosition);

                Vector3 deltaPosition = worldPosition - worldPrevPosition;
                float positionY = UICamera.transform.position.y - deltaPosition.y;

                float maxpitchposY = GetPivotHead() != null
                    ? GetPivotHead().position.y
                    : -(UIModuleModelData.ModelObject.transform.position.y);
                float minpitchposY = UIModuleModelData.ModelObject.transform.position.y;

                positionY = Mathf.Clamp(positionY, minpitchposY, maxpitchposY);

                UICamera.transform.position = new Vector3(UICamera.transform.position.x, positionY,
                    UICamera.transform.position.z);
                m_prevTouchPosition = touchPosition;
            }

            m_prevMousePosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_isDragging = false;
        }

        #endregion Unity EventSystems

        #region Methods

        IEnumerator OnSetModelInfo(GameObject modelObject, int modelObjectIDX)
        {
            if (UIModuleModelData != null)
            {
                UIModuleModelData.SetModelInfo(modelObject, modelObjectIDX, UICamera);
            }

            yield return new WaitForEndOfFrame();
        }

        public virtual void SetModelInfo(GameObject modelObject, int modelObjectIDX)
        {
            this.gameObject.SetActive(true);

            if (UICamera != null)
            {
                UICamera.transform.rotation = m_initializeCameraQuaternion;
                UICamera.transform.position = m_initializeCameraPosition;
            }

            if (Canvas != null)
            {
                Canvas.enabled = true;
            }

            if (UsePitchZooming == true)
            {
                m_zoomDistance = m_initializeCameraPosition.z;
            }

            StartCoroutine(OnSetModelInfo(modelObject, modelObjectIDX));
        }

        #endregion Methods
    }

}