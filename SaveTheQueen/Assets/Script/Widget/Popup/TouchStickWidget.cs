#pragma warning disable 0414

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lib.uGui;
using Aniz.Widget;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UIExtension;

namespace Aniz.Widget.Panel
{

    public class TouchStickWidget : WidgetBase
    {
        public delegate void OnUpdateTouchStickEvent(Vector2 stickDirection, bool isPressed);

        public OnUpdateTouchStickEvent OnUpdateEvent = null;

        private Canvas m_canvas = null;

        public UnityEngine.EventSystems.EventTrigger EventTrigger = null;
        public List<UnityEngine.UI.Toggle> DebuggingToggleList = null;

        public UnityEngine.UI.Image TouchStickBase = null;
        public UnityEngine.UI.Image TouchStick = null;
        public UnityEngine.EventSystems.EventTrigger TouchStickEventTrigger = null;

        public UnityEngine.UI.Toggle FixedBaseToggle = null;
        public UnityEngine.UI.Toggle AccelerationToggle = null;

        public bool IsMovementTouchStickBase = true;

        private bool m_isPressingTouchStick = false;

        public bool IsPressingTouchStick
        {
            get { return m_isPressingTouchStick; }
        }

        private bool m_hasInitializedStickPositions = false;

        private Vector3 m_stickInitialAnchoredPosition = Vector3.zero;
        private Vector3 m_stickInitialPosition = Vector3.zero;
        private Vector3 m_stickPivotPosition = Vector3.zero;
        private Vector2 m_stickTouchPosition = Vector2.zero;
        private Vector2 m_stickTouchInertiaOffset = Vector2.zero;

        public Vector2 StickTouchPosition
        {
            get { return m_stickTouchPosition; }
        }

        private Vector2 m_stickOffset = Vector2.zero;

        public Vector2 StickOffset
        {
            get { return (m_stickOffset + m_stickTouchInertiaOffset); }
        }

        private Vector2 m_prevStickOffset = Vector2.zero;
        private Vector2 m_stickOffsetDelta = Vector2.zero;

        public Vector2 StickOffsetDelta
        {
            get { return m_stickOffsetDelta; }
        }

        public Vector2 StickDirection
        {
            get
            {
                Vector3 stickDirection = Vector3.zero;
                if (m_isPressingTouchStick || IsUsingAcceleration)
                {
                    stickDirection.x = Mathf.Clamp(StickOffset.x / ClampRadius, -1.0f, 1.0f);
                    stickDirection.y = Mathf.Clamp(StickOffset.y / ClampRadius, -1.0f, 1.0f);
                }

                return stickDirection.normalized;
            }
        }

        private bool m_isTwoTouchState = false;

        public bool IsTwoTouchState
        {
            get { return m_isTwoTouchState; }
        }

        private Vector2 m_secondStickTouchPosition = Vector2.zero;

        public Vector2 SecondStickTouchPosition
        {
            get { return m_secondStickTouchPosition; }
        }

        private float m_twoTouchDistance = 0.0f;

        public float TwoTouchDistance
        {
            get { return m_twoTouchDistance; }
        }

        private float m_prevTwoTouchDistance = 0.0f;
        private float m_twoTouchDistanceDelta = 0.0f;

        public float TwoTouchDistanceDelta
        {
            get { return m_twoTouchDistanceDelta; }
        }

        private bool m_useScreenBoundInertiaOffset = true;
        private float m_screenBoundThreshold = 30.0f;
        private float m_screenBoundInertiaSpeed = 150.0f;
        private float m_scrrenBoundInertiaRestoreSpeed = 200.0f;

        private bool m_isUsingAcceleration = false;

        public bool IsUsingAcceleration
        {
            get { return m_isUsingAcceleration; }
            set
            {
                if (m_isUsingAcceleration != value)
                {
                    m_isUsingAcceleration = value;
                    UpdateAccelerationMode();
                }
            }
        }

        private Vector2 m_accelerationBase = new Vector2(0.0f, 0.5f);

        public Vector2 AccelerationBase
        {
            get { return m_accelerationBase; }
        }

        private float m_accelerationBaseRetoreSpeed = 0.5f;
        private float m_accelerationApplySpeed = 5.0f;
        private float m_accelerationResetSpeed = 5.0f;
        public bool AccelerationInverseUpDown = false;
        public bool AccelerationInverseLeftRight = false;
        public bool HideOnUsingAcceleration = true;

        public float ClampRadius = 100.0f;

