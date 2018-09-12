using Aniz.Data;
using Aniz.NodeGraph.Level.Group.Info;
using table.db;
using UnityEngine;

namespace Aniz.Contents.Entity.Info
{

    //-------------------------------------------------------------------
    // PCInfo
    //-------------------------------------------------------------------

    public class MonsterInfo : BaseEntityInfo
    {
        private DB_Character m_tableMonster;
        public MonsterInfo(eCombatType combatType, EntityAssetInfo assetInfo, string name, long netid, DB_Character tableMonster)
            : base(combatType, assetInfo, name, netid)
        {
            m_tableMonster = tableMonster;
            Debug.Assert(null != m_tableMonster);

            //m_spriteData = ActorTableHelper.GetAssetActorData(m_tableMonster.AssetID);
            //Debug.Assert(null != m_spriteData);

            m_playerStatInfo = new MonsterStatInfo(m_tableMonster, name);
            //m_playerStatInfo.SetStatus(StatInfo.Type.MOVE_SPEED, m_spriteData.MoveSpeed);

            BattleTeam = BattleTeamNum.MONSTER;
        }

        public DB_Character TableMonster
        {
            get { return m_tableMonster; }
        }

        public override BaseEntityInfo Clone()
        {
            MonsterInfo stock = new MonsterInfo(m_combatType, AssetInfo, Name, NetId, m_tableMonster);
            Copy(stock);
            return stock;
        }

    }
}