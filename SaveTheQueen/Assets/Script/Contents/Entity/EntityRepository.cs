using System;
using System.Collections.Generic;
using Lib.Event;
using UnityEngine;

namespace Aniz.Contents.Entity
{
    public class EntityRepository : Lib.Pattern.IRepository<long, IEntity>, IMessageHandler
    {
        private Dictionary<long, IEntity> m_players = new Dictionary<long, IEntity>();
        public void Initialize()
        {
        }

        public void Terminate()
        {
            m_players.Clear();
        }

        public bool Get(long id, out IEntity actor)
        {
            return m_players.TryGetValue(id, out actor);
        }

        public void Insert(IEntity node)
        {
            if (m_players.ContainsKey(node.NetID) == false)
            {
                m_players.Add(node.NetID, node);
            }
            else
            {
                Debug.LogError("ActorRepository Insert ID OverLap!!! " + node.Name + " ID : " + node.NetID.ToString());
            }
        }

        public bool Remove(long index)
        {
            return m_players.Remove(index);
        }

        public bool GetActors(ref List<IEntity> lstActors)
        {
            lstActors.AddRange(m_players.Values);
            return lstActors.Count > 0;
        }

        public bool GetActorsByCondition(ref List<IEntity> lstActors, Predicate<IEntity> conditionFunc)
        {
            foreach (var player in m_players.Values)
            {
                if (conditionFunc(player))
                    lstActors.Add(player);
            }

            return lstActors.Count > 0;
        }

        public bool OnMessage(IMessage message)
        {
            var itor = m_players.GetEnumerator();
            while (itor.MoveNext())
            {
                if (message.IsDeliveryMsg)
                {
                    IEntity entity = itor.Current.Value;
                    DeliveryMessage deliveryMessage = message as DeliveryMessage;
                    if (deliveryMessage.receiverID == entity.NetID)
                    {
                        if (entity.OnMessage(deliveryMessage))
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