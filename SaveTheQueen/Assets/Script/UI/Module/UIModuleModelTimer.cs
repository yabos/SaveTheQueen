using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lib.uGui
{
    public static class UIModuleModelTimer
    {
        internal class DelayedEditorAction
        {
            internal double m_timeToExecute;
            internal System.Action m_action;
            internal MonoBehaviour m_actionTarget;

            public DelayedEditorAction(double timeToExecute, System.Action action, MonoBehaviour actionTarget)
            {
                m_timeToExecute = timeToExecute;
                m_action = action;
                m_actionTarget = actionTarget;
            }
        }

#if UNITY_EDITOR
        static List<DelayedEditorAction> g_delayedEditorActions = new List<DelayedEditorAction>();

        static UIModuleModelTimer()
        {
            UnityEditor.EditorApplication.update += EditorUpdate;
        }
#endif

        static void EditorUpdate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) return;

            var actionsToExecute = g_delayedEditorActions.Where(dea => UnityEditor.EditorApplication.timeSinceStartup >= dea.m_timeToExecute).ToList();

            if (!actionsToExecute.Any()) return;

            foreach (var actionToExecute in actionsToExecute)
            {
                try
                {
                    if (actionToExecute.m_actionTarget != null)
                    {
                        actionToExecute.m_action.Invoke();
                    }
                }
                finally
                {
                    g_delayedEditorActions.Remove(actionToExecute);
                }
            }
#endif
        }

        public static void DelayedCall(float delay, System.Action action, MonoBehaviour actionTarget)
        {
            if (Application.isPlaying)
            {
                if (actionTarget.gameObject.activeInHierarchy) actionTarget.StartCoroutine(DelayedCall(delay, action));
            }
#if UNITY_EDITOR
            else
            {
                g_delayedEditorActions.Add(new DelayedEditorAction(UnityEditor.EditorApplication.timeSinceStartup + delay, action, actionTarget));
            }
#endif
        }

        private static IEnumerator DelayedCall(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);

            action.Invoke();
        }

        public static void AtEndOfFrame(System.Action action, MonoBehaviour actionTarget)
        {
            DelayedCall(0, action, actionTarget);
        }
    }
}