using System;
using Lib.Event;
using UnityEngine;
using Aniz.Event;

namespace Aniz
{


    public class ActionMessage : DeliveryMessage
    {

        public override uint MsgCode
        {
            get { return (uint)eMessage.Action; }
        }

        public ActionMessage(int _actorID)
        {
            receiverID = _actorID;
        }
    }

    //****************************************
    public class HitMsg : ActionMessage
    {
        private readonly int m_AttackerID;
        private readonly int m_DefenderID;

        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }

        public int Damage { get; set; }
        public int HPResult { get; set; }
        public int SkillEventID { get; set; }
        public bool FrontHit { get; set; }//앞,뒤 판정
        public byte HitDirection { get; set; }

        public override uint MsgCode
        {
            get { return (uint)eMessage.Hit; }
        }

        public int AttackerId
        {
            get { return m_AttackerID; }
        }

        public int DefenderId
        {
            get { return m_DefenderID; }
        }

        public HitMsg(int attackerID, int defenderID) : base(defenderID)
        {
            m_AttackerID = attackerID;
            m_DefenderID = defenderID;
        }
    }


    public class DeadMsg : DeliveryMessage
    {
        private readonly int m_AttackerID;
        private readonly int m_DefenderID;

        public byte Weapon { get; set; }
        public int Damage { get; set; }
        public byte Dir { get; set; }

        public override uint MsgCode
        {
            get { return (uint)eMessage.Dead; }
        }

        public int AttackerId
        {
            get { return m_AttackerID; }
        }

        public int DefenderId
        {
            get { return m_DefenderID; }
        }

        public DeadMsg(int attackerID, int defenderID)
        {
            m_AttackerID = attackerID;
            m_DefenderID = defenderID;
            receiverID = DefenderId;
        }
    }
}