using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lib.Event;
using Lib.Pattern;
using Lib.uGui;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{

    public class SplashWidget : WidgetBase
    {
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

        public override void OnNotify(Lib.Event.INotify notify)
        {
        }

        public void OnNextPageButtonClk()
        {
            //Global.NotificationMgr.NotifyToEventHandler("OnNotify", eNotifyHandler.Page, new PageEventNotify(eMessage.PageTransition));
        }
    }


}