using UnityEngine;

namespace Lib.uGui
{
    public class UIModuleSimple : IUIModule
    {
        public GameObject Panel = null;

        private bool m_enter = false;

        public virtual void OnEnterModule()
        {
            if (m_enter == true)
                return;

            ConnectEventsListener();

            ActivatePanel(true);
            m_enter = true;
        }

        public virtual void OnExitModule()
        {
            DisconnectEventsListener();
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

        public virtual void OnDispose()
        {
        }

        protected virtual void ConnectEventsListener()
        {
        }

        protected virtual void DisconnectEventsListener()
        {
        }

        private void ActivatePanel(bool isActive)
        {
            if (Panel == null)
                return;

            Panel.SetActive(isActive);
        }
    }

}