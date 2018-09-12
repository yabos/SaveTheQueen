using Aniz.Contents.Entity;
using Aniz.Event;
using Lib.Event;

namespace Aniz.Contents
{
    public class GameMessageListener : MessageListener
    {
        private GameManager m_gameManager;

        public GameMessageListener(GameManager gameManager)
        {
            m_gameManager = gameManager;

            AddHandler((uint)eMessage.PCEnter, OnPCEnter);
            AddHandler((uint)eMessage.MonsterEnter, OnMonsterEnter);

            AddHandler((uint)eMessage.Leave, OnLeave);

        }

        private bool OnPCEnter(IMessage message)
        {
            PCEnterMsg enterMsg = message as PCEnterMsg;
            //if (enterMsg.EnterInfo.CombatType == eCombatType.Hero)
            //{
            //    m_gameManager.CreateHero(enterMsg.EnterInfo);
            //}
            //else
            //{
            //    m_gameManager.CreateEnterPlayer(enterMsg.EnterInfo);
            //}
            return true;
        }

        private bool OnMonsterEnter(IMessage message)
        {
            MonsterEnterMsg enterMsg = message as MonsterEnterMsg;

            //IPlayer monster = m_gameManager.CreateEnterPlayer(enterMsg.EnterInfo);
            //SpawnerNotify spawnerEnterNotify = new SpawnerNotify(eMessage.SpawnMonster);
            //spawnerEnterNotify.ActorNetID = monster.NetID;
            //spawnerEnterNotify.SpawnerID = enterMsg.SpawnerID;
            //spawnerEnterNotify.WaveIndex = enterMsg.WaveIndex;

            //  ilkyo : 스폰 이벤트 어디다 두어야 할지 논의 해야함
            //monster.Fsm.SendEvent(spawnerEnterNotify.SpawnEvent);

            //Global.NotificationMgr.NotifyToEventHandler("OnNotify", spawnerEnterNotify);

            return true;
        }

        private bool OnTriggerBoxEnter(IMessage message)
        {
            UnityEngine.Debug.LogError("OnTriggerBoxEnter");
            return true;
        }

        private bool OnLeave(IMessage message)
        {
            //PlayerLeaveMsg leaveMsg = message as PlayerLeaveMsg;

            //IPlayer actor = Global.GameMgr.Repository.GetPlayer(leaveMsg.NetID);
            //if (actor.PlayerInfo.CombatType == eCombatType.Monster)
            //{
            //    SpawnerNotify spawnerEnterNotify = new SpawnerNotify(eMessage.DeadMonster);
            //    spawnerEnterNotify.ActorNetID = actor.NetID;
            //    Global.NotificationMgr.NotifyToEventHandler("OnNotify", spawnerEnterNotify);

            //    m_gameManager.DestroyPlayer(actor);
            //}

            return true;
        }
    }
}