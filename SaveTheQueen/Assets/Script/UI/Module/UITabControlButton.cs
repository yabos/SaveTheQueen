using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIExtension;
using UnityEngine.UI;

namespace Lib.uGui
{
    public class UITabControlButton : MonoBehaviour
    {
        public int Index { get; set; }
        bool selected = false;

        private UITabControl TabControl { get; set; }

        //private GameObject gameObject;

        // UI members
        public GameObject SelectedSprite;

        public GameObject DeselectedSprite;

        private Button m_tabButton;
        private Toggle m_tabToggle;

        public string Name
        {
            get
            {
                if (null != gameObject)
                {
                    return gameObject.name;
                }

                return null;
            }
        }

        public void Setup(UITabControl parent, int index)
        {
            this.Index = index;
            this.TabControl = parent;

            m_tabButton = transform.GetComponent<Button>();
            if (m_tabButton != null)
            {
                m_tabButton.AddOnClick(OnClickTab);
            }
            else
            {
                m_tabToggle = transform.GetComponent<Toggle>();
                m_tabToggle.AddOnChange(OnClickToggle);
            }

            SetUnselected();
        }

        public void SetButtonActive(bool active)
        {
            m_tabButton.enabled = active;
        }

        public bool Selected(bool value)
        {
            if (value != selected)
            {
                if (value)
                {
                    SetSelected();
                    return true;
                }
                else
                {
                    SetUnselected();
                }
            }

            return false;
        }

        public void SetSelected()
        {
            selected = true;

            if (null != SelectedSprite)
            {
                SelectedSprite.gameObject.SetActive(true);
            }

            if (null != DeselectedSprite)
            {
                DeselectedSprite.gameObject.SetActive(false);
            }
        }

        public void SetUnselected()
        {
            selected = false;

            if (null != SelectedSprite)
            {
                SelectedSprite.gameObject.SetActive(false);
            }

            if (null != DeselectedSprite)
            {
                DeselectedSprite.gameObject.SetActive(true);
            }
        }

        public void OnClickTab()
        {
            if (TabControl.SelectedTab == Index)
                return;

            TabControl.SelecteTab(Index);
        }

        public void OnClickToggle(bool toggle)
        {
            if (TabControl.SelectedTab == Index)
                return;

            if (toggle)
            {
                TabControl.SelecteTab(Index);
            }
        }
    }
}