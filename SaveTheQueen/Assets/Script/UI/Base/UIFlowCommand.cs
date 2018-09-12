using Lib.Pattern;
using Aniz.Widget;

namespace Lib.uGui
{
    public class UIFlowCommand : ICommand
    {
        private IUIDataParams m_data;
        private float m_activeTime = 0.0f;
        private readonly WidgetBase m_prevWidget;
        private readonly WidgetBase m_currentWidget;

        private bool m_undo = false;

        public string Name
        {
            get { return "UIFlow"; }
        }

        public string CurrentWndName
        {
            get
            {
                if (m_undo && m_prevWidget != null)
                {
                    return m_prevWidget.WidgetName;
                }

                return m_currentWidget.WidgetName;
            }
        }

        public UIFlowCommand(WidgetBase prev, WidgetBase current, float activeTime, IUIDataParams data)
        {
            m_prevWidget = prev;
            m_data = data;
            m_activeTime = activeTime;
            m_currentWidget = current;
        }

        public ICommand Clone()
        {
            UIFlowCommand uiFlowCommand = new UIFlowCommand(m_prevWidget, m_currentWidget, m_activeTime, m_data);
            return uiFlowCommand;
        }

        public bool Execute()
        {
            if (m_prevWidget != null)
                m_prevWidget.Hide();

            //m_currentWidget.Show(0.0f, m_data);
            m_undo = false;

            return true;
        }

        public void Redo()
        {
            if (m_prevWidget != null)
                m_prevWidget.Hide();

            m_currentWidget.Show(m_activeTime, m_data, true);
            m_undo = false;
        }

        public void Undo()
        {
            if (m_prevWidget != null)
                m_prevWidget.Show(m_activeTime, m_data, true);

            m_currentWidget.Hide();

            m_undo = true;
        }
    }
}