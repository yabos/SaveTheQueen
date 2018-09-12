using UnityEngine;
using UnityEngine.UI;

namespace Lib.uGui
{
    public class UIModuleScrollView<T> : ScrollViewListener<T> where T : class
    {
        public eScrollPattern ScrollPatternType = eScrollPattern.VerticalDown;

        [SerializeField]
        protected ScrollRect m_scrollRect = null;
        [SerializeField]
        protected ScrollItem m_element = null;

        private bool m_initFlag = false;

        public override void OnEnterModule()
        {
            base.OnEnterModule();

            Connect();
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

        public override void OnRefreshModule()
        {
            base.OnRefreshModule();

            OnScrollRefresh();
        }

        protected virtual void OnInit()
        {
            OnScrollInit(m_scrollRect, m_element.gameObject, ScrollPatternType);
        }

        protected virtual void SetScrollInfo()
        {
        }
    }

}