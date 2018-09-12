using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Node;

namespace Aniz.Contents.Entity
{
    public class Hero : PC
    {
        public override eCombatType CombatType
        {
            get { return eCombatType.Hero; }
        }

        public override bool IsUser
        {
            get { return true; }
        }

        public Hero(BaseEntityInfo info, ActorImplNode actorMain) : base(info, actorMain)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //m_aiPoolNode = GetChild<AIPoolNode>();
        }

        public override void Terminate()
        {
            Global.InputMgr.RemoveController(NetID);
            base.Terminate();
        }
    }
}