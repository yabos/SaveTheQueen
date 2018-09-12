using Lib.AnimationEvent;
using Lib.Battle;
using Lib.Event;
using Lib.Pattern;
using System.Collections.Generic;
using Aniz.Contents.Entity;
using Aniz.Contents.Entity.Info;
using Aniz.Contents.Repository;
using Aniz.Data;
using Aniz.NodeGraph.Level.Group;
using Aniz.NodeGraph.Level.Group.Node;
using Aniz.Graph;
using Aniz.Resource;
using Aniz.Resource.Unit;
using Aniz.NodeGraph.Level;
using table.db;
using UnityEngine;

namespace Aniz.Contents
{

    public class GameRepository : IBaseClass, IMessageHandler, ICombatFinder
    {
        private CombatRepository[] m_combatRepositories;
        private Hero m_hero;
        private GameManager m_gameManager;
        
        public GameRepository(GameManager gameManager)
        {
            m_gameManager = gameManager;

            m_hero = null;


            m_combatRepositories = new CombatRepository[(int)eCombatType.Max];
            for (int i = 0; i < (int)eCombatType.Max; i++)
            {
                m_combatRepositories[i] = new CombatRepository((eCombatType)(i));
            }

        }

        public Hero User
        {
            get { return m_hero; }
        }


        public void Initialize()
        {
            for (int i = 0; i < (int)eCombatType.Max; i++)
            {
                m_combatRepositories[i].Initialize();
            }

        }

        public void Terminate()
        {
            DestroyHero();

            for (int i = 0; i < (int)eCombatType.Max; i++)
            {
                m_combatRepositories[i].Terminate();
            }
        }

        public void DestroyHero()
        {
            if (m_hero != null)
            {
                Global.InputMgr.RemoveController(m_hero.NetID);
            }
            m_hero = null;
        }

        public void Insert(ICombatOwner actor)
        {
            CombatRepository repository = GetRepository(actor.CombatType);
            repository.Insert(actor);
        }

        public bool Remove(eCombatType actorType, long id)
        {
            CombatRepository repository = GetRepository(actorType);
            return repository.Remove(id);
        }

        public IEntity GetPlayer(long id)
        {
            IEntity actor = null;

            if (m_hero.NetID == id)
                return m_hero;

            for (int i = 0; i < (int)eCombatType.Max; i++)
            {
                if (m_combatRepositories[i].Get(id, out actor))
                {
                    return actor;
                }
            }
            return null;
        }

        public IEntity GetPlayer(eCombatType nodeType, long id)
        {
            if (m_hero.NetID == id)
                return m_hero;

            IEntity actor = null;
            CombatRepository repository = GetRepository(nodeType);
            if (repository.Get(id, out actor))
            {
                return actor;
            }
            return null;
        }

        #region ICombatFinder
        public IEntity FindNearestTarget(eCombatType targetType, Vector3 myPosition, float radius)
        {
            List<ICombatOwner> combatList = new List<ICombatOwner>();

            if (targetType == eCombatType.Hero || targetType == eCombatType.PC)
                combatList.Add(m_hero);

            CombatRepository repository = GetRepository(targetType);
            repository.GetCombats(ref combatList);

            float minDistant = float.MaxValue;
            IEntity actor = null;
            for (int i = 0; i < combatList.Count; i++)
            {
                float sqrDistant = (myPosition - combatList[i].GetOwnerPosition()).sqrMagnitude;
                if (sqrDistant < radius * radius && sqrDistant < minDistant)
                {
                    minDistant = sqrDistant;
                    actor = combatList[i] as IEntity;
                }
            }

            return actor;
        }

        public int GetCombatCount(eCombatType nodeType)
        {
            CombatRepository repository = GetRepository(nodeType);
            return repository.GetCount();
        }

        public bool GetCombats(eCombatType nodeType, ref List<ICombatOwner> lstActor)
        {
            CombatRepository repository = GetRepository(nodeType);
            return repository.GetCombats(ref lstActor);
        }

