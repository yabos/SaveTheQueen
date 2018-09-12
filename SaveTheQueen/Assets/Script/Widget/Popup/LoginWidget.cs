using Lib.uGui;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{
    public class LoginWidget : WidgetBase
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
        private void OnCancle()
        {

        }

        public void OnStart()
        {
            Global.SceneMgr.Transition<InGameScene>("Game",  0.5f, 0.3f, (code) =>
            {
                Global.SceneMgr.LogWarning(StringUtil.Format("Scene Transition -> {0}", "Game"));
            });
        }

    }
}