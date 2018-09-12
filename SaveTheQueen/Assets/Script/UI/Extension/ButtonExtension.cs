using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Lib.Pattern;

namespace UIExtension
{
    public static class ButtonExtension
    {
        #region Interactable

        public static void SetInteractable(this Button target, bool interactable, Color textColor, Color outlineColor)
        {
            if (target == null)
                return;

            target.interactable = interactable;

            Text buttonText = FindExtension.FindTextChild(target.transform);
            buttonText.SetColor(textColor);

            Outline buttonOutline = ComponentFactory.GetChildComponent<Outline>(target.gameObject, IfNotExist.ReturnNull);

            if (buttonOutline == null)
                return;

            buttonOutline.effectColor = outlineColor;
        }

        #endregion

        #region EventListner

        public static void AddOnClick(this Button button, UnityAction call)
        {
            if (button.IsNull() == true)
                return;

            button.onClick.AddListener(call);
        }

        public static void RemoveOnClick(this Button button, UnityAction call)
        {
            if (button.IsNull() == true)
                return;

            button.onClick.RemoveListener(call);
        }

        public static void RemoveAllOnClick(this Button button)
        {
            if (button.IsNull() == true)
                return;

            button.onClick.RemoveAllListeners();
        }

        #endregion

        public static void SetActive(this Button button, bool isActive)
        {
            if (button.IsNull() == true)
                return;

            button.gameObject.SetActive(isActive);
        }

        public static bool IsNull(this Button button, bool log = false)
        {
            bool isNull = (button == null);

            if (isNull & log == true)
                Debug.LogError("Button not found.");

            return isNull;
        }
    }
}


