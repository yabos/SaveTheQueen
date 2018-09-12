using UnityEngine;
using UnityEngine.UI;
using Lib.uGui;
using Lib.Event;
using System;
using Aniz.Widget;

namespace Aniz.Widget.Panel
{
    public class InGameStageWidget : WidgetBase
    {
        #region "UserProfile"

        private class UserProfile
        {
            public UIModuleModelImage ImgSimpleUserProfile = null;
            public Text TextUserLevel = null;
            public Text TextUserName = null;
            public UIModuleModelImage ImgFilledUserHP = null;
            public Text TextUserHP = null;
            public UIModuleModelImage ImgFilledUserSP = null;
            public Text TextUserSP = null;

            public UserProfile(Transform parent)
            {
                ImgSimpleUserProfile = parent.FindChildComponent<UIModuleModelImage>("Canvas/TopLeftPanel/User_Profile");
                Debug.Assert(null != ImgSimpleUserProfile);

                TextUserLevel = parent.FindChildComponent<Text>("Canvas/TopLeftPanel/User_Profile/Lv_Slot/Text");
                Debug.Assert(null != TextUserLevel);

                TextUserName = parent.FindChildComponent<Text>("Canvas/TopLeftPanel/User_Name");
                Debug.Assert(null != TextUserName);


                ImgFilledUserHP = parent.FindChildComponent<UIModuleModelImage>("Canvas/TopLeftPanel/User_HP/User_HP_Progress_Bar");
                Debug.Assert(null != ImgFilledUserHP);

                TextUserHP = parent.FindChildComponent<Text>("Canvas/TopLeftPanel/User_HP/Hp_Num");
                Debug.Assert(null != TextUserHP);

                ImgFilledUserSP = parent.FindChildComponent<UIModuleModelImage>("Canvas/TopLeftPanel/User_SP/User_SP_Progress_Bar");
                Debug.Assert(null != ImgFilledUserSP);

                TextUserSP = parent.FindChildComponent<Text>("Canvas/TopLeftPanel/User_SP/Sp_Num");
                Debug.Assert(null != TextUserSP);

                this.SetActive(false);
            }

            public void SetActive(bool isActive)
            {
                ImgSimpleUserProfile.gameObject.SetActive(isActive);
                TextUserLevel.gameObject.SetActive(isActive);
                TextUserName.gameObject.SetActive(isActive);
                ImgFilledUserHP.gameObject.SetActive(isActive);
                TextUserHP.gameObject.SetActive(isActive);
                ImgFilledUserSP.gameObject.SetActive(isActive);
                TextUserSP.gameObject.SetActive(isActive);
            }
        }

        #endregion "UserProfile"

        #region "BossProfile"

        private class BossProfile
        {
            public UIModuleModelImage ImgSimpleBossProfile = null;
            public Text TextBossLevel = null;
            public Text TextBossName = null;
            public Transform PanelBossHP = null;
            public UIModuleModelImage ImgFilledBossHP = null;
            public Text TextBossHP = null;

            public BossProfile(Transform parent)
            {
                ImgSimpleBossProfile = parent.FindChildComponent<UIModuleModelImage>("Canvas/TopRightPanel/Boss_Profile");
                Debug.Assert(null != ImgSimpleBossProfile);

                TextBossLevel = parent.FindChildComponent<Text>("Canvas/TopRightPanel/Boss_Profile/Lv_Slot/Text");
                Debug.Assert(null != TextBossLevel);

                TextBossName = parent.FindChildComponent<Text>("Canvas/TopRightPanel/Boss_Name");
                Debug.Assert(null != TextBossName);

                PanelBossHP = parent.FindChildComponent<Transform>("Canvas/TopRightPanel/Boss_HP");
                Debug.Assert(null != PanelBossHP);

                ImgFilledBossHP = parent.FindChildComponent<UIModuleModelImage>("Canvas/TopRightPanel/Boss_HP/Boss_HP_Progress_Bar");
                Debug.Assert(null != ImgFilledBossHP);

                TextBossHP = parent.FindChildComponent<Text>("Canvas/TopRightPanel/Boss_HP/Hp_Num");
                Debug.Assert(null != TextBossHP);

                this.SetActive(false);
            }

            public void SetActive(bool isActive)
            {
                ImgSimpleBossProfile.gameObject.SetActive(isActive);
                TextBossLevel.gameObject.SetActive(isActive);
                TextBossName.gameObject.SetActive(isActive);
                PanelBossHP.gameObject.SetActive(isActive);
                ImgFilledBossHP.gameObject.SetActive(isActive);
                TextBossHP.gameObject.SetActive(isActive);
            }
        }

        #endregion "BossProfile"

        #region "variables"

        private Transform _panelTopLeft = null;
        private UserProfile _userProfile = null;

        private Transform _panelTopRight = null;
        private BossProfile _bossProfile = null;

        private Transform _panelCenter = null;
        private UIModuleModelImage _imgWarningBlood = null;
        private Transform _panelCombo = null;
        private Transform _panelDungeonTitle = null;
        private Transform _panelGoTitle = null;

        private Transform _panelBottomCenter = null;

        #endregion "variables"

        #region "WidgetBase"

        public override bool IsFlow
        {
            get { return false; }
        }

        public override void BhvOnEnter()
        {
            InitUIModule();
        }

        public override void BhvOnLeave()
        {
        }

        protected override void ShowWidget(IUIDataParams data)
        {
            _userProfile.SetActive(true);
            _bossProfile.SetActive(false);
        }

        protected override void HideWidget()
        {
        }

        public override void FinalizeWidget()
        {
            base.FinalizeWidget();
        }

        #endregion "WidgetBase"

        private void InitUIModule()
        {
            // Top Left  Panel
            {
                _panelTopLeft = transform.FindChildComponent<Transform>("Canvas/TopLeftPanel");
                Debug.Assert(null != _panelTopLeft);

                _userProfile = new UserProfile(transform);
            }

            // Top Right Panel
            {
                _panelTopRight = transform.FindChildComponent<Transform>("Canvas/TopRightPanel");
                Debug.Assert(null != _panelTopRight);

                _bossProfile = new BossProfile(transform);
            }

            // Center Panel
            {
                _panelCenter = transform.FindChildComponent<Transform>("Canvas/CenterPanel");
                Debug.Assert(null != _panelCenter);

                _imgWarningBlood = transform.FindChildComponent<UIModuleModelImage>("Canvas/CenterPanel/Warning_blood");
                Debug.Assert(null != _imgWarningBlood);
                _imgWarningBlood.gameObject.SetActive(false);

                _panelCombo = transform.FindChildComponent<Transform>("Canvas/CenterPanel/Combo");
                Debug.Assert(null != _panelCombo);
                _panelCombo.gameObject.SetActive(false);

                _panelDungeonTitle = transform.FindChildComponent<Transform>("Canvas/CenterPanel/Dungeon_Title");
                Debug.Assert(null != _panelDungeonTitle);
                _panelDungeonTitle.gameObject.SetActive(false);

                _panelGoTitle = transform.FindChildComponent<Transform>("Canvas/CenterPanel/GO_Title");
                Debug.Assert(null != _panelGoTitle);
                _panelGoTitle.gameObject.SetActive(false);
            }

            // Bottom Center Panel
            {
                _panelBottomCenter = transform.FindChildComponent<Transform>("Canvas/BottomCenterPanel");
                Debug.Assert(null != _panelBottomCenter);
                _panelBottomCenter.gameObject.SetActive(false);
            }
        }

        public override void OnNotify(INotify notify)
        {
        }
    }
}
