using System;
using System.Collections;
using System.Collections.Generic;

namespace Aniz.Event
{
    public abstract class NetworkNotifyBase : EventNotifyBase
    {
        public virtual bool IsNetMsg
        {
            get
            {
                return true;
            }
        }

        public int ReceiverID { get; set; }

        public virtual bool IsDeliveryMsg
        {
            get { return true; }
        }
    }

    /**************************************************/

    public class NetworkNotify : NetworkNotifyBase
    {
        public override eMessage MsgType
        {
            get { return eMessage.Network; }
        }

        private eNetworkSubMessage m_subMessage;
        public eNetworkSubMessage SubMsgType
        {
            get { return m_subMessage; }
        }

        public NetworkNotify(eNetworkSubMessage subMessage)
        {
            m_subMessage = subMessage;
        }
    }

    /**************************************************/
    public class NetworkConnectionInfoNotify : NetworkNotify
    {
        public bool IsConnected
        {
            get; private set;
        }

        public NetworkConnectionInfoNotify(eNetworkSubMessage subMessage, bool isConnected = false) : base(subMessage)
        {
            IsConnected = isConnected;
        }
    }

    /**************************************************/
    public class NetworkChatMessageNotify : NetworkNotify
    {
        public string Nick
        {
            get; private set;
        }

        public string Content
        {
            get; private set;
        }

        public NetworkChatMessageNotify(string nick, string content) : base(eNetworkSubMessage.ChatMessage)
        {
            Nick = nick;
            Content = content;
        }
    }

    /**************************************************/
    public class NetworkLoginNotify : NetworkNotify
    {
        public string Nick
        {
            get; private set;
        }

        public int UserGuid
        {
            get; private set;
        }

        public int StreamId
        {
            get; private set;
        }

        public int Gold
        {
            get; private set;
        }

        public int Cash
        {
            get; private set;
        }

        public int GuildPoint
        {
            get; private set;
        }

        public int PvpPoint
        {
            get; private set;
        }

        public NetworkLoginNotify(string nick, int userGuid, int streamId, int gold, int cash, int guildPoint, int pvpPoint)
            : base(eNetworkSubMessage.Login)
        {
            Nick = nick;
            UserGuid = userGuid;
            StreamId = streamId;
            Gold = gold;
            Cash = cash;
            GuildPoint = guildPoint;
            PvpPoint = pvpPoint;
        }
    }

    /**************************************************/
    public class NetworkLogoutNotify : NetworkNotify
    {
        public NetworkLogoutNotify() : base(eNetworkSubMessage.Login)
        {
        }
    }

    /**************************************************/
    public class NetworkJoinRoomNotify : NetworkNotify
    {
        public int MyUid
        {
            get; private set;
        }

        public int Uid
        {
            get; private set;
        }

        public NetworkJoinRoomNotify(int myUid, int uid) : base(eNetworkSubMessage.JoinRoom)
        {
            MyUid = myUid;
            Uid = uid;
        }
    }

    /**************************************************/
    public class NetworkLeaveRoomNotify : NetworkNotify
    {
        public int MyUid
        {
            get; private set;
        }

        public int Uid
        {
            get; private set;
        }

        public NetworkLeaveRoomNotify(int myUid, int uid) : base(eNetworkSubMessage.LeaveRoom)
        {
            MyUid = myUid;
            Uid = uid;
        }
    }
}
