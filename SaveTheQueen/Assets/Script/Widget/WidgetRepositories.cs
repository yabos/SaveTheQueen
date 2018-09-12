using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aniz.Graph;
using Aniz.Resource;
using Aniz.Resource.Unit;
using Lib.Event;
using Lib.Pattern;
using Aniz.Widget.Panel;
using UnityEngine;

namespace Aniz.Widget
{
    public class WidgetRepositories : IGraphUpdatable
    {
        private WidgetManager m_widgetManager;
        protected GameObject m_root = null;

        protected GameObject m_staticPanel = null;
        protected GameObject m_dynamicPanel = null;

        private WidgetRepository m_widgetRepository;
        private WidgetRepository m_dontDestroy_widgetRepository;

        //protected List<KeyValuePair<string, WidgetBase>> m_widgets = new List<KeyValuePair<string, WidgetBase>>();
        //protected List<KeyValuePair<string, WidgetBase>> m_dontDestroy_widgets = new List<KeyValuePair<string, WidgetBase>>();

        //protected WidgetBase m_currentWidget = null;
        //public WidgetBase CurrentWidget
        //{
        //    get { return m_currentWidget; }
        //}

        public WidgetRepositories(WidgetManager widgetManager)
        {
            m_widgetManager = widgetManager;
        }

        #region IBhvUpdatable

        public void BhvOnEnter()
        {
            m_widgetRepository = new WidgetRepository();
            m_widgetRepository.Initialize();
            m_dontDestroy_widgetRepository = new WidgetRepository();
            m_dontDestroy_widgetRepository.Initialize();

            InitializeUiPanel();
            InitializeWidget();

            if (m_widgetRepository != null)
            {
                m_widgetRepository.BhvOnEnter();
            }

            if (m_dontDestroy_widgetRepository != null)
            {
                m_dontDestroy_widgetRepository.BhvOnEnter();
            }
        }

        public void BhvOnLeave()
        {
            if (m_widgetRepository != null)
            {
                m_widgetRepository.BhvOnLeave();
            }

            if (m_dontDestroy_widgetRepository != null)
            {
                m_dontDestroy_widgetRepository.BhvOnLeave();
            }

            FinalizeWidgets(true);
        }

        public void BhvUpdate(float dt)
        {
            if (m_dontDestroy_widgetRepository != null)
            {
                m_dontDestroy_widgetRepository.BhvUpdate(dt);
            }

            if (m_widgetRepository != null)
            {
                m_widgetRepository.BhvUpdate(dt);
            }
        }

        public void BhvLateUpdate(float dt)
        {
            if (m_dontDestroy_widgetRepository != null)
            {
                m_dontDestroy_widgetRepository.BhvLateUpdate(dt);
            }

            if (m_widgetRepository != null)
            {
                m_widgetRepository.BhvLateUpdate(dt);
            }
        }

        public void BhvFixedUpdate(float dt)
        {
            if (m_dontDestroy_widgetRepository != null)
            {
                m_dontDestroy_widgetRepository.BhvFixedUpdate(dt);
            }

            if (m_widgetRepository != null)
            {
                m_widgetRepository.BhvFixedUpdate(dt);
            }
        }

        public void BhvLateFixedUpdate(float dt)
        {
            if (m_dontDestroy_widgetRepository != null)
            {
                m_dontDestroy_widgetRepository.BhvLateFixedUpdate(dt);
            }

            if (m_widgetRepository != null)
            {
                m_widgetRepository.BhvLateFixedUpdate(dt);
            }
        }

        #endregion IBhvUpdatable

        public Dictionary<string, WidgetBase> GetWidgets()
        {
            return m_widgetRepository.Widgets;
        }

