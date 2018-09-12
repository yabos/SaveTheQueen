using UnityEngine.UI;

using Lib.uGui;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{
    public class LoadingWidget : WidgetBase
    {
        private Text m_loadingText = null;
        //private Text m_percentText = null;

        //private Image m_progressBar = null;

        protected bool m_isLoadingImage = false;

        public override bool IsFlow
        {
            get { return false; }
        }

        public override void BhvOnEnter()
        {
            m_loadingText = transform.FindChildComponent<Text>("Canvas/Panel/LoadingProgress/Loading/Text");

            //m_percentText = transform.FindChildComponent<Text>("Canvas/Panel/Progress/Loading_Num");
            //m_progressBar = transform.FindChildComponent<Image>("Canvas/Panel/LoadingProgress/Progress/Progress_Bar");
        }

        public override void BhvOnLeave()
        {

        }


        protected override void ShowWidget(IUIDataParams data)
        {
            m_loadingText.text = "Loading";
        }

        protected override void HideWidget()
        {
        }

        public override void OnNotify(Lib.Event.INotify notify)
        {
        }

        public void SetLoadingPanelInfo(string currentPageName, string nextPageName)
        {
            if (string.IsNullOrEmpty(currentPageName))
            {

            }

            if (string.IsNullOrEmpty(nextPageName))
            {

            }

            {

            }
        }

        public void SetLoadingProgressInfo(float progress)
        {
            if (m_loadingText == null)
            {
                return;

            }

            {
                int percent = (int)(progress * 100.0f);

                if (percent >= 100)
                {
                    m_loadingText.text = "Loading Completed.";
                    //m_percentText.text = string.Format("{0}%", percent);
                    //m_progressBar.fillAmount = 1;
                }
                else
                {
                    //m_percentText.text = string.Format("{0}%", percent);
                    //m_progressBar.fillAmount = progress;
                }
            }
        }
    }

}