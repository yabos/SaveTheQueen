using UnityEngine;
using System.Collections;
using Lib.Pattern;
using Lib.uGui;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{

    public enum eMessageBoxType : int
    {
        OK = 0,
        OKAndCancel
    }

    public class MessageBoxWidget : UIModuleWidgetBase
    {
        public UnityEngine.UI.Text Title;
        public UnityEngine.UI.Text Message;

        public eMessageBoxType MessageBoxType;

        protected System.Action<bool> m_completed = null;

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
            MessageBoxDataParam messageData = data as MessageBoxDataParam;

            {
                MessageBoxType = messageData.MessageBoxType;

                // ok button
                UIModuleButton okButton = GetButton(0);
                if (okButton != null)
                {
                    okButton.SetActive(true);
                    okButton.Set(OnOkButtonClick);
                }

                // cancel button
                UIModuleButton cancelButton = GetButton(1);
                if (cancelButton != null)
                {
                    cancelButton.SetActive(MessageBoxType == eMessageBoxType.OKAndCancel ? true : false);
                    cancelButton.Set(OnCancelButtonClick);
                }

                // close button
                SetButton(2, OnCancelButtonClick);
            }

            SetText(Title, messageData.TitleText);
            SetText(Message, messageData.MessageText);

            m_completed = messageData.CompletedAction;
        }

        protected override void HideWidget()
        {
        }

        void OnOkButtonClick()
        {
            Hide(0.2f);

            if (m_completed != null)
            {
                m_completed(true);
            }
        }

        void OnCancelButtonClick()
        {
            Hide(0.2f);

            if (m_completed != null)
            {
                m_completed(false);
            }
        }

        void SetText(UnityEngine.UI.Text uiText, string text)
        {
            if (uiText == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                uiText.text = string.Empty;
                return;
            }

            uiText.text = text;
        }

        public override void OnNotify(Lib.Event.INotify notify)
        {
        }
    }

}