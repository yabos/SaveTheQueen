using UnityEngine;
using System.Collections.Generic;

namespace Aniz.Contents.Entity.Info
{
    public abstract class PlayerStatInfo
    {
        private string m_name;

        private StatInfo[] m_statInfos;

        public PlayerStatInfo(string name)
        {
            m_name = name;

            m_statInfos = new StatInfo[(int)StatInfo.Type.Invalid];
            for (int i = 0; i < (int)StatInfo.Type.Invalid; i++)
            {
                m_statInfos[i] = new StatInfo((StatInfo.Type)i);
            }
        }

        public abstract void CalculateStat();

        public int GetStatus(StatInfo.Type sType)
        {
            return m_statInfos[(int)sType].Amount;
        }

        public void SetStatus(StatInfo.Type sType, int amount)
        {
            m_statInfos[(int)sType].SetValue(amount);
        }
        public void SetStatus(StatInfo.Type sType, float amount)
        {
            m_statInfos[(int)sType].SetValue(amount);
        }

        public void PrintCombatDebug()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < m_statInfos.Length; i++)
            {
                sb.AppendLine(string.Format("{0} : {1} ", m_statInfos[i].SType, m_statInfos[i].Amount));
            }

            Debug.Log("Name:" + m_name + "\n" + sb.ToString());
        }

        public void PrintSmallCombatDebug()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < m_statInfos.Length; i++)
            {
                sb.AppendLine(string.Format("{0} : {1} ", m_statInfos[i].SType, m_statInfos[i].Amount));
            }
            Debug.Log("Name:" + m_name + "\n" + sb.ToString());
        }

    }

}