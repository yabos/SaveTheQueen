using UnityEngine;
using UnityEngine.UI;

namespace Lib.uGui
{
    public class UIModuleGridScrollView<T> : GridScrollViewListener<T> where T : class
    {
        [SerializeField]
        protected ScrollRect m_scrollRect = null;
        [SerializeField]
        protected ScrollItem m_element = null;

        private bool m_initFlag = false;

        public override void OnEnterModule()
        {
            Connect();

            base.OnEnterModule();
        }

        public override void OnExitModule()
        {
            base.OnExitModule();
        }

        public override void OnDestroyModule()
        {
            base.OnDestroyModule();

            m_scrollRect = null;
        }

        private void Connect()
        {
            if (m_initFlag == false)
            {
                OnInit();
                m_initFlag = true;
            }

            SetScrollInfo();
        }

        protected virtual void OnInit()
        {
            OnScrollInit(m_scrollRect, m_element.gameObject);
        }

        protected virtual void SetScrollInfo()
        {
        }
    }

}