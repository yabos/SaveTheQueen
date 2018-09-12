using UnityEngine;
using UnityEngine.UI;

namespace UIExtension
{
    public static class TextExtension
    {
        #region Text

        public static void SetText(this Text target, string text)
        {
            if (target.IsNull() == true)
                return;

            target.text = text;
        }

        #endregion

        #region Color

        public static void SetColor(this Text target, Color targetColor, bool crossFade = false)
        {
            if (target.IsNull() == true)
                return;

            if (crossFade == true)
            {
                target.CrossFadeColor(targetColor, 0.5f, true, true);
            }
            else
            {
                target.color = targetColor;
            }
        }

        #endregion

        public static bool IsNull(this Text target, bool log = false)
        {
            bool isNull = (target == null);

            if (isNull & log == true)
            {
                Debug.LogWarning("Text not found.");
            }

            return isNull;
        }
    }
}


