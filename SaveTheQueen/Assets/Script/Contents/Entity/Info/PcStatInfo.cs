using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using table.db;

namespace Aniz.Contents.Entity.Info
{

    public class PcStatInfo : PlayerStatInfo
    {
        private readonly DB_Character m_curPCData;

        public PcStatInfo(DB_Character pcData, string name) : base(name)
        {
            m_curPCData = pcData;
        }

        public override void CalculateStat()
        {
            Debug.Assert(null != m_curPCData);
            if (null == m_curPCData)
            {
                return;
            }

            //m_curPCClassData.level
            //SetStatus(StatInfo.Type.CLASS_LEVEL, m_curPCData.Level);

            //// 임시 : ICombatOwner.CurHP가 int stat만 참조하기 때문에 PC Hp를 int로 cast - #jonghyuk
            //SetStatus(StatInfo.Type.MAX_HP, (int)m_curPCData.Hp);
            //SetStatus(StatInfo.Type.CUR_HP, (int)m_curPCData.Hp);

            //SetStatus(StatInfo.Type.P_ATTACK_POWER, m_curPCData.PAttackDamage);
            ////SetStatus(StatInfo.Type.MOVE_SPEED, m_curActorData.MoveSpeed);
        }
    }

}