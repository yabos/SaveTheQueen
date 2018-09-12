using System.Collections.Generic;
using System;
using Lib.InputButton;
using UnityEngine;
using UInput = UnityEngine.Input;

namespace Lib.InputButton
{
    public class JoystickInput : IInput
    {

        public enum E_JoystickType
        {
            XBOX,
            ONLIVE, // Windows(Onlive)
            BROADCOM, // Mobile(Onlive)
            GV155, // Mobile(Onlive)
            GV156, // Mobile(Onlive)
            NOT_SUPPORTED
        }

        // axes
        const int AXIS_LEFT_ANALOG_X = 0;
        const int AXIS_LEFT_ANALOG_Y = 1;
        const int AXIS_TRIGGER = 2;
        const int AXIS_RIGHT_ANALOG_X = 3;
        const int AXIS_RIGHT_ANALOG_Y = 4;
        const int AXIS_DPAD_X = 5;
        const int AXIS_DPAD_Y = 6;
        const int AXIS_LEFT_TRIGGER = 7;
        const int AXIS_RIGHT_TRIGGER = 8;

        const int AXIS_DPAD_LEFT = 5;
        const int AXIS_DPAD_RIGHT = 6;
        const int AXIS_DPAD_UP = 7;
        const int AXIS_DPAD_DOWN = 8;

        // buttons
        const int BUTTON_X = 9;
        const int BUTTON_Y = 10;
        const int BUTTON_A = 11;
        const int BUTTON_B = 12;

        const int BUTTON_LEFT_BUMPER = 13;
        const int BUTTON_RIGHT_BUMPER = 14;
        const int BUTTON_SELECT = 15;
        const int BUTTON_START = 16;
        const int BUTTON_LEFT_ANALOG = 17;
        const int BUTTON_RIGHT_ANALOG = 18;

        const int BUTTON_TL = 13;
        const int BUTTON_DL = 14;
        const int BUTTON_TR = 15;
        const int BUTTON_DR = 16;

        const int BUTTON_SELECT2 = 17;
        const int BUTTON_START2 = 18;

        // XBox Controller Layout
        //
        // 9A                       10A
        // 4B                       5B
        //
        //            6B    7B
        //
        //   2A                    3B
        // 1A  1A                2B  1B
        //   2A                    0B
        //
        //        7A             5A
        //      6A  6A         4A  4A
        //        7A             5A
        // Axis
        private static int[] XBOX_LAYOUT_INDEX = new int[]
        {
            // axes
            1, // AXIS_LEFT_ANALOG_X,
            2, // AXIS_LEFT_ANALOG_Y,
            3, // AXIS_LEFT/RIGHT TRIGGER,
            4, // AXIS_RIGHT_ANALOG_X,
            5, // AXIS_RIGHT_ANALOG_Y,
            6, // AXIS_DPAD_X,
            7, // AXIS_DPAD_Y,
            9, // AXIS_LEFT_TRIGGER,
            10, // AXIS_RIGHT_TRIGGER,

            // buttons
            2, // BUTTON_X
            3, // BUTTON_Y
            0, // BUTTON_A
            1, // BUTTON_B
            4, // BUTTON_LEFT_BUMPER
            5, // BUTTON_RIGHT_BUMPER
            6, // BUTTON_SELECT
            7, // BUTTON_START
            8, // BUTTON_LEFT_ANALOG
            9, // BUTTON_RIGHT_ANALOG
        };

