using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using EditorGUICtrl;

public class EventLogWindow : EditorWindow
{
    [MenuItem("Joker/EventLog Window %#e", false)]
    private static void OpenEditor()
    {
        EditorWindow.GetWindow<EventLogWindow>("EventLog");
    }

    private ListCtrl eventLogList = null;
    private int m_viewCount = 1000;
    private int m_currentPage = 1;

    private void OnEnable()
    {
        ListCtrl.Column c = null;

        eventLogList = new ListCtrl();
        eventLogList.SetStyle(ListCtrl.Style.SelectOneRow);

        c = eventLogList.AddColumn("Time", 120);
        c.Alignment = ListCtrl.ColumnAlignment.Left;

        c = eventLogList.AddColumn("Type", 160);
        c.Alignment = ListCtrl.ColumnAlignment.Left;

        c = eventLogList.AddColumn("Event");

        eventLogList.SetOnClickRow(OnClickEvent);
        eventLogList.SetOnDblClickRow(OnDblClickEvent);
        eventLogList.SetOnRightClickRow(OnRightClickEvent);

        LoadSetting();

        foreach (EventLogger.EventLog eventLog in EventLogger.EventList)
        {
            OnEventLog(eventLog);
        }

        EventLogger.AddEventLogListener(OnEventLog);
    }

    private void OnDisable()
    {
        SaveSettings();
    }

    private void OnUpdateNameFilter(object arg)
    {
        object[] args = arg as object[];

        string name = (string)args[0];
        bool on = (bool)args[1];

        nameFilterList.SetNameFilter(name, on);

        foreach (ListCtrl.Row row in eventLogList.RowList)
        {
            ApplyFilter(row);
        }
    }

    private void OnDestroy()
    {
        EventLogger.RemoveEventLogListener(OnEventLog);
    }

    private EventLogger.EventLog eventLog = null;
    private void OnClickEvent(ListCtrl.Row row)
    {
        ListCtrl.Item mainItem = row.GetMainItem();
        eventLog = mainItem.Data as EventLogger.EventLog;
    }

    private void OnDblClickEvent(ListCtrl.Row row)
    {
        ListCtrl.Item mainItem = row.GetMainItem();
        eventLog = mainItem.Data as EventLogger.EventLog;

        if (eventLog != null && eventLog.locationList != null && eventLog.locationList.Count >= 0)
        {
            GotoEventLog(eventLog.locationList[0]);
        }
    }

    private void OnRightClickEvent(ListCtrl.Row row)
    {
        ListCtrl.Item mainItem = row.GetMainItem();
        eventLog = mainItem.Data as EventLogger.EventLog;


        TextEditor te = new TextEditor();
        te.text = eventLog.message;
        te.OnFocus();
        te.Copy();
    }

    [System.Serializable]
    private class Filter
    {
        public string name = string.Empty;
        public bool on = true;
        public int count = 0;
    }

    [System.Serializable]
    private class FilterList
    {
        public List<Filter> list = new List<Filter>();

        private static FilterNameComparer filterNameComparer = new FilterNameComparer();
        private class FilterNameComparer : IComparer<Filter>
        {
            public int Compare(Filter lhs, Filter rhs)
            {
                return string.Compare(lhs.name, rhs.name);
            }
        }

        public void Reset()
        {
            list.Clear();
        }

        public void Save()
        {
            List<string> filterNames = new List<string>();
            for (int i = 0; i < list.Count; ++i)
            {
                Filter filter = list[i];
                if (filter.name != string.Empty)
                {
                    EditorPrefs.SetBool("EventLogFilter_" + filter.name, filter.on);

                    filterNames.Add(filter.name);
                }
            }

            string filters = string.Join(",", filterNames.ToArray());
            EditorPrefs.SetString("EventLogFilters", filters);
        }

        public void Load()
        {
            string filters = EditorPrefs.GetString("EventLogFilters");
            string[] filterNames = filters.Split(',');

            for (int i = 0; i < filterNames.Length; ++i)
            {
                string name = filterNames[i];
                bool on = EditorPrefs.GetBool("EventLogFilter_" + name);
                Filter filter = AddNameFilter(name);
                if (filter != null)
                {
                    filter.on = on;
                    filter.count = 0;
                }
            }
        }

