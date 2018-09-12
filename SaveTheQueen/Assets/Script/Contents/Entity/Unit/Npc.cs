using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Node;

namespace Aniz.Contents.Entity
{
    public class Npc : BaseEntity
    {
        public override eCombatType CombatType
        {
            get { return eCombatType.Npc; }
        }

        public override int BattleTeam
        {
            get { return BattleTeamNum.MONSTER; }
        }

        public override bool IsUser
        {
            get { return false; }
        }

        public Npc(BaseEntityInfo info, ActorImplNode actorMain) : base(info, actorMain)
        {
        }

    }
}