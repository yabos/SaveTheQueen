using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Aniz;
using Aniz.Cam;
using Aniz.Cam.Info;
using Aniz.Contents.Entity.Info;
using Aniz.Data;
using Aniz.NodeGraph.Level.Group;
using Aniz.NodeGraph.Level.Group.Info;
using Aniz.InputButton.Controller;
using Aniz.Resource;
using Lib.InputButton;
using table.db;

public class JokerToolEditor : EditorWindow
{
    #region Enums
    private enum eQualityLevel
    {
        Low,
        Medium,
        High
    }
    private enum eActorEditorType
    {
        Character,
        Monster,
    }

    private enum eViewType
    {
        Map,
        State,
        FX,
        AI,
    }
    #endregion Enums

    #region Member variables & Properties

    private const string AnimationEventPathFormat = "Assets/AssetBundle/Actor/{0}_animationevent.asset";

    private static JokerToolEditor m_instance = null;
    public static JokerToolEditor Instance
    {
        get
        {
            if (m_instance != null)
            {
                return m_instance;
            }
            return null;
        }
    }

    //private static UpdateManagerWindow m_updateManagerWindow = null;

    //private eQualityLevel m_qualityLevel = eQualityLevel.High;


    private eActorEditorType m_actorEditorType = eActorEditorType.Character;

    private int[] m_selectedLookIndex;

    private int m_selectedStateIndex = 0;

    private int m_selectedStateTransitionIndex = 0;

    private string m_selectedStateTransition = string.Empty;

    private List<BaseEntityInfo> m_actorInfoList = new List<BaseEntityInfo>();

    private Vector2 m_lookDBInfoScrollPosition = Vector2.zero;

    private Vector2 m_actorFsmStateSetScrollPosition = Vector2.zero;

    private Vector2 m_actorFsmStateTransitionScrollPosition = Vector2.zero;

    private Vector2 m_stateScrollPosition = Vector2.zero;

    private FSMStateSet m_curFsmStateSet = new FSMStateSet();

    private FSMStateElement m_selectFsmStateElement;
    public FSMStateElement SelectFsmStateElement
    {
        get { return m_selectFsmStateElement; }
    }

    private Dictionary<eViewType, ISubCharacterViewer> m_characterViewerDic = new Dictionary<eViewType, ISubCharacterViewer>();

    private eViewType m_viewType = eViewType.State;

    private ISubCharacterViewer m_currentSubCharacterViewer;

    private Vector2 m_leftScrollPosition;

    private bool m_cameraTarget = true;

    private bool m_hero = true;
    private bool m_replayMode = false;

    //private bool m_useSpawnWithTimeLineEditor = false;

    private bool m_stateLoop = false;

    private float m_animationSpeed = 1.0f;

    //private float m_moveRatio = 1.0f;

    private int m_currentFrame = 0;
    public int CurrentFrame
    {
        get { return m_currentFrame; }
    }

    private float m_currentTime = 0;

    private float m_currentNormalizeTime = 0;

    private ActorRoot m_actorRootLook = null;
    public ActorRoot CurrentActorRootLook
    {
        get { return m_actorRootLook; }
    }

    private PlayerButtonController m_playerButtonController;
    //private CameraStrategy.E_State m_cameraState = CameraStrategy.E_State.Game;

    private bool playLookDeveloper = false;

    private KeyCode RequestedKey { get; set; }

    private bool m_isRefreshGlobalManager = false;

    #endregion Member variables & Properties

    [MenuItem("Joker/Tool %#q", false, MenuItemUtil.CharacterToolGroup + 3)]
    public static void ShowCharacterViewer()
    {
        RefreshPanel();
    }

    public static void RefreshPanel()
    {
        JokerToolEditor panel = GetOpenEditor();
        panel.ShowPopup();
    }

    private static JokerToolEditor GetOpenEditor()
    {
        if (null == m_instance)
        {
            m_instance = EditorWindow.GetWindow<JokerToolEditor>("Character Tool");
        }
        return m_instance;
    }

