using Aniz.Widget.Panel;

namespace Lib.uGui
{
    public class MessageBoxDataParam : IUIDataParams, System.IDisposable
    {
        private string m_title = string.Empty;

        public string TitleText
        {
            get { return m_title; }
        }

        private string m_message = string.Empty;

        public string MessageText
        {
            get { return m_message; }
        }

        private eMessageBoxType m_messageBoxType = eMessageBoxType.OK;

        public eMessageBoxType MessageBoxType
        {
            get { return m_messageBoxType; }
        }

        private System.Action<bool> m_completed = null;

        public System.Action<bool> CompletedAction
        {
            get { return m_completed; }
        }

        private float m_activeTime = 0.0f;

        public float ActiveTime
        {
            get { return m_activeTime; }
        }

        public MessageBoxDataParam(string title, string message) : this(title, message, eMessageBoxType.OK)
        {
        }

        public MessageBoxDataParam(string title, string message, eMessageBoxType messageBoxType) : this(title, message,
            messageBoxType, null, 0.0f)
        {
        }

        public MessageBoxDataParam(string title, string message, eMessageBoxType messageBoxType,
            System.Action<bool> completed, float activeTime)
        {
            m_title = title;
            m_message = message;
            m_messageBoxType = messageBoxType;
            m_completed = completed;
            m_activeTime = activeTime;
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}