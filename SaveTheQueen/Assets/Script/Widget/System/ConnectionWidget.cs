using Lib.uGui;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{
    public class ConnectionWidget : WidgetBase
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

    }
}