using Aniz.NodeGraph.Level.Group.Info;
using table.db;

namespace Aniz.Contents.Entity.Info
{

    public abstract class BaseEntityInfo
    {
        protected readonly eCombatType m_combatType;
        private readonly string m_name;
        private readonly long m_NetID;
        private readonly int m_assetTableID;	//Actor Index
        private readonly bool m_isUser;

        protected DB_SpriteData m_spriteData;
        public DB_SpriteData SpriteData { get { return m_spriteData; } }

        public EntityAssetInfo AssetInfo { get; private set; }


        public uint AIIndex { get; set; }
        public byte Direction { get; set; }
        public int BattleTeam { get; set; }
        public int StartTick { get; set; }

        public string Name { get { return m_name; } }

        //public string BaseState { get; set; }
        //public string SubState { get; set; }

        protected PlayerStatInfo m_playerStatInfo;

        public BaseEntityInfo(eCombatType combatType, EntityAssetInfo assetInfo, string name, long netid)
        {
            m_name = name;
            m_NetID = netid;
            AssetInfo = assetInfo;
            m_combatType = combatType;

            if (combatType == eCombatType.Hero)
            {
                m_isUser = true;
            }
            else
            {
                m_isUser = false;
            }
            Init();
        }

        public long NetId
        {
            get { return m_NetID; }
        }

        public int TableId
        {
            get { return m_assetTableID; }
        }

        public eCombatType CombatType
        {
            get { return m_combatType; }
        }

        public PlayerStatInfo Stat
        {
            get { return m_playerStatInfo; }
        }

        public bool IsUser
        {
            get { return m_isUser; }
        }

        public void Init()
        {
        }

        public void InitStatus(PlayerStatInfo info)
        {
            m_playerStatInfo = info;
        }

        public void Copy(BaseEntityInfo info)
        {
            info.Direction = Direction;
            info.BattleTeam = BattleTeam;
            info.AssetInfo.Copy(AssetInfo);
        }

        public abstract BaseEntityInfo Clone();
    }



}