using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Lib.Pattern;

namespace Lib.uGui
{

    [System.Serializable]
    public class GridScrollController<T> where T : class
    {
        [SerializeField]
        private Object m_reousrce;

        [SerializeField]
        private RectTransform m_content;
        private ScrollRect m_rect;

        private ScrollPatternBase m_pattern;
        private IScrollReceiver m_receiver = null;
        private IUIAction m_action = null;

        private int m_bufferCount = 1;

        private int m_maxItemCount;
        private int m_maxDataCount;

        private bool m_initFlag;
        private bool m_updateFlag;

        private List<ScrollItem> m_items;
        private List<T> m_datas;

        private int m_beforeColumn;
        private int m_currentColumn;

        public void OnInit(ScrollRect rect, Vector2 spacing, GameObject resource)
        {
            if (m_initFlag == true)
                return;

            m_rect = rect;
            m_content = m_rect.content;
            m_reousrce = resource;

            Init(spacing);
        }

        public void OnEnter(List<T> datas, IScrollReceiver receiver)
        {
            if (m_initFlag == true)
            {
                m_beforeColumn = 0;
                m_currentColumn = 0;

                m_content.anchoredPosition = Vector2.zero;

                OnExit();

                m_receiver = receiver;
                m_datas = datas;
                m_maxDataCount = m_datas.Count;

                Reset();
                RefreshItemData();
                ConnectScrollEvent();

                m_updateFlag = true;
            }
        }

        public void RefreshData(bool hasUpdatePage = true)
        {
            m_maxDataCount = m_datas.Count;

            Reset();
            RefreshItemData();

            if (hasUpdatePage == true)
            {
                UpdatePageWithScrollEnd();
            }

            m_updateFlag = true;
        }

        public void AddData(T data)
        {
            if (m_datas == null)
            {
                m_datas = new List<T>();
            }

            m_datas.Add(data);
            m_maxDataCount = m_datas.Count;
            RefreshData();
        }

        public void ClearData()
        {
            if (m_datas == null)
            {
                m_datas = new List<T>();
            }

            m_beforeColumn = 0;
            m_currentColumn = 0;

            m_content.anchoredPosition = Vector2.zero;

            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].SetAlive(false);
            }

            m_datas.Clear();
            m_maxDataCount = m_datas.Count;

