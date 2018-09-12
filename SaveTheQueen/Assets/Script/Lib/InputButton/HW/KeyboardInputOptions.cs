using System.Collections.Generic;
using Lib.InputButton;
using UnityEngine;

namespace Lib.InputButton
{
    public class KeyboardInputOptions
    {
        public class ButtonAxisInput
        {
            public uint negativeButton;
            public uint positiveButton;
            public int axisIndex;
            public string axisName;
        }

        public class ButtonInput
        {
            public uint button;
            public KeyCode keyCode;
            public KeyCode iOSJoystickKeyCode;
        }

        private List<ButtonInput> m_buttonInputList = new List<ButtonInput>();
        private List<ButtonAxisInput> m_buttonAxisInputList = new List<ButtonAxisInput>();

        public List<ButtonInput> GetButtonInputList()
        {
            return m_buttonInputList;
        }

        public List<ButtonAxisInput> GetButtonAxisInputList()
        {
            return m_buttonAxisInputList;
        }


        public KeyboardInputOptions()
        {
            Reset(1);
        }

        private void SetButtonInput(ePlayerButtonEnum buttonEnum, KeyCode keyCode, KeyCode iOSJoystickKeyCode)
        {
            ButtonInput buttonInput = new ButtonInput();
            buttonInput.button = InputSystem.GetPlayerButton(buttonEnum);
            buttonInput.keyCode = keyCode;
            buttonInput.iOSJoystickKeyCode = iOSJoystickKeyCode;
            m_buttonInputList.Add(buttonInput);
        }



        private void SetButtonAxisInput(int joystickIndex, int axisNum, ePlayerButtonEnum negativeButtonEnum,
            ePlayerButtonEnum positiveButtonEnum)
        {
            ButtonAxisInput buttonAxisInput = new ButtonAxisInput();
            buttonAxisInput.negativeButton = InputSystem.GetPlayerButton(negativeButtonEnum);
            buttonAxisInput.positiveButton = InputSystem.GetPlayerButton(positiveButtonEnum);
            buttonAxisInput.axisIndex = axisNum;
            buttonAxisInput.axisName = GetAxisName(joystickIndex, axisNum);
            m_buttonAxisInputList.Add(buttonAxisInput);
        }

        private static string GetAxisName(int joystickIndex, int axisNum)
        {
            return string.Format("Joy{0} Axis {1}", joystickIndex, axisNum);
        }