    #region Called by Unity

    void Awake()
    {
        ResetSelectLook();
        CreateSubCharacterViewer();
    }

    void OnEnable()
    {
        if (m_instance != null)
        {
            return;
        }

        m_instance = this;

        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;

        OnEnableCharacterView();
    }

    void OnDisable()
    {
        if (m_instance == null)
        {
            return;
        }

        m_instance = null;

        EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;

        OnDisableCharacterView();
    }

    void Update()
    {
        if (playLookDeveloper)
        {
            playLookDeveloper = false;

            Scene scene = EditorSceneManager.GetActiveScene();
            if (scene.name.Contains("Tool") == false)
            {
                string lookDeveloperScene = "Assets/Scene/Tool/Tool.unity";

                if (EditorSceneManager.GetActiveScene().name != lookDeveloperScene)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(lookDeveloperScene);
                }
            }

            EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        if (null != m_actorRootLook)
        {
            //var actorAnimator = m_actorLook.Mesh.Anim;
            //if (null != actorAnimator)
            //{
            //    AnimatorStateInfo animatorStateInfo = actorAnimator.GetCurrentAnimatorStateInfo(0);
            //    m_currentNormalizeTime = animatorStateInfo.normalizedTime - (int)animatorStateInfo.normalizedTime;
            //    m_currentTime = animatorStateInfo.length * m_currentNormalizeTime;
            //    m_currentFrame = Mathf.RoundToInt(m_currentTime / TimelineWindow.FrameRate);

            //    if (m_stateLoop && null != m_selectFsmStateElement)
            //    {
            //        if (false == animatorStateInfo.IsName(m_selectFsmStateElement.StateName))
            //        {
            //            CurrentStatePlay();
            //        }
            //    }

            //    if (actorAnimator.speed != m_animationSpeed)
            //    {
            //        actorAnimator.speed = m_animationSpeed;
            //    }
            //}
        }
    }

    void OnGUI()
    {
        RefreshGlobalManager();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(500));
            {
                m_leftScrollPosition = EditorGUILayout.BeginScrollView(m_leftScrollPosition);
                {
                    OnGUICharacterViewInfoList();

                    if (m_actorRootLook != null)
                    {
                        OnGUIFsmStateSet();

                        GUILayout.BeginHorizontal();
                        {
                            OnGUIFSMState();

                            OnGUIFSMTransition();
                        }
                        GUILayout.EndHorizontal();

                        OnGUIStateInfo();

                        Repaint();
                    }

                    OnGUIOption();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            OnGUISubEditor();

            OnGUIHandleKeys();
        }
        EditorGUILayout.EndHorizontal();
    }

    #endregion Called by Unity

    #region OnGUI Func

