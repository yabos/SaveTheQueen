using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lib.Pattern;

namespace Lib.uGui
{
    public class UIModuleModelData : MonoBehaviour, IUIModule
    {
        private GameObject m_modelObject;

        public GameObject ModelObject
        {
            get { return m_modelObject; }
        }

        public bool PositionCenter = false;
        public bool PositionTableData = false;

        public bool IsInitializingQuaternion = true;
        protected Quaternion m_initializeQuaternion = Quaternion.identity;

        public bool IsInitializingPosition = true;
        protected Vector3 m_initializePosition = Vector3.zero;

        protected Vector3 m_localPosition = Vector3.zero;

        protected List<BoxCollider> m_boxColliders = new List<BoxCollider>();

        public List<BoxCollider> BoxColliders
        {
            get { return m_boxColliders; }
        }

        void Awake()
        {
            m_initializeQuaternion = transform.localRotation;

            m_initializePosition = transform.localPosition;
        }

        public void OnEnable()
        {
            if (IsInitializingQuaternion == true)
            {
                transform.localRotation = m_initializeQuaternion;
            }

            if (IsInitializingPosition == true)
            {
                transform.localPosition = m_initializePosition;
            }
        }

        void Update()
        {
            if (m_modelObject != null)
            {
                if (m_modelObject.transform.localPosition != m_localPosition)
                {
                    m_modelObject.transform.localPosition = m_localPosition;
                }
            }
        }

        #region IUIModule

        public virtual void OnEnterModule()
        {
        }

        public virtual void OnExitModule()
        {
            ResetModelInfo();
        }

        public virtual void OnRefreshModule()
        {
        }

        public virtual void OnDestroyModule()
        {
        }

        #endregion IUIModule

        #region Methods

        protected Transform m_originalModelObjectParent = null;
        protected Vector3 m_originalModelObjectPosition = Vector3.zero;
        protected Quaternion m_originalModelObjectRotation = Quaternion.identity;
        protected Vector3 m_originalModelObjectScale = Vector3.one;

        public virtual void SetModelInfo(GameObject modelObject, int modelObjectIDX, Camera uiCamera)
        {
            transform.localPosition = m_initializePosition;
            m_localPosition = new Vector3(0, -1f, 0);

            if (modelObject != null)
            {
                m_modelObject = modelObject;

                m_originalModelObjectParent = m_modelObject.transform.parent;
                m_modelObject.transform.SetParent(transform);

                m_originalModelObjectScale = m_modelObject.transform.localScale;
                m_originalModelObjectRotation = m_modelObject.transform.rotation;
                m_originalModelObjectPosition = m_modelObject.transform.position;

                m_modelObject.transform.localScale = Vector3.one;
                m_modelObject.transform.localRotation = Quaternion.identity;
                m_modelObject.transform.localPosition = m_localPosition;

                CalculateBoundBox(m_modelObject, true, false);
            }
        }

        public void ResetModelInfo()
        {
            transform.localPosition = m_initializePosition;
            m_localPosition = new Vector3(0, -1f, 0);

            if (m_modelObject != null)
            {
                m_modelObject.transform.SetParent(m_originalModelObjectParent);
                m_originalModelObjectParent = null;

                m_modelObject.transform.localScale = Vector3.one;
                m_modelObject.transform.localRotation = Quaternion.identity;
                m_modelObject.transform.localPosition = Vector3.zero;

                m_modelObject.transform.localScale = m_originalModelObjectScale;
                m_modelObject.transform.rotation = m_originalModelObjectRotation;
                m_modelObject.transform.position = m_originalModelObjectPosition;

                m_originalModelObjectScale = Vector3.one;
                m_originalModelObjectRotation = Quaternion.identity;
                m_originalModelObjectPosition = Vector3.zero;

                CalculateBoundBox(m_modelObject, true, true);
            }

            m_modelObject = null;
        }

        protected void CalculateBoundBox(GameObject modelObject, bool hasLayerSetting = true,
            bool hasDestroyBoundBox = false)
        {
            if (hasDestroyBoundBox == true)
            {
                if (m_boxColliders.Any())
                {
                    for (int i = 0; i < m_boxColliders.Count; ++i)
                    {
                        if (m_boxColliders[i] == null)
                        {
                            continue;
                        }

                        GameObjectFactory.DestroyComponent(m_boxColliders[i]);
                        m_boxColliders[i] = null;
                    }
                    m_boxColliders.Clear();
                }
            }
            else
            {
                m_boxColliders.Clear();
            }

            if (modelObject == null)
            {
                return;
            }

            Renderer[] renderers = ComponentFactory.GetChildComponents<Renderer>(modelObject, IfNotExist.ReturnNull);

            if (renderers != null)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    GameObject parentObject = renderers[i].gameObject;
                    Bounds bounds = renderers[i].bounds;

                    if (hasLayerSetting == true)
                    {
                        renderers[i].gameObject.layer = modelObject.transform.parent
                            ? modelObject.transform.parent.gameObject.layer
                            : LayerMask.NameToLayer("Default");
                    }

                    if (hasDestroyBoundBox == false)
                    {
                        SkinnedMeshRenderer skinnedMeshRenderer = renderers[i] as SkinnedMeshRenderer;
                        if (skinnedMeshRenderer != null)
                        {
                            parentObject = skinnedMeshRenderer.rootBone.gameObject;
                            bounds = skinnedMeshRenderer.localBounds;
                        }

                        BoxCollider boxCollider =
                            ComponentFactory.GetComponent<BoxCollider>(parentObject, IfNotExist.ReturnNull);
                        if (boxCollider == null)
                        {
                            boxCollider = ComponentFactory.GetComponent<BoxCollider>(parentObject, IfNotExist.AddNew);
                            boxCollider.center = bounds.center;
                            boxCollider.size = bounds.size;
                        }
                        m_boxColliders.Add(boxCollider);
                    }
                }
            }
        }

        #endregion Methods
    }

}