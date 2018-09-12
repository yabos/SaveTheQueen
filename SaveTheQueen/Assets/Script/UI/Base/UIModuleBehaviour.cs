using UnityEngine;
using Lib.Pattern;

namespace Lib.uGui
{
    public class UIModuleBehaviour : MonoBehaviour, IUIModule
    {
        public GameObject Panel = null;

        private CanvasGroup m_canvasGroup = null;
        private bool m_enter = false;

        public virtual void OnEnterModule()
        {
            if (m_enter == true)
                return;

            ActivatePanel(true);
            m_enter = true;

            Panel.SetActive(true);
        }

        public virtual void OnExitModule()
        {
            OnDispose();

            ActivatePanel(false);
            m_enter = false;
        }

        public virtual void OnRefreshModule()
        {
        }

        public virtual void OnDestroyModule()
        {
            Panel = null;
        }

        protected virtual void OnDispose()
        {
        }

        private void ActivatePanel(bool isActive)
        {
            if (Panel == null)
                Panel = this.gameObject;

            if (isActive == true)
            {
                OnShowModule();
            }
            else
            {
                OnHideModule();
            }
        }

        private void OnShowModule()
        {
            if (m_canvasGroup == null)
            {
                m_canvasGroup = ComponentFactory.GetComponent<CanvasGroup>(Panel, IfNotExist.AddNew);
            }

            m_canvasGroup.alpha = 1.0f;
            m_canvasGroup.blocksRaycasts = true;
            m_canvasGroup.interactable = true;
        }

        private void OnHideModule()
        {
            if (m_canvasGroup == null)
            {
                m_canvasGroup = ComponentFactory.GetComponent<CanvasGroup>(Panel, IfNotExist.AddNew);
            }

            m_canvasGroup.alpha = 0f;
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
        }
    }

}