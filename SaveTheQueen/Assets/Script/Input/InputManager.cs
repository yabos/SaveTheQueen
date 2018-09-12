using System.Collections;
using UnityEngine;
using Lib.Pattern;
using System.Collections.Generic;
using Lib.Event;
using Lib.InputButton;
using Aniz.InputButton.Controller;

public class InputManager : GlobalManagerBase<InputManagerSetting>
{
    private InputSystem m_inputSystem;
    private List<ButtonController> m_buttonControllerList = new List<ButtonController>();
    public const float deadZoneMagnitude = 0.25f;

    public InputSystem System
    {
        get { return m_inputSystem; }
    }

    #region Events
    public override void OnAppStart(ManagerSettingBase managerSetting)
    {
        m_name = typeof(InputManager).ToString();

        if (string.IsNullOrEmpty(m_name))
        {
            throw new System.Exception("manager name is empty");
        }

        m_setting = managerSetting as InputManagerSetting;

        if (null == m_setting)
        {
            throw new System.Exception("manager setting is null");
        }

        m_inputSystem = new InputSystem();

    }

    public override void OnAppEnd()
    {
        if (m_setting != null)
        {
            GameObjectFactory.DestroyComponent(m_setting);
            m_setting = null;
        }
    }

    public override void OnAppFocus(bool focused)
    {

    }

    public override void OnAppPause(bool paused)
    {

    }

    public override void OnPageEnter(string pageName)
    {
    }

    public override IEnumerator OnPageExit()
    {
        yield return new WaitForEndOfFrame();
    }

    #endregion Events

    #region IBhvUpdatable

    public override void BhvOnEnter()
    {
        m_inputSystem.Initialize();
    }

    public override void BhvOnLeave()
    {
        UnLoad();
        m_inputSystem.Terminate();
    }

    public override void BhvFixedUpdate(float dt)
    {
        for (int i = 0; i < m_buttonControllerList.Count; ++i)
        {
            m_buttonControllerList[i].OnFixedUpdate(dt);
        }
    }

    public override void BhvLateFixedUpdate(float dt)
    {
    }

    public override void BhvUpdate(float dt)
    {
        m_inputSystem.OnUpdate(dt);
        //for (int i = 0; i < m_buttonControllerList.Count; ++i)
        //{
        //    m_buttonControllerList[i].OnUpdate(dt);
        //}
    }

    public override void BhvLateUpdate(float dt)
    {
    }

    public override bool OnMessage(IMessage message)
    {
        return false;
    }

    #endregion IBhvUpdatable

    #region Methods

    public void RemoveController(ButtonController inputController)
    {
        m_buttonControllerList.Remove(inputController);
    }

    public void RemoveController(long id)
    {
        for (int i = m_buttonControllerList.Count - 1; i >= 0; --i)
        {
            if (m_buttonControllerList[i].playerNetID == id)
            {
                m_buttonControllerList.Remove(m_buttonControllerList[i]);
                return;
            }
        }
    }

    public void AddController(ButtonController inputController)
    {
        m_buttonControllerList.Add(inputController);
    }

    public void ClearInput()
    {
        m_inputSystem.ClearInputAll();
        for (int i = 0; i < m_buttonControllerList.Count; ++i)
        {
            m_buttonControllerList[i].SetInput(0);
        }
    }

    public void UnLoad()
    {
        ClearInput();
        m_buttonControllerList.Clear();
    }

    // main input
    public void SetButton(ePlayerButtonEnum button, bool down)
    {
        SetButton(InputSystem.PlayerButtonEnumToPlayerButton(button), down);
    }

    // main input
    public void SetButton(ePlayerButton button, bool down)
    {
        if (m_inputSystem != null)
        {
            m_inputSystem.SetButton(button, down);
        }
    }


    public void OnUpdateTouchStickEvent(Vector2 stickDirection, bool isPressed)
    {
        if (isPressed)
        {
            SetButton(ePlayerButton.Left, stickDirection.x < -deadZoneMagnitude);
            SetButton(ePlayerButton.Right, stickDirection.x > deadZoneMagnitude);
            SetButton(ePlayerButton.Down, stickDirection.y < -deadZoneMagnitude);
            SetButton(ePlayerButton.Up, stickDirection.y > deadZoneMagnitude);
            m_inputSystem.OnUpdateTouchStickEvent(stickDirection, isPressed);
        }
        else
        {
            SetButton(ePlayerButton.Left, false);
            SetButton(ePlayerButton.Right, false);
            SetButton(ePlayerButton.Down, false);
            SetButton(ePlayerButton.Up, false);
        }

        //Log(StringUtil.Format("stick direction : x {0}, y {1}", stickDirection.x, stickDirection.y));
    }

    #endregion Methods

    [System.Diagnostics.Conditional("USE_DEBUG_KEYS")]
    public void DumpAll(ref string s, int depth = 0)
    {
        s += "INPUTSYSTEMS: " + m_buttonControllerList.Count + "\n";
    }


    // 	[System.Diagnostics.Conditional("USE_DEBUG_KEYS")]
    // 	public void Dump(ref string s, int depth = 0)
    // 	{
    // 		s += Console.Indent(depth) + "INPUT: " + GetType() + " " + GetInput() + "\n";
    // 		s += Console.Indent(depth) + "AXIS: " + IsAxisInput(GetInput()) + "\n";
    //         s += Console.Indent(depth) + "inputEventList\n";
    //
    //         foreach (InputEvent ie in inputEventList)
    // 		{
    //             InputButton ib = ie.inputButton;
    //
    // 			s += Console.Indent(depth + 1) + ie.button + ": " + ib.E_ButtonState;
    // 			s += " - prevButtonDown:" + ib.prevButtonDown;
    // 		//	s += " - holdFrames:" + ib.holdFrames;
    // 		//	s += " - pressedFrame:" + ib.pressedFrame;
    // 			s += "\n";
    // 		}
    // 	}

}


