
using Aniz.NodeGraph.Level.Group.Info;
using Aniz.Event;
using Lib.Event;

namespace Aniz
{
    public class PCEnterMsg : MessageBase
    {
        public ActorEnterInfo EnterInfo { get; set; }

        public override uint MsgCode
        {
            get { return (uint)eMessage.PCEnter; }
        }
    }

    public class MonsterEnterMsg : MessageBase
    {
        public int SpawnerID { get; set; }
        public ActorEnterInfo EnterInfo { get; set; }

        public override uint MsgCode
        {
            get { return (uint)eMessage.MonsterEnter; }
        }
    }
}