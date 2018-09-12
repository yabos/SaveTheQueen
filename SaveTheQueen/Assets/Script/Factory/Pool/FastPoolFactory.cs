using UnityEngine;
using System.Collections.Generic;
using Lib.Pool;

namespace Aniz.Factory
{
    public class FastPoolFactory : PoolFactory
    {
        private Dictionary<int, FastPool> m_pools = new Dictionary<int, FastPool>();

        public FastPoolFactory(Transform root) : base(root)
        {
        }

        /// <summary>
        /// Create a new pool from provided component
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">Component which game object will be cloned</param>
        /// <param name="preloadCount">How much items must be preloaded and cached</param>
        /// <param name="capacity">Cache size (maximum amount of the cached items). [0 - unlimited]</param>
        /// <param name="warmOnLoad">Load source prefab in the memory while scene is loading. A bit slower scene loading, but much faster instantiating of new objects in pool</param>
        /// <returns>FastPool instance</returns>
        public FastPool CreatePoolC<T>(GameObject prefab, bool warmOnLoad = false, int preloadCount = 0, int capacity = 0) where T : Component
        {
            if (prefab != null)
            {
                if (m_pools.ContainsKey(prefab.GetInstanceID()) == false)
                {
                    Transform poolTransform = CreatePoolFactory(prefab);

                    FastPool newPool = new FastPool(prefab, poolTransform, warmOnLoad, preloadCount, capacity);
                    m_pools.Add(prefab.GetInstanceID(), newPool);
                    newPool.InitC<T>();
                    return newPool;
                }

                return m_pools[prefab.GetInstanceID()];
            }

            Debug.LogError("Creating pool with null object");
            return null;
        }

        /// <summary>
        /// Create a new pool from provided prefab
        /// </summary>
        /// <param name="prefab">Prefab that will be cloned</param>
        /// <param name="preloadCount">How much items must be preloaded and cached</param>
        /// <param name="capacity">Cache size (maximum amount of the cached items). [0 - unlimited]</param>
        /// <param name="warmOnLoad">Load source prefab in the memory while scene is loading. A bit slower scene loading, but much faster instantiating of new objects in pool</param>
        /// <returns>FastPool instance</returns>
        public FastPool CreatePool(GameObject prefab, bool warmOnLoad = false, int preloadCount = 0, int capacity = 0)
        {
            if (prefab != null)
            {
                if (m_pools.ContainsKey(prefab.GetInstanceID()) == false)
                {
                    Transform poolTransform = CreatePoolFactory(prefab);

                    FastPool newPool = new FastPool(prefab, poolTransform, warmOnLoad, preloadCount, capacity);
                    m_pools.Add(prefab.GetInstanceID(), newPool);
                    newPool.Init();
                    return newPool;
                }

                return m_pools[prefab.GetInstanceID()];
            }

            Debug.LogError("Creating pool with null object");
            return null;
        }



        /// <summary>
        /// Returns pool for the specified prefab or creates it if needed (with default params)
        /// </summary>
        /// <param name="prefab">Source Prefab</param>
        /// <returns></returns>
        public FastPool GetPool(GameObject prefab, Transform parent, bool createIfNotExists = true)
        {
            if (prefab != null)
            {
                if (m_pools.ContainsKey(prefab.GetInstanceID()))
                    return m_pools[prefab.GetInstanceID()];
                else
                    return CreatePool(prefab, parent);
            }
            else
            {
                Debug.LogError("Trying to get pool for null object");
                return null;
            }
        }

        public FastPool GetPool(int InstanceID)
        {
            if (m_pools.ContainsKey(InstanceID))
                return m_pools[InstanceID];

            return null;
        }

        /// <summary>
        /// Returns pool for the specified prefab or creates it if needed (with default params)
        /// </summary>
        /// <param name="component">Any component of the source prefab</param>
        /// <returns></returns>
        //public FastPool GetPool(Component component, Transform parent, bool createIfNotExists = true)
        //{
        //    if (component != null)
        //    {
        //        GameObject prefab = component.gameObject;
        //        if (m_pools.ContainsKey(prefab.GetInstanceID()))
        //            return m_pools[prefab.GetInstanceID()];
        //        else
        //            return CreatePool(prefab, parent);
        //    }
        //    else
        //    {
        //        Debug.LogError("Trying to get pool for null object");
        //        return null;
        //    }
        //}


        public GameObject CreatePoolObject(int prefabHash, Vector3 position, Quaternion rotation, Transform parentTransform, bool active)
        {
            FastPool fastPool = GetPool(prefabHash);
            if (fastPool != null)
            {
                return fastPool.FastInstantiate(active, position, rotation, parentTransform);
            }
            return null;
        }
        public GameObject CreatePoolObject(GameObject poolGameObject, Vector3 position, Quaternion rotation, Transform parentTransform, bool active)
        {
            FastPool fastPool = GetPool(poolGameObject.GetInstanceID());
            if (fastPool != null)
            {
                return fastPool.FastInstantiate(active, position, rotation, parentTransform);
            }
            return null;
        }

        public T CreatePoolComponent<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parentTransform, bool active) where T : Component
        {
            FastPool fastPool = GetPool(prefab.GetInstanceID());

            if (fastPool == null)
            {
                fastPool = CreatePoolC<T>(prefab);
            }

            return fastPool.FastInstantiate<T>(active, position, rotation, parentTransform);
        }


        public T CreatePoolComponent<T>(int poolHash, Vector3 position, Quaternion rotation, Transform parentTransform, bool active) where T : Component
        {
            FastPool fastPool = GetPool(poolHash);

            if (fastPool == null)
            {
                return null;
            }

            return fastPool.FastInstantiate<T>(active, position, rotation, parentTransform);
        }

        public void FastDestroy<T>(int InstanceID, T sceneObject) where T : Component
        {
            FastPool fastPool = GetPool(InstanceID);
            fastPool.FastDestroy(sceneObject);
        }

        public void FastDestroy(int InstanceID, GameObject sceneObject)
        {
            FastPool fastPool = GetPool(InstanceID);
            fastPool.FastDestroy(sceneObject);
        }

        public void FastDestroy(GameObject gameObject)
        {
            //FastPool fastPool = GetPool(InstanceID);
            //fastPool.FastDestroy(sceneObject);
        }

        /// <summary>
        /// Destroys provided pool and it's cached objects
        /// </summary>
        /// <param name="pool">Pool to destroy</param>
        public void DestroyPool(FastPool pool)
        {
            pool.ClearCache();
            DestroyPoolFactory(pool.ID);
            m_pools.Remove(pool.ID);
        }

        public void DestroyPool(int InstanceID)
        {
            FastPool fastPool = GetPool(InstanceID);

            if (fastPool != null)
            {
                fastPool.ClearCache();
                DestroyPoolFactory(fastPool.ID);
                m_pools.Remove(fastPool.ID);
            }
        }

        public override void DestroyPools()
        {
            foreach (FastPool pool in m_pools.Values)
            {
                pool.ClearCache();
            }
            m_pools.Clear();

            base.DestroyPools();
        }

    }

}