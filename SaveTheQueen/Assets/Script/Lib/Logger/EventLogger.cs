#if !UNITY_EDITOR
#undef DEBUG_EVENT_LOG
#endif // UNITY_EDITOR
//#define EVENT_DEBUG_LOG

using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;

public enum EventLogType
{
    None,

    Debug,
    Warning,
    Error,

    Message,
    Notify,

    ResourcesLoad,
    RandomSim,

    CameraShake,

    TriggerReceived,
    TriggerProcessed,
    TriggerFire,

    ActionCommand,

    SpawnEnemy,

    //playerMaker
    PlayerMakerFsmEvent,
    FsmEnter,
    FsmExit,
    AnimatorDefaultEvent,

    RefCount,

    AnimEvent_Combat,
    AnimEvent_VFX,
    AnimEvent_SFX,
    AnimEvent_State,

    Battle,

    SkillChain,
    SkillChainUI,

    Replay,
    Network_Sync,

    //asset

    EmitterStart,
    EmitterDestroy,
    EmitterAutoDestroy,
    RootMotion,
    BehaviorTree,
    FsmState
}

public static class EventLogger
{

    public delegate void OnEventLog(EventLog eventLog);

    [System.Serializable]
    public class EventLog
    {
        public EventLogType eventLogType;
        public float time;
        public int unityFrame;
        public int simulationFrame;
        public string message;
        public object detail;
        public List<EventLocation> locationList;
    }

    public class EventLocation
    {
        public string filepath;
        public int line;
        public string method;
    }

    private static List<EventLog> eventList = new List<EventLog>();
    public static List<EventLog> EventList
    {
        get { return eventList; }
    }

    private static Dictionary<int, string> m_stateNameHash = new Dictionary<int, string>();

    private static List<OnEventLog> eventLogListenerList = new List<OnEventLog>();

    public static void AddState(string state)
    {
        int key = Animator.StringToHash(state);
        if (m_stateNameHash.ContainsKey(key) == false)
        {
            m_stateNameHash.Add(key, state);
        }
    }

    public static string GetStateHash(int key)
    {
        if (m_stateNameHash.ContainsKey(key))
        {
            return m_stateNameHash[key];
        }
        return key.ToString();
    }

    public static void AddEventLogListener(OnEventLog onEventLog)
    {
        if (!eventLogListenerList.Contains(onEventLog))
        {
            eventLogListenerList.Add(onEventLog);
        }
    }

    public static void RemoveEventLogListener(OnEventLog onEventLog)
    {
        eventLogListenerList.Remove(onEventLog);
    }

    public static bool IsCombatEvent(EventLogType eventLogType)
    {
        return false;
        // NOTE: disabled for temporary until have proper format
        //return eventLogType >= EventLogType.Combat_Begin && eventLogType <= EventLogType.Combat_End;
    }




    [Conditional("DEBUG_EVENT_LOG")]
    public static void Log(EventLogType eventLogType, UnityEngine.GameObject gameObject)
    {
    }

    [Conditional("DEBUG_EVENT_LOG")]
    public static void LogError(string message)
    {
        Log(EventLogType.Error, message);
    }

    [Conditional("DEBUG_EVENT_LOG")]
    public static void LogDebug(string message)
    {
        Log(EventLogType.Debug, message);
    }

    [Conditional("DEBUG_EVENT_LOG")]
    public static void LogWarning(string message)
    {
        Log(EventLogType.Debug, message);
    }

    [Conditional("DEBUG_EVENT_LOG")]
    public static void LogDebug(string message, GameObject gameObject)
    {
        Log(EventLogType.Debug, string.Format("{0} {1} {2}", message, gameObject.name, gameObject.GetHashCode()));
    }

    [Conditional("DEBUG_EVENT_LOG")]
    public static void Log(EventLogType eventLogType, string message)
    {
        switch (eventLogType)
        {
            //case EventLogType.AnimEvent:
            case EventLogType.TriggerFire:
            case EventLogType.TriggerProcessed:
            case EventLogType.TriggerReceived:
                return;
        }


        EventLog e = new EventLog();
        e.eventLogType = eventLogType;
        e.time = UnityEngine.Time.realtimeSinceStartup;
        e.unityFrame = Time.frameCount;
        e.simulationFrame = Time.frameCount;
        e.message = message;
#if ENABLE_LOG_LOCATION
        e.locationList = GetEventLocation();
#endif // ENABLE_LOG_LOCATION
        eventList.Add(e);

#if EVENT_DEBUG_LOG
        UnityEngine.Debug.Log("EventLog: " + e.message);
#endif

        for (int i = 0; i < eventLogListenerList.Count; ++i)
        {
            eventLogListenerList[i](e);
        }
    }


    private static List<EventLocation> GetEventLocation()
    {
        List<EventLocation> locationList = new List<EventLocation>();
#if UNITY_EDITOR
        StackTrace stackTrace = new StackTrace(true);           // get call stack
        StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

        // TODO: should move to log window?
        int i = 0;
        foreach (StackFrame stackFrame in stackFrames)
        {
            if (i >= 2)
            {
                string filepath = stackFrame.GetFileName();
                if (filepath == null || filepath.Contains("EventLogger.cs"))
                    continue;

                System.Reflection.MethodBase methodBase = stackFrame.GetMethod();

                EventLocation location = new EventLocation();
                //location.method += methodBase.DeclaringType.Name + ":" + methodBase.Name;
                location.method = string.Format("{0}:{1}", methodBase.DeclaringType.Name, methodBase.Name);
                location.filepath = stackFrame.GetFileName();
                location.line = stackFrame.GetFileLineNumber();
                locationList.Add(location);
            }
            i++;
        }
#endif // UNITY_EDITOR
        return locationList;
    }

    public static void Clear()
    {
        eventList.Clear();
    }
}

#if UNITY_EDITOR
public static class StackTraceLogger
{

    public static string GetStackLog(int skipFrames = 2)
    {
        string log = "";

        StackTrace stackTrace = new StackTrace(skipFrames, true); // get call stack
        StackFrame[] stackFrames = stackTrace.GetFrames(); // get method calls (frames)

        foreach (StackFrame stackFrame in stackFrames)
        {
            System.Reflection.MethodBase methodBase = stackFrame.GetMethod();

            if (methodBase.DeclaringType.Name == "EditorGUIUtility")
                continue;
            if (methodBase.DeclaringType.Name == "GameView")
                continue;
            if (methodBase.DeclaringType.Name == "MonoMethod")
                continue;
            if (methodBase.DeclaringType.Name == "MethodBase")
                continue;
            if (methodBase.DeclaringType.Name == "HostView")
                continue;
            if (methodBase.DeclaringType.Name == "DockArea")
                continue;

            string filename = System.IO.Path.GetFileName(stackFrame.GetFileName());
            log += methodBase.DeclaringType.Name + ":" + methodBase.Name + " " + filename + " " + stackFrame.GetFileLineNumber();
            log += "\n";
        }
        return log;
    }

}
#endif // UNITY_EDITOR