            RefreshData();
        }

        public void OnRefresh()
        {
            if (m_initFlag == true)
            {
                DynamicContentSize();
                RefreshItemData();
            }
        }

        public void OnConnectModuleAction(IUIAction module)
        {
            m_action = module;
        }

        public void OnConnectReceiver(IScrollReceiver receiver)
        {
            m_receiver = receiver;
        }

        public void OnMovePosition(int index)
        {
            if (m_initFlag == true)
            {
                float itemSize = m_pattern.ItemSize;
                int lineCount = m_pattern.LineCount;
                float viewSize = m_pattern.ViewSize;

                float fpos = (index * itemSize / lineCount) - viewSize;
                m_content.anchoredPosition = new Vector2(0, fpos);
            }
        }

        public void OnExit()
        {
            Clear();
            DisConnectScrollEvent();
        }

        #region Init

        private void Init(Vector2 spacing)
        {
            if (m_reousrce != null && m_content != null)
            {
                RectTransform item = (m_reousrce as GameObject).transform as RectTransform;

                InitPattern();
                m_pattern.Init(m_rect, item, spacing);

                Create();
            }
        }

        private void ConnectScrollEvent()
        {
            if (m_rect == null)
                return;

            m_rect.onValueChanged.AddListener(UpdateScroll);
        }

        private void DisConnectScrollEvent()
        {
            if (m_rect == null)
                return;

            m_rect.onValueChanged.RemoveAllListeners();
        }

        private void InitPattern()
        {
            m_pattern = new ScrollGridPattern();
        }

        #endregion

        #region Update.

        private void UpdateScroll(Vector2 value)
        {
            if (m_updateFlag == false)
                return;

            m_currentColumn = GetScrollValue();

            if (m_currentColumn != m_beforeColumn && m_currentColumn >= 0)
            {
                UpdatePage();

                m_beforeColumn = m_currentColumn;
            }
        }

        private void UpdatePageWithScrollEnd()
        {
            UpdatePage();

            int viewCount = m_pattern.ViewCount;
            int loopCount = viewCount + m_bufferCount;
            if (loopCount + m_currentColumn >= m_maxDataCount)
            {
                if (m_rect.horizontalScrollbar)
                {
                    m_rect.horizontalNormalizedPosition = 0.0f;
                }

                if (m_rect.verticalScrollbar)
                {
                    m_rect.verticalNormalizedPosition = 0.0f;
                }
            }
        }

        private void UpdatePage()
        {
            int startIndex = 0;
            float itemSize = m_pattern.ItemSize;
            int viewCount = m_pattern.ViewCount;
            int lineCount = m_pattern.LineCount;
            int loopCount = viewCount + m_bufferCount;

            for (int i = 0; i < loopCount; i++)
            {
                // find start index.
                startIndex = (m_currentColumn + i) * lineCount;

                // find replace item index.
                int replaceIndex = startIndex % m_maxItemCount;
                if (replaceIndex < 0) continue;

                // find move position.
                float fpos = (m_currentColumn + i) * itemSize;
                Vector2 changePos = GetChangePosition(fpos);

                ChangePosition(replaceIndex, changePos);
                ChangeData(replaceIndex);
            }
        }

        private void ChangePosition(int replaceIndex, Vector2 changePos)
        {
            if (replaceIndex < 0)
            {
                return;
            }

            for (int i = 0; i < m_pattern.LineCount; i++)
            {
                if (m_items.Count <= replaceIndex + i)
                    continue;

                if (m_items[replaceIndex + i] == null)
                    continue;

                m_items[replaceIndex + i].SetPosition(changePos);
                changePos += new Vector2(m_pattern.ItemWidth, 0);
            }
        }

        private void ChangeData(int replaceIndex)
        {
            if (replaceIndex < 0)
            {
                return;
            }

            for (int i = 0; i < m_pattern.LineCount; i++)
            {
                if (m_items.Count <= replaceIndex + i)
                    continue;

                if (m_items[replaceIndex + i] == null)
                    continue;

                // Find DataIndex
                int dataIndex = m_pattern.GetDataIndex(m_items[replaceIndex + i].Owner);

                if (dataIndex >= 0 && dataIndex < m_datas.Count)
                {
                    // item info Settings.
                    m_items[replaceIndex + i].SetInfo(m_datas[dataIndex], m_receiver);
                    m_items[replaceIndex + i].ConnectActionModule(m_action);
                    m_items[replaceIndex + i].SetAlive(true);
                }
                else
                {
                    m_items[replaceIndex + i].SetAlive(false);
                }
            }
        }

        private int GetScrollValue()
        {
            float scrollValue = m_pattern.GetScrollValue(m_content.anchoredPosition);

            if (scrollValue > -1 && scrollValue < 0)
            {
                scrollValue = 0.0f;
            }

            return Mathf.FloorToInt(scrollValue);
        }

        private Vector2 GetChangePosition(float pos)
        {
            return m_pattern.GetChangePosition(pos);
        }

        private int GetItemDataIndex(int itemIndex)
        {
            return m_pattern.GetDataIndex(m_items[itemIndex].Owner);
        }

        #endregion

        #region Controller.

        private void RefreshItemData()
        {
            for (int i = 0; i < m_maxItemCount; i++)
            {
                // Find DataIndex
                int dataIndex = m_pattern.GetDataIndex(m_items[i].Owner);

                if (dataIndex >= 0 && dataIndex < m_datas.Count)
                {
                    m_items[i].SetInfo(m_datas[dataIndex], m_receiver);
                    m_items[i].ConnectActionModule(m_action);
                    m_items[i].SetAlive(true);
                }
                else
                {
                    m_items[i].SetAlive(false);
                }
            }
        }

        private void Create()
        {
            m_items = new List<ScrollItem>();

            m_maxItemCount = m_pattern.ViewCount * m_pattern.LineCount;
            m_maxItemCount += m_pattern.LineCount * m_bufferCount;

            for (int i = 0; i < m_maxItemCount; i++)
            {
                GameObject instanceObj =
                    GameObject.Instantiate(m_reousrce, Vector3.zero, Quaternion.identity, m_content) as GameObject;
                ScrollItem item = ComponentFactory.GetComponent<ScrollItem>(instanceObj, IfNotExist.AddNew);

                if (item != null)
                {
                    item.SetAlive(false);
                    item.UpdateOwner();

                    m_items.Add(item);
                }
            }

            m_initFlag = true;
        }

        private void DynamicContentSize()
        {
            m_maxDataCount = m_datas.Count;

            if (m_maxDataCount != 0)
            {
                float itemSize = m_pattern.ItemSize;
                float yCount = (float)m_maxDataCount / (float)m_pattern.LineCount;

                m_content.sizeDelta = new Vector2(m_rect.viewport.rect.width, Mathf.Ceil(yCount) * itemSize);
            }
        }

        public void Reset()
        {
            if (m_initFlag == true)
            {
                DynamicContentSize();

                float itemSize = m_pattern.ItemSize;
                int loopCount = m_pattern.ViewCount + m_bufferCount;

                // Change Position
                for (int i = 0; i < loopCount; i++)
                {
                    float fpos = itemSize * i;
                    Vector2 changePos = GetChangePosition(fpos);

                    for (int j = 0; j < m_pattern.LineCount; j++)
                    {
                        int itemIndex = i * m_pattern.LineCount + j;

                        m_items[itemIndex].SetPosition(changePos);
                        changePos += new Vector2(m_pattern.ItemWidth, 0);
                    }
                }
            }
        }

        private void Clear()
        {
            m_updateFlag = false;

            m_beforeColumn = 0;
            m_currentColumn = 0;

            if (m_datas != null)
            {
                //scrollDataList.Clear();
                for (int i = 0; i < m_items.Count; i++)
                {
                    m_items[i].SetAlive(false);
                }
            }

            m_receiver = null;
        }

        #endregion

    }

}