        public bool GetCombatTransforms(eCombatType nodeType, ref List<Transform> lstTransforms)
        {
            CombatRepository repository = GetRepository(nodeType);
            return repository.GetTransforms(ref lstTransforms);
        }

        /// <summary>
        /// 인자로 주어진 battleTeam과 다른 team의 actor 목록을 얻어옴. - 17.07.21. #jonghyuk
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="lstActor"></param>
        /// <param name="battleTeam"></param>
        /// <returns></returns>
        public bool GetCombatsInOtherTeam(eCombatType nodeType, ref List<ICombatOwner> lstActor, int battleTeam)
        {
            CombatRepository repository = GetRepository(nodeType);
            return repository.GetCombatsByCondition(ref lstActor, (combatOwner) => combatOwner.BattleTeam != battleTeam);
        }
        #endregion

        private CombatRepository GetRepository(eCombatType actorType)
        {
            if (actorType >= eCombatType.Max)
            {
                return null;
            }

            return m_combatRepositories[(int)actorType];
        }


        public IEntity CreatePlayer(BaseEntityInfo info, AnimationEventStates animationEventStates)
        {
            LevelBase level = Global.NodeGraphMgr.WorldLevelRoot.ActorLevel;

            ActorImplNode actorMain = Global.FactoryMgr.CreatePoolComponent<ActorImplNode>(ePath.Actor, info.AssetInfo.SpriteName,
                info.AssetInfo.Position, info.AssetInfo.Rotation, level.transform);


            //actorMain.Initialize(playMakerFsm, animationEventStates);

            ActorRoot actor = new ActorRoot();
            actorMain.Lock();
            actor.AttachChild(actorMain);

            IEntity entity = MakePlayer(info, actorMain);
            if (info.CombatType == eCombatType.Hero)
            {
                m_hero = entity as Hero;
            }
            else if (info.CombatType == eCombatType.Monster)
            {

            }


            AttachActor(level, actor);

            return entity;
        }

        private IEntity MakePlayer(BaseEntityInfo info, ActorImplNode actorMain)
        {
            IEntity entity = null;
            if (info.CombatType == eCombatType.PC)
            {
                entity = new PC(info, actorMain);
            }
            else if (info.CombatType == eCombatType.Monster)
            {
                entity = new Monster(info, actorMain);
            }
            else if (info.CombatType == eCombatType.Hero)
            {
                entity = new Hero(info, actorMain);
            }

            if (entity != null)
            {
                entity.Initialize();
                //actorMain.InitPlayer(player);
            }

            return entity;
        }

