using System;
using System.Collections.Generic;
using Aniz.Event;
using UnityEngine;
using UnityEngine.UI;
using Lib.Event;
using Lib.InputButton;
using Lib.Pattern;
using Lib.uGui;
using Aniz.Widget;
using ePlayerButton = Lib.InputButton.ePlayerButton;

namespace Aniz.Widget.Panel
{
    public class InGameWidget : WidgetBase
    {
        #region "variables"

        private const string SkillIconPathFormat = "Common/Icon/Skill/{0}";

        public TouchStickWidget TouchStickWidget = null;

        public UIModuleCoolTimeIcon AttackButton = null;
        private Image _imgAttack = null;

        public UIModuleCoolTimeIcon[] SkillButtons = null;
        public Button OptionButton = null;

        #endregion "variables"

        #region "WidgetBase"

        public override bool IsFlow
        {
            get { return false; }
        }

        public override void BhvOnEnter()
        {
            InitUIModule();

            if (TouchStickWidget != null)
            {
                TouchStickWidget.InitializeWidget("TouchStickWidget");

                TouchStickWidget.OnUpdateEvent = Global.InputMgr.OnUpdateTouchStickEvent;
            }

            // #MEPI_TEMP 12/08/30 일 PVP 데모로 인해 X 버튼 감춰 둡니다.
            if (null != OptionButton)
            {
                OptionButton.gameObject.SetActive(false);
            }
            // #MEPI_TEMP 12/08/30 일 PVP 데모로 인해 X 버튼 감춰 둡니다.
        }

        public override void BhvOnLeave()
        {

        }

        protected override void ShowWidget(IUIDataParams data)
        {
        }

        protected override void HideWidget()
        {
        }

        public override void OnNotify(INotify notify)
        {
        }

        public override void FinalizeWidget()
        {
            if (TouchStickWidget != null)
            {
                TouchStickWidget.FinalizeWidget();
            }

            base.FinalizeWidget();
        }

        #endregion "WidgetBase"

        private void InitUIModule()
        {
            _imgAttack = transform.FindChildComponent<Image>("Canvas/RightPanel/AttackButton/Image");
            Debug.Assert(null != _imgAttack);
            _imgAttack.gameObject.SetActive(false);

        }


        #region "Btn Click Event"

        public void OnNextPageButtonClk()
        {
            Global.NotificationMgr.NotifyToEventHandler("OnNotify", eNotifyHandler.Util_GameFlow, new SendMessage((uint)eMessage.PageTransition));
            //Lib.Event.SendMessage message = new Lib.Event.SendMessage((uint)eMessage.PageTransition);
            //Global.SceneMgr.CurrentScene.OnNotify(message);
        }

        private void OnAttackButtonClick(UnityEngine.UI.Button button, bool isPress)
        {
            {
                Global.InputMgr.SetButton(ePlayerButton.FIRE3, isPress);
            }

            if (isPress == false)
            {
                UIModuleCoolTimeIcon coolTimeIcon =
                    ComponentFactory.GetComponent<UIModuleCoolTimeIcon>(button.gameObject, IfNotExist.ReturnNull);
                if (coolTimeIcon != null)
                {
                    coolTimeIcon.SetCoolTimeIcon(1.0f, false, null);
                }
            }
        }

        private void OnSkillkButtonClick(UnityEngine.UI.Button button, bool isPress)
        {
            ePlayerButtonEnum val = (ePlayerButtonEnum)System.Enum.Parse(typeof(ePlayerButtonEnum), button.name);
            Global.InputMgr.SetButton(val, isPress);

            if (isPress == false)
            {
                UIModuleCoolTimeIcon coolTimeIcon =
                    ComponentFactory.GetComponent<UIModuleCoolTimeIcon>(button.gameObject, IfNotExist.ReturnNull);
                if (coolTimeIcon != null)
                {
                    coolTimeIcon.SetCoolTimeIcon(1.5f, true, null);
                }
            }
        }

        public void OnAttackButtonDown(UnityEngine.UI.Button button)
        {
            if (button == null || button.gameObject == null)
            {
                return;
            }

            OnAttackButtonClick(button, true);
        }

        public void OnAttackButtonUp(UnityEngine.UI.Button button)
        {
            if (button == null || button.gameObject == null)
            {
                return;
            }

            OnAttackButtonClick(button, false);
        }

        public void OnSkillButtonDown(UnityEngine.UI.Button button)
        {
            if (button == null || button.gameObject == null)
            {
                return;
            }

            OnSkillkButtonClick(button, true);
        }

        public void OnSkillButtonUp(UnityEngine.UI.Button button)
        {
            if (button == null || button.gameObject == null)
            {
                return;
            }

            OnSkillkButtonClick(button, false);
        }

        #endregion "Btn Click Event"
    }
}