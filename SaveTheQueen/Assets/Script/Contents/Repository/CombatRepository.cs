using Lib.Battle;
using Lib.Event;
using System;
using System.Collections.Generic;
using Aniz.Contents.Entity;
using UnityEngine;

namespace Aniz.Contents.Repository
{
    public class CombatRepository : Lib.Pattern.IRepository<long, ICombatOwner>, IMessageHandler
    {
        private readonly eCombatType m_combatType;
        private Dictionary<long, ICombatOwner> m_combatOwners = new Dictionary<long, ICombatOwner>();

        public CombatRepository(eCombatType combatType)
        {
            m_combatType = combatType;
        }

        public void Initialize()
        {
        }

        public void Terminate()
        {
            m_combatOwners.Clear();
        }

        public bool Get(long id, out ICombatOwner actor)
        {
            return m_combatOwners.TryGetValue(id, out actor);
        }

        public bool Get(long id, out IEntity actor)
        {
            actor = null;
            if (m_combatType < eCombatType.Anim)
            {
                return false;
            }

            ICombatOwner combat = null;
            if (m_combatOwners.TryGetValue(id, out combat))
            {
                actor = combat as IEntity;
                return true;
            }

            return false;
        }

        public void Insert(ICombatOwner node)
        {
            if (m_combatOwners.ContainsKey(node.NetID) == false)
            {
                m_combatOwners.Add(node.NetID, node);
            }
            else
            {
                Debug.LogError("ActorRepository Insert ID OverLap!!! " + node.Name + " ID : " + node.NetID.ToString());
            }
        }

        public bool Remove(long index)
        {
            return m_combatOwners.Remove(index);
        }

        public int GetCount()
        {
            return m_combatOwners.Count;
        }

        public bool GetCombats(ref List<ICombatOwner> lstActors)
        {
            lstActors.AddRange(m_combatOwners.Values);
            return lstActors.Count > 0;
        }

        public bool GetTransforms(ref List<Transform> lstTransforms)
        {
            using (var itor = m_combatOwners.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    ICombatOwner combatOwner = itor.Current.Value;
                    if (combatOwner.GetOwnerTransform() != null)
                    {
                        lstTransforms.Add(combatOwner.GetOwnerTransform());
                    }
                }
            }

            return lstTransforms.Count > 0;
        }

        //public bool GetCombats(ref List<IPlayer> lstActors)
        //{
        //    if (m_combatType > eCombatType.Anim)
        //    {
        //        using (var itor = m_combatOwners.GetEnumerator())
        //        {
        //            while (itor.MoveNext())
        //            {
        //                lstActors.Add(itor.Current.Value as IPlayer);
        //            }
        //        }

        //        return lstActors.Count > 0;
        //    }
        //    return false;
        //}


        public bool GetCombatsByCondition(ref List<ICombatOwner> lstActors, Predicate<ICombatOwner> conditionFunc)
        {
            foreach (var player in m_combatOwners.Values)
            {
                if (conditionFunc(player))
                    lstActors.Add(player);
            }

            return lstActors.Count > 0;
        }

        public bool OnMessage(IMessage message)
        {
            var itor = m_combatOwners.GetEnumerator();
            while (itor.MoveNext())
            {
                if (message.IsDeliveryMsg)
                {
                    ICombatOwner combatOwner = itor.Current.Value;
                    DeliveryMessage deliveryMessage = message as DeliveryMessage;
                    if (deliveryMessage.receiverID == combatOwner.NetID)
                    {
                        if (combatOwner.OnMessage(deliveryMessage))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}