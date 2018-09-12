using System;
using UnityEngine;
using System.Collections.Generic;

namespace Lib.uGui
{
    [System.Serializable]
    public class UIModuleButton : IUIModule
    {
        protected bool m_isSetListener = false;

        protected bool m_isclickeventprocessing = false;

        public UnityEngine.UI.Button Button = null;

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

        public void Set(UnityEngine.Events.UnityAction action)
        {
            m_isclickeventprocessing = false;

            if (m_isSetListener == true)
            {
                Button.onClick.RemoveAllListeners();
            }

            if (Button != null)
            {
                m_isSetListener = true;

                Button.onClick.AddListener(() =>
                {
                    OnclickAction(action);
                });
            }
        }

        public void SetActive(bool active)
        {
            if (Button != null)
            {
                Button.gameObject.SetActive(active);
            }
        }

        void OnclickAction(UnityEngine.Events.UnityAction action)
        {
            if (m_isclickeventprocessing == false)
            {
                m_isclickeventprocessing = true;

                if (action != null)
                {
                    action();
                }
            }

            m_isclickeventprocessing = false;
        }

        public void ClearListener()
        {
            m_isSetListener = false;

            if (Button == null)
                return;

            Button.onClick.RemoveAllListeners();
        }

        #endregion Methods
    }

}