        private bool m_isUsingFixedBase = false;

        public bool IsUsingFixedBase
        {
            get { return m_isUsingFixedBase; }
            set
            {
                if (m_isUsingFixedBase != value)
                {
                    m_isUsingFixedBase = value;
                    if (m_isUsingFixedBase)
                    {
                        m_stickPivotPosition = m_stickInitialPosition;
                    }
                }
            }
        }

        public override bool IsFlow
        {
            get { return false; }
        }

        void InitializeStickPositions()
        {
            if (!m_hasInitializedStickPositions)
            {
                if (m_canvas != null && TouchStickBase != null)
                {
                    m_stickInitialAnchoredPosition = TouchStickBase.rectTransform.anchoredPosition3D;
                    m_stickInitialPosition = TouchStickBase.rectTransform.GetNotAnchoredPosition(m_canvas);
                    m_stickPivotPosition = m_stickInitialPosition;
                    m_hasInitializedStickPositions = true;
                }
            }
        }

        public override void BhvOnEnter()
        {
            m_canvas = GetComponentInParent<Canvas>();

            OnChangeIsUsingFixedBase(true);
            OnChangeIsUsingAcceleration(false);

            AddEventTrigger(OnPressedTouchStick, EventTriggerType.PointerDown);
            AddEventTrigger(OnReleasedTouchStick, EventTriggerType.PointerUp);

            if (DebuggingToggleList != null)
            {
                for (int i = 0; i < DebuggingToggleList.Count; i++)
                {
                    if (DebuggingToggleList[i] == null)
                    {
                        continue;
                    }

                    UnityEngine.UI.Toggle toggle = DebuggingToggleList[i];

                    switch (i)
                    {
                        //FixedBase
                        case 0:
                            toggle.onValueChanged.AddListener((bool ison) => { OnChangeIsUsingFixedBase(ison); });
                            break;
                        //Acceleration
                        case 1:
                            toggle.onValueChanged.AddListener((bool ison) => { OnChangeIsUsingAcceleration(ison); });
                            break;
                    }
                }
            }

            InitializeStickPositions();

            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.OSXEditor)
            {
                m_accelerationBase.x = Input.mousePosition.x;
                m_accelerationBase.y = Input.mousePosition.y;
            }

            if (FixedBaseToggle != null)
            {
                IsUsingFixedBase = FixedBaseToggle.isOn;
            }

            if (AccelerationToggle != null)
            {
                IsUsingAcceleration = AccelerationToggle.isOn;
            }
        }

        public override void BhvOnLeave()
        {

        }

        public override void FinalizeWidget()
        {
        }

        protected override void ShowWidget(IUIDataParams data)
        {
        }

        protected override void HideWidget()
        {
        }

