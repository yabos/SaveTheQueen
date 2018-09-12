using Lib.InputButton;
using System.Collections.Generic;
using UnityEngine;

namespace Aniz.InputButton.Controller
{
    public enum eMoveDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
    }

    public abstract class ButtonController
    {
        protected uint m_input = 0;
        protected List<ButtonEvent> m_controlEventList = new List<ButtonEvent>();

        private int m_lastInputActionTimestamp;
        private uint m_lastInput;
        private int m_previousFrame = 0;
        protected Vector2 m_axisInput = Vector2.zero;

        protected uint m_moveDirection;
        protected byte m_moveDir;

        public long playerNetID { get; protected set; }

        public ButtonController()
        {
            for (int i = 0; i < (int)ePlayerButtonEnum.kMax; ++i)
            {
                ButtonEvent inputEvent = new ButtonEvent((ePlayerButtonEnum)i);
                m_controlEventList.Add(inputEvent);
            }
        }

        public uint GetCharacterInput()
        {
            return m_input & (uint)ePlayerButtonMasks.CharacterControls;
        }

        public bool IsMovementControls()
        {
            return (m_input & (uint)ePlayerButtonMasks.MovementControls) > 0;
        }

        public void SetInput(uint input)
        {
            m_input = input;
        }

        public void SetAxisInput(Vector2 axis)
        {
            m_axisInput = axis;
        }

        public virtual void OnFixedUpdate(float deltaTime)
        {
            UpdateButtonState();
        }

        public void UpdateButtonState()
        {
            for (int i = 0; i < m_controlEventList.Count; ++i)
            {
                ButtonEvent inputEvent = m_controlEventList[i];

                bool curButtonDown = (m_input & (uint)inputEvent.Button) != 0;

                if (curButtonDown)
                {
                    if (inputEvent.prevButtonDown)
                    {
                        inputEvent.State = ButtonEvent.eState.Hold;
                        inputEvent.holdFrames = TimeManager.frameCount - inputEvent.pressedFrame;
                    }
                    else
                    {
                        inputEvent.State = ButtonEvent.eState.Pressed;
                        inputEvent.pressedFrame = TimeManager.frameCount;
                    }
                }
                else
                {
                    inputEvent.State = inputEvent.prevButtonDown ? ButtonEvent.eState.Released : ButtonEvent.eState.Up;
                    inputEvent.holdFrames = 0;
                }
                inputEvent.prevButtonDown = curButtonDown;
            }
        }

        public bool GetButtonDown(ePlayerButtonEnum pbe)
        {
            ButtonEvent inputEvent = m_controlEventList[(int)pbe];
            return inputEvent.State == ButtonEvent.eState.Pressed;
        }

        public bool GetButtonHold(ePlayerButtonEnum pbe)
        {
            ButtonEvent inputEvent = m_controlEventList[(int)pbe];
            return inputEvent.State == ButtonEvent.eState.Hold;
        }

        public bool AnyButtonDown()
        {
            for (int i = 0; i < m_controlEventList.Count; ++i)
            {
                ButtonEvent inputEvent = m_controlEventList[i];
                if (inputEvent.State == ButtonEvent.eState.Pressed)
                    return true;
            }

            return false;
        }

        //protected void SetDirection(eMoveDirection eType, bool down)
        //{
        //    if (down)
        //    {
        //        m_moveDirection = ((uint)eType | m_moveDirection);
        //    }
        //    else
        //    {
        //        m_moveDirection = (~(uint)eType & m_moveDirection);
        //    }
        //    m_moveDir = LEMath.ToByteDir(m_axisInput);
        //}

        /*
        private void UpdateInput()
        {
            if (TimeManager.Paused == false)
            {
                uint input = GetCharacterInput();
                if ((TimeManager.frameCount - m_lastInputActionTimestamp) > 1
                    && (input & InputSystem.ATTACK_BUTTONS) != 0)
                {
                    m_lastInputActionTimestamp = TimeManager.frameCount;
                    m_lastInput = input;
                }
                else if (input != m_lastInput)
                {
                    m_lastInputActionTimestamp = 0;
                }
            }
        }
        */

        public static bool IsButton(uint input, ePlayerButton ePlayerButton)
        {
            return ((ePlayerButton)input & ePlayerButton) == ePlayerButton;
        }
    }
}