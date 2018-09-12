using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Node;

namespace Aniz.Contents.Entity
{
    public class PC : BaseEntity
    {
        public override eCombatType CombatType
        {
            get { return eCombatType.PC; }
        }

        public override int BattleTeam
        {
            get { return m_entityInfo.BattleTeam; }
        }

        public override bool IsUser
        {
            get { return false; }
        }

        public PC(BaseEntityInfo info, ActorImplNode actorMain) : base(info, actorMain)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // 현재 PC만 Skill Aniimation 동적 로딩 사용 - 17.06.27. jonghyuk
        }


    }
}