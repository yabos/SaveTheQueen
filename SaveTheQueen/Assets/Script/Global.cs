#pragma warning disable 0162
#pragma warning disable 0219

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aniz.Cam;
using Aniz.Contents;
using Aniz.Data;
using Aniz.Event;
using Aniz.Factory;
using Aniz.Graph;
using Aniz.Resource;
using Lib.Event;
using Lib.Pattern;
using Aniz.Widget;


public enum eLayerMask
{
    BG = 8,
    Static = 10,
    Dynamic = 11,
    Actor = 12,
    UI = 14,
    Max,
}



public enum eGameLayer
{
    BG,
    BGObject,
    Game,
    GameObject,
    Text,
    UI,
    Round,
    Max,
}

public class Global : SingletonMonoBehaviour<Global>
{

    #region Public Fields

    public bool UseDebugLog = false;

    [ConditionalHideInspector("UseDebugLog", true)]
    public string UseDebugLogTag = "";

    [ColorUsage(true, true, 0f, 8f, 0.125f, 3f), ConditionalHideInspector("UseDebugLog", true)]
    public Color UseDebugLogColor;


    #endregion // Public Fields

    #region Private Fields

    protected bool m_isInitialized = false;

    private bool m_hasSetOriginalScreenResolution = false;

    private const int m_highMaxResolutionWidth = 1280;
    private int m_originalScreenWidth = 1280;
    private int m_originalScreenHeight = 720;



    #endregion // Private Fields

    #region Static Methods

    protected static System.Threading.Thread m_mainThread = null;
    public static System.Threading.Thread MainThread { get { return m_mainThread; } }
    public static bool IsMainThread
    {
        get
        {
#if UNITY_EDITOR
            if (m_mainThread == null) return true;
#endif //UNITY_EDITOR
            return (System.Threading.Thread.CurrentThread == m_mainThread);
        }
    }

#if UNITY_EDITOR
    public static void CheckGlobalForEditor()
    {
        return;

        if (Global.Instance != null || Global.IsMainThread == false)
        {
            return;
        }

        if (!Application.isPlaying && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            GameObject globalGameObject = GameObject.Find("EditorGlobal") as GameObject;

            Global global = null;
            if (globalGameObject != null)
            {
                global = globalGameObject.GetComponent<Global>();
            }

            if (global == null)
            {
                globalGameObject = new GameObject();
                globalGameObject.name = "EditorGlobal";
                global = globalGameObject.AddComponent<Global>();
            }

            global.InitializeForEditor();
        }
    }

    public static void ClearGlobalForEditor()
    {
        return;

        if (Global.Instance == null || Global.IsMainThread == false)
        {
            return;
        }

        if (!Application.isPlaying)
        {
            Global global = null;

            GameObject globalGameObject = GameObject.Find("EditorGlobal") as GameObject;
            if (globalGameObject != null)
            {
                global = globalGameObject.GetComponent<Global>();
            }

            if (global != null)
            {
                global.FinalizeForEditor();
            }
        }
    }
#endif // UNITY_EDITOR



    #endregion // Static Methods

    #region Static Properites

