using UnityEngine;
using System.Collections;
using System;

namespace Lib.Event
{
    public enum eNotifyHandler : int
    {
        Default = 0x00000001,
        Page = 0x00000002,
        Widget = 0x00000004,
        Node = 0x00000008,

        // util
        Util_GameFlow = Page | Widget,
        Util_All = Default | Util_GameFlow,
    }

    public interface INotifyHandler
    {
        string HandlerName { get; }

        bool IsConnected { get; }

        eNotifyHandler GetHandlerType();

        int GetOrder();

        bool IsActiveAndEnabled();

        void OnConnectHandler();

        void OnDisconnectHandler();

        void OnNotify(INotify notify);
    }

    public abstract class NotifyHanlder : INotifyHandler
    {
        //protected string m_name = string.Empty;

        protected bool m_isConnected = false;

        #region IEventHandler

        public abstract string HandlerName { get; set; }


        public virtual bool IsConnected
        {
            get { return m_isConnected; }
        }

        public virtual bool IsActiveAndEnabled()
        {
            if (m_isConnected == false)
                return false;

            return true;
        }

        public virtual eNotifyHandler GetHandlerType()
        {
            return eNotifyHandler.Default;
        }

        public virtual int GetOrder()
        {
            return (int)GetHandlerType();
        }

        public virtual void OnConnectHandler()
        {
            m_isConnected = true;
        }

        public virtual void OnDisconnectHandler()
        {
            m_isConnected = false;
        }

        public abstract void ConnectHandler();

        public abstract void DisconnectHandler();

        public abstract void OnNotify(INotify notify);

        #endregion IEventHandler



    }

}