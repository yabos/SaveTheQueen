using UnityEngine;
using UnityEngine.UI;

namespace Lib.uGui
{
    public class ScrollGridPattern : ScrollPatternBase
    {
        public override void Init(ScrollRect scrollRect, RectTransform item, Vector2 spacing)
        {
            base.Init(scrollRect, item, spacing);

            Vector2 anchorPos;

            m_scrollviewSize = scrollRect.viewport.rect.height;

            anchorPos = new Vector2(0, 1);

            m_content.pivot = anchorPos;
            m_content.anchorMax = anchorPos;
            m_content.anchorMin = anchorPos;

            item.pivot = anchorPos;
            item.anchorMax = anchorPos;
            item.anchorMin = anchorPos;
            item.localPosition = Vector3.zero;

            ItemSize = m_itemHeight;
            ViewCount = m_gridYCount;
            LineCount = m_gridXCount;
        }

        public override int GetDataIndex(RectTransform owner)
        {
            int gridYIndex = Mathf.FloorToInt(-owner.anchoredPosition.y / (owner.rect.height + Spacing.y));
            int gridXIndex = Mathf.FloorToInt(owner.anchoredPosition.x / (owner.rect.width + Spacing.x));

            return gridYIndex * LineCount + gridXIndex;
        }

        public override float GetScrollValue(Vector2 currentPos)
        {
            return currentPos.y / ItemSize;
        }

        public override Vector2 GetChangePosition(float currentPos)
        {
            return new Vector2(0, -currentPos);
        }

        public override Vector2 GetContentSize(int dataCount)
        {
            return new Vector2(m_itemWidth, m_itemHeight * dataCount);
        }
    }

}