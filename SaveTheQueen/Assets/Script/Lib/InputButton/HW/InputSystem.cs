using System.Collections.Generic;
using Lib.Pattern;
using UnityEngine;
using UInput = UnityEngine.Input;

namespace Lib.InputButton
{
    public class InputSystem : IBaseClass
    {
        public const int FRAMEINPUT_UP = 32;
        private const int MAX_CONTROLLER = 4;
        private const int MAX_KEYBOARD_CONTROLLER = 1;

        public const uint NAVIGATION_BUTTONS = (uint)(ePlayerButton.Up | ePlayerButton.Down | ePlayerButton.Left | ePlayerButton.Right);

        public const uint ATTACK_BUTTONS = (uint)(ePlayerButton.FIRE1 | ePlayerButton.FIRE2 | ePlayerButton.FIRE3 | ePlayerButton.JUMP |
                                                  ePlayerButton.ABILITY1 | ePlayerButton.ABILITY2 | ePlayerButton.ABILITY3 | ePlayerButton.ABILITY4 |
                                                  ePlayerButton.Revive1 | ePlayerButton.Revive3 | ePlayerButton.Revive4 | ePlayerButton.QTE |
                                                  ePlayerButton.CHAIN1 | ePlayerButton.CHAIN2 | ePlayerButton.CHAIN3 | ePlayerButton.CHAIN4);

        public const uint ITEM_BUTTONS = (uint)(ePlayerButton.Item1 | ePlayerButton.Item2 | ePlayerButton.Item3 | ePlayerButton.Item4);

        private List<IInput> m_inputList = new List<IInput>();

        public bool JoystickInput { get; set; }

        public IInput MainInput
        {
            get { return m_inputList.Count > 0 ? m_inputList[0] : null; }
        }

        private Vector2 m_touchStickDirection = Vector2.zero;
        public Vector2 TouchStickDirection
        {
            get
            {
                return m_touchStickDirection;
            }
        }

        private bool m_isPressingTouchStick = false;

        public bool IsPressMovingTouchStick
        {
            get
            {
                return m_isPressingTouchStick;
            }
        }

        public bool IsPressMovingKey
        {
            get
            {
                return (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) || (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f);
            }
        }


        public void Initialize()
        {
            InitializeControllers();
        }

        public void Terminate()
        {
            ClearInputAll();
        }


        public void OnUpdate(float dt)
        {
            OnUpdateInputAll();

#if UNITY_EDITOR
            MouseUpdate();
#endif
        }

        #region Methods

        public static bool IsAxisInput(uint input)
        {
            return (input & NAVIGATION_BUTTONS) != 0;
        }

        public static bool IsAttackInput(uint input)
        {
            return (input & ATTACK_BUTTONS) != 0;
        }

        public static uint GetPlayerButton(ePlayerButtonEnum pb_enum)
        {
            return (uint)(1 << (int)pb_enum);
        }

        private void InitializeControllers()
        {
#if USE_JOYSTICK_AS_MAIN_CONTROLLER
        InitializeJoystickControllers();

        InitializeKeyboardControllers();
#else // USE_JOYSTICK_AS_MAIN_CONTROLLER
            InitializeKeyboardInputs();

            InitializeJoystickInputs();
#endif // USE_JOYSTICK_AS_MAIN_CONTROLLER

            if (m_inputList.Count == 0)
            {
                Debug.LogError("It should have at least one controller");
                return;
            }
        }

        private void InitializeKeyboardInputs()
        {
            for (int i = 0; i < MAX_KEYBOARD_CONTROLLER && m_inputList.Count < MAX_CONTROLLER; ++i)
            {
                IInput input = new KeyboardInput();
                input.SetInputIndex(i);
                m_inputList.Add(input);
            }

#if UNITY_IOS && !UNITY_EDITOR
        iCadeInput.Activate(true);
#endif

        }

        private void InitializeJoystickInputs()
        {
#if UNITY_EDITOR || USE_JOYSTICK_AS_MAIN_CONTROLLER
            string[] joystickNames = UInput.GetJoystickNames();
            Debug.Log(string.Format("[Controller] detected {0} joysticks", joystickNames.Length));
            for (int i = 0; i < joystickNames.Length; ++i)
            {
                Debug.Log(i + " " + joystickNames[i]);
            }

            for (int i = 0; i < joystickNames.Length && m_inputList.Count < MAX_CONTROLLER; ++i)
            {
                JoystickInput input = new JoystickInput();
                input.SetInputIndex(i);
                input.SetJoystickType(joystickNames[i]);
                m_inputList.Add(input);
                Debug.Log(m_inputList.Count + " controller added: " + input.ToString());
            }
#endif
        }

        public IInput GetInput(int index)
        {
            return m_inputList[index];
        }

        public int InputListCount
        {
            get { return m_inputList.Count; }
        }

        // main controller
        public void SetButton(ePlayerButton button, bool down)
        {
            if (down)
            {
                MainInput.SetButtonDown(button);
            }
            else
            {
                MainInput.SetButtonUp(button);
            }
        }

        public void ClearInputAll()
        {
            for (int i = 0; i < m_inputList.Count; ++i)
            {
                m_inputList[i].ClearInput();
            }
        }

        private void OnUpdateInputAll()
        {
            for (int i = 0; i < m_inputList.Count; ++i)
            {
                m_inputList[i].OnUpdate();
            }
        }

        public uint GetInput()
        {
            uint buttonInput = 0;
            for (int i = 0; i < m_inputList.Count; ++i)
            {
                buttonInput |= m_inputList[i].GetInput();
            }

            return buttonInput;
        }

