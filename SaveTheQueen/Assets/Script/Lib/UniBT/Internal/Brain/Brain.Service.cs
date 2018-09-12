using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lib.UniBt
{
    public partial class Brain : MonoBehaviour
    {
        private List<RuntimeService> _runtimeServices = new List<RuntimeService>();

        private void InitializeService(Composite composite)
        {
            if (composite.services.Length > 0)
            {
                for (int i = 0; i < composite.services.Length; i++)
                {
                    Service sv = composite.services[i];
                    RuntimeService rs = new RuntimeService(composite, sv.tick, sv.targetMethod);
                    MonoBehaviour comp = GetEqualTypeComponent(sv.targetScript.GetType()) as MonoBehaviour;
                    if (comp == null)
                    {
                        comp = gameObject.AddComponent(sv.targetScript.GetType()) as MonoBehaviour;
                        IBTAI ibtai = comp as IBTAI;
                        ibtai.InitializeAI();
                    }
                    rs.comp = comp;
                    rs.serviceAction = (System.Action)Delegate.CreateDelegate(typeof(System.Action), comp, sv.targetMethod);
                    _runtimeServices.Add(rs);
                }
            }
        }


        private void StartService(RuntimeService runtimeService)
        {
            runtimeService.serviceAction();

            if (runtimeService.tick > 0)
                runtimeService.OnStart();
            //if (runtimeService.subscription != null)
            //    runtimeService.subscription.Dispose();

            //runtimeService.subscription = Observable.Interval(TimeSpan.FromSeconds(runtimeService.tick))
            //    .Subscribe(_ =>
            //    {
            //        runtimeService.serviceAction();
            //    })
            //    .AddTo(this);
        }

        private void FinishServices(Composite composite)
        {
            for (int i = 0; i < _runtimeServices.Count; i++)
                if (_runtimeServices[i].parent == composite)
                {
                    if (_runtimeServices[i].tick > 0)
                        _runtimeServices[i].OnFinish();
                    //if (_runtimeServices[i].subscription != null)
                    //    _runtimeServices[i].subscription.Dispose();
                }
        }
    }
}
