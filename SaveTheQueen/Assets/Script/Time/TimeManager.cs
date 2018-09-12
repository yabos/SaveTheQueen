using System.Collections;
using Aniz.Graph;
using Lib.Event;
using Lib.Pattern;
using UnityEngine;

public struct GameTimeSetting
{
    public static float OneDay = 30.0f;
    public static float OneHour = OneDay / 24.0f;
    public static float OneMinute = OneHour / 60.0f;
    public static float OneSecond = OneMinute / 60.0f;
}

public class TimeManager : GlobalManagerBase<ManagerSettingBase>
{
    public enum E_Type
    {
        Real,
        GamePlay,
        Simulation,
        Max
    }

    #region Static Properites

    public readonly static int UPDATING_FRAME_COUNT = 1; // 1(60fps), 2(30fps)

    // current fps(30 or 60)
    public readonly static int SIMULATION_RATES_CURRENT = 60 / UPDATING_FRAME_COUNT;
    public readonly static float SIMULATION_RATES_CURRENT_FIXED = SIMULATION_RATES_CURRENT;
    public readonly static int SIMULATION_RATES_60 = 60;
    public readonly static float SIMULATION_RATES_60_FIXED = SIMULATION_RATES_60;
    public readonly static float SIMULATION_TIME_60 = 1.0f / SIMULATION_RATES_60;
    public readonly static float SIMULATION_TIME_60_FIXED = 1.0f / SIMULATION_RATES_60_FIXED;

    //NOTE: TimeScale 0인 상황에서 third-party lib때문에 Time.time은 server sync로 동작하도록 GameStageState에서만 사용을 강제한다.
    // Application의 전체 Unscaled time은 필요에 따라 별도 변수로 구현한다. [Mordor 20161018]
    public static float time { get; private set; }
    public static float deltaTime { get; private set; }
    public static float deltaTimeReal { get; private set; }
    public static int frameCount { get; private set; }
    public static int fixframeCount { get; private set; }
    public static bool Paused { get; private set; }

    public static float unscaledTime { get; private set; }
    public static float unscaledDeltaTime { get; private set; }

    public static int serverTick { get; private set; }

    #endregion Static Properites

    #region Properites

    private float mStartTime = -1.0f;
    private float mAccumulatedTime = 0;
    private float mGameStartTime = -1.0f;
    private float mGameAccumulatedTime = 0.0f;
    private float mLastUpdatedTime;

    private float mCurGameDay = 0.0f;

    private int m_serverOffset = 0;

    public float CurGameDay
    {
        get { return mCurGameDay; }
    }

    private float mCurGameTime = 0.0f;

    public float CurGameTime
    {
        set { mCurGameTime = value; }
        get { return mCurGameTime; }
    }

    private float mCurGameMinute = 0.0f;

    public float CurGameMinute
    {
        get { return mCurGameMinute; }
    }

    private float mCurGameSecond = 0.0f;

    public float CurGameSecond
    {
        get { return mCurGameSecond; }
    }

    public float GameAccumulatedTime
    {
        get { return mGameAccumulatedTime; }
    }

    #endregion Properites

    #region Events
    public override void OnAppStart(ManagerSettingBase managerSetting)
    {
        m_name = typeof(TimeManager).ToString();

        if (string.IsNullOrEmpty(m_name))
        {
            throw new System.Exception("manager name is empty");
        }

        m_setting = managerSetting;

        if (null == m_setting)
        {
            throw new System.Exception("manager setting is null");
        }

        Initialize(0.0f);
    }

    public override void OnAppEnd()
    {
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
        yield return new WaitForEndOfFrame();
    }

    #endregion Events
    #region IGraphUpdatable

    public override void BhvOnEnter()
    {
        fixframeCount = 0;
    }

    public override void BhvOnLeave()
    {
        fixframeCount = 0;
    }

    public override void BhvFixedUpdate(float dt)
    {
        fixframeCount++;
    }

    public override void BhvLateFixedUpdate(float dt)
    {

    }

    public override void BhvUpdate(float delta)
    {
        TimeManager.time += delta;
        TimeManager.deltaTime = delta;
        TimeManager.deltaTimeReal = Time.realtimeSinceStartup - mLastUpdatedTime;
        TimeManager.frameCount = Time.frameCount;


        mLastUpdatedTime = Time.realtimeSinceStartup;

        serverTick = (int)(time * 1000.0f) + m_serverOffset;

        //여기도 리팩토링이 필요하다.
        if (mStartTime >= 0.0f)
            mAccumulatedTime += delta;
        if (mGameStartTime >= 0.0f)
            mGameAccumulatedTime += delta;

        if (mGameAccumulatedTime >= 0.0f)
            UpdateCurTime();

        UpdateShaderTime();
    }

    public override void BhvLateUpdate(float dt)
    {

    }


    public override bool OnMessage(IMessage message)
    {
        return false;
    }

    #endregion IGraphUpdatable



    #region Methods

    public TimeManager()
    {
        Paused = false;
    }

    public void Initialize(float startTime)
    {
        Reset();

        InitializeGameTime(startTime);
    }

    public void InitializeGameTime(float startTime)
    {
        mGameStartTime = startTime;
    }

    public void EndGameTime()
    {
        mGameStartTime = -1.0f;
    }

    public void Reset()
    {
        time = 0f;
        deltaTime = 0f;
        deltaTimeReal = 0f;
        frameCount = 0;

        mLastUpdatedTime = Time.realtimeSinceStartup;

        serverTick = 0;
        m_serverOffset = 0;
    }

    public void UpdateServer(int time)
    {
        m_serverOffset = time;
    }

    private void UpdateCurTime()
    {
        mCurGameDay = mGameAccumulatedTime / GameTimeSetting.OneDay;
        mCurGameTime = (mGameAccumulatedTime % GameTimeSetting.OneDay) / GameTimeSetting.OneHour;
        mCurGameMinute = (mGameAccumulatedTime % GameTimeSetting.OneHour) / GameTimeSetting.OneMinute;
        mCurGameSecond = (mGameAccumulatedTime % GameTimeSetting.OneMinute) / GameTimeSetting.OneSecond;
    }

    private void UpdateShaderTime()
    {
        Vector4 t = new Vector4(
            Time.realtimeSinceStartup * (1.0f / 20.0f),
            Time.realtimeSinceStartup,
            Time.realtimeSinceStartup * Mathf.PI,
            Time.realtimeSinceStartup * Mathf.PI * 2.0f // for trigonometric function
            );
        Shader.SetGlobalVector("_OrcaTime", t);
    }

    public static IEnumerator WaitForSeconds(float delay)
    {
        float timer = Time.realtimeSinceStartup + delay;
        while (Time.realtimeSinceStartup <= timer)
        {
            yield return null;
        }
    }

    public static float SimulationFrameToTime(int frame)
    {
        return frame * SIMULATION_TIME_60;
    }

    public static int TimeToSimulationFrame(float time)
    {
        return (int)(time * SIMULATION_RATES_60);
    }

    public static int SecondToFrame(int sec)
    {
        return sec * SIMULATION_RATES_60;
    }

    #endregion Methods
}
