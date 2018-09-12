using UnityEngine;

namespace Lib.InputButton
{
    public struct MouseEvent
    {
        public enum E_Buttons
        {
            None = 0,
            Left,
            Right,
            Middle,
        }

        public enum E_EventState
        {
            Move = 0,
            LDown,
            LUp,
            MDown,
            MUp,
            RDown,
            RUp,
            Wheel,
        }

        public struct MouseInfo
        {
            public bool leftButton;
            public bool rightButton;
            public bool middleButton;
            public Vector2 position;
            public float delta;

            public MouseInfo(bool leftButton, bool rightButton, Vector2 position)
                : this(leftButton, false, rightButton, position, 0)
            {
            }

            public MouseInfo(bool leftButton, bool middleButton, bool rightButton, Vector2 position)
                : this(leftButton, middleButton, rightButton, position, 0)
            {
            }

            public MouseInfo(bool leftButton, bool middleButton, bool rightButton, Vector2 position, float delta)
            {
                this.leftButton = leftButton;
                this.middleButton = middleButton;
                this.rightButton = rightButton;
                this.position = position;
                this.delta = delta;
            }

            public static bool operator ==(MouseInfo lhs, MouseInfo rhs)
            {
                if ((lhs.leftButton == rhs.leftButton) &&
                    (lhs.middleButton == rhs.middleButton) &&
                    (lhs.delta == rhs.delta) &&
                    (lhs.position == rhs.position) &&
                    (lhs.rightButton == rhs.rightButton))
                {
                    return true;
                }
                return false;
            }

            public static bool operator !=(MouseInfo lhs, MouseInfo rhs)
            {
                return !(lhs == rhs);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is MouseInfo))
                    return false;

                return this == (MouseInfo)obj;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private MouseInfo m_curInfo;
        private Vector2 m_previousPosition;
        private E_EventState m_state;

        public MouseEvent(E_EventState eEventState, MouseInfo mouseInfo, Vector2 previousPosition)
        {
            m_curInfo = mouseInfo;
            m_state = eEventState;
            m_previousPosition = previousPosition;
        }

        public MouseEvent(E_EventState eEventState, MouseInfo mouseInfo)
            : this(eEventState, mouseInfo, new Vector2(0, 0))
        {
        }

        public static MouseEvent None = new MouseEvent(E_EventState.Move, new MouseInfo(false, false, new Vector2(0, 0)),
            new Vector2(0, 0));

        //public MouseEvent()
        //	: this()
        //{
        //}

        public MouseInfo Info
        {
            get { return m_curInfo; }
            //set
            //{
            //	m_previousPosition = m_curInfo.position;
            //	m_curInfo = value;
            //}
        }

        public E_EventState State
        {
            get { return m_state; }
            //set { m_state = value; }
        }

        public Vector2 PreviousPosition
        {
            get { return m_previousPosition; }
        }

        public static bool operator ==(MouseEvent lhs, MouseEvent rhs)
        {
            if ((lhs.m_previousPosition == rhs.m_previousPosition) &&
                (lhs.m_state == rhs.m_state) &&
                (lhs.m_curInfo == rhs.m_curInfo))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(MouseEvent lhs, MouseEvent rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseEvent))
                return false;

            return this == (MouseEvent)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}