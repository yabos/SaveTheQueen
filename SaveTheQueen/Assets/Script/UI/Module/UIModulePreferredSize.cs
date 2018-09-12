using UnityEngine;
using UnityEngine.UI;

namespace Lib.uGui
{
    [System.Serializable]
    public class UIModulePreferredSize : UIModuleSimple
    {

        [SerializeField]
        private ScrollRect m_scrollRect = null;
        [SerializeField]
        private ScrollItem m_element = null;

        private float m_contentSize = 0.0f;

        public ScrollItem[] GetElementArray(int count)
        {
            return UIExtension.ElementExtension.GetElementArray(m_element, count);
        }

        public void OnPreferredContentsSize(float preferredSize)
        {
            if (m_scrollRect == null && m_scrollRect.content == null)
                return;

            m_contentSize += preferredSize;

            m_scrollRect.content.sizeDelta = new Vector2(0.0f, m_contentSize);
        }

        public override void OnExitModule()
        {
            m_contentSize = 0.0f;

            base.OnExitModule();
        }

        public override void OnDestroyModule()
        {
            m_scrollRect = null;
            m_element = null;

            base.OnDestroyModule();
        }
    }

}