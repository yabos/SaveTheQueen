using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Lib.Pattern;

namespace Lib.uGui
{
    public class ScrollViewController<T> where T : class
    {
        protected Object m_reousrce;
        protected ScrollRect m_rect;

        private ScrollPatternBase m_pattern;
        private IScrollReceiver m_receiver;
        private IUIAction m_action = null;

        private int m_maxItemCount;
        private int m_maxDataCount;

        private bool m_initFlag;
        private bool m_updateFlag;

        private List<ScrollItem> m_items;
        private List<T> m_datas;

        private int m_beforePageNumber = 0;
        private int m_currentPageNumber = 0;
        private int m_changePageStartIndex = 0;

        public void OnInit(ScrollRect rect, Vector2 spacing, GameObject resource, eScrollPattern patternType)
        {
            Init(rect, spacing, resource, patternType);
        }

        public void OnEnter(List<T> datas)
        {
            m_beforePageNumber = 0;
            m_currentPageNumber = 0;
            m_changePageStartIndex = 0;

            if (m_initFlag == true)
            {
                m_updateFlag = false;

                m_rect.content.anchoredPosition = Vector2.zero;

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

            m_rect.content.anchoredPosition = Vector2.zero;

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

        public void OnExit()
        {
            Clear();
            DisConnectScrollEvent();
        }

        public void ItemClear()
        {

        }

        #region Init

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


        private void Init(ScrollRect rect, Vector2 spacing, GameObject resource, eScrollPattern patternType)
        {
            m_reousrce = resource;
            m_rect = rect;

            if (m_reousrce != null && m_rect != null)
            {
                RectTransform item = (m_reousrce as GameObject).transform as RectTransform;

                InitPattern(patternType);
                m_pattern.Init(m_rect, item, spacing);

                Create();
            }
        }

        private void InitPattern(eScrollPattern patternType)
        {
            switch (patternType)
            {
                case eScrollPattern.VerticalDown:
                    {
                        m_pattern = new ScrollDownPattern();
                    }
                    break;
                case eScrollPattern.VerticalUp:
                    {
                        m_pattern = new ScrollUpPattern();
                    }
                    break;
                case eScrollPattern.Horizontal:
                    {
                        m_pattern = new ScrollLeftPattern();
                    }
                    break;
            }
        }

        #endregion

        #region Update.

        private void UpdateScroll(Vector2 value)
        {
            if (m_updateFlag == false)
                return;

            float scrollValue = GetScrollValue(value);

            m_currentPageNumber = (int)scrollValue / m_pattern.ViewCount;

            if (m_currentPageNumber != m_beforePageNumber)
            {
                if (m_currentPageNumber > m_beforePageNumber)
                {
                    NextPage();
                }
                else
                {
                    PrevPage();
                }

                m_beforePageNumber = m_currentPageNumber;
            }
        }

        private void NextPage()
        {
            int viewCount = m_pattern.ViewCount;
            int startIndex = m_changePageStartIndex;

            m_changePageStartIndex = (m_changePageStartIndex + viewCount) % m_maxItemCount;

            float itemSize = m_pattern.ItemSize;

            for (int i = 0; i < viewCount; i++)
            {
                int replaceIndex = startIndex + i;

                float fpos = ((m_currentPageNumber + 1) * viewCount * itemSize) + (itemSize * i);
                Vector2 changePos = GetChangePosition(fpos);

                int dataIndex = (int)(fpos / itemSize);

                NextPosition(replaceIndex, changePos);
                NextReplace(replaceIndex, dataIndex);
            }
        }

        private void NextPosition(int replaceIndex, Vector2 changePos)
        {
            m_items[replaceIndex].SetPosition(changePos);
        }

        private void NextReplace(int replaceIndex, int dataIndex)
        {
            if (dataIndex >= 0 && dataIndex < m_datas.Count)
            {
                m_items[replaceIndex].SetInfo(m_datas[dataIndex], m_receiver);
                m_items[replaceIndex].ConnectActionModule(m_action);
                m_items[replaceIndex].SetAlive(true);
            }
            else
            {
                m_items[replaceIndex].SetAlive(false);
            }
        }

        private void PrevPage()
        {
            int viewCount = m_pattern.ViewCount;
            m_changePageStartIndex = m_changePageStartIndex - viewCount;

            if (m_changePageStartIndex < 0)
            {
                m_changePageStartIndex += m_maxItemCount;
            }

            int startIndex = m_changePageStartIndex;
            float itemSize = m_pattern.ItemSize;

            for (int i = 0; i < viewCount; i++)
            {
                int replaceIndex = startIndex + i;
                float fpos = ((m_currentPageNumber - 1) * viewCount * itemSize) + (itemSize * i);

                Vector2 changePos = GetChangePosition(fpos);
                int dataIndex = (int)(fpos / itemSize);

                PrevPosition(replaceIndex, changePos);
                PrevReplace(replaceIndex, dataIndex);
            }
        }

        private void PrevPosition(int replaceIndex, Vector2 changePos)
        {
            m_items[replaceIndex].SetPosition(changePos);
        }

        private void PrevReplace(int replaceIndex, int dataIndex)
        {
            if (dataIndex >= 0 && dataIndex < m_datas.Count)
            {
                m_items[replaceIndex].SetInfo(m_datas[dataIndex], m_receiver);
                m_items[replaceIndex].ConnectActionModule(m_action);
                m_items[replaceIndex].SetAlive(true);
            }
            else
            {
                m_items[replaceIndex].SetAlive(false);
            }
        }


        private float GetScrollValue(Vector2 value)
        {
            return m_pattern.GetScrollValue(m_rect.content.anchoredPosition);
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
                int itemDataIndex = GetItemDataIndex(i);

                if (itemDataIndex >= 0 && itemDataIndex < m_datas.Count)
                {
                    m_items[i].SetInfo(m_datas[itemDataIndex], m_receiver);
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

            m_maxItemCount = m_pattern.ViewCount * 3;

            for (int i = 0; i < m_maxItemCount; i++)
            {
                GameObject instanceObj =
                    GameObject.Instantiate(m_reousrce, Vector3.zero, Quaternion.identity, m_rect.content) as GameObject;
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
                m_rect.content.sizeDelta = m_pattern.GetContentSize(m_maxDataCount);
            }
        }

        public void Reset()
        {
            if (m_initFlag == true)
            {
                DynamicContentSize();

                float itemSize = m_pattern.ItemSize;

                // Change Position
                for (int i = 0; i < m_items.Count; i++)
                {
                    float fpos = (i - m_pattern.ViewCount) * itemSize;
                    Vector2 changePos = GetChangePosition(fpos);

                    m_items[i].SetPosition(changePos);
                }
            }
        }

        private void Clear()
        {
            m_updateFlag = false;

            if (m_datas != null)
            {
                m_datas.Clear();

                for (int i = 0; i < m_items.Count; i++)
                {
                    m_items[i].SetAlive(false);
                }
            }

            m_beforePageNumber = 0;
            m_currentPageNumber = 0;
            m_changePageStartIndex = 0;
        }

        #endregion

    }

}