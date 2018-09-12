using Lib.InputButton;

namespace Lib.InputButton
{
    public class ButtonEvent
    {
        private readonly ePlayerButtonEnum m_buttonEnum;
        private readonly ePlayerButton m_button;

        public enum eState
        {
            Pressed,
            Hold,
            Released,
            Up
        }

        public bool prevButtonDown { get; set; }
        public int pressedFrame { get; set; }

        public eState State = eState.Up;
        public int holdFrames { get; set; }

        public ePlayerButtonEnum ButtonEnum
        {
            get { return m_buttonEnum; }
        }

        public ePlayerButton Button
        {
            get { return m_button; }
        }

        public ButtonEvent(ePlayerButtonEnum btn)
        {
            m_buttonEnum = btn;
            m_button = (ePlayerButton)InputSystem.GetPlayerButton((ePlayerButtonEnum)btn);
        }
    }
}