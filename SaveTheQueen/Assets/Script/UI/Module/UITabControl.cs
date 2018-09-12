using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lib.uGui
{
    public class UITabControl : MonoBehaviour
    {
        int selectedTab = -1;

        public int SelectedTab
        {
            get { return selectedTab; }
            private set { selectedTab = value; }
        }

        public List<UITabControlButton> TabButtons = new List<UITabControlButton>();

        public delegate void TabClick(int index);

        private TabClick m_tabClick;
        private int AfterSelectIndex = 0;

        public int SelectIndex
        {
            get { return AfterSelectIndex; }
        }

        public void Awake()
        {
            InitTabButtons(gameObject);
            SelecteTab(AfterSelectIndex);
        }


        public void InitTabButtons(GameObject parent)
        {
            if (null == parent)
            {
                Debug.LogError("Target object is null.");
                return;
            }

            for (int i = 0; i < TabButtons.Count; ++i)
            {
                UITabControlButton button = TabButtons[i];
                button.Setup(this, i);
            }

        }


        public void Init(TabClick click, int _InitialSelectIndex = 0)
        {
            m_tabClick = click;
            AfterSelectIndex = _InitialSelectIndex;
        }

        public void TabButtonActive(bool active)
        {
            for (int i = 0; i < TabButtons.Count; ++i)
            {
                TabButtons[i].SetButtonActive(active);
            }
        }

        public void SelecteTab(int selectedindex)
        {
            if (m_tabClick != null)
            {
                m_tabClick(selectedindex);
            }

            for (int i = 0; i < TabButtons.Count; ++i)
            {
                if (TabButtons[i].Selected(selectedindex == i))
                {
                    selectedTab = selectedindex;
                    AfterSelectIndex = selectedindex;
                }
            }
        }
    }
}