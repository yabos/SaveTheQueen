using Lib.AnimationEvent;
using Lib.Battle;
using Lib.Event;
using Lib.Pattern;
using System.Collections;
using Aniz.Contents.Entity;
using Aniz.Contents.Entity.Info;
using Aniz.NodeGraph.Level.Group.Info;
using Aniz.Graph;
using Aniz.InputButton.Controller;
using Aniz.Resource;
using Aniz.SFX;
using UnityEngine;
using MonsterInfo = Aniz.Contents.Entity.Info.MonsterInfo;
using PCInfo = Aniz.Contents.Entity.Info.PCInfo;

namespace Aniz.Contents
{
    public class GameManager : GlobalManagerBase<GameManagerSetting>
    {
        private BattleEmulator m_battleEmulator;
        private GameMessageListener m_gameMessageListener;
        private GameRepository m_gameRepository;
        private PlayerButtonController m_playerButtonController;


        private bool m_isPlay = false;

        public BattleEmulator Battle
        {
            get { return m_battleEmulator; }
        }

        public GameRepository Repository
        {
            get { return m_gameRepository; }
        }


        #region Events
        public override void OnAppStart(ManagerSettingBase managerSetting)
        {
            m_name = typeof(GameManager).ToString();

            if (string.IsNullOrEmpty(m_name))
            {
                throw new System.Exception("manager name is empty");
            }

            m_setting = managerSetting as GameManagerSetting;

            if (null == m_setting)
            {
                throw new System.Exception("manager setting is null");
            }

            CreateRootObject(m_setting.transform, "GameManager");

            m_gameMessageListener = new GameMessageListener(this);
            m_gameRepository = new GameRepository(this);
            m_battleEmulator = new BattleEmulator(m_gameRepository);
        }

        public override void OnAppEnd()
        {
            DestroyRootObject();

            if (m_setting != null)
            {
                GameObjectFactory.DestroyComponent(m_setting);
                m_setting = null;
            }
        }

        public override void OnAppFocus(bool focused)
        {

        }

        public override void OnAppPause(bool paused)
        {

        }

        public override void OnPageEnter(string pageName)
        {
            //m_gameRepository.AttachLevel();
            m_isPlay = true;
        }

        public override IEnumerator OnPageExit()
        {
            m_isPlay = false;


            m_battleEmulator.Terminate();

            m_gameRepository.DestroyHero();
            m_gameRepository.Terminate();

            AudioListenerSingle.Instance.Reset();

            yield return new WaitForEndOfFrame();
        }


        #endregion Events


        #region IBhvUpdatable

        public override void BhvOnEnter()
        {
            m_battleEmulator.Initialize();
            m_gameRepository.Initialize();
        }

        public override void BhvOnLeave()
        {
            m_battleEmulator.Terminate();
            m_gameRepository.Terminate();
        }

        public override void BhvFixedUpdate(float dt)
        {
            if (!m_isPlay)
                return;

            m_battleEmulator.OnFixedUpdate(dt);
        }

        public override void BhvLateFixedUpdate(float dt)
        {
            if (!m_isPlay)
                return;

        }

        public override void BhvUpdate(float dt)
        {
            if (!m_isPlay)
                return;

        }

        public override void BhvLateUpdate(float dt)
        {
            if (!m_isPlay)
                return;

        }

        public override bool OnMessage(IMessage message)
        {
            if (m_gameMessageListener.OnMessage(message))
            {
                return true;
            }

            if (m_gameRepository.OnMessage(message))
            {
                return true;
            }


            return false;
        }
        #endregion IBhvUpdatable

        public IEntity MakePlayer(BaseEntityInfo info)
        {
            IEntity actor = m_gameRepository.CreatePlayer(info, null);
            if (actor != null)
            {
                if (info.IsUser)
                {
                    AudioListenerSingle.Instance.transform.SetParent(actor.Main.transform, false);
                }
                else
                {
                    m_gameRepository.Insert(actor);
                }
            }

            return actor;
        }


