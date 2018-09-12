using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lib.uGui;

namespace Aniz.Widget.Module
{

    [System.Serializable]
    public class CommandData
    {
        public int IDX;
        public string Message;

        public CommandData(int idx, string message)
        {
            IDX = idx;
            Message = message;
        }
    }

    [System.Serializable]
    public class CommandGridScrollView : UIModuleGridScrollView<CommandData>
    {
        protected CommandScrollElement CurrentSelectedElement = null;

        protected int CurrentSelectedIDX = -1;

        public void SetInfo(List<CommandData> list)
        {
            base.OnEnterModule();

            CurrentSelectedElement = null;
            CurrentSelectedIDX = -1;

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

        public override void OnDestroyModule()
        {
            base.OnDestroyModule();
        }
    }
}