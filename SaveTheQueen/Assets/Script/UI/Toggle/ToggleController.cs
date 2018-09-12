using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Lib.uGui
{
    [System.Serializable]
    public class ToggleController
    {

        [SerializeField]
        private Toggle[] m_toggleGroup;

        private class ToggleItem
        {
            private int m_toggleID;
            public Toggle Toggle;

            public int ToggleID
            {
                get { return m_toggleID; }
            }

            public ToggleItem(int id, Toggle toggle)
            {
                m_toggleID = id;
                Toggle = toggle;
            }
        }

        private bool m_matchFlag;

        private List<ToggleItem> m_items;

        public void MatchToggle(int[] groupID)
        {
            if (m_toggleGroup.Length == groupID.Length)
            {
                m_matchFlag = true;
                m_items = new List<ToggleItem>();

                for (int i = 0; i < m_toggleGroup.Length; i++)
                {
                    m_items.Add(new ToggleItem(groupID[i], m_toggleGroup[i]));
                }
            }
            else
            {
                Debug.LogError("failed to toggle group.");
            }
        }

        public bool CheckToggle(int toggleID)
        {
            if (m_items != null)
            {
                if (m_items.Count != 0)
                {
                    if (m_matchFlag == true)
                    {
                        int index = m_items.FindIndex(list => list.ToggleID == toggleID);

                        if (index != -1)
                        {
                            return m_items[index].Toggle.isOn;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

    }

}