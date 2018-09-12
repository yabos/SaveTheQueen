using Lib.InputButton;
using System.Collections.Generic;
using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Node;

namespace Aniz.Contents.Entity
{
    public class Monster : Npc
    {

        //public override eBhvNodeType NodeType
        //{
        //    get { return eBhvNodeType.Monster; }
        //}

        public override eCombatType CombatType
        {
            get { return eCombatType.Monster; }
        }

        public override int BattleTeam
        {
            get { return BattleTeamNum.MONSTER; }
        }

        public Monster(BaseEntityInfo info, ActorImplNode actorMain) : base(info, actorMain)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

        }


        public override void Terminate()
        {

            base.Terminate();
        }

    }
}