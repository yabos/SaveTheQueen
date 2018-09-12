using System.Collections.Generic;
using Lib.uGui;

namespace Aniz.Widget
{

    //¾ðÁ¨°¡´Â »èÁ¦ÇÒ³ð...
    public abstract class UIModuleWidgetBase : WidgetBase
    {
        public List<UIModuleButton> Buttons = null;

        public List<UIModuleToggle> Toggles = null;

        public void SetButton(int index, UnityEngine.Events.UnityAction action)
        {
            if (Buttons == null)
            {
                return;
            }

            if (Buttons.Count > index && Buttons[index] != null)
            {
                if (action != null)
                {
                    Buttons[index].Set(action);
                }
                else
                {
                    Buttons[index].ClearListener();
                }
            }
        }

        public UIModuleButton GetButton(int index)
        {
            if (Buttons == null)
            {
                return null;
            }

            if (Buttons.Count > index)
            {
                return Buttons[index];
            }
            return null;
        }

        public void SetToggle(int index, UnityEngine.Events.UnityAction<bool> action)
        {
            if (Toggles == null)
            {
                return;
            }

            if (Toggles.Count > index && Toggles[index] != null)
            {
                if (action != null)
                {
                    Toggles[index].Set(action);
                }
                else
                {
                    Toggles[index].ClearListener();
                }
            }
        }

        public UIModuleToggle GetToggle(int index)
        {
            if (Toggles == null)
            {
                return null;
            }

            if (Toggles.Count > index)
            {
                return Toggles[index];
            }
            return null;
        }

    }
}