        public Entity.Hero CreateHero(BaseEntityInfo info)
        {
            //Vector3 dir = LEMath.RadianToDir3(LEMath.ByteToRadian(playerEnterInfo.SyncState._syncPos._direction));
            //PCInfo pcInfo = new PCInfo(eCombatType.Hero, playerEnterInfo.Name, playerEnterInfo.NetID, playerEnterInfo.TableID);
            //{
            //    pcInfo.ActorAsset.Pos = playerEnterInfo.SyncState._syncPos._pos;
            //    pcInfo.ActorAsset.Rotate = Quaternion.LookRotation(dir);
            //    pcInfo.ActorAsset.Path = pcInfo.SpriteData.assetName;
            //    pcInfo.BattleTeam = playerEnterInfo.CombatTeam;
            //    pcInfo.StartTick = playerEnterInfo.SyncState._startTick;
            //}

            //Player.Hero hero = MakePlayer(pcInfo) as Player.Hero;

            //PlayerButtonController playerButtonController = new PlayerButtonController();
            //playerButtonController.InitActor(hero, hero.NetID);

            //Global.InputMgr.AddController(playerButtonController);
            //Global.CameraMgr.SetHero(hero.Main.transform);

            return null;
        }

        public void DestroyHero()
        {
            m_gameRepository.DestroyHero();
        }

        public IEntity CreateEnterPlayer(BaseEntityInfo entityEnterInfo)
        {
            BaseEntityInfo info = null;
            if (entityEnterInfo.CombatType == eCombatType.PC)
            {
                PCInfo pcinfo = entityEnterInfo as PCInfo;

                info = pcinfo;

                //info.AssetInfo.sp = pcinfo.SpriteData.assetName;
            }
            else if (entityEnterInfo.CombatType == eCombatType.Monster)
            {
                MonsterInfo monsterInfo = entityEnterInfo as MonsterInfo;

                info = monsterInfo;

                //info.AssetInfo.Path = monsterInfo.SpriteData.spriteName;
            }

            //info.ActorAsset.Pos = playerEnterInfo.SyncState._syncPos._pos;
            //Vector3 dir = LEMath.RadianToDir3(LEMath.ByteToRadian(playerEnterInfo.SyncState._syncPos._direction));
            //info.ActorAsset.Rotate = Quaternion.LookRotation(dir);

            IEntity actor = MakePlayer(info);


            return actor;
        }

        public void DestroyCombat(int id)
        {
            //m_gameRepository.DestroyPlayer(id);
        }


        public void DestroyPlayer(IEntity entity)
        {
            m_gameRepository.DestroyPlayer(entity);
        }


        public Hero CreateHero(ActorEnterInfo actorEnterInfo)
        {
            //Vector3 dir = LEMath.RadianToDir3(LEMath.ByteToRadian(actorEnterInfo.SyncState.syncPos.direction));
            //PCInfo pcInfo = new PCInfo(eNodeType.Hero, actorEnterInfo.Name, actorEnterInfo.NetID, actorEnterInfo.TableID);
            //{
            //    pcInfo.Mesh.Pos = actorEnterInfo.SyncState.syncPos.position;
            //    pcInfo.Mesh.Rotate = Quaternion.LookRotation(dir);
            //    pcInfo.Mesh.Path = pcInfo.ClassData.Resource;
            //    pcInfo.FsmPath = pcInfo.ClassData.PlayerController;
            //}

            //Hero hero = MakeActor(pcInfo) as Hero;

            //m_playerButtonController = new PlayerButtonController();
            //m_playerButtonController.InitActor(hero);

            //Global.InputMgr.AddController(m_playerButtonController);
            //Global.CameraMgr.SetTargetActor(hero);

            return null;
        }

        public void CreateAutoTileCache(int size)
        {
            Global.FactoryMgr.CreatePoolCache(ePath.MapAutoTile, "AutoTile", size, size + size / 4);
        }
        

        public static eNodeStorageType GetStorageType(IGraphNode node)
        {
            eNodeStorageType storageType = eNodeStorageType.Actor;
            switch (node.NodeCategory)
            {
                case eNodeCategory.Actor:
                    storageType = eNodeStorageType.Actor;
                    break;
                case eNodeCategory.Entity:
                    storageType = eNodeStorageType.Entity;
                    break;
                case eNodeCategory.Scene:
                    storageType = eNodeStorageType.Scene;
                    break;
            }
            return storageType;
        }

#if UNITY_EDITOR

        public IEntity MakeActor_Editor(BaseEntityInfo info, AnimationEventStates animationEventStates)
        {
            IEntity actor = m_gameRepository.CreatePlayer(info, animationEventStates);
            if (actor != null)
            {
                if (info.IsUser)
                {
                    AudioListenerSingle.Instance.transform.SetParent(actor.Main.transform, false);
                }
                else
                {
                    m_gameRepository.Insert(actor);
                }
            }

            return actor;
        }

#endif
    }
}