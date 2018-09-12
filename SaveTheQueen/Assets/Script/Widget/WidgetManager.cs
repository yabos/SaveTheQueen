using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Aniz.Event;
using Lib.Event;
using Lib.InputButton;
using Lib.Pattern;
using Lib.uGui;
using Aniz.Widget.Panel;

namespace Aniz.Widget
{
    public class WidgetManager : GlobalManagerBase<WidgetManagerSetting>
    {

        private UnityEngine.EventSystems.EventSystem m_eventSystem = null;

        public UnityEngine.EventSystems.EventSystem UnityEventSystem
        {
            get { return m_eventSystem; }
        }

        private UnityEngine.EventSystems.StandaloneInputModule m_standaloneInputModule = null;

        public UnityEngine.EventSystems.StandaloneInputModule UnityStandaloneInputModule
        {
            get { return m_standaloneInputModule; }
        }

        protected Queue<MessageBoxDataParam> m_messageBoxQueue = new Queue<MessageBoxDataParam>();

        private WidgetRepositories m_widgetRepositories;
        private UISpriteRepository m_uiSpriteRepository;
        private CommandManager m_commandManager;
        private string m_currentUIName = string.Empty;

        #region Events

        public override void OnAppStart(ManagerSettingBase managerSetting)
        {
            m_name = typeof(WidgetManager).ToString();

            if (string.IsNullOrEmpty(m_name))
            {
                throw new System.Exception("manager name is empty");
            }

            m_setting = managerSetting as WidgetManagerSetting;

            if (null == m_setting)
            {
                throw new System.Exception("manager setting is null");
            }

            CreateRootObject(m_setting.transform, "UIManager");
            m_widgetRepositories = new WidgetRepositories(this);

            m_commandManager = new CommandManager
            {
                MaxCommand = 10
            };

            InitializeUiInputEventSystem();
            BhvOnEnter();
        }

        public override void OnAppEnd()
        {
            m_commandManager.Reset();

            DestroyRootObject();

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
            UnLoad();
            yield return new WaitForEndOfFrame();
        }

        #endregion Events

        #region IBhvUpdatable

        public override void BhvOnEnter()
        {
            m_uiSpriteRepository = new UISpriteRepository();
            m_uiSpriteRepository.Initialize();

            m_widgetRepositories.BhvOnEnter();
        }

        public override void BhvOnLeave()
        {
            ClearMessageBoxQueue();
            m_widgetRepositories.BhvOnLeave();

            m_uiSpriteRepository.Terminate();
        }

        public override void BhvFixedUpdate(float dt)
        {
            m_widgetRepositories.BhvFixedUpdate(dt);
        }

        public override void BhvLateFixedUpdate(float dt)
        {
            m_widgetRepositories.BhvLateFixedUpdate(dt);
        }

        public override void BhvUpdate(float dt)
        {
            UpdateMessageBoxQueue(dt);
            m_widgetRepositories.BhvUpdate(dt);

        }

        public override void BhvLateUpdate(float dt)
        {
            m_widgetRepositories.BhvLateUpdate(dt);

            if (Global.InputMgr.System.GetButtonUp(ePlayerButton.Pause))
            {
                BackFlow();
            }
        }


        #endregion IBhvUpdatable

        public override bool OnMessage(IMessage message)
        {
            uint category = (uint)message.MsgCode >> 16;
            if (category != (uint)eMessageCategory.UI)
            {
                return false;
            }
            return m_widgetRepositories.OnMessage(message);
        }


        #region Methods

#if UNITY_EDITOR
        public Dictionary<string, WidgetBase> GetWidgets()
        {
            return m_widgetRepositories.GetWidgets();
        }
#endif //UNITY_EDITOR