        private void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
        {
            if (EventTrigger == null)
            {
                return;
            }

            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action());

            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
            EventTrigger.triggers.Add(entry);
        }

        void Update()
        {
            // touch stick update
            {
                m_prevStickOffset = StickOffset;

                if (IsUsingAcceleration)
                {
                    UpdateStickPositionByAcceleration();
                }
                else
                {
                    UpdateStickPosition();
                }

                m_stickOffsetDelta = StickOffset - m_prevStickOffset;
            }

            // event update
            {
                if (OnUpdateEvent != null)
                {
                    OnUpdateEvent(StickDirection, m_isPressingTouchStick);
                }
            }
        }

        void UpdateStickPosition()
        {
            bool isTwoTouchState = false;
            Vector3 notAnchoredStickPosition = Vector3.zero;

            if (m_isPressingTouchStick)
            {
                if (TouchStick != null)
                {
                    // Process for Mobile Device.
                    if (Input.touches.Length > 0)
                    {
                        float firstNearDistance = float.MaxValue;
                        Vector2 firstNearTouchPosition = Vector2.zero;
                        float secondNearDistance = float.MaxValue;
                        Vector2 secondNearTouchPosition = Vector2.zero;

                        for (int i = 0; i < Input.touches.Length; ++i)
                        {
                            Touch touch = Input.touches[i];
                            float distanceFromPrev = (touch.position - m_stickTouchPosition).magnitude;
                            if (distanceFromPrev < firstNearDistance)
                            {
                                secondNearDistance = firstNearDistance;
                                secondNearTouchPosition = firstNearTouchPosition;
                                firstNearDistance = distanceFromPrev;
                                firstNearTouchPosition = touch.position;
                            }
                            else if (distanceFromPrev < secondNearDistance)
                            {
                                secondNearDistance = distanceFromPrev;
                                secondNearTouchPosition = touch.position;
                            }
                        }

                        if (firstNearDistance < float.MaxValue)
                        {
                            Vector2 position = new Vector2(firstNearTouchPosition.x, firstNearTouchPosition.y);
                            Vector2 delta =
                                Vector2.ClampMagnitude(
                                    position - new Vector2(m_stickInitialPosition.x, m_stickInitialPosition.y),
                                    ClampRadius);
                            firstNearTouchPosition.x = m_stickInitialPosition.x + delta.x;
                            firstNearTouchPosition.y = m_stickInitialPosition.y + delta.y;

                            m_stickTouchPosition = firstNearTouchPosition;
                            TouchStick.rectTransform.SetAnchoredPosition(m_canvas,
                                new Vector3(m_stickTouchPosition.x, m_stickTouchPosition.y,
                                    TouchStick.rectTransform.anchoredPosition3D.z));

                            if (secondNearDistance < float.MaxValue)
                            {
                                m_secondStickTouchPosition = secondNearTouchPosition;
                                m_twoTouchDistance = (secondNearTouchPosition - firstNearTouchPosition).magnitude;
                                isTwoTouchState = true;
                            }
                        }
                    }
                    else
                    {
                        // Process for Editor.
                        Vector2 position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        Vector2 delta =
                            Vector2.ClampMagnitude(
                                position - new Vector2(m_stickInitialPosition.x, m_stickInitialPosition.y),
                                ClampRadius);
                        position.x = m_stickInitialPosition.x + delta.x;
                        position.y = m_stickInitialPosition.y + delta.y;

                        m_stickTouchPosition = position;
                        TouchStick.rectTransform.SetAnchoredPosition(m_canvas,
                            new Vector3(m_stickTouchPosition.x, m_stickTouchPosition.y,
                                TouchStick.rectTransform.anchoredPosition3D.z));
                    }

                    // Update Pad Stick Offset.
                    notAnchoredStickPosition = TouchStick.rectTransform.GetNotAnchoredPosition(m_canvas);

                    m_stickOffset.x = notAnchoredStickPosition.x - m_stickPivotPosition.x;
                    m_stickOffset.y = notAnchoredStickPosition.y - m_stickPivotPosition.y;
                }
            }

            if (isTwoTouchState)
            {
                if (m_isTwoTouchState)
                {
                    m_twoTouchDistanceDelta = m_twoTouchDistance - m_prevTwoTouchDistance;
                }
                else
                {
                    m_twoTouchDistanceDelta = 0.0f;
                }
                m_prevTwoTouchDistance = m_twoTouchDistance;
                m_isTwoTouchState = true;
            }
            else
            {
                m_twoTouchDistance = 0.0f;
                m_twoTouchDistanceDelta = 0.0f;
                m_prevTwoTouchDistance = 0.0f;
                m_isTwoTouchState = false;
            }

            // Update Stick Touch Inertia Offset.
            bool isStickInScreenBound = true;
            if (m_isPressingTouchStick && m_useScreenBoundInertiaOffset)
            {
                if (notAnchoredStickPosition.x < m_screenBoundThreshold)
                {
                    m_stickTouchInertiaOffset.x -= m_screenBoundInertiaSpeed * Time.deltaTime;
                    isStickInScreenBound = false;
                }
                else if (notAnchoredStickPosition.x > ((float)Screen.width - m_screenBoundThreshold))
                {
                    m_stickTouchInertiaOffset.x += m_screenBoundInertiaSpeed * Time.deltaTime;
                    isStickInScreenBound = false;
                }

                if (notAnchoredStickPosition.y < m_screenBoundThreshold)
                {
                    m_stickTouchInertiaOffset.y -= m_screenBoundInertiaSpeed * Time.deltaTime;
                    isStickInScreenBound = false;
                }
                else if (notAnchoredStickPosition.y > ((float)Screen.height - m_screenBoundThreshold))
                {
                    m_stickTouchInertiaOffset.y += m_screenBoundInertiaSpeed * Time.deltaTime;
                    isStickInScreenBound = false;
                }
            }

            if (isStickInScreenBound)
            {
                m_stickTouchInertiaOffset = Vector2.MoveTowards(m_stickTouchInertiaOffset, Vector2.zero,
                    m_scrrenBoundInertiaRestoreSpeed * Time.deltaTime);
            }
        }

        void UpdateStickPositionByAcceleration()
        {
            // Update Aim Control Offset by mouse position.
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.OSXEditor)
            {
                float horizontalControlRatio = 2.0f;
                float verticalControlRatio = 2.0f;

                Vector2 aimControlOffsetTarget = new Vector2(
                    (m_accelerationBase.x + Input.mousePosition.x) * horizontalControlRatio,
                    (m_accelerationBase.y + Input.mousePosition.y) * verticalControlRatio);

                aimControlOffsetTarget.x *= (AccelerationInverseLeftRight ? -1.0f : 1.0f);
                aimControlOffsetTarget.y *= (AccelerationInverseUpDown ? -1.0f : 1.0f);

                m_stickOffset = Vector2.Lerp(m_stickOffset, aimControlOffsetTarget,
                    m_accelerationApplySpeed * Time.deltaTime);

                m_accelerationBase.x = Mathf.Lerp(m_accelerationBase.x, -Input.mousePosition.x,
                    m_accelerationBaseRetoreSpeed * Time.deltaTime);

                m_accelerationBase.y = Mathf.Lerp(m_accelerationBase.y, -Input.mousePosition.y,
                    m_accelerationBaseRetoreSpeed * Time.deltaTime);
            }
            else
            {
                // Update Aim Control Offset by acceleration.
                float horizontalControlRatio = 500.0f;
                float verticalControlRatio = 500.0f;

                Vector2 aimControlOffsetTarget = new Vector2(
                    Mathf.Asin(Mathf.Clamp(m_accelerationBase.x + Input.acceleration.x, -1.0f, 1.0f)) *
                    horizontalControlRatio,
                    Mathf.Asin(Mathf.Clamp(m_accelerationBase.y + Input.acceleration.y, -1.0f, 1.0f)) *
                    verticalControlRatio);

                aimControlOffsetTarget.x *= (AccelerationInverseLeftRight ? -1.0f : 1.0f);
                aimControlOffsetTarget.y *= (AccelerationInverseUpDown ? -1.0f : 1.0f);

                m_stickOffset = Vector2.Lerp(m_stickOffset, aimControlOffsetTarget,
                    m_accelerationApplySpeed * Time.deltaTime);

                m_accelerationBase.x = Mathf.Lerp(m_accelerationBase.x, -Input.acceleration.x,
                    m_accelerationBaseRetoreSpeed * Time.deltaTime);

                m_accelerationBase.y = Mathf.Lerp(m_accelerationBase.y, -Input.acceleration.y,
                    m_accelerationBaseRetoreSpeed * Time.deltaTime);

                m_accelerationBase.x = Mathf.Clamp(m_accelerationBase.x, -0.25f, 0.25f);
                m_accelerationBase.y = Mathf.Clamp(m_accelerationBase.y, 0.25f, 0.75f);
            }

            m_twoTouchDistance = 0.0f;
            m_prevTwoTouchDistance = 0.0f;
            m_twoTouchDistanceDelta = 0.0f;
            m_stickTouchInertiaOffset = Vector2.zero;
        }

        public void ResetStickBasePositionToStick(float resetRatio = 1.0f)
        {
            // Reset Aim Control Base by mouse position.
            if (IsUsingAcceleration)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.OSXEditor)
                {
                    m_accelerationBase.x = -Input.mousePosition.x;
                    m_accelerationBase.y = -Input.mousePosition.y;
                }
                else
                {
                    // Reset Aim Control Base by acceleration.
                    m_accelerationBase.x = -Input.acceleration.x;
                    m_accelerationBase.y = -Input.acceleration.y;

                    m_accelerationBase.x = Mathf.Clamp(m_accelerationBase.x, -0.25f, 0.25f);
                    m_accelerationBase.y = Mathf.Clamp(m_accelerationBase.y, 0.25f, 0.75f);
                }
            }
            else
            {
                if (TouchStick != null)
                {
                    Vector3 touchStickPosition = TouchStick.rectTransform.GetNotAnchoredPosition(m_canvas);
                    m_stickPivotPosition = Vector3.Lerp(m_stickPivotPosition, touchStickPosition, resetRatio);
                    if (TouchStickBase != null)
                    {
                        TouchStickBase.rectTransform.SetAnchoredPosition(m_canvas, m_stickPivotPosition);
                    }
                }
            }
        }

        void UpdateAccelerationMode()
        {
            if (TouchStickBase != null)
            {
                TouchStickBase.gameObject.SetActive(!IsUsingAcceleration);
            }

            if (TouchStick != null)
            {
                TouchStick.gameObject.SetActive(!IsUsingAcceleration | !HideOnUsingAcceleration);
            }

            ImplReleasedTouchStick();

            m_stickPivotPosition = m_stickInitialPosition;
            if (TouchStickBase != null && m_hasInitializedStickPositions)
            {
                TouchStickBase.rectTransform.SetAnchoredPosition(m_canvas, m_stickPivotPosition);
            }

            if (TouchStick != null)
            {
                TouchStick.rectTransform.SetAnchoredPosition(m_canvas, m_stickPivotPosition);
            }
        }

        public void OnPressedTouchStick()
        {
            ImplPressedTouchStick();
        }

        private void ImplPressedTouchStick()
        {
            // Record Touch Stick Pivot Position.
            m_isPressingTouchStick = true;
            m_stickOffset = Vector2.zero;

            if (!IsUsingAcceleration)
            {
                if (Input.touches.Length > 0)
                {
                    // Process for Mobile Device.
                    float minDistanceFromInitial = float.MaxValue;
                    for (int i = 0; i < Input.touches.Length; ++i)
                    {
                        Touch touch = Input.touches[i];
                        Vector3 targetPosition =
                            new Vector3(touch.position.x, touch.position.y, m_stickPivotPosition.z);
                        float distanceFromInitial = (targetPosition - m_stickInitialPosition).magnitude;
                        if (distanceFromInitial < minDistanceFromInitial)
                        {
                            minDistanceFromInitial = distanceFromInitial;
                            m_stickPivotPosition = (IsUsingFixedBase ? m_stickInitialPosition : targetPosition);
                            m_stickTouchPosition = touch.position;
                        }
                    }
                }
                else
                {
                    // Process for Editor.
                    m_stickPivotPosition = (IsUsingFixedBase
                        ? m_stickInitialPosition
                        : new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_stickPivotPosition.z));
                    m_stickTouchPosition = m_stickPivotPosition;

                }

                if (TouchStickBase != null && IsMovementTouchStickBase == true)
                {
                    TouchStickBase.rectTransform.SetAnchoredPosition(m_canvas, m_stickPivotPosition);
                }
            }

        }

        public void OnReleasedTouchStick()
        {
            ImplReleasedTouchStick();
        }

        private void ImplReleasedTouchStick()
        {
            m_isPressingTouchStick = false;
            m_stickOffset = Vector2.zero;

            m_stickPivotPosition = m_stickInitialPosition;

            if (TouchStickBase != null && m_hasInitializedStickPositions)
            {
                TouchStickBase.rectTransform.anchoredPosition3D = m_stickInitialAnchoredPosition;
            }

            if (TouchStick != null && m_hasInitializedStickPositions)
            {
                TouchStick.rectTransform.anchoredPosition3D = m_stickInitialAnchoredPosition;
            }
        }

        public Vector3 GetDirectionVector(Vector3 forward, Vector3 right, Vector3 movingDirection)
        {
            if (forward.y != 0.0f)
            {
                forward.y = 0.0f;
                forward.Normalize();
            }

            if (right.y != 0.0f)
            {
                right.y = 0.0f;
                right.Normalize();
            }

            Vector2 stickOffset = Vector2.zero;
            if (IsUsingAcceleration == true)
            {
                // Apply Moving Direction for TouchStick.
                stickOffset = StickOffset;
                stickOffset.x = Mathf.Clamp(0.01f * stickOffset.x, -1.0f, 1.0f);
                stickOffset.y = Mathf.Clamp(0.01f * stickOffset.y, -1.0f, 1.0f);

                movingDirection += forward * stickOffset.y + right * stickOffset.x;
            }

            stickOffset.x = Input.GetAxis("Horizontal");
            stickOffset.y = Input.GetAxis("Vertical");

            bool isPressingMovingKey = (Mathf.Abs(stickOffset.x) > 0.1f) || (Mathf.Abs(stickOffset.y) > 0.1f);
            if (isPressingMovingKey == true)
            {
                // Apply Moving Direction for Keyboard & GamePad.
                movingDirection += forward * stickOffset.y + right * stickOffset.x;
            }

            return movingDirection;
        }

        private void OnChangeIsUsingAcceleration(bool enable)
        {
            IsUsingAcceleration = enable;
        }

        private void OnChangeIsUsingFixedBase(bool enable)
        {
            IsUsingFixedBase = enable;
        }

        public override void OnNotify(Lib.Event.INotify notify)
        {
        }
    }
}