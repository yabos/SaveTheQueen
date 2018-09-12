using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Lib.Pattern;

namespace UIExtension
{
    public static class RectTransformExtension
    {
        public static Vector3 GetNotAnchoredPosition(this RectTransform rectTransform, Canvas canvas)
        {
            if (rectTransform != null)
            {
                Vector3 notAnchoredPosition = rectTransform.anchoredPosition3D;
                if ((canvas != null) && (canvas.renderMode == RenderMode.ScreenSpaceOverlay))
                {
                    if (canvas.transform.localScale.x > 0.0f) { notAnchoredPosition.x *= canvas.transform.localScale.x; }
                    if (canvas.transform.localScale.y > 0.0f) { notAnchoredPosition.y *= canvas.transform.localScale.y; }
                }
                notAnchoredPosition.x += (rectTransform.anchorMin.x + rectTransform.anchorMax.x) * 0.5f * (float)Screen.width;
                notAnchoredPosition.y += (rectTransform.anchorMin.y + rectTransform.anchorMax.y) * 0.5f * (float)Screen.height;
                return notAnchoredPosition;
            }
            return Vector3.zero;
        }

        public static void SetAnchoredPosition(this RectTransform rectTransform, Canvas canvas, Vector3 position)
        {
            if (rectTransform != null)
            {
                Vector3 anchoredPosition = position;
                anchoredPosition.x -= (rectTransform.anchorMin.x + rectTransform.anchorMax.x) * 0.5f * (float)Screen.width;
                anchoredPosition.y -= (rectTransform.anchorMin.y + rectTransform.anchorMax.y) * 0.5f * (float)Screen.height;
                if ((canvas != null) && (canvas.renderMode == RenderMode.ScreenSpaceOverlay))
                {
                    if (canvas.transform.localScale.x > 0.0f) { anchoredPosition.x /= canvas.transform.localScale.x; }
                    if (canvas.transform.localScale.y > 0.0f) { anchoredPosition.y /= canvas.transform.localScale.y; }
                }
                rectTransform.anchoredPosition3D = anchoredPosition;
            }
        }
    }
}