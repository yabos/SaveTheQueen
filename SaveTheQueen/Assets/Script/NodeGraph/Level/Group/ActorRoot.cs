using Aniz.Graph;
using System;
using Aniz.NodeGraph.Level.Group.Node;
using Lib.Event;
using UnityEngine;


namespace Aniz.NodeGraph.Level.Group
{
    public class ActorRoot : GraphNodeGroup
    {

        private int m_targetID;

        public override eNodeType NodeType
        {
            get { return eNodeType.ActorRoot; }
        }


        public override void BhvOnEnter()
        {


            base.BhvOnEnter();
        }

        public override void BhvOnLeave()
        {
            base.BhvOnLeave();
        }

        public override void BhvUpdate(float dt)
        {
            base.BhvUpdate(dt);
        }

        public override bool OnMessage(IMessage message)
        {
            if (message.IsDeliveryMsg)
            {
                DeliveryMessage deliveryMessage = message as DeliveryMessage;
                if (deliveryMessage.receiverID == Uid)
                {
                    return true;
                }
            }
            return base.OnMessage(message);
        }


    }
}