        // OnLive Controller Layout
        //
        // 7A                       8A
        // 12B                      11B
        //
        //            8B    7B
        //
        //   6A                    13B
        // 5A  5A               14B  15B
        //   6A                    16B
        //
        //        2A             4A
        //      1A  1A         3A  3A
        //        2A             4A
        // Axis
        private static int[] ONLIVE_LAYOUT_INDEX = new int[]
        {
            // axes
            1, // AXIS_LEFT_ANALOG_X,
            2, // AXIS_LEFT_ANALOG_Y,
            -1, // AXIS_LEFT/RIGHT TRIGGER,
            3, // AXIS_RIGHT_ANALOG_X,
            4, // AXIS_RIGHT_ANALOG_Y,
            5, // AXIS_DPAD_X,
            6, // AXIS_DPAD_Y,
            7, // AXIS_LEFT_TRIGGER,
            8, // AXIS_RIGHT_TRIGGER,

            // buttons
            14, // BUTTON_X             3
            13, // BUTTON_Y             4
            16, // BUTTON_A             0
            15, // BUTTON_B             1
            12, // BUTTON_LEFT_BUMPER   4
            11, // BUTTON_RIGHT_BUMPER  5
            8, // BUTTON_SELECT         11
            7, // BUTTON_START          10
            10, // BUTTON_LEFT_ANALOG   8
            9, // BUTTON_RIGHT_ANALOG   9
        };

        private static int[] BROADCOM_LAYOUT_INDEX = new int[]
        {
            // axes
            1, // AXIS_LEFT_ANALOG_X,
            2, // AXIS_LEFT_ANALOG_Y,
            -1, // AXIS_LEFT/RIGHT TRIGGER,
            3, // AXIS_RIGHT_ANALOG_X,
            4, // AXIS_RIGHT_ANALOG_Y,
            5, // AXIS_DPAD_X,
            6, // AXIS_DPAD_Y,
            7, // AXIS_LEFT_TRIGGER,
            8, // AXIS_RIGHT_TRIGGER,

            // buttons
            2, // BUTTON_X
            3, // BUTTON_Y
            0, // BUTTON_A
            1, // BUTTON_B
            4, // BUTTON_LEFT_BUMPER
            5, // BUTTON_RIGHT_BUMPER
            11, // BUTTON_SELECT
            10, // BUTTON_START
            8, // BUTTON_LEFT_ANALOG
            9, // BUTTON_RIGHT_ANALOG
        };

        private static int[] USB_LAYOUT_INDEX = new int[]
        {
            // axes
            1, // AXIS_LEFT_ANALOG_X,
            2, // AXIS_LEFT_ANALOG_Y,
            -1, // AXIS_LEFT/RIGHT TRIGGER,
            4, // AXIS_RIGHT_ANALOG_X,
            3, // AXIS_RIGHT_ANALOG_Y,

            5, // AXIS_DPAD_X,
            6, // AXIS_DPAD_Y,
            7, // AXIS_LEFT_TRIGGER,
            8, // AXIS_RIGHT_TRIGGER,

            // buttons
            1, // BUTTON_X Fire3
            3, // BUTTON_Y Fire2
            2, // BUTTON_A Jump
            0, // BUTTON_B Fire1
            4, // BUTTON_TL
            5, // BUTTON_DL
            6, // BUTTON_TR
            7, // BUTTON_DR
            //8, // BUTTON_LEFT_ANALOG
            //9, // BUTTON_RIGHT_ANALOG
        };

        private static int[] GV_LAYOUT_INDEX = new int[]
        {
            // axes
            1, // AXIS_LEFT_ANALOG_X,
            2, // AXIS_LEFT_ANALOG_Y,
            -1, // AXIS_LEFT/RIGHT TRIGGER,
            3, // AXIS_RIGHT_ANALOG_X,
            4, // AXIS_RIGHT_ANALOG_Y,

            7, // AXIS_DPAD_L,
            5, // AXIS_DPAD_R,
            4, // AXIS_DPAD_U,
            6, // AXIS_DPAD_D,

            // buttons
            15, // BUTTON_X Fire3
            12, // BUTTON_Y Fire2
            14, // BUTTON_A Jump
            13, // BUTTON_B Fire1

            8, // BUTTON_TL
            9, // BUTTON_DL
            10, // BUTTON_TR
            11, // BUTTON_DR
        };

        public class ButtonAxisInput
        {
            public uint negativeButton;
            public uint positiveButton;
            public string axisName;
        }

        public class ButtonInput
        {
            public uint button;
            public KeyCode keyCode;
        }

