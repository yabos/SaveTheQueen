
using table.db;
using UnityEngine;

namespace Aniz.Contents.Entity.Info
{

    public class MonsterStatInfo : PlayerStatInfo
    {
        private DB_Character m_curMonsterData;

        public MonsterStatInfo(DB_Character curMonsterData, string name) : base(name)
        {
            m_curMonsterData = curMonsterData;
        }

        public override void CalculateStat()
        {
            Debug.Assert(null != m_curMonsterData);
            if (null == m_curMonsterData)
            {
                return;
            }

            SetStatus(StatInfo.Type.MAX_HP, m_curMonsterData.default_hp);
            SetStatus(StatInfo.Type.CUR_HP, m_curMonsterData.default_hp);
        }
    }
}