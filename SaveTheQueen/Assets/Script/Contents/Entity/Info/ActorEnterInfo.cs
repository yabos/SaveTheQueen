namespace Aniz.NodeGraph.Level.Group.Info
{
    public class ActorEnterInfo
    {
        private readonly int m_netID;
        private readonly int m_tableID;
        private readonly eNodeType m_nodeType;
        private EntityAssetInfo EntityAsset { get; set; }

        public string Name { get; set; }
        public ActorEnterInfo(int netID, int table, eNodeType nodeType)
        {
            m_netID = netID;
            m_tableID = table;
            m_nodeType = nodeType;
        }

        public eNodeType NodeType
        {
            get { return m_nodeType; }
        }

        public int TableID
        {
            get { return m_tableID; }
        }

        public int NetID
        {
            get { return m_netID; }
        }
    }

}