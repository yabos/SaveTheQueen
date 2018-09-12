using UnityEngine;
using System.Collections;

namespace Lib.uGui
{

    [RequireComponent(typeof(UIModuleModelObject))]
    [AddComponentMenu("UI/ModelObjectRotate")]
    public class UIModuleModelObjectRotate : MonoBehaviour
    {
        public enum eRotationMode
        {
            Constant,
            WhenMouseIsOver,
            WhenMouseIsOverThenSnapBack
        }

        public eRotationMode RotationMode = eRotationMode.Constant;

        public bool RotateX = false;
        public float RotateXSpeed = 45f;

        public bool RotateY = true;
        public float RotateYSpeed = 45f;

        public bool RotateZ = false;
        public float RotateZSpeed = 45f;

        public float snapbackTime = 0.25f;

        private UIModuleModelObject m_uiModuleModelObject = null;

        private bool mouseIsOver = false;

        private Vector3 initialRotation = Vector3.zero;

        void Awake()
        {
            m_uiModuleModelObject = this.GetComponent<UIModuleModelObject>();
            initialRotation = UIModuleModelUtils.NormalizeRotation(m_uiModuleModelObject.TargetRotation);

            SetupEvents();
        }

        void Update()
        {
            switch (RotationMode)
            {
                case eRotationMode.Constant:
                    {
                        UpdateRotation();
                    }
                    break;
                case eRotationMode.WhenMouseIsOver:
                case eRotationMode.WhenMouseIsOverThenSnapBack:
                    {
                        if (mouseIsOver) UpdateRotation();
                    }
                    break;
            }
        }

        void UpdateRotation()
        {
            m_uiModuleModelObject.TargetRotation += new Vector3(
                    RotateX ? RotateXSpeed * Time.deltaTime : 0,
                    RotateY ? RotateYSpeed * Time.deltaTime : 0,
                    RotateZ ? RotateZSpeed * Time.deltaTime : 0
                );
        }

        void SetupEvents()
        {
            UnityEngine.EventSystems.EventTrigger trigger = this.GetComponent<UnityEngine.EventSystems.EventTrigger>() ?? this.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

            var onPointerEnter = new UnityEngine.EventSystems.EventTrigger.Entry { eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter };
            var onPointerExit = new UnityEngine.EventSystems.EventTrigger.Entry { eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit };

            onPointerEnter.callback.AddListener((e) => OnPointerEnter());
            onPointerExit.callback.AddListener((e) => OnPointerExit());

            trigger.triggers.Add(onPointerEnter);
            trigger.triggers.Add(onPointerExit);
        }

        void OnPointerEnter()
        {
            mouseIsOver = true;
        }

        void OnPointerExit()
        {
            mouseIsOver = false;

            if (RotationMode == eRotationMode.WhenMouseIsOverThenSnapBack)
            {
                StartCoroutine(SnapBack(snapbackTime));
            }
        }

        IEnumerator SnapBack(float time)
        {
            var timeStarted = Time.time;

            float percentageComplete = 0f;
            Vector3 snapStartRotation = UIModuleModelUtils.NormalizeRotation(m_uiModuleModelObject.TargetRotation);

            float desiredX = (Mathf.Abs(snapStartRotation.x - initialRotation.x) >= 180f) ? (initialRotation.x - 180f) : initialRotation.x;
            float desiredY = (Mathf.Abs(snapStartRotation.y - initialRotation.y) >= 180f) ? (initialRotation.y - 180f) : initialRotation.y;
            float desiredZ = (Mathf.Abs(snapStartRotation.z - initialRotation.z) >= 180f) ? (initialRotation.z - 180f) : initialRotation.z;

            while (percentageComplete < 1f)
            {
                m_uiModuleModelObject.TargetRotation = new Vector3(
                    (RotateX ? Mathf.Lerp(snapStartRotation.x, desiredX, percentageComplete) : desiredX),
                    (RotateY ? Mathf.Lerp(snapStartRotation.y, desiredY, percentageComplete) : desiredY),
                    (RotateZ ? Mathf.Lerp(snapStartRotation.z, desiredZ, percentageComplete) : desiredZ)
                    );

                percentageComplete = (Time.time - timeStarted) / time;

                yield return null;
            }

            m_uiModuleModelObject.TargetRotation = initialRotation;
        }
    }
}