    public static GameObject GameObject
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
            if (m_instance != null)
            {
                return m_instance.gameObject;
            }
            else
            {
                return null;
            }
#endif // UNITY_EDITOR
            return m_instance.gameObject;
        }
    }

    private TimeManager m_timeManager = null;
    public static TimeManager TimeMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_timeManager;
        }
    }

    private ResourceManager m_resourceManager = null;
    public static ResourceManager ResourceMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_resourceManager;
        }
    }

    private InputManager m_inputManager = null;
    public static InputManager InputMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_inputManager;
        }
    }

    private NotificationManager m_notificationManager = null;
    public static NotificationManager NotificationMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_notificationManager;
        }
    }


    private NodeGraphManager m_nodeGraphManager = null;
    public static NodeGraphManager NodeGraphMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_nodeGraphManager;
        }
    }

    private FactoryManager m_factoryManager = null;
    public static FactoryManager FactoryMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_factoryManager;
        }
    }

    private CameraManager m_cameraManager = null;
    public static CameraManager CameraMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_cameraManager;
        }
    }


    private SceneManager m_sceneManager = null;
    public static SceneManager SceneMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_sceneManager;
        }
    }

    private WidgetManager m_widgetManager = null;
    public static WidgetManager WidgetMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_widgetManager;
        }
    }


    private GameManager m_gameManager = null;
    public static GameManager GameMgr
    {
        get
        {
#if UNITY_EDITOR
            CheckGlobalForEditor();
#endif // UNITY_EDITOR
            return m_instance.m_gameManager;
        }
    }

    #endregion // Static Properties

    #region Events

    void Awake()
    {
        Log("Awake()");

#if UNITY_EDITOR
        if (gameObject.name.Equals("EditorGlobal"))
        {
            Log(StringUtil.Format("Awake({) -> EditorGlobal Destroy {0}", name));

            FinalizeManager();
            GameObjectFactory.Destroy(gameObject);
            return;
        }
#endif // UNITY_EDITOR

        if (null != m_instance)
        {
            FinalizeManager();
            GameObjectFactory.Destroy(gameObject);
            return;
        }
        else
        {
            m_instance = this;
        }

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        Global.m_mainThread = System.Threading.Thread.CurrentThread;

        Application.targetFrameRate = 30;

        if ((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.IPhonePlayer))
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        SetResoultion();

        if (Application.isPlaying)
        {
            DontDestroyOnLoad(this);
        }

        LEMath.Init();
        InitializeManager();
    }

    void Start()
    {
        Log("Start()");

        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                if (m_managers[i].Value is WidgetManager) continue;

                m_managers[i].Value.BhvOnEnter();
            }
        }
    }

    void OnApplicationQuit()
    {
        Log("OnApplicationQuit()");

#if UNITY_EDITOR
        QualitySettings.shadowProjection = ShadowProjection.CloseFit;
#endif // UNITY_EDITOR
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!m_isInitialized)
        {
            return;
        }

        Log(StringUtil.Format("OnApplicationFocus({0})", focusStatus));

        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.OnAppFocus(focusStatus);
            }
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!m_isInitialized)
        {
            return;
        }

        Log(StringUtil.Format("OnApplicationPause({0})", pauseStatus));

        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.OnAppPause(pauseStatus);
            }
        }
    }

    void Update()
    {
        if (!m_isInitialized)
        {
            return;
        }

        float delta = Time.deltaTime;
        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.BhvUpdate(delta);
            }
        }
    }

    void LateUpdate()
    {
        if (!m_isInitialized)
        {
            return;
        }
        float delta = Time.deltaTime;

        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.BhvLateUpdate(delta);
            }
        }
    }

    void FixedUpdate()
    {
        if (!m_isInitialized)
        {
            return;
        }

        float delta = Time.fixedDeltaTime;


        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.BhvFixedUpdate(delta);
            }
        }

        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.BhvLateFixedUpdate(delta);
            }
        }
    }

    private void OnEnable()
    {
        Log("OnEnable()");

#if UNITY_EDITOR
        if (gameObject.name.Equals("EditorGlobal"))
        {
            Log("OnEnable() = -> m_instance = this");
            m_instance = this;
        }
#endif // UNITY_EDITOR
    }

    private void OnDisable()
    {
        Log("OnDisable()");

#if UNITY_EDITOR
        if (gameObject.name.Equals("EditorGlobal") && m_instance == this)
        {
            Log("OnDisable() = -> m_instance = null");
            m_instance = null;
        }
#endif // UNITY_EDITOR
    }

    void OnDestroy()
    {
        Log("OnDestroy()");

        FinalizeManager();
    }

    #endregion // Events

    #region Methods

    public IEnumerator OnAppPreload(System.Action completed)
    {
        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null)
            {
                yield return m_managers[i].Value.OnAppPreload();
            }
        }

        yield return new WaitForEndOfFrame();

        if (completed != null)
        {
            completed();
        }
    }

    public void AppPreload(System.Action completed)
    {
        StartCoroutine(OnAppPreload(completed));
    }


    private bool MessageProcess(IMessage message)
    {
        for (int i = 0; i < m_managers.Count; ++i)
        {
            if (m_managers[i].Value != null && m_managers[i].Value.OnMessage(message))
            {
                return true;
            }
        }
        return false;
    }

    protected List<KeyValuePair<string, ManagerBase>> m_managers = new List<KeyValuePair<string, ManagerBase>>();

    protected bool CreateManager<T1, T2>(ref T1 manager) where T1 : ManagerBase, new() where T2 : ManagerSettingBase
    {
        if (manager != null)
        {
            // error message
            return false;
        }

        T2 setting = ComponentFactory.GetChildComponent<T2>(GameObject, IfNotExist.ReturnNull);
        if (setting == null)
        {
            setting = ComponentFactory.GetComponent<T2>(GameObject, IfNotExist.AddNew);
            if (Application.isPlaying)
            {
                LogWarning(StringUtil.Format("{0} component is automatically created", typeof(T2).ToString()));
            }
        }

        manager = new T1();
        manager.OnAppStart(setting);
        return (manager != null) ? true : false;
    }

    protected void DestroyManager<T>(ref T manager) where T : ManagerBase
    {
        if (manager == null)
        {
            // error message
            return;
        }

        manager.OnAppEnd();
        RemoveManager(manager);
        manager = null;
    }

    protected void AddManager(ManagerBase manager)
    {
        KeyValuePair<string, ManagerBase> keyValuePair = m_managers.FirstOrDefault(c => (c.Key.IndexOf(manager.Name, System.StringComparison.OrdinalIgnoreCase) >= 0));
        if (keyValuePair.Value != null)
        {
            return;
        }

        m_managers.Add(new KeyValuePair<string, ManagerBase>(manager.Name, manager));
    }

    protected void RemoveManager(ManagerBase manager)
    {
        RemoveManager(manager.Name);
    }

    protected void RemoveManager(string name)
    {
        KeyValuePair<string, ManagerBase> keyValuePair = m_managers.FirstOrDefault(c => (c.Key.IndexOf(name, System.StringComparison.OrdinalIgnoreCase) >= 0));
        if (keyValuePair.Value == null)
        {
            return;
        }

        m_managers.Remove(keyValuePair);
    }