        public void Reset(int controllerIndex)
        {
            m_buttonInputList.Clear();

            // navigation
            SetButtonInput(ePlayerButtonEnum.Up, KeyCode.UpArrow, KeyCode.JoystickButton4);
            SetButtonInput(ePlayerButtonEnum.Down, KeyCode.DownArrow, KeyCode.JoystickButton6);
            SetButtonInput(ePlayerButtonEnum.Left, KeyCode.LeftArrow, KeyCode.JoystickButton7);
            SetButtonInput(ePlayerButtonEnum.Right, KeyCode.RightArrow, KeyCode.JoystickButton5);

            SetButtonInput(ePlayerButtonEnum.JUMP, KeyCode.Space, KeyCode.JoystickButton14);
            SetButtonInput(ePlayerButtonEnum.FIRE1, KeyCode.A, KeyCode.JoystickButton13);
            SetButtonInput(ePlayerButtonEnum.FIRE2, KeyCode.S, KeyCode.JoystickButton12);
            SetButtonInput(ePlayerButtonEnum.FIRE3, KeyCode.D, KeyCode.JoystickButton15);

            SetButtonInput(ePlayerButtonEnum.ABILITY1, KeyCode.Q, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY2, KeyCode.W, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY3, KeyCode.E, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY4, KeyCode.R, KeyCode.None);

            SetButtonInput(ePlayerButtonEnum.QTE, KeyCode.G, KeyCode.None);
            //SetButtonInput(ePlayerButtonEnum.Ability1, KeyCode.Q, KeyCode.None, KeyCodeiCade.JoystickRUp);
            //SetButtonInput(ePlayerButtonEnum.Ability2, KeyCode.W, KeyCode.None, KeyCodeiCade.JoystickRDown);
            //SetButtonInput(ePlayerButtonEnum.Ability3, KeyCode.E, KeyCode.None, KeyCodeiCade.JoystickRLeft);
            //SetButtonInput(ePlayerButtonEnum.Ability4, KeyCode.R, KeyCode.None, KeyCodeiCade.JoystickRRight);
            //SetButtonInput(ePlayerButtonEnum.Ability1, KeyCode.Q, KeyCode.None, KeyCodeiCade.ButtonL1);
            //SetButtonInput(ePlayerButtonEnum.Ability2, KeyCode.W, KeyCode.None, KeyCodeiCade.ButtonR1);
            //SetButtonInput(ePlayerButtonEnum.Ability3, KeyCode.E, KeyCode.None, KeyCodeiCade.ButtonL2);
            //SetButtonInput(ePlayerButtonEnum.Ability4, KeyCode.R, KeyCode.None, KeyCodeiCade.ButtonR2);

            //SetButtonInput(ePlayerButtonEnum.Fire4, KeyCode.T, KeyCode.None, KeyCodeiCade.ButtonStart);

            //SetButtonInput(ePlayerButtonEnum.Item1, KeyCode.Alpha1, KeyCode.JoystickButton4, KeyCodeiCade.JoystickRUp);
            //SetButtonInput(ePlayerButtonEnum.Item2, KeyCode.Alpha2, KeyCode.JoystickButton5, KeyCodeiCade.JoystickRDown);
            //SetButtonInput(ePlayerButtonEnum.Item3, KeyCode.Alpha3, KeyCode.JoystickButton6, KeyCodeiCade.JoystickRLeft);
            //SetButtonInput(ePlayerButtonEnum.Item4, KeyCode.Alpha4, KeyCode.JoystickButton7, KeyCodeiCade.JoystickRRight);

            SetButtonInput(ePlayerButtonEnum.Item1, KeyCode.Alpha1, KeyCode.JoystickButton8);
            SetButtonInput(ePlayerButtonEnum.Item2, KeyCode.Alpha2, KeyCode.JoystickButton9);
            SetButtonInput(ePlayerButtonEnum.Item3, KeyCode.Alpha3, KeyCode.JoystickButton10);
            SetButtonInput(ePlayerButtonEnum.Item4, KeyCode.Alpha4, KeyCode.JoystickButton11);


            SetButtonInput(ePlayerButtonEnum.Pause, KeyCode.Escape, KeyCode.JoystickButton0);
            SetButtonInput(ePlayerButtonEnum.Skip, KeyCode.Return, KeyCode.JoystickButton0);
            SetButtonInput(ePlayerButtonEnum.Skip, KeyCode.KeypadEnter, KeyCode.JoystickButton14);
            //SetButtonInput(ePlayerButtonEnum.Skip, KeyCode.Mouse0, KeyCode.None, KeyCodeiCade.None);

            SetButtonInput(ePlayerButtonEnum.CHAIN1, KeyCode.Z, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.CHAIN2, KeyCode.X, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.CHAIN3, KeyCode.C, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.CHAIN4, KeyCode.V, KeyCode.None);

            m_buttonAxisInputList.Clear();

            SetButtonAxisInput(controllerIndex, 1, ePlayerButtonEnum.Left, ePlayerButtonEnum.Right);
            SetButtonAxisInput(controllerIndex, 2, ePlayerButtonEnum.Up, ePlayerButtonEnum.Down);
            //SetButtonAxisInput(controllerIndex, 3, PlayerButtonEnum.Ability3, PlayerButtonEnum.Ability4);
            //SetButtonAxisInput(controllerIndex, 4, PlayerButtonEnum.Ability1, PlayerButtonEnum.Ability2);
            SetButtonAxisInput(controllerIndex, 3, ePlayerButtonEnum.Item3, ePlayerButtonEnum.Item4);
            SetButtonAxisInput(controllerIndex, 4, ePlayerButtonEnum.Item1, ePlayerButtonEnum.Item2);
        }

        public void ResetAlt(int controllerIndex)
        {
            m_buttonInputList.Clear();

            // navigation
            //        SetButtonInput(PlayerButtonEnum.Up, KeyCode.P);
            //        SetButtonInput(PlayerButtonEnum.Down, KeyCode.Semicolon);
            //        SetButtonInput(PlayerButtonEnum.Left, KeyCode.L);
            //        SetButtonInput(PlayerButtonEnum.Right, KeyCode.Quote);
            SetButtonInput(ePlayerButtonEnum.Up, KeyCode.Keypad5, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Down, KeyCode.Keypad2, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Left, KeyCode.Keypad1, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Right, KeyCode.Keypad3, KeyCode.None);

            SetButtonInput(ePlayerButtonEnum.JUMP, KeyCode.Comma, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.FIRE1, KeyCode.Y, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.FIRE2, KeyCode.U, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.FIRE3, KeyCode.I, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY1, KeyCode.Alpha6, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY2, KeyCode.Alpha7, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY3, KeyCode.Alpha8, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY4, KeyCode.Alpha9, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY1, KeyCode.Alpha6, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY2, KeyCode.Alpha7, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY3, KeyCode.Alpha8, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.ABILITY4, KeyCode.Alpha9, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.QTE, KeyCode.G, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Powerup, KeyCode.LeftBracket, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Item1, KeyCode.F5, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Item2, KeyCode.F6, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Item3, KeyCode.F7, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Item4, KeyCode.F8, KeyCode.None);
            //SetButtonInput(PlayerButtonEnum.Pause, KeyCode.P, KeyCode.None, E_iCadeCode.None);
            SetButtonInput(ePlayerButtonEnum.Skip, KeyCode.Return, KeyCode.None);
            SetButtonInput(ePlayerButtonEnum.Skip, KeyCode.KeypadEnter, KeyCode.None);
            //SetButtonInput(ePlayerButtonEnum.Skip, KeyCode.Mouse0, KeyCode.None, KeyCodeiCade.None);

            m_buttonAxisInputList.Clear();
        }

    }
}