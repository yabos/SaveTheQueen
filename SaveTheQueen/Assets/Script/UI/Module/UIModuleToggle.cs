using UnityEngine;
using System.Collections;

namespace Lib.uGui
{
    [System.Serializable]
    public class UIModuleToggle : IUIModule
    {
        protected bool m_isSetListener = false;

        public UnityEngine.UI.Toggle Toggle = null;

        #region IUIModule

        public virtual void OnEnterModule()
        {
        }

        public virtual void OnExitModule()
        {

        }

        public virtual void OnRefreshModule()
        {

        }

        public virtual void OnDestroyModule()
        {

        }

        #endregion IUIModule

        #region Methods

        public void Set(UnityEngine.Events.UnityAction<bool> action)
        {
            if (m_isSetListener)
            {
                return;
            }

            if (Toggle != null)
            {
                m_isSetListener = true;

                Toggle.onValueChanged.AddListener((bool isOn) =>
                {
                    OnClickAction(isOn, action);
                });
            }
        }

        public void ClearListener()
        {
            if (Toggle != null)
            {
                m_isSetListener = false;

                Toggle.onValueChanged.RemoveAllListeners();
            }
        }

        void OnClickAction(bool isOn, UnityEngine.Events.UnityAction<bool> action)
        {
            if (action != null)
            {
                action(isOn);
            }
        }

        #endregion Methods
    }

}