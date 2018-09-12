using Lib.Event;
using UnityEngine;

namespace Lib.Battle
{
    public interface ICombatOwner : IMessageHandler
    {
        string Name { get; }

        long NetID { get; }
        eCombatType CombatType { get; }
        int BattleTeam { get; }
        bool IsPlayer { get; }

        int CurHP { get; }
        int MAXHP { get; }

        bool IsDeath { get; set; }

        void SetHP(int hp);

        Vector3 GetOwnerPosition();
        Transform GetOwnerTransform();

        bool TakeDamage(DamageInfo damageInfo);
        void SetDeathState(DamageInfo damageInfo);
        void SendHitEvent(BattleHitInfo info);
    }
}