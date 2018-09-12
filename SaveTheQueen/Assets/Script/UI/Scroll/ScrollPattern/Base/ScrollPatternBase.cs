using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Lib.uGui
{
    public enum eScrollPattern
    {
        VerticalDown,
        VerticalUp,
        Horizontal,
    }

    public class ScrollPatternBase
    {
        public float ItemSize = 0f;
        public Vector2 Spacing = Vector2.zero;

        public int ViewCount = 0;
        public int LineCount = 0;

        protected RectTransform m_content;

        protected int m_gridXCount = 1;

        public int GridXCount
        {
            get { return m_gridXCount; }
        }

        protected int m_gridYCount = 1;

        public int GridYCount
        {
            get { return m_gridYCount; }
        }

        protected float m_itemWidth = 0f;

        public float ItemWidth
        {
            get { return m_itemWidth; }
        }

        protected float m_itemHeight = 0f;

        public float ItemHeight
        {
            get { return m_itemHeight; }
        }

        protected float m_scrollviewSize;

        public float ViewSize
        {
            get { return m_scrollviewSize; }
        }

        public virtual void Init(ScrollRect scrollRect, RectTransform item, Vector2 spacing)
        {
            m_itemWidth = item.rect.width + spacing.x;
            m_itemHeight = item.rect.height + spacing.y;
            Spacing = spacing;

            m_content = scrollRect.content;

            RectTransform viewport = scrollRect.viewport;
            m_gridYCount = Mathf.Clamp(Mathf.CeilToInt(viewport.rect.height / m_itemHeight), 1, int.MaxValue);
            m_gridXCount = Mathf.Clamp(Mathf.FloorToInt(viewport.rect.width / m_itemWidth), 1, int.MaxValue);
        }

        public virtual float GetScrollValue(Vector2 currentPos)
        {
            return 0;
        }

        public virtual Vector2 GetChangePosition(float currentPos)
        {
            return Vector2.zero;
        }

        public virtual int GetDataIndex(RectTransform owner)
        {
            return 0;
        }

        public virtual Vector2 GetContentSize(int dataCount)
        {
            return Vector2.zero;
        }
    }

}