        void InitializeUiInputEventSystem()
        {
            if (Application.isPlaying)
            {
                GameObject root = RootObject != null ? RootObject : Setting.gameObject;

                if (m_eventSystem != null)
                {
                    Object.Destroy(m_eventSystem);
                }

                m_eventSystem = root.AddComponent<UnityEngine.EventSystems.EventSystem>();

                if (m_standaloneInputModule != null)
                {
                    Object.Destroy(m_standaloneInputModule);
                }

                m_standaloneInputModule = root.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }


        public void UnLoad()
        {
            m_currentUIName = string.Empty;
            m_commandManager.Reset();
            m_widgetRepositories.FinalizeWidgets(false);
        }



        public void ShowLoadingWidget(float activeTime = 0.0f, string currentPageName = "", string nextPageName = "")
        {
            LoadingWidget widget = m_widgetRepositories.FindWidget("LoadingWidget") as LoadingWidget;
            if (widget == null)
            {
                widget = m_widgetRepositories.CreateWidget<LoadingWidget>("System/LoadingWidget");
            }

            if (widget != null)
            {
                widget.Show(activeTime);
                widget.SetLoadingPanelInfo(currentPageName, nextPageName);
                widget.SetLoadingProgressInfo(0.0f);
            }
        }

        public void SetLoadingPanelInfo(string currentPageName, string nextPageName)
        {
            LoadingWidget widget = m_widgetRepositories.FindWidget("LoadingWidget") as LoadingWidget;
            if (widget != null)
            {
                widget.SetLoadingPanelInfo(currentPageName, nextPageName);
            }
        }

        public void SetLoadingProgressInfo(float progress)
        {
            LoadingWidget widget = m_widgetRepositories.FindWidget("LoadingWidget") as LoadingWidget;
            if (widget != null)
            {
                widget.SetLoadingProgressInfo(progress);
            }
        }

        public void HideLoadingWidget(float deactiveTime = 0.0f)
        {
            LoadingWidget widget = m_widgetRepositories.FindWidget("LoadingWidget") as LoadingWidget;
            if (widget != null)
            {
                widget.Hide(deactiveTime);
            }
        }


        public void ShowConnectionWidget(float activeTime = 0.0f, string currentPageName = "", string nextPageName = "")
        {
            ConnectionWidget widget = m_widgetRepositories.FindWidget("ConnectionWidget") as ConnectionWidget;
            if (widget == null)
            {
                widget = m_widgetRepositories.CreateWidget<ConnectionWidget>("System/ConnectionWidget");
            }

            if (widget != null)
            {
                widget.Show(activeTime);
            }
        }

        public void HideConnectionWidget(float deactiveTime = 0.0f)
        {
            ConnectionWidget widget = m_widgetRepositories.FindWidget("ConnectionWidget") as ConnectionWidget;
            if (widget != null)
            {
                widget.Hide(deactiveTime);
            }
        }

        public void ShowMessageBox(string title, string message, eMessageBoxType messageBoxType,
            System.Action<bool> completed = null, float activeTime = 0.0f)
        {
            MessageBoxWidget widget =
                m_widgetRepositories.FindWidget("MessageBoxWidget") as MessageBoxWidget;
            if (widget == null)
            {
                widget = m_widgetRepositories.CreateWidget<MessageBoxWidget>("System/MessageBoxWidget");
            }

            if (widget != null)
            {
                if (widget.IsActive == true)
                {
                    m_messageBoxQueue.Enqueue(
                        new MessageBoxDataParam(title, message, messageBoxType, completed, activeTime));
                }
                else
                {
                    MessageBoxDataParam messageBoxDataParam =
                        new MessageBoxDataParam(title, message, messageBoxType, completed, activeTime);
                    widget.Show(activeTime, messageBoxDataParam);
                }
            }
        }

        protected void ClearMessageBoxQueue()
        {
            if (m_messageBoxQueue.Any() == false)
            {
                return;
            }

            for (int i = 0; i < m_messageBoxQueue.Count; ++i)
            {
                MessageBoxDataParam messageBoxDataParam = m_messageBoxQueue.Dequeue();
                if (messageBoxDataParam != null)
                {
                    messageBoxDataParam.Dispose();
                }
            }

            m_messageBoxQueue.Clear();
        }

        protected void UpdateMessageBoxQueue(float delta)
        {
            if (m_messageBoxQueue.Any() == false)
            {
                return;
            }

            MessageBoxWidget widget = m_widgetRepositories.FindWidget("MessageBoxWidget") as MessageBoxWidget;
            if (widget != null && widget.IsGameOjectActive != true)
            {
                MessageBoxDataParam messageBoxDataParam = m_messageBoxQueue.Dequeue();
                if (messageBoxDataParam != null)
                {
                    widget.Show(messageBoxDataParam.ActiveTime, messageBoxDataParam);
                }
                messageBoxDataParam.Dispose();
            }
        }

        #region UI Flow

        public void BackFlow()
        {
            UIFlowCommand command = (UIFlowCommand)m_commandManager.Undo();
            if (command != null)
            {
                m_currentUIName = command.CurrentWndName;
            }
        }

        public void AddFlow(WidgetBase currentWidget, float activeTime, IUIDataParams data)
        {
            if (currentWidget.IsFlow == false)
            {
                return;
            }

            WidgetBase prevWidget = null;
            if (string.IsNullOrEmpty(m_currentUIName) == false)
            {
                prevWidget = FindWidget(m_currentUIName);
            }

            if (prevWidget != null)
            {
                m_commandManager.CurrentCommand = new UIFlowCommand(prevWidget, currentWidget, activeTime, data);
                m_commandManager.Execute();
            }
            m_currentUIName = currentWidget.WidgetName;
        }

        #endregion UI Flow

        public WidgetBase ShowWidget(string widgetName, IUIDataParams data = null, float activeTime = 0.0f)
        {
            WidgetBase widget = FindWidget(widgetName);

            if (widget == null)
            {
                widget = CreateWidget<WidgetBase>(widgetName);
            }

            widget.Show(activeTime, data);
            return widget;
        }

        public void Hide(string widgetName, float activeTime = 0.0f)
        {
            WidgetBase widget = FindWidget(widgetName);

            if (widget != null)
            {
                widget.Hide(activeTime);
            }
        }

        public T CreateWidget<T>(string path, bool dontDestroyOnLoad = false) where T : WidgetBase
        {
            return m_widgetRepositories.CreateWidget<T>(path, dontDestroyOnLoad);
        }

        public IEnumerator OnCreateWidgetAsync<T>(string path, System.Action<T> action, bool dontDestroyOnLoad = false)
            where T : WidgetBase
        {
            yield return m_widgetRepositories.OnCreateWidgetAsync<T>(path, action, dontDestroyOnLoad);
        }

        public WidgetBase FindWidget(string widgetType)
        {
            return m_widgetRepositories.FindWidget(widgetType);
        }

        public void HideAllWidgets(float deactiveTime = 0.0f)
        {
            m_widgetRepositories.HideAllWidgets(deactiveTime);
        }

        public Sprite GetSprite(string name)
        {
            Sprite sprite = null;
            m_uiSpriteRepository.Get(name, out sprite);
            return sprite;
        }

        #endregion Methods

    }

}