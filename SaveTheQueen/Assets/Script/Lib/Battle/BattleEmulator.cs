using Lib.Pattern;
using System;
using System.Collections.Generic;
using Aniz.Graph;
using UnityEngine;

namespace Lib.Battle
{
    public class BattleEmulator : IBaseClass
    {
        private LinkedList<AttackInfo> m_attackerList = new LinkedList<AttackInfo>();
        private LinkedList<AttackInfo> m_attackerMonsterList = new LinkedList<AttackInfo>();

        private List<ICombatOwner> m_targets = new List<ICombatOwner>();
        private List<ICombatOwner> m_targetMonsters = new List<ICombatOwner>();
        private List<ICombatOwner> m_targetPCsInOtherTeam = new List<ICombatOwner>();

        private ICombatFinder m_finder;

        public BattleEmulator(ICombatFinder finder)
        {
            m_finder = finder;
        }

        #region IBaseClass
        public void Initialize()
        {
        }

        public void Terminate()
        {
            m_targets.Clear();
            m_targetMonsters.Clear();
            m_targetPCsInOtherTeam.Clear();

            m_attackerMonsterList.Clear();
            m_attackerList.Clear();
        }
        #endregion

        public void OnFixedUpdate(float dt)
        {
            if (m_attackerList.Count > 0)
                UpdatePCAttackers(dt);

            if (m_attackerMonsterList.Count > 0)
                UpdateMonsterAttackers(dt);
        }

        private void UpdatePCAttackers(float dt)
        {
            bool collectMonsters = CollectActors(ref m_targetMonsters, true);
            var atkNode = m_attackerList.First;
            while (atkNode != null)
            {
                var nextNode = atkNode.Next;
                var attackerInfo = atkNode.Value;

                if (collectMonsters)
                    m_targets.AddRange(m_targetMonsters);

                // PC의 경우 다른 team인 PC도 attack target이 될 수 있다.
                bool collectInOtherTeam = CollectPCsInOtherTeam(ref m_targetPCsInOtherTeam, attackerInfo.Attacker.BattleTeam);
                if (collectInOtherTeam)
                    m_targets.AddRange(m_targetPCsInOtherTeam);

                if (UpdateAttacker(m_targets, attackerInfo, dt))
                {
                    m_attackerList.Remove(atkNode);
                }

                m_targets.Clear();
                m_targetPCsInOtherTeam.Clear();

                atkNode = nextNode;
            }

            m_targetMonsters.Clear();
        }

        private void UpdateMonsterAttackers(float dt)
        {
            CollectActors(ref m_targets, false);

            var atkNode = m_attackerMonsterList.First;
            while (atkNode != null)
            {
                var nextNode = atkNode.Next;
                if (UpdateAttacker(m_targets, atkNode.Value, dt))
                {
                    m_attackerMonsterList.Remove(atkNode);
                }

                atkNode = nextNode;
            }

            m_targets.Clear();
        }

        public void Add(ICombatOwner attacker, Vector2 attackDir, float hitTime)
        {

            if (attacker != null)
            {
                AttackInfo info = new AttackInfo();
                info.Attacker = attacker;
                info.attackDir = attackDir;
                info.HitTime = hitTime;


                if (attacker.CombatType <= eCombatType.Hero)
                    m_attackerList.AddLast(info);
                else
                    m_attackerMonsterList.AddLast(info);
            }
        }

        /// <summary>
        /// 타격 판정 시점에 들어왔으면 true를 return하여 Attacker List에서 제외하도록 함.
        /// </summary>
        /// <param name="lstActor"></param>
        /// <param name="attackerInfo"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool UpdateAttacker(List<ICombatOwner> lstActor, AttackInfo attackerInfo, float dt)
        {
            attackerInfo.DeltaTime += dt;
            if (attackerInfo.DeltaTime > attackerInfo.HitTime)
            {
                //if (lstActor.Count > 0)
                //    TestHit(lstActor, attackerInfo);

                return true;
            }

            return false;
        }

        private bool CollectActors(ref List<ICombatOwner> lstActor, bool collectMonster)
        {
            bool hasActor = false;
            if (collectMonster)
            {
                hasActor = m_finder.GetCombats(eCombatType.Monster, ref lstActor);
            }
            else
            {
                hasActor = m_finder.GetCombats(eCombatType.PC, ref lstActor);
            }

            return hasActor;
        }

        private bool CollectPCsInOtherTeam(ref List<ICombatOwner> lstActor, int combatTeam)
        {
            return m_finder.GetCombatsInOtherTeam(eCombatType.PC, ref lstActor, combatTeam);
        }
    }
}