        public uint GetInputDown()
        {
            uint buttonInputDown = 0;
            for (int i = 0; i < m_inputList.Count; ++i)
            {
                buttonInputDown |= m_inputList[i].GetInputDown();
            }

            return buttonInputDown;
        }

        public uint GetInputUp()
        {
            uint buttonInputUp = 0;
            for (int i = 0; i < m_inputList.Count; ++i)
            {
                buttonInputUp |= m_inputList[i].GetInputUp();
            }

            return buttonInputUp;
        }

        public bool GetButton(ePlayerButton button)
        {
            uint input = GetInput();
            return ((input & (uint)button) != 0);
        }

        public bool GetButtonDown(ePlayerButton button)
        {
            uint input = GetInputDown();
            return ((input & (uint)button) != 0);
        }

        public bool GetButtonUp(ePlayerButton button)
        {
            uint input = GetInputUp();
            return ((input & (uint)button) != 0);
        }

        public bool GetButtonDown(ePlayerButtonEnum buttonenum)
        {
            return GetButtonDown(PlayerButtonEnumToPlayerButton(buttonenum));
        }

        public bool AnyButtonDown()
        {
            return GetInputDown() != 0;
        }

#if UNITY_EDITOR
        public bool EditMouseUpdate = false;
        private MouseEventTranslator m_mouseTranslator = new MouseEventTranslator();

        public delegate void OnMouseEvent(MouseEvent mouseEvent);

        public OnMouseEvent CallBackMouseEvent;
        private bool m_mouseDown = false;

        public MouseEventTranslator MouseTranslator
        {
            get { return m_mouseTranslator; }
        }


        private void MouseUpdate()
        {
            if (EditMouseUpdate == false)
                return;

            float scroll = UInput.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                Vector2 mousePos = UInput.mousePosition;
                Rect gameScreen = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
                if (gameScreen.Contains(mousePos))
                {
                    MouseEvent mouseEvent = m_mouseTranslator.MouseWheel(mousePos, -scroll);
                    if (CallBackMouseEvent != null && mouseEvent != MouseEvent.None)
                    {
                        CallBackMouseEvent(mouseEvent);
                    }
                }
            }

            bool updateRaycast = false;
            for (int i = 0; i < 3; ++i)
            {
                if (UInput.GetMouseButton(i) || UInput.GetMouseButtonUp(i) || m_mouseDown)
                {
                    updateRaycast = true;
                    break;
                }
            }

            Vector3 mouseposition;
            if (updateRaycast)
            {
                mouseposition = UInput.mousePosition;
            }
            else
            {
                return;
            }

            MouseEvent.E_Buttons eventbutton = MouseEvent.E_Buttons.Left;
            for (int i = 0; i < 3; ++i)
            {
                bool pressed = UInput.GetMouseButtonDown(i);
                bool unpressed = UInput.GetMouseButtonUp(i);
                //bool pressing = Input.GetMouseButton(i);

                //E_MOUSE_EVENT dowingEvent = E_MOUSE_EVENT.LDOWNING;


                if (i == 0)
                {
                    eventbutton = MouseEvent.E_Buttons.Left;
                    //dowingEvent = E_MOUSE_EVENT.LDOWNING;
                }
                else if (i == 1)
                {
                    eventbutton = MouseEvent.E_Buttons.Right;
                    //dowingEvent = E_MOUSE_EVENT.RDOWNING;
                }
                else if (i == 2)
                {
                    eventbutton = MouseEvent.E_Buttons.Middle;
                    //dowingEvent = E_MOUSE_EVENT.MDOWNING;
                }


                if (pressed)
                {
                    PushTouchesEventMsg(mouseposition, true, eventbutton);
                    m_mouseDown = true;
                }

                if (unpressed)
                {
                    PushTouchesEventMsg(mouseposition, false, eventbutton);
                    m_mouseDown = false;
                }
            }


            bool Moved = UInput.GetAxis("Mouse X") != 0.0f || UInput.GetAxis("Mouse Y") != 0.0f;
            if (Moved)
            {
                PushTouchesEventMsg(mouseposition, false, eventbutton, true);
            }

        }

        public void PushTouchesEventMsg(Vector3 cursorInfo, bool bDrag, MouseEvent.E_Buttons mouseevent,
            bool move = false)
        {
            MouseEvent mouseEvent = MouseEvent.None;

            if (move)
            {
                mouseEvent = m_mouseTranslator.MouseMove(mouseevent, cursorInfo);
            }
            else
            {
                if (bDrag)
                {
                    mouseEvent = m_mouseTranslator.MouseDown(mouseevent, cursorInfo);
                }
                else
                {
                    mouseEvent = m_mouseTranslator.MouseUp(mouseevent, cursorInfo);
                }
            }

            if (CallBackMouseEvent != null && mouseEvent != MouseEvent.None)
            {
                CallBackMouseEvent(mouseEvent);
            }
        }
#endif

        #endregion Methods


        public void OnUpdateTouchStickEvent(Vector2 stickDirection, bool isPressed)
        {
            m_isPressingTouchStick = isPressed;
            m_touchStickDirection = stickDirection;

            MainInput.SetAxisInput(m_touchStickDirection);
        }

        public Vector3 GetMovingDirection(Vector3 forward, Vector3 right)
        {
            forward.y = 0.0f;
            forward.Normalize();

            right.y = 0.0f;
            right.Normalize();

            Vector3 movingDirection = Vector3.zero;

            // touch static dir
            movingDirection += forward * TouchStickDirection.y + right * TouchStickDirection.x;

            // keyboard dir
            movingDirection += forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");

            return movingDirection;
        }


        public static ePlayerButton PlayerButtonEnumToPlayerButton(ePlayerButtonEnum button)
        {
            return (ePlayerButton)(0x1 << (int)(button));
        }

    }
}