        private List<ButtonAxisInput> buttonAxisInputList = new List<ButtonAxisInput>();
        private List<ButtonInput> buttonInputList = new List<ButtonInput>();
        private string leftAnalogX = string.Empty;
        private string leftAnalogY = string.Empty;
        private string rightAnalogX = string.Empty;
        private string rightAnalogY = string.Empty;
        private bool axis1Inverse = true;
        private bool axis2Inverse = true;

        private int rightStickOverrideCount = 0;

        public const float deadZoneMagnitude = 0.25f;
        private const float dashMagnitude = 0.707f;

        private int controllerIndex;

        private static string GetAxisName(int joystickIndex, int axisNum)
        {
            return string.Format("Joy{0} Axis {1}", joystickIndex, axisNum);
        }

        public void SetInputIndex(int index)
        {
            controllerIndex = index + 1;
        }

        private E_JoystickType GetJoystickType(string joystickName)
        {
            if (joystickName.IndexOf("XBOX", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return E_JoystickType.XBOX;
            }
            else if (joystickName.IndexOf("OnLive", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return E_JoystickType.ONLIVE;
            }
            else if (joystickName.IndexOf("Broadcom", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return E_JoystickType.BROADCOM;
            }
            else if (joystickName.IndexOf("GV155", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return E_JoystickType.GV155;
            }
            else if (joystickName.IndexOf("GV156", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return E_JoystickType.GV156;
            }

            Debug.LogWarning(joystickName + " is not supported");
            return E_JoystickType.NOT_SUPPORTED;
        }

        private int[] GetLayoutIndex(E_JoystickType eJoystickType)
        {
            if (eJoystickType == E_JoystickType.XBOX)
                return XBOX_LAYOUT_INDEX;
            else if (eJoystickType == E_JoystickType.ONLIVE)
                return ONLIVE_LAYOUT_INDEX;
            else if (eJoystickType == E_JoystickType.BROADCOM)
                return BROADCOM_LAYOUT_INDEX;
            else if (eJoystickType == E_JoystickType.GV155)
                return GV_LAYOUT_INDEX;
            else if (eJoystickType == E_JoystickType.GV156)
                return GV_LAYOUT_INDEX;
            return null;
        }

        public void SetJoystickType(string joystickName)
        {
            E_JoystickType eJoystickType = GetJoystickType(joystickName);
            int[] layoutIndex = GetLayoutIndex(eJoystickType);

            if (layoutIndex == null)
            {
                Debug.LogWarning(eJoystickType + " is not supported properly");
                layoutIndex = USB_LAYOUT_INDEX;
            }

            Debug.Log(eJoystickType);

            leftAnalogX = GetAxisName(controllerIndex, layoutIndex[AXIS_LEFT_ANALOG_X]);
            leftAnalogY = GetAxisName(controllerIndex, layoutIndex[AXIS_LEFT_ANALOG_Y]);

            rightAnalogX = GetAxisName(controllerIndex, layoutIndex[AXIS_RIGHT_ANALOG_X]);
            rightAnalogY = GetAxisName(controllerIndex, layoutIndex[AXIS_RIGHT_ANALOG_Y]);

            // Fire1 : Block
            // Fire2 : Heavy
            // Fire3 : Light

            //AddButtonInput(controllerIndex, layoutIndex[BUTTON_A], ePlayerButtonEnum.Jump);
            //AddButtonInput(controllerIndex, layoutIndex[BUTTON_B], ePlayerButtonEnum.Fire1);
            //AddButtonInput(controllerIndex, layoutIndex[BUTTON_X], ePlayerButtonEnum.Fire3);
            //AddButtonInput(controllerIndex, layoutIndex[BUTTON_Y], ePlayerButtonEnum.Fire2);


            AddButtonInput(controllerIndex, layoutIndex[BUTTON_A], ePlayerButtonEnum.CHAIN1);
            AddButtonInput(controllerIndex, layoutIndex[BUTTON_B], ePlayerButtonEnum.CHAIN2);
            AddButtonInput(controllerIndex, layoutIndex[BUTTON_X], ePlayerButtonEnum.FIRE3);
            AddButtonInput(controllerIndex, layoutIndex[BUTTON_Y], ePlayerButtonEnum.CHAIN3);

            //AddAxisButtonInput(controllerIndex, layoutIndex[AXIS_LEFT_TRIGGER], ePlayerButtonEnum.Ability1, ePlayerButtonEnum.Ability1);
            //AddAxisButtonInput(controllerIndex, layoutIndex[AXIS_RIGHT_TRIGGER], ePlayerButtonEnum.Ability4, ePlayerButtonEnum.Ability4);

            //AddButtonInput(controllerIndex, layoutIndex[BUTTON_LEFT_BUMPER], ePlayerButtonEnum.Ability2);
            //AddButtonInput(controllerIndex, layoutIndex[BUTTON_RIGHT_BUMPER], ePlayerButtonEnum.Ability3);

            if (eJoystickType == E_JoystickType.GV155 || eJoystickType == E_JoystickType.GV156)
            {
                AddButtonInput(controllerIndex, layoutIndex[AXIS_DPAD_LEFT], ePlayerButtonEnum.Left);
                AddButtonInput(controllerIndex, layoutIndex[AXIS_DPAD_RIGHT], ePlayerButtonEnum.Right);
                AddButtonInput(controllerIndex, layoutIndex[AXIS_DPAD_UP], ePlayerButtonEnum.Up);
                AddButtonInput(controllerIndex, layoutIndex[AXIS_DPAD_DOWN], ePlayerButtonEnum.Down);

                AddButtonInput(controllerIndex, layoutIndex[BUTTON_TL], ePlayerButtonEnum.ABILITY1);
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_DL], ePlayerButtonEnum.ABILITY1);

                // 아래 두 줄이 R1, R2를 활성화 시키는 게 아니라 L2, R2를 활성화 시킴. 디버깅은 해보지 않음.
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_TR], ePlayerButtonEnum.ABILITY1);
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_DR], ePlayerButtonEnum.ABILITY1);
            }
            else
            {
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_TL], ePlayerButtonEnum.Item1);
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_DL], ePlayerButtonEnum.Item2);
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_TR], ePlayerButtonEnum.Item3);
                AddButtonInput(controllerIndex, layoutIndex[BUTTON_DR], ePlayerButtonEnum.Item4);

                AddAxisButtonInput(controllerIndex, layoutIndex[AXIS_DPAD_X], ePlayerButtonEnum.Item1,
                    ePlayerButtonEnum.Item4);
                AddAxisButtonInput(controllerIndex, layoutIndex[AXIS_DPAD_Y], ePlayerButtonEnum.Item3,
                    ePlayerButtonEnum.Item2);
            }

            // map "Pause" to Start button
            AddButtonInput(controllerIndex, layoutIndex[BUTTON_START], ePlayerButtonEnum.Pause);
            AddButtonInput(controllerIndex, layoutIndex[BUTTON_SELECT], ePlayerButtonEnum.Skip);

            // START
            // AddButtonInput(player, PlayerButtonEnum.Pause, KeyCode.JoystickButton5);

        }

        public void Reset()
        {
            prevButtonInputFlags = 0;
            buttonInputFlags = 0;
            leftStick = Vector2.zero;
            rightStick = Vector2.zero;
            rightStickOverrideCount = 0;
        }

        private void AddAxisButtonInput(int joystickIndex, int axisNum, ePlayerButtonEnum negativeButtonEnum,
            ePlayerButtonEnum positiveButtonEnum)
        {
            ButtonAxisInput buttonAxisInput = new ButtonAxisInput();
            buttonAxisInput.negativeButton = InputSystem.GetPlayerButton(negativeButtonEnum);
            buttonAxisInput.positiveButton = InputSystem.GetPlayerButton(positiveButtonEnum);
            buttonAxisInput.axisName = GetAxisName(joystickIndex, axisNum);
            buttonAxisInputList.Add(buttonAxisInput);
        }

        private const int KEY_CODE_OFFSET = KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0;

        private void AddButtonInput(int joystickIndex, int buttonNum, ePlayerButtonEnum buttonEnum)
        {
            ButtonInput buttonInput = new ButtonInput();
            buttonInput.button = InputSystem.GetPlayerButton(buttonEnum);
            buttonInput.keyCode = KeyCode.JoystickButton0 + joystickIndex * KEY_CODE_OFFSET + buttonNum;
            buttonInputList.Add(buttonInput);
        }

        private Vector2 leftStick = Vector2.zero;
        private Vector2 rightStick = Vector2.zero;
        private uint stuckButtonInputFlags = 0;
        private uint prevButtonInputFlags = 0;
        private uint buttonInputFlags = 0;
        private uint buttonDoubleClickFlag = 0;

        public bool IsPressAxis
        {
            get { return leftStick != Vector2.zero; }
        }

        public Vector2 GetAxisInput()
        {
            return leftStick;
        }

        public uint GetInput()
        {
            return buttonInputFlags;
        }

        public uint GetInputDown()
        {
            // leading edges
            uint buttonFlags = buttonInputFlags | stuckButtonInputFlags;
            return (buttonFlags ^ prevButtonInputFlags) & buttonFlags;
        }

        public uint GetInputUp()
        {
            // trailing edges
            uint buttonFlags = buttonInputFlags | stuckButtonInputFlags;
            return (buttonFlags ^ prevButtonInputFlags) & prevButtonInputFlags;
        }

        public uint GetFireButtonDown()
        {
            return buttonInputFlags | stuckButtonInputFlags;
        }

        public uint GetFireButtonUp()
        {
            return (buttonInputFlags | stuckButtonInputFlags) & prevButtonInputFlags;
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

        public void SetAxisInput(Vector2 axis)
        {
            leftStick = axis;
        }
        public void SetButtonDown(ePlayerButton button)
        {
            stuckButtonInputFlags |= (uint)button;
        }

        public void SetButtonUp(ePlayerButton button)
        {
            stuckButtonInputFlags -= (stuckButtonInputFlags & (uint)button);
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

        public void ClearInput()
        {
            leftStick = Vector2.zero;
            rightStick = Vector2.zero;
            prevButtonInputFlags = buttonInputFlags = stuckButtonInputFlags = 0;
            Input.ResetInputAxes();
            //       UIUtils.ClearInput();
        }

        public void OnUpdate()
        {
            leftStick.Set(axis1Inverse ? -Input.GetAxis(leftAnalogX) : Input.GetAxis(leftAnalogX),
                axis2Inverse ? -Input.GetAxis(leftAnalogY) : Input.GetAxis(leftAnalogY));
            rightStick.Set(axis1Inverse ? -Input.GetAxis(rightAnalogX) : Input.GetAxis(rightAnalogX),
                axis2Inverse ? -Input.GetAxis(rightAnalogY) : Input.GetAxis(rightAnalogY));

            prevButtonInputFlags = buttonInputFlags; // | stuckButtonInputFlags;
            buttonInputFlags = stuckButtonInputFlags;

            // 'dash' on right joystick
            //DashState prevDashState = dashState;
            if (rightStick.magnitude > deadZoneMagnitude)
            {
                if (rightStickOverrideCount > 0)
                {
                    rightStick.Normalize();
                }

                leftStick = rightStick;
                rightStickOverrideCount += 1;
            }
            else
            {
                rightStickOverrideCount = 0;
            }


            UpdateButtonInput();
        }


        private void UpdateButtonInput()
        {
            float value;
            for (int i = 0; i < buttonAxisInputList.Count; ++i)
            {
                ButtonAxisInput buttonAxisInput = buttonAxisInputList[i];
                value = Input.GetAxis(buttonAxisInput.axisName);
                if (value != 0.0f)
                {
                    if (value == -1.0f)
                    {
                        buttonInputFlags |= buttonAxisInput.negativeButton;
                    }
                    else if (value == 1.0f)
                    {
                        buttonInputFlags |= buttonAxisInput.positiveButton;
                    }
                }
            }

            for (int i = 0; i < buttonInputList.Count; ++i)
            {
                ButtonInput buttonInput = buttonInputList[i];
                if (UInput.GetKey(buttonInput.keyCode))
                {
                    buttonInputFlags |= buttonInput.button;
                }
            }
        }
    }

}