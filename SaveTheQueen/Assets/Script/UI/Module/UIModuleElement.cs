using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Lib.uGui
{
    [System.Serializable]
    public class UIModuleElement : UIModuleSimple, IScrollReceiver
    {
        [SerializeField]
        private ScrollRect m_scrollRect = null;
        [SerializeField]
        private GridLayoutGroup m_layoutGroup = null;
        [SerializeField]
        protected ScrollItem m_baseScrollItem = null;

        public ScrollItem ScrollItem
        {
            get { return m_baseScrollItem; }
        }

        virtual public void OnSetInfoEvent<T1>(T1 value = default(T1))
        {
        }

        virtual public void OnSelectEvent<T1>(T1 value = default(T1))
        {
        }

        virtual public void OnDoubleClickEvent<T1>(T1 value = default(T1))
        {
        }

        public void SetElementInfo<T>(List<T> list) where T : class
        {
            var values = UIExtension.ElementExtension.GetElementArray(m_baseScrollItem, list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                if (values[i] != null)
                {
                    values[i].SetInfo(list[i], this);
                    values[i].SetAlive(true);
                }
            }

            DynamicContentSize(list.Count);
        }

        private void DynamicContentSize(int count)
        {
            if (count != 0)
            {
                if (m_scrollRect.horizontal)
                {
                    m_scrollRect.content.sizeDelta =
                        new Vector2(m_layoutGroup.cellSize.x * count, m_layoutGroup.cellSize.y);
                }
                else if (m_scrollRect.vertical)
                {
                    m_scrollRect.content.sizeDelta =
                        new Vector2(m_layoutGroup.cellSize.x, m_layoutGroup.cellSize.y * count);
                }
            }

            m_scrollRect.content.anchoredPosition = Vector2.zero;
        }

        public override void OnExitModule()
        {
            base.OnExitModule();
        }

        public override void OnDestroyModule()
        {
            m_scrollRect = null;
            m_layoutGroup = null;
            m_baseScrollItem = null;

            base.OnDestroyModule();
        }
    }

}