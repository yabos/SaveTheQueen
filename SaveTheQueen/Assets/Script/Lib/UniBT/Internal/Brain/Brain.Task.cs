using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lib.UniBt
{
    public partial class Brain : MonoBehaviour
    {
#if UNITY_EDITOR
        public Dictionary<Task, RuntimeTask> runtimeTasks
        {
            get { return this._runtimeTasks; }
        }
#endif
        private Dictionary<Task, RuntimeTask> _runtimeTasks = new Dictionary<Task, RuntimeTask>();
        private RuntimeTask m_currentRuntimeTask = null;

        private void InitializeTask(Task task)
        {
            RuntimeTask rt = null;
            //if (task is Wait)
            //    rt = Initialize_Wait(task);
            //else
            {
                if (task.targetScript == null)
                {
                    Debug.LogError(string.Format("{0} : task.targetScript = null", task.Name));
                    return;
                }

                rt = new RuntimeTask(task, task.targetMethod);

                MonoBehaviour comp = GetEqualTypeComponent(task.targetScript.GetType()) as MonoBehaviour;
                if (comp == null)
                {
                    comp = gameObject.AddComponent(task.targetScript.GetType()) as MonoBehaviour;
                    IBTAI ibtai = comp as IBTAI;
                    ibtai.InitializeAI();
                }

                if (task.isCoroutine)
                    rt.comp = comp;
                else
                {
                    Func<eBTStatus> tempFunc = Delegate.CreateDelegate(typeof(Func<eBTStatus>), comp, task.targetMethod) as Func<eBTStatus>;
                    rt.taskFunc = tempFunc;
                }
            }

            _runtimeTasks.Add(task, rt);
        }

        private void StartTask(Node node)
        {
            _aliveBehavior = node;
            if (node is Task)
            {
                RuntimeTask rt = GetRuntimeTask(_aliveBehavior as Task);
#if UNITY_EDITOR
                rt.closed = false;
#endif
                rt.OnStart();

                m_currentRuntimeTask = rt;
            }
        }

        private void OnTaskUpdate(float dt)
        {
            if (m_currentRuntimeTask != null)
            {
                if (m_currentRuntimeTask.OnUpdate() == eBTStatus.Success)
                {
                    m_currentRuntimeTask = null;
                    FinishExecute(true);
                }
            }
        }

        private void FinishTask()
        {
            if (_aliveBehavior is Task)
            {
                RuntimeTask rt = GetRuntimeTask(_aliveBehavior as Task);
#if UNITY_EDITOR
                rt.closed = true;
#endif
                rt.OnFinish();
            }
        }

        private RuntimeTask GetRuntimeTask(Task task)
        {
            RuntimeTask value = null;
            if (_runtimeTasks.ContainsKey(task))
            {
                value = _runtimeTasks[task];
            }
            return value;
        }
    }
}
