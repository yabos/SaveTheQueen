using Lib.Battle;
using Lib.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Node;
using UnityEngine;
using Lib.Pattern;

namespace Aniz.Contents.Entity
{
    public abstract class BaseEntity : IEntity
    {
        protected ActorImplNode m_actorMain;
        protected readonly BaseEntityInfo m_entityInfo;

        protected EntityMessageListener m_messageListener;
        private long m_netTargetID;

        public ActorImplNode Main
        {
            get { return m_actorMain; }
        }

        public BaseEntityInfo EntityInfo
        {
            get { return m_entityInfo; }
        }

        public IEntity TargetEntity { get; private set; }

        public string Name { get; private set; }

        public abstract eCombatType CombatType { get; }

        public abstract int BattleTeam { get; }


        public abstract bool IsUser { get; }

        public bool IsPlayer { get { return true; } }

        public bool IsDeath { get; set; }

        public BaseEntity(BaseEntityInfo info, ActorImplNode actorMain)
        {
            m_actorMain = actorMain;
            m_entityInfo = info;

            m_messageListener = new EntityMessageListener(this);
            EntityInfo.Stat.CalculateStat();
            //actorMain.Speed = MoveSpeed;
        }

        public virtual void Initialize()
        {
        }

        public virtual void OnUpdate(float dt)
        {
        }

        public virtual void Terminate()
        {
            m_actorMain = null;
        }

        #region "ICombatOwner"

        public int CurHP
        {
            get { return EntityInfo.Stat.GetStatus(StatInfo.Type.CUR_HP); }
        }

        public int MAXHP
        {
            get { return EntityInfo.Stat.GetStatus(StatInfo.Type.MAX_HP); }
        }

        public float MoveSpeed
        {
            // #ilkyo 대체예정
            // get { return PlayerInfo.Stat.GetStatus(StatInfo.Type.MOVE_SPEED); }
            get { return 5f; }
        }

        public long NetID
        {
            get { return EntityInfo.NetId; }
        }

        public long NetTargetId
        {
            get { return m_netTargetID; }
            set { m_netTargetID = value; }
        }

        public bool IsTarget
        {
            get { return m_netTargetID != 0; }
        }


        public virtual void SetHP(int hp)
        {
            //m_statusNode.SetHP(hp);
            EntityInfo.Stat.SetStatus(StatInfo.Type.CUR_HP, hp);
        }

        public Vector3 GetOwnerPosition()
        {
            return Main.transform.position;
        }

        public Transform GetOwnerTransform()
        {
            return Main.transform;
        }

        public bool TakeDamage(DamageInfo damageInfo)
        {

            int minHP = 0;
            int damagedHealth = CurHP > damageInfo.Damage ? damageInfo.Damage : CurHP;

            int curHP = Math.Max(minHP, CurHP - damagedHealth);
            SetHP(curHP);

            if (curHP <= 0)
            {
                SetDeathState(damageInfo);
            }

            return true;
        }

        public void SetDeathState(DamageInfo damageInfo)
        {
            float duration = 3.0f;


            Global.Instance.StartCoroutine(StartDeath(duration));
        }

        private IEnumerator StartDeath(float duration)
        {
            yield return new WaitForSeconds(duration);

            //NetBattleHandler.SendLeave(NetID, GetOwnerPosition(), Main.Move.Forward);
        }

        public void SendHitEvent(BattleHitInfo info)
        {
            //NetBattleHandler.SendHit(info);
        }

        #endregion "ICombatOwner"

        public bool OnMessage(IMessage message)
        {
            if (message.IsDeliveryMsg)
            {
                DeliveryMessage deliveryMessage = message as DeliveryMessage;
                if (deliveryMessage.receiverID == NetID)
                {
                    if (m_messageListener.OnMessage(deliveryMessage))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}