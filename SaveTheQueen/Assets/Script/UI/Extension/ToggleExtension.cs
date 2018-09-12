using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UIExtension
{
    public static class ToggleExtension
    {
        #region Interactable

        public static void SetInteractable(this Toggle toggle, bool interatable)
        {
            if (toggle.IsNull() == true)
                return;

            toggle.interactable = interatable;
        }

        #endregion

        #region EventListener

        public static void AddOnChange(this Toggle toggle, UnityAction<bool> call)
        {
            if (toggle.IsNull() == true)
                return;

            toggle.onValueChanged.AddListener(call);
        }

        public static void RemoveOnChange(this Toggle toggle, UnityAction<bool> call)
        {
            if (toggle.IsNull() == true)
                return;

            toggle.onValueChanged.RemoveListener(call);
        }

        public static void RemoveAllOnChange(this Toggle toggle)
        {
            if (toggle.IsNull() == true)
                return;

            toggle.onValueChanged.RemoveAllListeners();
        }

        #endregion

        #region Text

        public static void SetToggleText(this Toggle toggle, string text)
        {
            if (toggle.IsNull() == true)
                return;

            FindExtension.FindTextChild(toggle.transform).SetText(text);
        }

        #endregion

        public static bool IsNull(this Toggle toggle, bool log = true)
        {
            bool isNull = (toggle == null);

            if (isNull & log == true)
                Debug.LogError("Toggle not found.");

            return isNull;
        }
    }
}