        void InitializeUiPanel()
        {
            List<GameObject> rootGameObjects = new List<GameObject>();
            if (m_root != null)
            {
                rootGameObjects.Add(m_root);
            }
            else
            {
                rootGameObjects = GameObjectFactory.GetRootGameObject()
                    .Where(c => c.layer == LayerMask.NameToLayer("UI")).ToList();
                if (rootGameObjects == null || rootGameObjects.Any() == false)
                {
                    m_root = new GameObject("UIPanel");
                    m_root.transform.SetParent(
                        m_widgetManager.RootObject != null
                            ? m_widgetManager.RootObject.transform
                            : m_widgetManager.Setting.transform, true);

                    GameObject staticPanel = new GameObject("StaticPanel");
                    staticPanel.transform.SetParent(m_root.transform, true);

                    GameObject dynamicPanel = new GameObject("DynamicPanel");
                    dynamicPanel.transform.SetParent(m_root.transform, true);

                    rootGameObjects.Add(m_root);
                }
            }

            m_staticPanel = null;
            m_dynamicPanel = null;

            for (int i = 0; i < rootGameObjects.Count; ++i)
            {
                Transform[] transforms =
                    ComponentFactory.GetChildComponents<Transform>(rootGameObjects[i], IfNotExist.ReturnNull);
                if (transforms == null)
                {
                    continue;
                }

                for (int j = 0; j < transforms.Length; ++j)
                {
                    if (m_staticPanel == null && transforms[j].name
                            .IndexOf("StaticPanel", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        m_staticPanel = transforms[j].gameObject;
                    }

                    if (m_dynamicPanel == null && transforms[j].name
                            .IndexOf("DynamicPanel", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        m_dynamicPanel = transforms[j].gameObject;
                    }

                    if (m_staticPanel != null && m_dynamicPanel != null)
                    {
                        return;
                    }
                }
            }
        }


        void InitializeWidget()
        {


            LoadingWidget loadingWidget = LoadWiget<LoadingWidget>("System/LoadingWidget", true);
            if (loadingWidget != null)
            {
                loadingWidget.Hide();
            }

            MessageBoxWidget messageBoxWidget = LoadWiget<MessageBoxWidget>("System/MessageBoxWidget", true);
            if (messageBoxWidget != null)
            {
                messageBoxWidget.Hide();
            }

            ConnectionWidget connectionWidget = LoadWiget<ConnectionWidget>("System/ConnectionWidget", true);
            if (connectionWidget != null)
            {
                connectionWidget.Hide();
            }

        }


        public void FinalizeWidgets(bool dontDestroy)
        {
            List<WidgetBase> removeWidgets = new List<WidgetBase>();
            m_widgetRepository.GetWidgets(ref removeWidgets);
            m_widgetRepository.Terminate();

            if (dontDestroy)
            {
                m_dontDestroy_widgetRepository.GetWidgets(ref removeWidgets);
                m_dontDestroy_widgetRepository.Terminate();
            }

            for (int i = 0; i < removeWidgets.Count; ++i)
            {
                if (removeWidgets[i] != null)
                {
                    FinalizeWidget(null, removeWidgets[i]);

                    GameObjectFactory.Destroy(removeWidgets[i].gameObject);
                }
            }
            removeWidgets.Clear();
        }

        private void FinalizeWidget(Transform rootTransform, WidgetBase widget)
        {
            if (widget == null || widget.transform == null)
            {
                return;
            }

            widget.FinalizeWidget();

            widget.transform.SetParent(rootTransform, true);
            widget.transform.localPosition = Vector3.zero;
            widget.transform.localRotation = Quaternion.identity;
        }

        public T CreateWidget<T>(string path, bool dontDestroyOnLoad = false) where T : WidgetBase
        {
            WidgetBase widget = FindWidget(path);

            if (widget != null)
            {
                InitWidget(path, widget, null);

                if (widget.IsPopupType)
                {
                    RemoveWidget(widget);
                    AddWidget(path, widget, dontDestroyOnLoad);
                }

                return widget as T;
            }
            else
            {
                return LoadWiget<T>(path, dontDestroyOnLoad);
            }
        }

        //public void CreateWidgetAsync<T>(string path, bool dontDestroyOnLoad, System.Action<T> action) where T : WidgetBase
        //{
        //    WidgetBase widget = FindWidget(typeof(T).ToString());

        //    if (widget != null)
        //    {
        //        InitWidget(widget, null);

        //        if (widget.IsPopupType)
        //        {
        //            RemoveWidget(widget);
        //            AddWidget(widget, dontDestroyOnLoad);
        //        }

        //        if (action != null)
        //        {
        //            action(widget as T);
        //        }
        //    }
        //    else
        //    {
        //        m_uiManager.Setting.StartCoroutine(LoadWigetAsync<T>(path, dontDestroyOnLoad, action));
        //    }
        //}

        public IEnumerator OnCreateWidgetAsync<T>(string path, System.Action<T> action, bool dontDestroyOnLoad)
            where T : WidgetBase
        {
            WidgetBase widget = FindWidget(path);

            if (widget != null)
            {
                InitWidget(path, widget, null);

                if (widget.IsPopupType)
                {
                    RemoveWidget(widget);
                    AddWidget(path, widget, dontDestroyOnLoad);
                }

                action(widget as T);
            }
            else
            {
                yield return LoadWigetAsync<T>(path, dontDestroyOnLoad, action);
            }
        }

        protected T LoadWiget<T>(string path, bool dontDestroyOnLoad) where T : WidgetBase
        {
            PrefabResource resource = Global.ResourceMgr.CreateUIResource(path, ePath.UI, dontDestroyOnLoad);

            T widget = null;

            if (resource != null)
            {
                widget = ComponentFactory.GetComponent<T>(GameObject.Instantiate(resource.ResourceData) as GameObject,
                    IfNotExist.ReturnNull);
            }

            if (widget != null)
            {
                InitWidget(path, widget, null);

                AddWidget(path, widget, dontDestroyOnLoad);
            }

            return widget;
        }

        protected IEnumerator LoadWigetAsync<T>(string path, bool dontDestroyOnLoad, System.Action<T> action)
            where T : WidgetBase
        {
            PrefabResource resource = null;
            yield return Global.ResourceMgr.CreateUIResourceAsync(path, ePath.UI, dontDestroyOnLoad,
                o => { resource = o; });

            WidgetBase widget = null;
            if (resource != null)
            {
                widget = ComponentFactory.GetComponent<T>(GameObject.Instantiate(resource.ResourceData) as GameObject,
                    IfNotExist.ReturnNull);
            }

            if (widget != null)
            {
                InitWidget(path, widget, null);

                AddWidget(path, widget, dontDestroyOnLoad);
            }

            action(widget as T);
        }


        protected void InitWidget(string path, WidgetBase widget, System.Action<WidgetBase> action)
        {
            if (widget == null)
                return;

            if (widget.IsPopupType == true)
            {
                widget.transform.SetParent(m_dynamicPanel != null ? m_dynamicPanel.transform : m_root.transform, true);
            }
            else
            {
                widget.transform.SetParent(m_staticPanel != null ? m_staticPanel.transform : m_root.transform, true);
            }

            if (Global.SceneMgr != null && Global.SceneMgr.CurrentScene != null)
            {
                widget.SetCamera(Global.SceneMgr.CurrentScene.UICamera, ++Global.SceneMgr.CurrentScene.WidgetLayer);
            }
            else
            {
                widget.SetCamera(null, 100);
            }

            widget.transform.localPosition = Vector3.zero;
            widget.transform.localRotation = Quaternion.identity;

            widget.InitializeWidget(path);

            //m_currentWidget = widget;

            if (action != null)
            {
                action(widget);
            }
        }


        protected void AddWidget(string path, WidgetBase widget, bool dontDestroyOnLoad)
        {
            if (m_widgetRepository == null || m_dontDestroy_widgetRepository == null)
            {
                return;
            }

            string widgetName = path;

            if (FindWidget(widgetName) != null)
            {
                return;
            }

            if (dontDestroyOnLoad)
            {
                m_dontDestroy_widgetRepository.Insert(widget);
            }
            else
            {
                m_widgetRepository.Insert(widget);
            }
        }

        protected bool RemoveWidget(WidgetBase widget)
        {
            if (m_widgetRepository == null || m_dontDestroy_widgetRepository == null)
            {
                return false;
            }

            if (m_widgetRepository.Remove(widget))
            {
                return true;
            }
            if (m_dontDestroy_widgetRepository.Remove(widget))
            {
                return true;
            }
            return false;
        }

        public WidgetBase FindWidget(string widgetType)
        {
            if (m_widgetRepository == null || m_dontDestroy_widgetRepository == null)
            {
                return null;
            }

            string widgetName = UIPathToName(widgetType);

            WidgetBase widget;
            if (m_widgetRepository.Get(widgetName, out widget))
            {
                return widget;
            }
            if (m_dontDestroy_widgetRepository.Get(widgetName, out widget))
            {
                return widget;
            }
            return null;
        }


        public void HideAllWidgets(float deactiveTime = 0.0f)
        {
            if (m_widgetRepository == null || m_dontDestroy_widgetRepository == null)
            {
                return;
            }

            m_widgetRepository.HideAllWidgets(deactiveTime);
            //m_dontDestroy_widgetRepository.HideAllWidgets(deactiveTime);
        }

        public bool OnMessage(IMessage message)
        {
            if (m_widgetRepository == null || m_dontDestroy_widgetRepository == null)
            {
                return false;
            }

            if (m_widgetRepository.OnMessage(message))
            {
                return true;
            }
            if (m_dontDestroy_widgetRepository.OnMessage(message))
            {
                return true;
            }

            return false;
        }


        private static string UIPathToName(string path)
        {
            string name = string.Empty;
            if (path.Contains("/"))
            {
                string[] split = path.Split('/');
                name = split[split.Length - 1];
            }
            else if (path.Contains("\\"))
            {
                string[] split = path.Split('\\');
                name = split[split.Length - 1];
            }
            else if (path.Contains("."))
            {
                string[] split = path.Split('.');
                name = split[split.Length - 1];
            }
            else
            {
                name = path;
            }
            return name;
        }
    }
}