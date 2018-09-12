using UnityEngine;
using System.Collections;
using Aniz.Graph;
using Aniz.NodeGraph.Level;
using Lib.Event;
using Lib.Pattern;

public class NodeGraphManager : GlobalManagerBase<NodeGraphManagerSetting>
{
    private IGraphNodeGroup m_rootNodeGroup;
    private WorldLevel m_worldLevel;


    public IGraphNodeGroup RootNode
    {
        get { return m_rootNodeGroup; }
    }

    public WorldLevel WorldLevelRoot
    {
        get { return m_worldLevel; }
    }

    public TileMapLevel TileMapLevelRoot
    {
        get
        {
            if (WorldLevelRoot != null)
            {
                return WorldLevelRoot.TileMapLevel;
            }
            return null;
        }
    }
    
    public ActorLevel ActorLevelRoot
    {
        get
        {
            if (WorldLevelRoot != null)
            {
                return WorldLevelRoot.ActorLevel;
            }
            return null;
        }
    }

    #region Events
    public override void OnAppStart(ManagerSettingBase managerSetting)
    {
        m_name = typeof(NodeGraphManager).ToString();

        if (string.IsNullOrEmpty(m_name))
        {
            throw new System.Exception("manager name is empty");
        }

        m_setting = managerSetting as NodeGraphManagerSetting;

        if (null == m_setting)
        {
            throw new System.Exception("manager setting is null");
        }

        //CreateRootObject(null, "NodePoolRepository");
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

    #region IGraphUpdatable

    public override void BhvOnEnter()
    {
#if UNITY_EDITOR
        string pageType = string.Empty;

        UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (activeScene != null)
        {
            UnityEditor.EditorBuildSettingsScene[] outscenes = UnityEditor.EditorBuildSettings.scenes;
            foreach (UnityEditor.EditorBuildSettingsScene item in outscenes)
            {
                string[] paths = StringUtil.Split(item.path, "/");
                string sceneName = paths[paths.Length - 1];
                sceneName = sceneName.Replace(".unity", "");

                if (activeScene.name == sceneName)
                {
                    pageType = sceneName;
                }
            }
        }
#endif //UNITY_EDITOR
    }

    public override void BhvOnLeave()
    {
        ReleaseWorld();
    }

    public override void BhvFixedUpdate(float dt)
    {
        if (m_rootNodeGroup != null)
        {
            m_rootNodeGroup.BhvFixedUpdate(dt);
        }
    }

    public override void BhvLateFixedUpdate(float dt)
    {
        if (m_rootNodeGroup != null)
        {
            m_rootNodeGroup.BhvLateFixedUpdate(dt);
        }
    }

    public override void BhvUpdate(float dt)
    {
        if (m_rootNodeGroup != null)
        {
            m_rootNodeGroup.BhvUpdate(dt);
        }
    }

    public override void BhvLateUpdate(float dt)
    {
        if (m_rootNodeGroup != null)
        {
            m_rootNodeGroup.BhvLateUpdate(dt);
        }
    }

    public override bool OnMessage(IMessage message)
    {

        if (m_rootNodeGroup.OnMessage(message))
        {
            return true;
        }
        return false;
    }

    #endregion IGraphUpdatable


    public void CreateWorld()
    {
        m_worldLevel = new WorldLevel();

        if (m_worldLevel != null)
        {
            m_worldLevel.CreateFirstLevel();
        }

        m_rootNodeGroup = m_worldLevel;

        if (m_rootNodeGroup != null)
        {
            m_rootNodeGroup.BhvOnEnter();
        }
    }

    //public void OpenMap(int level)
    //{
    //    TileMapLevelRoot.OpenMap(level);
    //}

    public void OpenMap(string mapname, System.Action<float> OnProgressAction)
    {
        TileMapLevelRoot.OpenMap(mapname, OnProgressAction);
    }

    public void ReleaseMap()
    {
        
        TileMapLevelRoot.DetachAllChildren();
        TileMapLevelRoot.ReleaseMap();
    }

    public IEnumerator OnLoadLevel(System.Action<float> OnProgressAction)
    {
        if (m_rootNodeGroup != null)
        {
            int childCount = m_rootNodeGroup.GetNumChildren();
            if (childCount <= 0)
            {
                yield return new WaitForEndOfFrame();

                if (OnProgressAction != null)
                {
                    OnProgressAction(1.0f);
                }

                yield break;
            }

            float baseProgress = 0.0f;
            float loadingProgressRate = 1.0f / childCount;

            for (int i = 0; i < childCount; ++i)
            {
                LevelBase level = m_rootNodeGroup.GetChild(i) as LevelBase;
                if (level != null)
                {
                    yield return level.OnLoadLevel((progress) =>
                    {
                        OnProgressAction(baseProgress + (progress * loadingProgressRate));
                    });

                    baseProgress += loadingProgressRate;
                }
            }
        }

        yield return new WaitForEndOfFrame();

        if (OnProgressAction != null)
        {
            OnProgressAction(1.0f);
        }
    }

    public void OnStartLevel()
    {
        if (m_rootNodeGroup != null)
        {
            int childCount = m_rootNodeGroup.GetNumChildren();

            for (int i = 0; i < childCount; ++i)
            {
                LevelBase level = m_rootNodeGroup.GetChild(i) as LevelBase;
                if (level != null)
                {
                    level.OnStartLevel();
                }
            }
        }
    }

    public void ReleaseWorld()
    {
        if (m_rootNodeGroup != null)
        {
            m_rootNodeGroup.BhvOnLeave();
            m_rootNodeGroup = null;
        }

        if (m_worldLevel != null)
        {
            m_worldLevel.ClearFirstLevel();
            m_worldLevel = null;
        }
    }

}
