using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Aniz.Graph;
using Aniz.Resource;
using Aniz.Resource.Unit;
using Lib.Event;
using Lib.Pattern;
using Lib.Pool;
using table.db;

namespace Aniz.Factory
{
    public class FactoryManager : GlobalManagerBase<FactoryManagerSetting>
    {
        private List<IPoolFactory> m_poolFactorieList = new List<IPoolFactory>();
        private FastPoolFactory m_fastPoolFactory;

        public FastPoolFactory FastPool
        {
            get { return m_fastPoolFactory; }
        }


        #region Events
        public override void OnAppStart(ManagerSettingBase managerSetting)
        {
            m_name = typeof(FactoryManager).ToString();

            if (string.IsNullOrEmpty(m_name))
            {
                throw new System.Exception("manager name is empty");
            }

            m_setting = managerSetting as FactoryManagerSetting;

            if (null == m_setting)
            {
                throw new System.Exception("manager setting is null");
            }

            CreateRootObject(null, "PoolFactory");
            GameObject.DontDestroyOnLoad(m_rootObject);
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
        }

        public override IEnumerator OnPageExit()
        {
            yield return new WaitForEndOfFrame();
        }

        #endregion Events

        #region IBhvUpdatable

        public override void BhvOnEnter()
        {
            m_fastPoolFactory = new FastPoolFactory(m_rootObject.transform);
            m_poolFactorieList.Add(m_fastPoolFactory);
        }

        public override void BhvOnLeave()
        {
            for (int i = m_poolFactorieList.Count - 1; i >= 0; --i)
            {
                m_poolFactorieList[i].DestroyPools();
            }
            m_poolFactorieList.Clear();
        }

        public override void BhvFixedUpdate(float dt)
        {
        }

        public override void BhvLateFixedUpdate(float dt)
        {
        }

        public override void BhvUpdate(float dt)
        {
        }

        public override void BhvLateUpdate(float dt)
        {
        }

        public override bool OnMessage(IMessage message)
        {
            return false;
        }

        #endregion IBhvUpdatable

        #region Methods

        public void FastDestroy<T>(int InstanceID, T sceneObject) where T : Component
        {
            m_fastPoolFactory.FastDestroy(InstanceID, sceneObject);
        }

        public void FastDestroyPool(int InstanceID)
        {
            m_fastPoolFactory.DestroyPool(InstanceID);
        }

        public T CreatePoolComponent<T>(ePath path, string prefabName, Vector3 position, Quaternion rotation, Transform parentTransform, bool active = true) where T : Component
        {
            GameObject gameObject = LoadPrefab(path, prefabName);
            return CreatePoolComponent<T>(gameObject, position, rotation, parentTransform, active);
        }





        public T CreatePoolComponent<T>(int poolHash, Vector3 position, Quaternion rotation, Transform parentTransform, bool active = true) where T : Component
        {
            T gameoObject = m_fastPoolFactory.CreatePoolComponent<T>(poolHash, position, rotation, parentTransform, active);
            if (gameoObject == null)
                return null;

            return gameoObject;
        }

        private T CreatePoolComponent<T>(GameObject poolGameObject, Vector3 position, Quaternion rotation, Transform parentTransform, bool active = true) where T : Component
        {
            if (poolGameObject != null)
            {
                T gameoObject = m_fastPoolFactory.CreatePoolComponent<T>(poolGameObject, position, rotation, parentTransform, active);
                if (gameoObject == null)
                    return null;

                return gameoObject;
            }
            return null;
        }

        public void FastDestory(GraphMonoPoolNode poolNode)
        {
            m_fastPoolFactory.FastDestroy(poolNode.ParentInstanceID, poolNode);
        }

        public void FastDestory<T>(int InstanceID, T sceneObject) where T : Component
        {
            m_fastPoolFactory.FastDestroy<T>(InstanceID, sceneObject);
        }

        public IEnumerator CreatePrefabCacheAsysc<T>(ePath path, string prefabName, int preloadCount, System.Action<int> completed) where T : Component
        {
            GameObject gameObject = null;

            yield return LoadActorAsysc(path, prefabName, resource =>
            {
                gameObject = resource;
            });

            int resultCode = -1;
            if (gameObject != null)
            {
                resultCode = CreatePoolCache<T>(gameObject, false, preloadCount);

                if (completed != null)
                {
                    completed(resultCode);
                }
            }
            else
            {
                if (completed != null)
                {
                    completed(resultCode);
                }
            }
        }


        public int CreatePoolCache<T>(GameObject gameObject, bool warmOnLoad = false, int preloadCount = 0, int capacity = 0) where T : Component
        {
            if (gameObject != null)
            {
                FastPool fastPool = m_fastPoolFactory.CreatePoolC<T>(gameObject, warmOnLoad, preloadCount, capacity);
                return fastPool.ID;
            }
            return -1;
        }

        public int CreatePoolCache(ePath path, string prefabName, int preloadCount, int capacity)
        {
            GameObject gameObject = LoadPrefab(path, prefabName);
            return CreatePoolCache(gameObject, preloadCount, capacity);
        }

        public int CreatePoolCache(GameObject gameObject, int preloadCount, int capacity)
        {
            if (gameObject != null)
            {
                FastPool fastPool = m_fastPoolFactory.CreatePool(gameObject, false, preloadCount, capacity);
                return fastPool.ID;
            }
            return -1;
        }


        private GameObject LoadPrefab(ePath path, string prefabName)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                return null;
            }

            PrefabResource resource = Global.ResourceMgr.CreatePrefabResource(prefabName, path);
            if (resource != null)
            {
                return resource.ResourceGameObject;
            }

            return null;
        }


        private IEnumerator LoadActorAsysc(ePath path, string prefabName, System.Action<GameObject> action)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                if (action != null)
                {
                    action(null);
                }

                yield break;
            }

            PrefabResource prefabResource = null;
            yield return Global.ResourceMgr.CreatePrefabResourceAsync(path, prefabName, resource =>
           {
               if (action != null)
               {
                   prefabResource = resource;
                   action(prefabResource != null ? resource.ResourceGameObject : null);
               }
           });
        }
        
        #endregion Methods
    }
}