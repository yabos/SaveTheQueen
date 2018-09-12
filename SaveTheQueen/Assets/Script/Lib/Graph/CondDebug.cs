#if UNITY_EDITOR
    #define LOG_NAMESPACE_META_FD
//  #define LOG_NAMESPACE_META_FD_VERBOSE
#endif

namespace Aniz.Graph
{
    // Coditional Debug
    public sealed class CondDebug
    {
        //
        // LOG_NAMESPACE_META_FD
        //

        [System.Diagnostics.Conditional("LOG_NAMESPACE_META_FD")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [System.Diagnostics.Conditional("LOG_NAMESPACE_META_FD")]
        public static void Log(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }

        //
        // LOG_NAMESPACE_META_FD_VERBOSE
        //

        [System.Diagnostics.Conditional("LOG_NAMESPACE_META_FD_VERBOSE")]
        public static void LogVerbose(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [System.Diagnostics.Conditional("LOG_NAMESPACE_META_FD_VERBOSE")]
        public static void LogVerbose(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }
    }
}
