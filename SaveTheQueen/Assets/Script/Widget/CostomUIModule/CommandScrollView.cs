using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lib.uGui;

namespace Aniz.Widget.Module
{
    [System.Serializable]
    public class CommandScrollView : UIModuleScrollView<CommandData>
    {
        protected CommandScrollElement CurrentSelectedElement = null;

        protected int CurrentSelectedIDX = -1;

        protected System.Action<CommandScrollElement> OnDoubleClickAction = null;

        public void SetInfo(List<CommandData> list)
        {
            base.OnEnterModule();

            OnScrollEnter(list);
        }

        public override void OnExitModule()
        {
            base.OnExitModule();
        }

        protected override void OnInit()
        {
            base.OnInit();

            CurrentSelectedElement = null;
            CurrentSelectedIDX = -1;
        }

        public void SetDoubleClickAction(System.Action<CommandScrollElement> action)
        {
            OnDoubleClickAction = action;
        }

        public override void OnSetInfoEvent<T1>(T1 value = default(T1))
        {
            CommandScrollElement element = value as CommandScrollElement;

            bool selected = false;

            if (CurrentSelectedIDX != -1)
            {
                selected = (CurrentSelectedIDX == element.Data.IDX) ? true : false;
            }

            if (selected == true)
            {
                CurrentSelectedElement = element;
                CurrentSelectedIDX = CurrentSelectedElement.Data.IDX;
            }

            element.SetSelectedEffectObject(selected);
        }

        public override void OnSelectEvent<T1>(T1 value = default(T1))
        {
            if (CurrentSelectedElement != null)
            {
                CurrentSelectedElement.SetSelectedEffectObject(false);
            }

            CurrentSelectedElement = value as CommandScrollElement;
            CurrentSelectedIDX = CurrentSelectedElement.Data.IDX;

            if (CurrentSelectedElement != null)
            {
                CurrentSelectedElement.SetSelectedEffectObject(true);
            }
        }

        public override void OnDoubleClickEvent<T1>(T1 value = default(T1))
        {
            CommandScrollElement CommandScrollElement = value as CommandScrollElement;
            if (CurrentSelectedElement == CommandScrollElement)
            {
                if (OnDoubleClickAction != null)
                {
                    OnDoubleClickAction(CommandScrollElement);
                }
            }
        }

        public override void OnDestroyModule()
        {
            base.OnDestroyModule();
        }
    }
}