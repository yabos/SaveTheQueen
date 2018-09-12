using UnityEngine;
using System.Collections;

namespace Lib.uGui
{

    [RequireComponent(typeof(UIModuleModelObject))]
    [AddComponentMenu("UI/ModelObjectDragRotate")]
    public class UIModuleModelObjectDragRotate : MonoBehaviour
    {
        [Header("X")]
        public bool RotateX = true;
        public bool InvertX = false;
        private int m_xMultiplier
        {
            get { return InvertX ? -1 : 1; }
        }

        [Header("Y")]
        public bool RotateY = true;
        public bool InvertY = false;
        private int m_yMultiplier
        {
            get { return InvertY ? -1 : 1; }
        }

        [Header("Sensitivity")]
        public float Sensitivity = 0.4f;

        private UIModuleModelObject m_uiModuleModelObject = null;

        void Awake()
        {
            m_uiModuleModelObject = this.GetComponent<UIModuleModelObject>();

            SetupEvents();
        }

        void SetupEvents()
        {
            UnityEngine.EventSystems.EventTrigger trigger = this.GetComponent<UnityEngine.EventSystems.EventTrigger>() ?? this.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

            var onDrag = new UnityEngine.EventSystems.EventTrigger.Entry { eventID = UnityEngine.EventSystems.EventTriggerType.Drag };
            onDrag.callback.AddListener((e) => OnDrag(e as UnityEngine.EventSystems.PointerEventData));
            trigger.triggers.Add(onDrag);
        }

        void OnDrag(UnityEngine.EventSystems.PointerEventData e)
        {
            if (RotateX)
            {
                var x = e.delta.x * Sensitivity * m_xMultiplier * -1;
                var xRotation = Quaternion.AngleAxis(x, Vector3.up);
                m_uiModuleModelObject.TargetRotation = (xRotation * Quaternion.Euler(m_uiModuleModelObject.TargetRotation)).eulerAngles;
            }

            if (RotateY)
            {
                var y = e.delta.y * Sensitivity * m_yMultiplier;
                var yRotation = Quaternion.AngleAxis(y, Vector3.right);
                m_uiModuleModelObject.TargetRotation = (yRotation * Quaternion.Euler(m_uiModuleModelObject.TargetRotation)).eulerAngles;
            }
        }
    }
}