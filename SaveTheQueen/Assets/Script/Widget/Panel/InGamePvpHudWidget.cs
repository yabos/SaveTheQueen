using UnityEngine;
using UnityEngine.UI;
using Lib.uGui;
using Lib.Event;
using System;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{
    public class InGamePvpHudWidget : WidgetBase
    {
        #region "WidgetBase"

        public override bool IsFlow
        {
            get { return false; }
        }

        public override void BhvOnEnter()
        {
        }

        public override void BhvOnLeave()
        {
        }

        protected override void ShowWidget(IUIDataParams data)
        {
        }

        protected override void HideWidget()
        {
        }

        public override void FinalizeWidget()
        {
            base.FinalizeWidget();
        }

        public override void OnNotify(Lib.Event.INotify notify)
        {
        }

        #endregion "WidgetBase"
    }
}
