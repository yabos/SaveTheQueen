using System;
using System.Collections.Generic;
using System.Linq;
using Aniz.Graph;
using Lib.Event;
using Aniz.Widget.Panel;
using UnityEngine;

namespace Aniz.Widget
{
    public class WidgetRepository : Lib.Pattern.IRepository<string, WidgetBase>, IGraphUpdatable
    {
        private Dictionary<string, WidgetBase> m_widgets = new Dictionary<string, WidgetBase>(StringComparer.CurrentCultureIgnoreCase);

        public Dictionary<string, WidgetBase> Widgets
        {
            get { return m_widgets; }
        }

        public void Initialize()
        {
        }

        public void Terminate()
        {
            m_widgets.Clear();
        }

        public bool Get(string widgetType, out WidgetBase widget)
        {
            return m_widgets.TryGetValue(widgetType, out widget);

            //KeyValuePair<string, WidgetBase> keyValuePair =
            //    m_widgets.FirstOrDefault(
            //        c => (c.Key.IndexOf(widgetType, System.StringComparison.OrdinalIgnoreCase) >= 0));
            //if (keyValuePair.Value != null)
            //{
            //    widget = keyValuePair.Value;
            //    return true;
            //}

            //widget = null;
            //return false;
        }

        public void Insert(WidgetBase widget)
        {
            WidgetBase resultwidget;
            string widgetType = widget.WidgetName;
            if (Get(widgetType, out resultwidget) == false)
            {
                m_widgets.Add(widgetType, widget);
            }
            else
            {
                Debug.LogError("WidgetRepository Insert ID OverLap!!! " + widget.name + " ID : " +
                               widget.WidgetID.ToString());
            }
        }

        public bool Remove(WidgetBase widget)
        {
            string widgetType = widget.UniqueName;

            return Remove(widgetType);
        }

        public bool Remove(string widgetType)
        {
            //KeyValuePair<string, WidgetBase> keyValuePair =
            //    m_widgets.FirstOrDefault(
            //        c => (c.Key.IndexOf(widgetType, System.StringComparison.OrdinalIgnoreCase) >= 0));
            //if (keyValuePair.Value != null)
            //{
            //    m_widgets.Remove(keyValuePair);
            //    return true;
            //}

            return m_widgets.Remove(widgetType);
        }


        public bool GetWidgets(ref List<WidgetBase> lstActors)
        {
            foreach (KeyValuePair<string, WidgetBase> keyValuePair in m_widgets)
            {
                lstActors.Add(keyValuePair.Value);
            }
            return lstActors.Count > 0;
        }

        public bool OnMessage(IMessage message)
        {
            foreach (KeyValuePair<string, WidgetBase> keyValuePair in m_widgets)
            {
                if (keyValuePair.Value.OnMessage(message))
                {
                    return true;
                }
            }

            return false;
        }

        public void HideAllWidgets(float deactiveTime = 0.0f)
        {
            foreach (KeyValuePair<string, WidgetBase> keyValuePair in m_widgets)
            {
                WidgetBase widgetBase = keyValuePair.Value;


                if (widgetBase is LoadingWidget)
                {
                    continue;
                }

                if (widgetBase is MessageBoxWidget)
                {
                    continue;
                }

                if (widgetBase != null && widgetBase.IsActive == true)
                {
                    widgetBase.Hide(deactiveTime);
                }
            }

        }

        #region IBhvUpdatable

        public void BhvOnEnter()
        {
        }

        public void BhvOnLeave()
        {
        }

        public void BhvUpdate(float dt)
        {
            using (var itor = m_widgets.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    Debug.Assert(itor.Current.Value != null);
                    if (itor.Current.Value.IsActive)
                    {
                        itor.Current.Value.BhvUpdate(dt);
                    }
                }
            }

        }

        public void BhvLateUpdate(float dt)
        {
            using (var itor = m_widgets.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    Debug.Assert(itor.Current.Value != null);
                    if (itor.Current.Value.IsActive)
                    {
                        itor.Current.Value.BhvLateUpdate(dt);
                    }
                }
            }
        }

        public void BhvFixedUpdate(float dt)
        {
            using (var itor = m_widgets.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    Debug.Assert(itor.Current.Value != null);
                    if (itor.Current.Value.IsActive)
                    {
                        itor.Current.Value.BhvFixedUpdate(dt);
                    }
                }
            }
        }

        public void BhvLateFixedUpdate(float dt)
        {
            using (var itor = m_widgets.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    Debug.Assert(itor.Current.Value != null);
                    if (itor.Current.Value.IsActive)
                    {
                        itor.Current.Value.BhvLateFixedUpdate(dt);
                    }
                }
            }
        }


        #endregion IBhvUpdatable
    }
}