        public void SetNameFilter(string name, bool on)
        {
            foreach (Filter filter in list)
            {
                if (filter.name == name)
                {
                    filter.on = on;
                    break;
                }
            }
        }

        public Filter AddNameFilter(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            foreach (Filter filter in list)
            {
                if (filter.name == name)
                    return filter;
            }

            Filter newFilter = new Filter();
            newFilter.name = name;
            list.Add(newFilter);
            list.Sort(filterNameComparer);
            return newFilter;
        }

        public bool GetNameFilter(string name)
        {
            foreach (Filter filter in list)
            {
                if (filter.name == name)
                    return filter.on;
            }
            return false;
        }
    }

    private FilterList nameFilterList = new FilterList();

    private void OnEventLog(EventLogger.EventLog eventLog)
    {
        ListCtrl.Row row = eventLogList.AddRow();

        ListCtrl.Item mainItem = null;
        ListCtrl.Item nameItem = null;

        //         if (EventLogger.IsCombatEvent(eventLog.eventLogType))
        //         {
        //             EventLogger.CombatLogDetail detail = eventLog.detail as EventLogger.CombatLogDetail;
        //             if (detail != null)
        //             {
        //                 mainItem = row.AddItem(string.Format("{0:0.000}({1})", eventLog.time, detail.frame));
        //                 nameItem = row.AddItem(detail.name);
        //                 row.AddItem(eventLog.message);
        //             }
        //         }
        //         else
        {
            mainItem = row.AddItem(string.Format("{0:0.000}({1})", eventLog.time, eventLog.simulationFrame));
            nameItem = row.AddItem(eventLog.eventLogType.ToString());
            row.AddItem(eventLog.message);
        }

        mainItem.SetData(eventLog);

        Filter filter = nameFilterList.AddNameFilter(nameItem.Text);
        if (filter != null)
        {
            filter.count++;
        }

        ApplyFilter(row);

        Repaint();
    }

    private void ApplyFilter(ListCtrl.Row row)
    {
        ListCtrl.Item mainItem = row.GetMainItem();
        EventLogger.EventLog eventLog = mainItem.Data as EventLogger.EventLog;

        row.Visible = IsVisible(eventLog);
    }

