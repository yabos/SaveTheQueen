using System;
using System.Reflection;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lib.uGui
{
    public class UIModuleAction : IUIAction
    {
        private Dictionary<string, object> m_actionContainer;

        public UIModuleAction()
        {
            m_actionContainer = new Dictionary<string, object>();
        }

        public void MessageCallback(string key, params object[] args)
        {
            if (CheckAction(key) == false)
            {
                UnityEngine.Debug.LogError(StringUtil.Format("key not found :{0}", key));
                return;
            }

            // Check Type
            Type[] types = new Type[args.Length];
            for (int i = 0; i < types.Length; i++)
            {
                if (args[i] != null)
                {
                    types[i] = args[i].GetType();
                }
            }

            // Get Method
            MethodInfo method = m_actionContainer[key].GetType().GetMethod("Invoke", types);

            if (method != null)
            {
                try
                {
                    // Invoke Method
                    method.Invoke(m_actionContainer[key], args);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                UnityEngine.Debug.LogError("matching failure");
            }
        }

        public void ConnectAction(string key, object action)
        {
            AddAction(key, action);
        }

        public void ConnectAction(UnityAction action)
        {
            AddAction(action.Method.Name, action);
        }

        public void ConnectAction<T>(UnityAction<T> action)
        {
            AddAction(action.Method.Name, action);
        }

        public void ConnectAction<T1, T2>(UnityAction<T1, T2> action)
        {
            AddAction(action.Method.Name, action);
        }

        public void ConnectAction<T1, T2, T3>(UnityAction<T1, T2, T3> action)
        {
            AddAction(action.Method.Name, action);
        }

        public void DisConnectAction()
        {

        }

        private void AddAction(string key, object action)
        {
            if (action == null)
                return;

            if (CheckAction(key) == true)
            {
                return;
            }

            m_actionContainer.Add(key, action);
        }

        private bool CheckAction(string key)
        {
            return m_actionContainer.ContainsKey(key);
        }


        public void Clear()
        {
            m_actionContainer.Clear();
        }
    }

}