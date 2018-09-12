using Aniz.Data;
using Aniz.NodeGraph.Level.Group.Info;
using table.db;
using UnityEngine;

namespace Aniz.Contents.Entity.Info
{
    public class PCInfo : BaseEntityInfo
    {
        private DB_Character m_pcData;

        public PCInfo(eCombatType combatType, EntityAssetInfo assetInfo, string name, long netid, DB_Character pcData)
            : base(combatType, assetInfo, name, netid)
        {
            m_pcData = pcData;//DataManager.Instance.GetScriptData<CharacterData>().GetCharacter(tableid);
            //Debug.Assert(null != m_pcData);

            //m_spriteData = ActorTableHelper.GetAssetActorData(m_pcData.AssetID);
            //Debug.Assert(null != m_spriteData);

            PcStatInfo pcStatInfo = new PcStatInfo(m_pcData, Name);
            m_playerStatInfo = pcStatInfo;
            //m_playerStatInfo.SetStatus(StatInfo.Type.MOVE_SPEED, m_spriteData.MoveSpeed);
        }

        //public Table.Asset.Actor ActorData
        //{
        //    get { return m_actorData; }
        //}

        public override BaseEntityInfo Clone()
        {
            PCInfo stock = new PCInfo(m_combatType, AssetInfo, Name, NetId, m_pcData);
            Copy(stock);
            return (stock);
        }
    }
}