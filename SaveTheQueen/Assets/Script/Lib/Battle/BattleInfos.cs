using UnityEngine;

namespace Lib.Battle
{
    public class AttackInfo
    {
        public Vector2 attackDir { get; set; }
        public ICombatOwner Attacker { get; set; }

        public float Angle { get; set; }
        public float AttackDistSqr { get; set; }


        public float HitTime { get; set; }
        public float DeltaTime { get; set; }

    };

    public class BattleHitInfo
    {
        public long AttackerId { get; set; }
        public long DefenderId { get; set; }
        public int SkillEventId { get; set; }
        public float HitDistance { get; set; }
        public Vector3 Direction { get; set; }
        public bool IsInFrontOf { get; set; }
        public int StartTick { get; set; }
    }

    public class DamageInfo
    {
        public ICombatOwner Attacker { get; set; }
        public ICombatOwner Defender { get; set; }
        public int Damage { get; set; }
        public int HPResult { get; set; }

        //public DamageInfo() { }
        public DamageInfo(ICombatOwner attacker, ICombatOwner defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
    }

    public class SpawnInfo
    {
        public int spawnerID { get; set; }
        public int waveIndex { get; set; }
        public int monsterID { get; set; }
        public Vector3 position { get; set; }
        public Vector3 dir { get; set; }
        public string spawnEvent { get; set; }
    }
}