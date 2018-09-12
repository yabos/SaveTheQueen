using System;
using UnityEngine;

namespace Lib.UniBt
{
    public enum eBTStatus
    {
        Inactive = 0,
        Failure = 1,
        Success = 2,
        Running = 3
    }

    public interface IRuntime
    {
        void OnStart();
        void OnFinish();

        eBTStatus OnUpdate();
    }

    public class RuntimeTask : IRuntime
    {
        public Task parent;
        public string methodName;
        public System.Func<eBTStatus> taskFunc;
        public MonoBehaviour comp;
#if UNITY_EDITOR
        public bool closed;
#endif
        //private System.IDisposable _disposable;

        public RuntimeTask(Task parent, string methodName)
        {
            this.parent = parent;
            this.methodName = methodName;
        }

        public void OnStart()
        {
            if (parent.isCoroutine)
            {
                comp.StopCoroutine(methodName);
                comp.StartCoroutine(methodName);
            }
            //else
            //{
            //    if (_disposable != null)
            //        _disposable.Dispose();
            //    _disposable = taskFunc();
            //}
        }

        public void OnFinish()
        {
            if (parent.isCoroutine)
                comp.StopCoroutine(methodName);
            //else if (_disposable != null)
            //    _disposable.Dispose();
        }

        public eBTStatus OnUpdate()
        {
            if (taskFunc != null)
                return taskFunc();

            return eBTStatus.Inactive;
        }

    }
}
