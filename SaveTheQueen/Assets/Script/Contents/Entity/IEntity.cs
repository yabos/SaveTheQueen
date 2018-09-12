using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Node;
using Lib.Battle;
using Lib.InputButton;
using Lib.Pattern;
using UnityEngine;

namespace Aniz.Contents.Entity
{
    public interface IEntity : ICombatOwner, IBaseClass
    {
        bool IsUser { get; }
        long NetTargetId { get; set; }
        bool IsTarget { get; }

        ActorImplNode Main { get; }
        BaseEntityInfo EntityInfo { get; }

        IEntity TargetEntity { get; }

        void OnUpdate(float dt);
    }
}