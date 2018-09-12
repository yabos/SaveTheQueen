using UnityEngine;
using System.Collections.Generic;
using System;
using UInput = UnityEngine.Input;

namespace Lib.InputButton
{
    public class KeyboardInput : IInput
    {
        private KeyboardInputOptions controlOptions = new KeyboardInputOptions();
        private Vector2 axisInput = Vector2.zero;
        private uint stuckButtonInputFlags = 0;
        private uint prevButtonInputFlags = 0;
        private uint buttonInputFlags = 0;
        private int controllerIndex = 0;
        private uint buttonDoubleClickFlag = 0;
        private bool m_keyboardInput = true;

        public void SetInputIndex(int index)
        {
            controllerIndex = index + 1;
            if (index == 0)
            {
                controlOptions.Reset(controllerIndex);
            }
            else
            {
                controlOptions.ResetAlt(controllerIndex);
            }
        }

        public Vector2 GetAxisInput()
        {
            return axisInput;
        }

        public bool IsPressAxis
        {
            get { return axisInput != Vector2.zero; }
        }

        public uint GetInput()
        {
            return buttonInputFlags;
        }

        public uint GetInputDown()
        {
            //return buttonInputFlags;
            // leading edges
            uint buttonFlags = buttonInputFlags | stuckButtonInputFlags;
            return (buttonFlags ^ prevButtonInputFlags) & buttonFlags;
        }

        public uint GetInputUp()
        {
            //return buttonInputFlags;
            // trailing edges
            uint buttonFlags = buttonInputFlags | stuckButtonInputFlags;
            return (buttonFlags ^ prevButtonInputFlags) & prevButtonInputFlags;
        }

        public bool GetButton(ePlayerButton button)
        {
            return (GetInput() & (uint)button) != 0;
        }

        public bool GetButtonDown(ePlayerButton button)
        {
            return (GetInputDown() & (uint)button) != 0;
        }

        public bool GetButtonUp(ePlayerButton button)
        {
            return (GetInputUp() & (uint)button) != 0;
        }

        public bool GetButtonDoubleClick(ePlayerButton button)
        {
            return buttonDoubleClickFlag == (uint)button;
        }

        public void SetButtonDoubleClick(ePlayerButton button)
        {
            buttonDoubleClickFlag = (uint)button;
        }

        public void ResetButtonDoubleClick()
        {
            buttonDoubleClickFlag = 0;
        }

        public void SetAxisInput(Vector2 axis)
        {
            axisInput = axis;
        }

        public void SetButtonDown(ePlayerButton button)
        {
            stuckButtonInputFlags |= (uint)button;
        }

        public void SetButtonUp(ePlayerButton button)
        {
            stuckButtonInputFlags -= (stuckButtonInputFlags & (uint)button);
        }

        public void ClearInput()
        {
            axisInput = Vector2.zero;
            prevButtonInputFlags = buttonInputFlags = stuckButtonInputFlags = 0;
            UInput.ResetInputAxes();
        }

        public void OnUpdate()
        {
            prevButtonInputFlags = buttonInputFlags;
            buttonInputFlags = stuckButtonInputFlags;

            List<KeyboardInputOptions.ButtonInput> keyboardButtonOptions = controlOptions.GetButtonInputList();

            for (int i = 0; i < keyboardButtonOptions.Count; ++i)
            {
                KeyboardInputOptions.ButtonInput buttonInput = keyboardButtonOptions[i];
                //#if UNITY_IOS
                //if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (buttonInput.iOSJoystickKeyCode != KeyCode.None && UInput.GetKey(buttonInput.iOSJoystickKeyCode))
                    {
                        buttonInputFlags |= buttonInput.button;
                        if (m_keyboardInput)
                        {
                            m_keyboardInput = false;
                        }
                    }
                }
                //#endif
                if (UInput.GetKey(buttonInput.keyCode))
                {
                    buttonInputFlags |= buttonInput.button;
                    if (m_keyboardInput == false)
                    {
                        m_keyboardInput = true;
                    }
                }
            }

            axisInput = Vector2.zero;

            if ((buttonInputFlags & (uint)ePlayerButton.Left) == (uint)ePlayerButton.Left)
            {
                axisInput.x = -1.0f;
            }
            else if ((buttonInputFlags & (uint)ePlayerButton.Right) == (uint)ePlayerButton.Right)
            {
                axisInput.x = 1.0f;
            }

            if ((buttonInputFlags & (uint)ePlayerButton.Up) == (uint)ePlayerButton.Up)
            {
                axisInput.y = 1.0f;
            }
            else if ((buttonInputFlags & (uint)ePlayerButton.Down) == (uint)ePlayerButton.Down)
            {
                axisInput.y = -1.0f;
            }
            //#if UNITY_IOS
            //if (Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    List<KeyboardInputOptions.ButtonAxisInput> axisOptions = controlOptions.GetButtonAxisInputList();
            //    for (int i = 0; i < axisOptions.Count; i++)
            //    {
            //        KeyboardInputOptions.ButtonAxisInput buttonAxisInput = axisOptions[i];
            //        float axisValue = UInput.GetAxis(buttonAxisInput.axisName);
            //        if (axisValue < -0.25f)
            //            buttonInputFlags |= buttonAxisInput.negativeButton;
            //        else if (axisValue > 0.25f)
            //            buttonInputFlags |= buttonAxisInput.positiveButton;

            //        if (buttonAxisInput.axisIndex == 1)
            //            axisInput.x = axisValue;
            //        else if (buttonAxisInput.axisIndex == 2)
            //            axisInput.y = axisValue;
            //    }
            //}
            //#endif

            //if (prevButtonInputFlags != buttonInputFlags)
            //{
            //    Debug.Log("buttons = " + (buttonInputFlags & (uint)PlayerButtonMasks.CharacterControls) + " - " + Time.frameCount);
            //}
        }

    }

}