    private bool IsVisible(EventLogger.EventLog eventLog)
    {
        if (!nameFilterList.GetNameFilter(eventLog.eventLogType.ToString()))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(eventFilter) && !eventLog.message.ToLower().Contains(eventFilter.ToLower()))
            return false;
        return true;
    }

    private void SaveSettings()
    {
        nameFilterList.Save();

        EditorPrefs.SetBool("EventLog_ScrollLock", eventLogList.ScrollLock);
    }

    private void LoadSetting()
    {
        nameFilterList.Load();

        bool scrollLock = EditorPrefs.GetBool("EventLog_ScrollLock");
        eventLogList.ScrollLock = scrollLock;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                OnGUIEventList();

                OnGUIEventFilter();
            }
            EditorGUILayout.EndHorizontal();

            OnGUIEventLogDetail();
        }
        EditorGUILayout.EndVertical();

        EditorGUIEventUtil.ResetFocus();
    }

    private Vector2 scrollEventLog = Vector2.zero;
    private enum FilterType
    {
        Name,
        Event
    }
    private FilterType filterType = FilterType.Name;
    //private string[] FilterTypeStrings = new string[] { "Name", "Event" };

    private Vector2 eventFilterScroll = Vector2.zero;
    private void OnGUIEventFilter()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(300));
        {
            //filterType = (FilterType)GUILayout.Toolbar((int)filterType, FilterTypeStrings);
            if (filterType == FilterType.Name)
            {
                eventFilterScroll = EditorGUILayout.BeginScrollView(eventFilterScroll);
                bool changed = false;
                foreach (Filter filter in nameFilterList.list)
                {
                    bool oldSelected = filter.on;
                    bool selected = GUILayout.Toggle(oldSelected, string.Format("{0}({1})", filter.name, filter.count));
                    if (selected != oldSelected)
                    {
                        filter.on = selected;
                        changed = true;
                    }
                }

                if (changed)
                {
                    foreach (ListCtrl.Row row in eventLogList.RowList)
                    {
                        ApplyFilter(row);
                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("All"))
                {
                    foreach (Filter filter in nameFilterList.list)
                    {
                        filter.on = true;
                    }
                    foreach (ListCtrl.Row row in eventLogList.RowList)
                    {
                        ApplyFilter(row);
                    }
                }
                if (GUILayout.Button("None"))
                {
                    foreach (Filter filter in nameFilterList.list)
                    {
                        filter.on = false;
                    }
                    foreach (ListCtrl.Row row in eventLogList.RowList)
                    {
                        ApplyFilter(row);
                    }
                }
                if (GUILayout.Button("X"))
                {
                    nameFilterList.Reset();

                    foreach (ListCtrl.Row row in eventLogList.RowList)
                    {
                        ApplyFilter(row);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

        }
        EditorGUILayout.EndVertical();
    }

    private string eventFilter = string.Empty;
    private void OnGUIEventList()
    {
        EditorGUILayout.BeginVertical();
        {
            eventLogList.OnGUI(m_viewCount);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Width(60)))
            {
                eventFilter = string.Empty;
                //
                foreach (ListCtrl.Row row in eventLogList.RowList)
                {
                    ApplyFilter(row);
                }
            }
            string oldEventFilter = eventFilter;
            eventFilter = GUILayout.TextField(eventFilter, 128);
            if (GUI.changed && oldEventFilter != eventFilter)
            {
                //
                foreach (ListCtrl.Row row in eventLogList.RowList)
                {
                    ApplyFilter(row);
                }
            }

            eventLogList.ScrollLock = GUILayout.Toggle(eventLogList.ScrollLock, "Scroll Lock", GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            int count = EditorGUILayout.IntField("ViewCount", m_viewCount);
            if (m_viewCount != count)
                m_viewCount = count;

            m_viewCount = Mathf.Max(m_viewCount, 100);

            int maxPage = 0;
            if (eventLogList != null && eventLogList.RowList != null)
            {
                maxPage = eventLogList.RowList.Count / m_viewCount;
                GUILayout.Label(string.Format("AllLogCount : {0} Page 1/{1}", eventLogList.RowList.Count, maxPage));
            }
            int goPage = EditorGUILayout.IntField("GoPage", m_currentPage);

            if (GUILayout.Button("<", GUILayout.Width(30)))
            {
                m_currentPage -= 1;
                if (m_currentPage <= 0)
                {
                    m_currentPage = 1;
                }
                eventLogList.GoPage = m_currentPage;
                eventLogList.ScrollLock = true;
            }
            if (GUILayout.Button(">", GUILayout.Width(30)))
            {
                m_currentPage += 1;
                if (m_currentPage >= maxPage)
                {
                    m_currentPage = maxPage;
                }
                eventLogList.GoPage = m_currentPage;
                eventLogList.ScrollLock = true;
            }
            if (GUILayout.Button("GoPage", GUILayout.Width(70)))
            {
                m_currentPage = goPage;
                eventLogList.GoPage = m_currentPage;
                eventLogList.ScrollLock = true;
            }
            if (GUILayout.Button("Current", GUILayout.Width(70)))
            {
                m_currentPage = maxPage;
                eventLogList.GoPage = -1;
                eventLogList.ScrollLock = false;
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIEventLogDetail()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Height(200));
        {
            scrollEventLog = EditorGUILayout.BeginScrollView(scrollEventLog);
            if (eventLog != null && eventLog.locationList != null)
            {
                foreach (EventLogger.EventLocation location in eventLog.locationList)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Go", GUILayout.Width(60)))
                    {
                        GotoEventLog(location);
                    }

                    GUILayout.Label(location.method + " (at " + location.filepath + ":" + location.line + ")");
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void GotoEventLog(EventLogger.EventLocation location)
    {
        string assetPath = location.filepath.Replace("\\", "/");
        assetPath = "Assets" + assetPath.Replace(Application.dataPath, string.Empty);

        Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
        if (asset != null)
        {
            AssetDatabase.OpenAsset(asset, location.line);
        }
    }

}
