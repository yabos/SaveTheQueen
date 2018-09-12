using System;
using System.Collections;
using UnityEngine;

namespace Lib.UniBt
{
    public class RuntimeService : IRuntime
    {
        public Composite parent;
        public System.Action serviceAction;
        public float tick;
        //public System.IDisposable subscription;
        public MonoBehaviour comp;

        IEnumerator service = null;
        public string TempMethodName;

        public RuntimeService(Composite parent, float tick)
        {
            this.parent = parent;
            this.tick = tick;
        }

        public RuntimeService(Composite parent, float tick, string name)
        {
            this.parent = parent;
            this.tick = tick;
            this.TempMethodName = name;
        }


        IEnumerator RunService()
        {
            while (true)
            {
                yield return new WaitForSeconds(tick);
                serviceAction();
            }
        }


        public void OnStart()
        {
            if (service != null)
                comp.StopCoroutine(service);

            service = RunService();
            comp.StartCoroutine(service);
        }

        public void OnFinish()
        {
            if (service != null)
            {
                comp.StopCoroutine(service);
                service = null;
            }
        }

        public eBTStatus OnUpdate()
        {
            //TimeManager.time
            //serviceAction();

            return eBTStatus.Inactive;
        }
    }
}