    private void OnGUICharacterViewInfoList()
    {
        GUI.enabled = Application.isPlaying;

        EditorGUILayout.BeginVertical("box");
        {
            GUI.color = Color.cyan;
            eActorEditorType newState = (eActorEditorType)GUILayout.Toolbar((int)m_actorEditorType, System.Enum.GetNames(typeof(eActorEditorType)));
            GUI.color = Color.white;
            if (m_actorEditorType != newState)
            {
                m_actorEditorType = newState;
                RefreshCharacterList();
            }

            m_lookDBInfoScrollPosition = EditorGUILayout.BeginScrollView(m_lookDBInfoScrollPosition, GUILayout.MinHeight(160));
            {
                for (int i = 0; i < m_actorInfoList.Count; ++i)
                {
                    BaseEntityInfo actorInfo = m_actorInfoList[i];

                    bool selected = i == GetSelectedLookIndex(m_actorEditorType);

                    EditorGUIUtil.Label(actorInfo.Name, selected, 0, 2, EditorGUIUtil.GetSelectedTextColorByEditorSkin(selected));
                    if (EditorGUIEventUtil.IsLastRectClicked())
                    {
                        CreateActorBySelectLookID(i, m_actorEditorType);
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            GUI.enabled = true;
            string play = Application.isPlaying == false ? "Play" : "Stop";
            Color color = Application.isPlaying == false ? Color.green : Color.red;
            GUI.color = color;
            if (GUILayout.Button(play, GUILayout.Height(25)))
            {
                playLookDeveloper = true;
            }
            GUI.color = Color.white;
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIFsmStateSet()
    {
        GUILayout.BeginVertical("box", GUILayout.MinHeight(50));
        {
            GUILayout.Label("State Set", EditorStyles.boldLabel);
            m_actorFsmStateSetScrollPosition = GUILayout.BeginScrollView(m_actorFsmStateSetScrollPosition);
            {
                if (null != m_curFsmStateSet)
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(m_curFsmStateSet.name + " #"))
                        {
                            SelectStateSet(m_curFsmStateSet);
                        }

                        string weight = m_curFsmStateSet.weight.ToString();
                        weight = GUILayout.TextField(weight, GUILayout.Width(30));
                        m_curFsmStateSet.weight = float.Parse(weight);
                    }
                    GUILayout.EndHorizontal();
                }

            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }

    private void OnGUIFSMState()
    {
        if (m_curFsmStateSet == null)
            return;

        EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(200), GUILayout.MinHeight(200));
        {
            GUILayout.Label("State List", EditorStyles.boldLabel);
            m_stateScrollPosition = GUILayout.BeginScrollView(m_stateScrollPosition);
            {
                var stateElementList = m_curFsmStateSet.stateElementList;
                for (int i = 0; i < stateElementList.Count; ++i)
                {
                    string name = stateElementList[i].StateName;

                    bool selected = i == m_selectedStateIndex;
                    EditorGUIUtil.Label(name, selected, 0, 2, EditorGUIUtil.GetSelectedTextColorByEditorSkin(selected));

                    if (EditorGUIEventUtil.IsLastRectClicked())
                    {
                        SelectState(i);
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIFSMTransition()
    {
        if (m_selectFsmStateElement == null)
            return;

        GUILayout.BeginVertical("box", GUILayout.MinHeight(200));
        {
            GUILayout.Label("State Transition", EditorStyles.boldLabel);
            m_actorFsmStateTransitionScrollPosition = GUILayout.BeginScrollView(m_actorFsmStateTransitionScrollPosition);
            {
                for (int i = 0; i < m_selectFsmStateElement.LstGlobalTransition.Count; ++i)
                {
                    string name = m_selectFsmStateElement.LstGlobalTransition[i];

                    bool selected = i == m_selectedStateTransitionIndex;
                    EditorGUIUtil.Label(name, selected, 0, 2, selected ? Color.green : Color.red);
                    if (EditorGUIEventUtil.IsLastRectClicked())
                    {
                        SelectStateTransition(i);
                    }
                }

                for (int i = 0; i < m_selectFsmStateElement.LstTransition.Count; ++i)
                {
                    string name = m_selectFsmStateElement.LstTransition[i];

                    bool selected = i == m_selectedStateTransitionIndex;
                    EditorGUIUtil.Label(name, selected, 0, 2, EditorGUIUtil.GetSelectedTextColorByEditorSkin(selected));
                    if (EditorGUIEventUtil.IsLastRectClicked())
                    {
                        SelectStateTransition(i);
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }

    private void OnGUIStateInfo()
    {
        if (m_actorRootLook == null)
            return;

        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        {
            GUILayout.Label("State Info", EditorStyles.boldLabel);

            EditorGUI.indentLevel += 2;
            GUI.color = Color.green;
            //GUILayout.Label(string.Format("Sub-State Name : {0}", m_actorLook.Fsm.CurrentState));
            //GUILayout.BeginHorizontal();
            //{
            //    GUILayout.Label(string.Format("Sub-State Length : {0}", m_actorLook.Fsm.CurrentSubStateAnimLength));
            //    GUILayout.Label(string.Format("Is Loop? : {0}", m_actorLook.Fsm.IsCurrentSubStateLooping));
            //}
            //GUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUI.color = Color.yellow;
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(string.Format("CurrentFrame : {0}", m_currentFrame));
                GUILayout.Label(string.Format("CurrentTime : {0:F3}", m_currentTime));
                GUILayout.Label(string.Format("NormalizeTime : {0:F3}", m_currentNormalizeTime));
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
            EditorGUI.indentLevel -= 2;

            GUILayout.BeginHorizontal("box");
            {
                m_stateLoop = GUILayout.Toggle(m_stateLoop, "State Loop");
                EditorGUILayout.BeginVertical("box");
                {
                    m_animationSpeed = EditorGUILayout.FloatField("Speed : ", m_animationSpeed);
                    //m_moveRatio = EditorGUILayout.FloatField("Move : ", m_moveRatio);
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.color = Color.green;
            if (GUILayout.Button("Current State Play"))
            {
                CurrentStatePlay();
            }
            GUI.color = Color.white;
        }
        GUILayout.EndVertical();
    }

    private void OnGUIHandleKeys()
    {
        if (EditorWindow.focusedWindow == this)
        {
            if (Event.current != null && Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.UpArrow:
                        RequestedKey = KeyCode.UpArrow;
                        m_selectedStateIndex = (m_selectedStateIndex <= 0) ? 0 : --m_selectedStateIndex;
                        ScrollTo();
                        break;
                    case KeyCode.DownArrow:
                        RequestedKey = KeyCode.DownArrow;
                        m_selectedStateIndex = (m_selectedStateIndex >= m_curFsmStateSet.stateElementList.Count) ? (m_curFsmStateSet.stateElementList.Count - 1) : ++m_selectedStateIndex;
                        ScrollTo();
                        break;
                    case KeyCode.Return:
                        RequestedKey = KeyCode.Return;
                        CurrentStatePlay();
                        break;
                    case KeyCode.LeftArrow:
                        RequestedKey = KeyCode.LeftArrow;
                        break;
                    case KeyCode.RightArrow:
                        RequestedKey = KeyCode.RightArrow;
                        break;
                    default:
                        RequestedKey = KeyCode.None;
                        break;
                }
            }
            else
            {
                RequestedKey = KeyCode.None;
            }
        }
    }

    private void OnGUIOption()
    {
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        {
            GUILayout.Label("Option List", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            {
                GUI.color = Color.yellow;
                //GUI.enabled = null == m_selectAnimationEvent ? false : true;
                //if (GUILayout.Button("Animation Timeline Event (Ctrl+Shift+T)", GUILayout.Height(32)))
                //{
                //}

                //GUI.enabled = true;
                //if (GUILayout.Button("PlayMakerEditor Open", GUILayout.Height(32)))
                //{
                //}

                GUI.color = Color.white;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            {
                GUILayout.Label("Option", EditorStyles.boldLabel);

                //bool tmp = m_useSpawnWithTimeLineEditor;
                //m_useSpawnWithTimeLineEditor = GUILayout.Toggle(m_useSpawnWithTimeLineEditor, "Always Spawn With TimeLineWindow");
                //if (tmp != m_useSpawnWithTimeLineEditor)
                //    EditorPrefs.SetBool("UseSpawnWithTimeLineEditor", m_useSpawnWithTimeLineEditor);

                m_hero = GUILayout.Toggle(m_hero, "CreateHero", GUILayout.Width(100));


                if (Application.isPlaying)
                {
                    if (Global.CameraMgr != null && Global.CameraMgr.Impl != null)
                    {
                        if (Global.CameraMgr.Impl.Strategy.CurState.State != CameraStrategy.eState.Free)
                        {
                            m_cameraTarget = true;
                        }
                    }
                }

                bool newCameraTarget = GUILayout.Toggle(m_cameraTarget, "Camera Lock", GUILayout.Width(100));
                if (m_cameraTarget != newCameraTarget)
                {
                    m_cameraTarget = newCameraTarget;
                    if (m_cameraTarget)
                    {
                        if (m_actorRootLook != null)
                        {
                            Global.CameraMgr.SetTargetActor(m_actorRootLook);
                        }

                        //CameraStock stock = Global.CameraMgr.Impl.Strategy.CurState.CurrentInfo.GetConversionStock();
                        Global.CameraMgr.Impl.Strategy.Change(CameraStrategy.eState.Target);
                        //Global.CameraMgr.Impl.Strategy.SetStock(stock);
                    }
                    else
                    {
                        //m_cameraState = Global.CameraMgr.Impl.Strategy.CurrentState;
                        //CameraStock stock = Global.CameraMgr.Impl.Strategy.CurState.CurrentInfo.GetConversionStock();
                        Global.CameraMgr.Impl.Strategy.Change(CameraStrategy.eState.Free);
                        //Global.CameraMgr.Impl.Strategy.SetStock(stock);
                        Global.CameraMgr.DestoryTarget();
                    }
                }
            }

            bool replayMode = GUILayout.Toggle(m_replayMode, "Replay", GUILayout.Width(100));
            if (replayMode != m_replayMode)
            {
                m_replayMode = replayMode;
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }

    private void OnGUISubEditor()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
        {
            eViewType newState = (eViewType)GUILayout.Toolbar((int)m_viewType, System.Enum.GetNames(typeof(eViewType)));
            if (m_viewType != newState)
            {
                SetViewer(newState);
            }

            EditorGUILayout.BeginVertical("box");
            {
                wantsMouseMove = true;
                if (m_currentSubCharacterViewer != null)
                {
                    m_currentSubCharacterViewer.OnGUI();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

    #endregion OnGUI Func

    #region Func

    private void RefreshCharacterList()
    {
        TableLoad();
        ScriptCharcterLookDB(m_actorEditorType);
        ResetSelectLook();
    }

    private void TableLoad()
    {
        DataManager.Instance.LoadData();
    }

    private void ScriptCharcterLookDB(eActorEditorType actorEditorType)
    {
        m_actorInfoList.Clear();

        ResData resData = DataManager.Instance.GetScriptData<ResData>();
        CharacterData characterData = DataManager.Instance.GetScriptData<CharacterData>();

        if (actorEditorType == eActorEditorType.Character)
        {
            if (resData != null)
            {
                List<DB_SpriteData> lstDatas = resData.GetCharacterSpriteList();
                foreach (DB_SpriteData spriteData in lstDatas)
                {
                    if (spriteData.objType == E_ObjectType.Pc)
                    {
                        string Name = string.Format("{0}_{1}", spriteData.id, spriteData.assetName);
                        EntityAssetInfo entityAssetInfo = new EntityAssetInfo(IDFactory.GenerateActorID(), eLayerMask.Actor, spriteData.spriteName, spriteData.spriteName);

                        PCInfo pcInfo = new PCInfo(eCombatType.PC, entityAssetInfo, Name, entityAssetInfo.ID, null);
                        m_actorInfoList.Add(pcInfo);
                    }
                }
            }
        }
        if (actorEditorType == eActorEditorType.Monster)
        {
            if (resData != null)
            {
                List<DB_SpriteData> lstDatas = resData.GetCharacterSpriteList();
                foreach (DB_SpriteData spriteData in lstDatas)
                {
                    if (spriteData.objType == E_ObjectType.Monster)
                    {
                        string name = string.Format("{0}_{1}", spriteData.id, spriteData.assetName);
                        EntityAssetInfo entityAssetInfo = new EntityAssetInfo(IDFactory.GenerateActorID(), eLayerMask.Actor, spriteData.spriteName, spriteData.spriteName);

                        MonsterInfo monsterInfo = new MonsterInfo(eCombatType.Monster, entityAssetInfo, name, entityAssetInfo.ID, null);
                        m_actorInfoList.Add(monsterInfo);
                    }
                }
            }
        }
    }

    private void ResetSelectLook()
    {
        if (m_selectedLookIndex == null)
        {
            m_selectedLookIndex = new int[(int)eActorEditorType.Monster + 1];
        }
        for (int i = 0; i < (int)eActorEditorType.Monster + 1; i++)
        {
            m_selectedLookIndex[i] = -1;
        }
    }

    private void OnEnableCharacterView()
    {
        m_isRefreshGlobalManager = true;
        //m_useSpawnWithTimeLineEditor = EditorPrefs.GetBool("UseSpawnWithTimeLineEditor");

        CreateSubCharacterViewer();

        if (true == m_characterViewerDic.ContainsKey(m_viewType))
        {
            m_currentSubCharacterViewer = m_characterViewerDic[m_viewType];
            m_currentSubCharacterViewer.Init(this);

            foreach (KeyValuePair<eViewType, ISubCharacterViewer> keyValuePair in m_characterViewerDic)
            {
                keyValuePair.Value.OnEnable();
            }
        }
    }

    private void OnDisableCharacterView()
    {
        m_isRefreshGlobalManager = false;

        foreach (KeyValuePair<eViewType, ISubCharacterViewer> keyValuePair in m_characterViewerDic)
        {
            keyValuePair.Value.OnDisable();
        }

        m_curFsmStateSet = null;
        m_selectFsmStateElement = null;
        m_actorInfoList.Clear();
        m_characterViewerDic.Clear();
        if (m_actorRootLook != null)
        {
            Global.CameraMgr.DestoryTarget();
            Global.GameMgr.DestroyCombat(m_actorRootLook.Uid);
            m_actorRootLook = null;

            if (m_currentSubCharacterViewer != null)
                m_currentSubCharacterViewer.ChangeActor(null);
        }
    }

    private void RefreshGlobalManager()
    {
        if (m_isRefreshGlobalManager == false)
        {
            return;
        }

        if (EditorApplication.isPlaying == false)
        {
            return;
        }

        if (EditorApplication.isPlayingOrWillChangePlaymode == false)
        {
            return;
        }

        m_isRefreshGlobalManager = false;

        RefreshCharacterList();

        Global.InputMgr.System.EditMouseUpdate = true;
        Global.InputMgr.System.CallBackMouseEvent = OnMouseEvent;
    }

    private void CreateSubCharacterViewer()
    {
        if (m_characterViewerDic.Count <= 0 && m_characterViewerDic.Count < (int)eViewType.AI + 1)
        {
            m_characterViewerDic.Clear();
            //m_characterViewerDic.Add(eViewType.Map, new SubMapViewer());
            //m_characterViewerDic.Add(eViewType.AI, new SubCharacterAiViewer());
            //m_characterViewerDic.Add(eViewType.Skill, new SubCharacterSkillViewer());
            //m_characterViewerDic.Add(eViewType.UI, new SubCharacterUIViewer());
        }
    }

    private void SetViewer(eViewType viewType)
    {
        if (m_characterViewerDic.ContainsKey(viewType))
        {
            m_viewType = viewType;
            m_currentSubCharacterViewer = m_characterViewerDic[m_viewType];
            m_currentSubCharacterViewer.Init(this);

            if (m_actorRootLook != null)
            {

            }

            if (m_viewType == eViewType.Map)
            {
            }
        }
    }

    private void SelectStateTransition(int index)
    {
        if (m_selectFsmStateElement == null)
            return;

        m_selectedStateTransitionIndex = index;
        if (m_selectedStateTransitionIndex >= 0 && m_selectedStateTransitionIndex < m_selectFsmStateElement.LstTransition.Count)
        {
            m_selectedStateTransition = m_selectFsmStateElement.LstTransition[m_selectedStateTransitionIndex];
            //m_actorRootLook.Fsm.SendEvent(m_selectedStateTransition);
        }
    }

    public void SelectState(string name)
    {
        for (int i = 0; i < m_curFsmStateSet.stateElementList.Count; i++)
        {
            if (m_curFsmStateSet.stateElementList[i].StateName.Equals(name))
            {
                m_selectedStateIndex = i;
                m_selectFsmStateElement = m_curFsmStateSet.stateElementList[m_selectedStateIndex];
                CurrentStatePlay();
                break;
            }
        }
    }

    private void SelectState(int index, bool isPlay = true)
    {
        if (m_curFsmStateSet == null)
            return;

        m_selectedStateIndex = index;
        if (m_selectedStateIndex >= 0 && m_selectedStateIndex < m_curFsmStateSet.stateElementList.Count)
        {
            m_selectFsmStateElement = m_curFsmStateSet.stateElementList[m_selectedStateIndex];

            if (isPlay)
                CurrentStatePlay();

            //if (m_actorLook.Fsm.EventStates != null)
            //{
            //    AnimationEventState state = m_actorLook.Fsm.EventStates.GetState(m_selectFsmStateElement.StateName);
            //    SubCharacterStateViewer subCharacterStateViewer = m_characterViewerDic[eViewType.State] as SubCharacterStateViewer;
            //    subCharacterStateViewer.SetState(state, m_selectFsmStateElement);
            //}
        }
    }

    private void CurrentStatePlay()
    {
        if (m_selectFsmStateElement != null && string.IsNullOrEmpty(m_selectFsmStateElement.StateName) == false)
        {
            //m_actorLook.Fsm.PlayMaker.Fsm.Stop();
            //m_actorLook.Fsm.PlayMaker.Fsm.StartState = m_selectFsmStateElement.StateName;

            //if (AnimationEventUtil.HasDummyAnimState(m_selectFsmStateElement.StateName))
            //{
            //    m_actorLook.Fsm.EventStates.InitCacheSelectedState(m_selectFsmStateElement.StateName);
            //    m_actorLook.Mesh.OverrideAnimation(m_selectFsmStateElement.StateName);
            //}

            //m_actorLook.Fsm.PlayMaker.Fsm.Start();
            //m_actorLook.Fsm.OnFSMStateUpdate(true);
        }
    }

    private int GetSelectedLookIndex(eActorEditorType actorEditorType)
    {
        return m_selectedLookIndex[(int)actorEditorType];
    }

    private void SetSelectedLookIndex(eActorEditorType actorEditorType, int index)
    {
        m_selectedLookIndex[(int)actorEditorType] = index;
    }

    private void CreateActorBySelectLookID(int lookIndex, eActorEditorType actorEditorType)
    {
        int selectedLookIndex = GetSelectedLookIndex(actorEditorType);

        if (selectedLookIndex == lookIndex)
            return;

        SetSelectedLookIndex(actorEditorType, lookIndex);

        BaseEntityInfo actorInfo = null;
        if (lookIndex >= 0 && lookIndex < m_actorInfoList.Count)
        {
            actorInfo = m_actorInfoList[lookIndex];
        }
        else
        {
            return;
        }


        if (m_actorRootLook != null)
        {
            Global.GameMgr.DestroyCombat(m_actorRootLook.Uid);
            m_actorRootLook = null;

            if (m_currentSubCharacterViewer != null)
                m_currentSubCharacterViewer.ChangeActor(null);
        }

        //if (m_hero && actorInfo is PCInfo)
        //{
        //    EntityAssetInfo entityAssetInfo = new EntityAssetInfo(IDFactory.GenerateActorID(), eLayerMask.Actor);
        //    entityAssetInfo.SpriteName = actorInfo.Entity.SpriteName;
        //    entityAssetInfo.Path = actorInfo.Entity.Path;

        //    PCInfo pcInfo = new PCInfo(eNodeType.Hero, entityAssetInfo, entityAssetInfo.ID, null);

        //    m_actorRootLook = Global.GameMgr.MakePlayer(pcInfo, ePath.MapActorAsset);

        //    SelectActorView(m_actorRootLook, pcInfo);
        //}
        //else
        //{
        //    m_actorRootLook = Global.GameMgr.MakeActor(actorInfo, ePath.MapActorAsset);

        //    SelectActorView(m_actorRootLook, actorInfo);
        //}

        if (m_playerButtonController == null)
        {
            m_playerButtonController = new PlayerButtonController();
            Global.InputMgr.AddController(m_playerButtonController);
        }
        //m_playerButtonController.InitActor(m_actorRootLook);
        Global.CameraMgr.SetTargetActor(m_actorRootLook);

        UpdateFSMStateList();

        Repaint();
    }

    private void SelectActorView(ActorRoot actorRootLook, BaseEntityInfo actorViewInfo)
    {

        m_actorRootLook = actorRootLook;

        m_animationSpeed = 1.0f;

        foreach (ISubCharacterViewer subCharacterViewer in m_characterViewerDic.Values)
        {
            subCharacterViewer.ChangeActor(m_actorRootLook);
        }
    }

    private void UpdateFSMStateList()
    {
        if (m_actorRootLook != null)
        {
            if (null != m_curFsmStateSet)
            {
                m_curFsmStateSet.Clear();
            }

            //m_curFsmStateSet.name = m_actorLook.Fsm.PlayMaker.FsmName;
            //foreach (FsmState state in m_actorLook.Fsm.PlayMaker.FsmStates)
            //{
            //    m_curFsmStateSet.AddElement(state);
            //}
            m_curFsmStateSet.Sort();

            m_selectedStateIndex = 0;
            m_selectedStateTransitionIndex = 0;
            m_stateScrollPosition = Vector2.zero;
            m_selectFsmStateElement = null;
        }
    }

    private void SelectStateSet(FSMStateSet fsmStateSet)
    {
        //m_selectFsmStateSet = fsmStateSet;
    }

    private void ScrollTo()
    {
        const int scrollViewHeight = 200;
        int heightOffset = scrollViewHeight / m_curFsmStateSet.stateElementList.Count;
        Vector2 scrollOffSet = RequestedKey == KeyCode.UpArrow ? new Vector2(0, -20f + heightOffset) : new Vector2(0, 20f - heightOffset);

        m_stateScrollPosition += scrollOffSet;
        m_selectedStateTransitionIndex = 0;
        SelectState(m_selectedStateIndex, false);
    }

    private Vector3 ConvertOffset(Vector2 ptPrev, Vector2 ptCur)
    {
        CameraStock fixCameraStock = new CameraStock();//= Global.CameraMgr.Impl.Strategy.FixCameraStock;

        Vector2 v2DirY = LEMath.RadianToDir(LEMath.DegreeToRadian(-fixCameraStock.Horizon));
        Vector2 v2DirX = LEMath.RadianToDir(LEMath.DegreeToRadian(-fixCameraStock.Horizon) - LEMath.s_fHalfPI);
        float fDistance = fixCameraStock.Distance;

        Vector2 slidePoint = (v2DirX * (ptCur.x - ptPrev.x) * fDistance) - (v2DirY * (ptCur.y - ptPrev.y) * fDistance);
        float y = (ptCur.y - ptPrev.y);

        return new Vector3(slidePoint.x, y, slidePoint.y);
    }
    #endregion Func

    #region Call Back Func

    private void OnPlaymodeStateChanged()
    {
        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            OnEnableCharacterView();
        }

        if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            OnDisableCharacterView();
        }

        ResetSelectLook();

        Repaint();
    }

    private void OnMouseEvent(MouseEvent mouseEvent)
    {
        if (mouseEvent.State == MouseEvent.E_EventState.Move)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 offset = ConvertOffset(mouseEvent.Info.position, mouseEvent.PreviousPosition);
                offset = offset * 0.001f;
                Global.CameraMgr.Move(offset);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                Vector2 offset = mouseEvent.PreviousPosition - mouseEvent.Info.position;
                offset = offset * 0.05f;
                Global.CameraMgr.Rotate(offset);
            }
            else
            {
                Vector3 offset = ConvertOffset(mouseEvent.Info.position, mouseEvent.PreviousPosition);
                offset = offset * 0.001f;
                Global.CameraMgr.Slide(offset);
            }
        }
        else if (mouseEvent.State == MouseEvent.E_EventState.Wheel)
        {
            Global.CameraMgr.Zoom(mouseEvent.Info.delta);
        }
    }

    #endregion Call Back
}
