using Lib.InputButton;
using UnityEngine;

namespace Lib.InputButton
{
    public class MouseEventTranslator
    {
        private MouseEvent m_previousEvent;

        public MouseEvent MouseMove(MouseEvent.E_Buttons e, Vector2 currentpos)
        {
            MouseEvent.MouseInfo mouseInfo = new MouseEvent.MouseInfo(false, false, currentpos);
            InheritButtonStatesFromPreviousMouseEvent(ref mouseInfo);

            MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.Move, mouseInfo,
                m_previousEvent.Info.position);
            m_previousEvent = mouseEvent;
            return mouseEvent;
        }

        public MouseEvent MouseDown(MouseEvent.E_Buttons e, Vector2 currentpos)
        {
            MouseEvent.MouseInfo mouseInfo = new MouseEvent.MouseInfo(false, false, currentpos);
            InheritButtonStatesFromPreviousMouseEvent(ref mouseInfo);

            if (e == MouseEvent.E_Buttons.Left)
            {
                mouseInfo.leftButton = true;
                MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.LDown, mouseInfo, mouseInfo.position);
                m_previousEvent = mouseEvent;
                return mouseEvent;
            }
            else if (e == MouseEvent.E_Buttons.Middle)
            {
                mouseInfo.middleButton = true;
                MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.MDown, mouseInfo, mouseInfo.position);
                m_previousEvent = mouseEvent;
                return mouseEvent;
            }
            else if (e == MouseEvent.E_Buttons.Right)
            {
                mouseInfo.rightButton = true;
                MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.RDown, mouseInfo, mouseInfo.position);
                m_previousEvent = mouseEvent;
                return mouseEvent;
            }

            return MouseEvent.None;
        }

        private void InheritButtonStatesFromPreviousMouseEvent(ref MouseEvent.MouseInfo mouseInfo)
        {
            mouseInfo.leftButton = m_previousEvent.Info.leftButton;
            mouseInfo.middleButton = m_previousEvent.Info.middleButton;
            mouseInfo.rightButton = m_previousEvent.Info.rightButton;
        }

        public MouseEvent MouseUp(MouseEvent.E_Buttons e, Vector2 currentpos)
        {
            MouseEvent.MouseInfo mouseInfo = new MouseEvent.MouseInfo(false, false, currentpos);
            InheritButtonStatesFromPreviousMouseEvent(ref mouseInfo);

            if (e == MouseEvent.E_Buttons.Left)
            {
                mouseInfo.leftButton = false;
                MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.LUp, mouseInfo,
                    m_previousEvent.Info.position);
                m_previousEvent = mouseEvent;
                return mouseEvent;
            }
            else if (e == MouseEvent.E_Buttons.Middle)
            {
                mouseInfo.middleButton = false;
                MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.MUp, mouseInfo,
                    m_previousEvent.Info.position);
                m_previousEvent = mouseEvent;
                return mouseEvent;
            }
            else if (e == MouseEvent.E_Buttons.Right)
            {
                mouseInfo.rightButton = false;
                MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.RUp, mouseInfo,
                    m_previousEvent.Info.position);
                m_previousEvent = mouseEvent;
                return mouseEvent;
            }

            return MouseEvent.None;
        }

        public MouseEvent MouseWheel(Vector2 currentpos, float delta)
        {
            MouseEvent.MouseInfo mouseInfo = new MouseEvent.MouseInfo(false, false, false, currentpos, delta);
            InheritButtonStatesFromPreviousMouseEvent(ref mouseInfo);
            MouseEvent mouseEvent = new MouseEvent(MouseEvent.E_EventState.Wheel, mouseInfo);
            m_previousEvent = mouseEvent;

            return mouseEvent;
        }
    }

}