using UnityEngine;
using UnityEngine.UI;

namespace Lib.uGui
{
    public class ScrollLeftPattern : ScrollPatternBase
    {

        public override void Init(ScrollRect scrollRect, RectTransform item, Vector2 spacing)
        {
            base.Init(scrollRect, item, spacing);

            m_content.pivot = new Vector2(0, 1);
            m_content.anchorMax = new Vector2(0, 0.5f);
            m_content.anchorMin = new Vector2(0, 0.5f);

            item.pivot = new Vector2(0, 0);
            item.anchorMax = new Vector3(0, 0.5f);
            item.anchorMin = new Vector3(0, 0.5f);
            item.localPosition = Vector3.zero;

            ItemSize = m_itemWidth;
            ViewCount = m_gridXCount;
        }

        public override int GetDataIndex(RectTransform owner)
        {
            return Mathf.FloorToInt(owner.anchoredPosition.x / (owner.rect.width + Spacing.x));
        }

        public override float GetScrollValue(Vector2 currentPos)
        {
            return -currentPos.x / ItemSize;
        }

        public override Vector2 GetChangePosition(float currentPos)
        {
            return new Vector2(currentPos, 0);
        }

        public override Vector2 GetContentSize(int dataCount)
        {
            return new Vector2(m_itemWidth * dataCount, m_itemHeight);
        }
    }

}