        public void DestroyPlayer(IEntity entity)
        {
            eCombatType combatType = entity.CombatType;

            IGraphNodeGroup parentnode = entity.Main.Parent as IGraphNodeGroup;
            Debug.Assert(parentnode != null);

            entity.Terminate();
            Remove(entity.CombatType, entity.NetID);

            LevelBase level = Global.NodeGraphMgr.WorldLevelRoot.ActorLevel;
            DetachActor(level, parentnode);

            //Global.FactoryMgr.Node.Destroy(actor);
        }
        /*
        public IEntity CreateEntity(EntityInfo info, AnimationEventStates animationEventStates)
        {
            LevelBase level = Global.SceneGraphMgr.WorldLevelRoot.GetLevel(info.CombatType);

            ObjectMain objectMain = null;
            if (info.CombatType == eCombatType.Anim)
            {
                ObjectAnimMain objectAnimMain = Global.FactoryMgr.CreatePoolComponent<ObjectAnimMain>(info.ActorAsset.PathType, info.ActorAsset.Path,
                    info.ActorAsset.Pos, info.ActorAsset.Rotate, level.transform);

                if (animationEventStates == null)
                {
                    animationEventStates = Global.FactoryMgr.CreateAnimationEvent(info.ActorAsset.PathType, info.ActorAsset.AnimationEventPath);
                }

                objectAnimMain.Initialize(animationEventStates);

                objectMain = objectAnimMain;

            }
            else
            {
                objectMain = Global.FactoryMgr.CreatePoolComponent<ObjectMain>(info.ActorAsset.PathType, info.ActorAsset.Path,
                    info.ActorAsset.Pos, info.ActorAsset.Rotate, level.transform);
            }

            IEntity entity = MakeEntity(info, objectMain);

            objectMain.Lock();

            ActorRoot root = new ActorRoot();
            root.AttachChild(objectMain);

            AttachActor(level, root);

            return entity;
        }

        private IEntity MakeEntity(EntityInfo info, ObjectMain objectMain)
        {
            IEntity entity = null;
            if (info.CombatType == eCombatType.Static)
            {
                entity = new StaticEntity(info, objectMain);
            }
            else if (info.CombatType == eCombatType.Anim)
            {
                entity = new AnimEntity(info, objectMain);
            }

            if (entity != null)
            {
                entity.Initialize();
            }
            return entity;
        }

        public void DestroyEntity(IEntity entity)
        {
            eCombatType combatType = entity.CombatType;

            Remove(entity.CombatType, entity.NetID);

            IGraphNodeGroup parentnode = entity.Main.Parent as IGraphNodeGroup;
            Debug.Assert(parentnode != null);

            entity.Terminate();

            LevelBase level = Global.SceneGraphMgr.WorldLevelRoot.GetLevel(combatType);
            DetachActor(level, parentnode);
        }
        */

        public void KillAllNPCs(bool awardXP)
        {
            IEntity attacker = null;
            List<ICombatOwner> lstActors = new List<ICombatOwner>();
            if (GetCombats(eCombatType.Monster, ref lstActors))
            {
                if (lstActors.Count <= 0)
                    return;
            }
            if (m_hero != null)
            {
                attacker = m_hero;
            }

            for (int i = 0; i < lstActors.Count; i++)
            {
                ICombatOwner actor = lstActors[i];
                //NetBattleHandler.SendDead(attacker.NetID, actor.NetID, 13, 0, 0, TimeManager.serverTick);
            }
        }

        public bool OnMessage(IMessage message)
        {
            if (m_hero != null && m_hero.OnMessage(message))
            {
                return true;
            }
            for (int i = 0; i < m_combatRepositories.Length; i++)
            {
                if (m_combatRepositories[i].OnMessage(message))
                {
                    return true;
                }
            }
            return false;
        }




        private void AttachActor(LevelBase level, IGraphNodeGroup nodeGroup)
        {
            Debug.Assert(level != null);

            nodeGroup.BhvOnEnter();
            level.AttachChild(nodeGroup);
        }

        private void DetachActor(LevelBase level, IGraphNodeGroup nodeGroup)
        {
            Debug.Assert(level != null);

            level.DetachChild(nodeGroup); // 레벨에서 삭제하고

            nodeGroup.BhvOnLeave(); //자식들을 종료시키고

            List<GraphMonoPoolNode> lstPoolNodes = nodeGroup.GetChilds<GraphMonoPoolNode>(); // 풀노드만 빼온후

            nodeGroup.DetachAllChildren(); //자식들을 제거함

            for (int i = 0; i < lstPoolNodes.Count; i++)
            {
                GraphMonoPoolNode poolNode = lstPoolNodes[i];
                poolNode.Unlock();
                Global.FactoryMgr.FastDestory(poolNode); //다시 풀링으로..
            }

            lstPoolNodes.Clear();
        }
        

        private string GetSortingLayerName(E_TileMaterial tileMaterial)
        {
            if (tileMaterial <= E_TileMaterial.RoomFloor)
            {
                return eGameLayer.BG.ToString();
            }
            else if (tileMaterial <= E_TileMaterial.Max)
            {
                return eGameLayer.BGObject.ToString();
            }
            else if (tileMaterial <= E_TileMaterial.Enemy)
            {
                return eGameLayer.GameObject.ToString();
            }
            return eGameLayer.BG.ToString();
        }

    }
}