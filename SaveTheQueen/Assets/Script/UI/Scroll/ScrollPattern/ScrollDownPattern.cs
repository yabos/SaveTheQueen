using UnityEngine;
using UnityEngine.UI;

namespace Lib.uGui
{
    public class ScrollDownPattern : ScrollPatternBase
    {

        public override void Init(ScrollRect scrollRect, RectTransform item, Vector2 spacing)
        {
            base.Init(scrollRect, item, spacing);
            Vector2 anchorPos;

            anchorPos = new Vector2(0.5f, 1f);

            m_content.pivot = anchorPos;
            m_content.anchorMax = anchorPos;
            m_content.anchorMin = anchorPos;

            anchorPos.x = 0f;

            item.pivot = anchorPos;
            item.anchorMax = anchorPos;
            item.anchorMin = anchorPos;
            item.localPosition = Vector3.zero;

            ItemSize = m_itemHeight;
            ViewCount = GridYCount;
        }

        public override int GetDataIndex(RectTransform owner)
        {
            return Mathf.FloorToInt(-owner.anchoredPosition.y / (owner.rect.height + Spacing.y));
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