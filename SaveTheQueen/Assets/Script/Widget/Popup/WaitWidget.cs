using Lib.uGui;
using Aniz.Widget;
using UIExtension;
using UnityEngine.UI;

namespace Aniz.Widget.Panel
{
    public class WaitWidget : WidgetBase
    {
        private Button m_cancleButton;

        public override bool IsFlow
        {
            get { return false; }
        }

        public override void BhvOnEnter()
        {
            m_cancleButton = transform.FindChildComponent<Button>("Canvas/Background/Button");
            m_cancleButton.AddOnClick(OnCancle);
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
        private void OnCancle()
        {

        }

    }
}