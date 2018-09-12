﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lib.UniBt
{
    public partial class Brain : MonoBehaviour
    {
#if UNITY_EDITOR
        public List<RuntimeDecorator> runtimeDecorators
        {
            get { return this._runtimeDecorators; }
        }
#endif

        private List<RuntimeDecorator> _runtimeDecorators = new List<RuntimeDecorator>();

        private void InitializeDecorator(Node node)
        {
            if (node.decorators.Length > 0)
            {
                for (int i = 0; i < node.decorators.Length; i++)
                {
                    Decorator dc = node.decorators[i];
                    RuntimeDecorator rd = new RuntimeDecorator(node, dc, dc.inversed);
                    MonoBehaviour comp = GetEqualTypeComponent(dc.targetScript.GetType()) as MonoBehaviour;
                    if (comp == null)
                    {
                        comp = gameObject.AddComponent(dc.targetScript.GetType()) as MonoBehaviour;
                        IBTAI ibtai = comp as IBTAI;
                        ibtai.InitializeAI();
                    }
                    rd.decoratorFunc = Delegate.CreateDelegate(typeof(Func<bool>), comp, dc.targetMethod) as Func<bool>;
                    _runtimeDecorators.Add(rd);
                }
            }
        }

        private bool StartDecorator(RuntimeDecorator runtimeDecorator)
        {
            bool value = runtimeDecorator.inversed ? runtimeDecorator.decoratorFunc() : !runtimeDecorator.decoratorFunc();

            if (value)
            {
#if UNITY_EDITOR
                runtimeDecorator.closed = true;
#endif
                return false;
            }
#if UNITY_EDITOR
            runtimeDecorator.closed = false;
#endif

            return true;
        }

        private void FinishDecorators()
        {
            if (_aliveBehavior.decorators.Length > 0)
            {
                for (int i = 0; i < _aliveBehavior.decorators.Length; i++)
                {
                    RuntimeDecorator rd = GetRuntimeDecorator(_aliveBehavior.decorators[i]);
                    if (rd.activeSelf)
                        rd.activeSelf = false;
                }
            }
        }

        private RuntimeDecorator GetRuntimeDecorator(Decorator decorator)
        {
            for (int i = 0; i < _runtimeDecorators.Count; i++)
            {
                if (_runtimeDecorators[i].decorator == decorator)
                    return _runtimeDecorators[i];
            }
            return null;
        }
    }
}