#if UNITY_EDITOR

    public void InitializeForEditor()
    {
        Log("InitializeForEditor()");

        m_instance = this;

        InitializeManager();
    }

    public void FinalizeForEditor()
    {
        Log("FinalizeForEditor()");

        FinalizeManager();

        GameObjectFactory.Destroy(gameObject);
        m_instance = null;
    }

#endif // UNITY_EDITOR

    void InitializeManager()
    {
        Log("InitializeManager()");

        if (m_managers != null)
        {
            m_managers.Clear();
        }

        // Input Manager
        {
            if (CreateManager<InputManager, InputManagerSetting>(ref m_inputManager))
            {
                AddManager(m_inputManager);
            }
        }

        // Time Manager
        {
            if (CreateManager<TimeManager, ManagerSettingBase>(ref m_timeManager))
            {
                AddManager(m_timeManager);
            }
        }

        // Resource Manager
        {
            if (CreateManager<ResourceManager, ManagerSettingBase>(ref m_resourceManager))
            {
                AddManager(m_resourceManager);
            }
        }


        //// Factory Manager
        {
            if (CreateManager<FactoryManager, FactoryManagerSetting>(ref m_factoryManager))
            {
                AddManager(m_factoryManager);
            }
        }

        // Notification Manager
        {
            if (CreateManager<NotificationManager, NotificationManagerSetting>(ref m_notificationManager))
            {
                AddManager(m_notificationManager);
            }
        }

        // Camera Manager
        {
            if (CreateManager<CameraManager, CameraSetting>(ref m_cameraManager))
            {
                AddManager(m_cameraManager);
            }
        }

        // SceneGraph Manager
        {
            if (CreateManager<NodeGraphManager, NodeGraphManagerSetting>(ref m_nodeGraphManager))
            {
                AddManager(m_nodeGraphManager);
            }
        }

        // Game Manager
        {
            if (CreateManager<GameManager, GameManagerSetting>(ref m_gameManager))
            {
                AddManager(m_gameManager);
            }
        }

        // UI Manager
        {
            if (CreateManager<WidgetManager, WidgetManagerSetting>(ref m_widgetManager))
            {
                AddManager(m_widgetManager);
            }
        }

        // Page Manager
        {
            if (CreateManager<SceneManager, SceneManagerSetting>(ref m_sceneManager))
            {
                AddManager(m_sceneManager);
            }
        }
        DataManager.Instance.LoadData();

        m_isInitialized = true;
    }

    void FinalizeManager()
    {
        Log("FinalizeManager()");

        for (int i = m_managers.Count - 1; i >= 0; --i)
        {
            if (m_managers[i].Value != null)
            {
                m_managers[i].Value.BhvOnLeave();
            }
        }


        // Page Manager
        {
            DestroyManager<SceneManager>(ref m_sceneManager);
        }

        // UI Manager
        {
            DestroyManager(ref m_widgetManager);
        }

        // Game Manager
        {
            DestroyManager(ref m_gameManager);
        }

        // SceneGraph Manager
        {
            DestroyManager<NodeGraphManager>(ref m_nodeGraphManager);
        }

        // Camera Manager
        {
            DestroyManager(ref m_cameraManager);
        }

        // Notification Manager
        {
            DestroyManager<NotificationManager>(ref m_notificationManager);
        }

        // Factory Manager
        {
            DestroyManager<FactoryManager>(ref m_factoryManager);
        }

        // Resource Manager
        {
            DestroyManager<ResourceManager>(ref m_resourceManager);
        }

        // Time Manager
        {
            DestroyManager<TimeManager>(ref m_timeManager);
        }

        // Input Manager
        {
            DestroyManager<InputManager>(ref m_inputManager);
        }

        m_isInitialized = false;
    }

    void SetResoultion()
    {
        if (!m_hasSetOriginalScreenResolution)
        {
            m_hasSetOriginalScreenResolution = true;
            m_originalScreenWidth = Screen.width;
            m_originalScreenHeight = Screen.height;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int maxResolutionWidth = m_highMaxResolutionWidth;
            int modwidth = m_originalScreenWidth;
            int modheight = m_originalScreenHeight;
            if (m_originalScreenWidth > maxResolutionWidth)
            {
                modwidth = maxResolutionWidth;
                modheight = (int)(m_originalScreenHeight * ((float)modwidth / (float)m_originalScreenWidth));
            }
            else
            {
                modwidth = m_originalScreenWidth;
                modheight = m_originalScreenHeight;
            }

            Screen.SetResolution(modwidth, modheight, Screen.fullScreen);
        }

        Log(StringUtil.Format("SetResoultion({0}, {1}, {2})", Screen.width, Screen.height, Screen.fullScreen));
    }

    #endregion // Methods

    #region Log Methods
    public void Log(string msg)
    {
        msg = StringUtil.Format("<color=#ffffffff>[Global] {0}</color>", msg);
        if (UseDebugLog)
        {
            Debug.Log(msg);
        }
    }

    public void LogWarning(string msg)
    {
        msg = StringUtil.Format("<color=#ffff00ff>[Global] {0}</color>", msg);
        if (UseDebugLog)
        {
            Debug.LogWarning(msg);
        }
    }

    public void LogError(string msg)
    {
        msg = StringUtil.Format("<color=#ff0000ff>[Global] {0}</color>", msg);
        if (UseDebugLog)
        {
            Debug.LogError(msg);
        }
    }

    #endregion //Log Methods


}

