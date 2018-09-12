using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Lib.uGui
{
    public class GridScrollViewListener<T> : UIModuleSimple, IScrollReceiver where T : class
    {
        [SerializeField]
        public float Spacing = 0.0f;

        [HideInInspector]
        public UIModuleAction m_action = new UIModuleAction();

        // ScrollEventHandler.
        [SerializeField]
        protected GridScrollController<T> m_controller;

        protected List<T> m_datas;

        virtual public void OnSetInfoEvent<T1>(T1 value = default(T1))
        {
        }

        virtual public void OnSelectEvent<T1>(T1 value = default(T1))
        {
        }

        virtual public void OnDoubleClickEvent<T1>(T1 value = default(T1))
        {
        }

        protected void OnScrollInit(ScrollRect rect, GameObject resource)
        {
            m_controller = new GridScrollController<T>();
            m_controller.OnInit(rect, new Vector2(Spacing, Spacing), resource);
        }

        protected void OnScrollEnter(List<T> list)
        {
            if (list == null)
                return;

            m_datas = list.ToList();

            m_controller.OnConnectModuleAction(m_action);
            m_controller.OnConnectReceiver(this);

            m_controller.OnEnter(list, this);
        }

        public void RefreshData()
        {
            m_controller.RefreshData(true);
        }

        public void AddData(T data)
        {
            if (m_datas == null)
            {
                m_datas = new List<T>();
            }

            m_controller.AddData(data);
        }

        public void ClearData()
        {
            if (m_datas == null)
            {
                m_datas = new List<T>();
            }

            m_datas.Clear();

            m_controller.ClearData();
        }

        public override void OnRefreshModule()
        {
            base.OnRefreshModule();

            OnScrollRefresh();
        }

        public override void OnExitModule()
        {
            if (m_controller == null)
                return;

            m_controller.OnExit();
            m_datas.Clear();

            base.OnExitModule();
        }

        public override void OnDestroyModule()
        {
            base.OnDestroyModule();

            m_controller = null;
            m_datas = null;
        }

        protected void OnScrollRefresh()
        {
            m_controller.OnRefresh();
        }

        protected void OnMovePosition(int index)
        {
            m_controller.OnMovePosition(index);
        }
    }

}