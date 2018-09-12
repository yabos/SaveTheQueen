#define AUTO_SIZE_POOL

using UnityEngine;
using System.Collections.Generic;

namespace Lib.Pool
{

    public enum PoolItemNotificationType
    {
        None,
        Interface,
        SendMessage,
        BroadcastMessage
    }

    [System.Serializable]
    public class FastPool
    {
        /// <summary>
        /// ID of the source Prefab
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Name of the source Prefab
        /// </summary>
        public string Name
        {
            get { return sourcePrefab.name; }
        }

        /// <summary>
        /// Cached objects count
        /// </summary>
        public int Cached
        {
            get { return cache.Count; }
        }

        /// <summary>
        /// Shows if the pool has been successfully initialized. Has the same value as the last Init() call returns
        /// </summary>
        public bool IsValid { get; private set; }


        /// <summary>
        /// Prefab that would be used as source
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab that would be used as source")]
        private GameObject sourcePrefab;

        /// <summary>
        /// Cache size (maximum amount of the cached items).\r\n[0 - unlimited]
        /// </summary>
        [Tooltip("Cache size (maximum amount of the cached items).\r\n[0 - unlimited]")]
        public int Capacity;

        /// <summary>
        /// How much items must be cached at the start
        /// </summary>
        [Tooltip("How much items must be cached at the start")]
        public int PreloadCount;

        /// <summary>
        /// How to call events OnFastInstantiate and OnFastDestroy. Note, that if use choose notification via Interface, you must implement IFastPoolObject in any script on your sourcePrefab
        /// </summary>
        [Tooltip(
            "How to call events OnFastInstantiate and OnFastDestroy. Note, that if use choose notification via Interface, you must implement IFastPoolObject in any script on your sourcePrefab"
        )]
        public PoolItemNotificationType NotificationType = PoolItemNotificationType.Interface;

        /// <summary>
        /// Load source prefab in the memory while scene is loading. A bit slower scene loading, but faster instantiating of new objects in pool
        /// </summary>
        [Tooltip(
            "Load source prefab in the memory while scene is loading. A bit slower scene loading, but faster instantiating of new objects in pool"
        )]
        public bool WarmOnLoad = true;

        /// <summary>
        /// Parent cached objects to FastPoolManager game object.\r\n[WARNING] Turning this option on will make objects cached a bit slower.
        /// </summary>
        [Tooltip(
            "Parent cached objects to FastPoolManager game object.\r\n[WARNING] Turning this option on will make objects cached a bit slower."
        )]
        public bool ParentOnCache = false;


        //helper variable to display count of cached objects by property drawer
        [SerializeField]
        [HideInInspector]
        int cached_internal;

        Stack<GameObject> cache;
        Transform parentTransform;

        /// <summary>
        /// Creates a new instance of FastPool
        /// </summary>
        /// <param name="prefab">Prefab that would be used as source</param>
        /// <param name="rootTransform">Transform to parent cached objects on. Use null if you don't want to change parent</param>
        /// <param name="warmOnLoad">Load source prefab in the memory while scene is loading. A bit slower scene loading, but faster instantiating of new objects in pool</param>
        /// <param name="preloadCount">How much items must be cached at the start</param>
        /// <param name="capacity">Cache size (maximum amount of the cached items)  [0 - unlimited]</param>
        public FastPool(GameObject prefab, Transform parent, bool warmOnLoad, int preloadCount, int capacity)
        {
            sourcePrefab = prefab;
            PreloadCount = preloadCount;
            Capacity = capacity;
            WarmOnLoad = warmOnLoad;
            parentTransform = parent;
            if (parentTransform != null)
            {
                ParentOnCache = true;
            }
        }


        /// <summary>
        /// Initialize pool with current parameters
        /// </summary>
        /// <param name="rootTransform">Transform to parent cached object on. Use null if you don't want to change parent</param>
        /// <returns></returns>
        public bool Init()
        {
            if (sourcePrefab != null)
            {
                cached_internal = 0;
                cache = new Stack<GameObject>();
                ID = sourcePrefab.GetInstanceID();

                if (WarmOnLoad)
                {
                    string name = string.Concat(sourcePrefab.name, "_SceneSource");
                    sourcePrefab = MakeClone(Vector3.zero, Quaternion.identity, parentTransform);
                    sourcePrefab.name = name;
                    sourcePrefab.SetActive(false);
                }

                //Preload
                for (int i = cache.Count; i < PreloadCount; i++)
                {
                    FastCache(MakeClone(Vector3.zero, Quaternion.identity, parentTransform));
                }

                IsValid = true;
            }
            else
            {
                Debug.LogError("Creating pool with null prefab!");
                IsValid = false;
            }

            return IsValid;
        }

        public bool InitC<T>() where T : Component
        {
            if (sourcePrefab != null)
            {
                cached_internal = 0;
                cache = new Stack<GameObject>();
                ID = sourcePrefab.GetInstanceID();

                if (WarmOnLoad)
                {
                    string name = string.Concat(sourcePrefab.name, "_SceneSource");
                    sourcePrefab = MakeClone<T>(Vector3.zero, Quaternion.identity, parentTransform);
                    sourcePrefab.name = name;
                    sourcePrefab.SetActive(false);
                }

                //Preload
                for (int i = cache.Count; i < PreloadCount; i++)
                {
                    FastCache(MakeClone<T>(Vector3.zero, Quaternion.identity, parentTransform));
                }

                IsValid = true;
            }
            else
            {
                Debug.LogError("Creating pool with null prefab!");
                IsValid = false;
            }

            return IsValid;
        }


        /// <summary>
        /// Unload all cached objects from memory
        /// </summary>
        public void ClearCache()
        {
            while (cache.Count > 0)
            {
                GameObject.Destroy(cache.Pop());
            }
            sourcePrefab = null;
            cached_internal = 0;
        }


        #region Instantiate

        /// <summary>
        /// Quickly instantiate GameObject from pool and return provided component from an instantiated GameObject.
        /// </summary>
        /// <typeparam name="T">Component to find on instantiated GameObject</typeparam>
        /// <param name="parent">Make instantiated GameObject the child of provided transform. Null if no parent needed</param>
        /// <returns></returns>
        public T FastInstantiate<T>(bool active, Transform parent = null)
            where T : Component
        {
            return FastInstantiate<T>(active, Vector3.zero, Quaternion.identity, parent);
        }

        /// <summary>
        /// Quickly instantiate GameObject from pool and return provided component from an instantiated GameObject.
        /// </summary>
        /// <typeparam name="T">Component to find on instantiated GameObject</typeparam>
        /// <param name="position">Position for the instantiated GameObject</param>
        /// <param name="rotation">Rotation of instantiated GameObject</param>
        /// <param name="parent">Make instantiated GameObject the child of provided transform. Null if no parent needed</param>
        /// <returns></returns>
        public T FastInstantiate<T>(bool active, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : Component
        {
            GameObject clone = FastInstantiate(active, position, rotation, parent);

            if (clone == null)
            {
                Debug.LogError("FastInstantiate null");
            }

            T component = clone.GetComponent<T>();
            if (component == null)
            {
                component = clone.AddComponent<T>();
            }

            switch (NotificationType)
            {
                case PoolItemNotificationType.Interface:
                    {
                        IFastPoolItem poolItem = clone.GetComponent<IFastPoolItem>();
                        if (poolItem != null)
                        {
                            poolItem.OnFastInstantiate(ID);
                        }
                    }
                    break;
                case PoolItemNotificationType.SendMessage:
                    clone.SendMessage("OnFastInstantiate");
                    break;
                case PoolItemNotificationType.BroadcastMessage:
                    clone.BroadcastMessage("OnFastInstantiate");
                    break;
            }

            return component;
        }

        /// <summary>
        /// Quickly instantiate GameObject from pool.
        /// </summary>
        /// <param name="parent">Make instantiated GameObject the child of provided transform. Null if no parent needed</param>
        /// <returns></returns>
        public GameObject FastInstantiate(bool active, Transform parent = null)
        {
            return FastInstantiate(active, Vector3.zero, Quaternion.identity, parent);
        }


        /// <summary>
        /// Quickly instantiate GameObject from pool.
        /// </summary>
        /// <param name="position">Position for the instantiated GameObject</param>
        /// <param name="rotation">Rotation of instantiated GameObject</param>
        /// <param name="parent">Make instantiated GameObject the child of provided transform. Null if no parent needed</param>
        /// <returns></returns>
        public GameObject FastInstantiate(bool active, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject clone = null;

            //find not null item in the cache
            while (cache.Count > 0)
            {
                clone = cache.Pop();
                cached_internal--;

                if (clone != null)
                {
                    Transform tr = clone.transform;
                    tr.localPosition = position;
                    tr.localRotation = rotation;
                    if (parent != null)
                    {
                        tr.SetParent(parent, false);
                    }

                    //
                    clone.SetActive(active);

                    return clone;
                }
                else
                {
                    if (sourcePrefab != null)
                        Debug.LogWarning("The pool with the " + sourcePrefab.name +
                                         " prefab contains null entry. Don't destroy cached items manually!");
                    else
                        Debug.LogWarning(
                            "The pool with the prefab contains null entry. Don't destroy cached items manually!");
                }
            }

            if (clone == null)
            {
                /// 더이상 캐싱된것이없으면 Capcaity사이즈를 기존사이즈의 절반만큼 늘려 다시 FastInistantiate를 돌도록 하자.
                int addSize = 2;//(int)(PreloadCount / 2) + 1;
                                //if (Capacity == 0 || Capacity < PreloadCount + addSize)
                {
                    PreloadCount += addSize;
                    for (int i = 0; i < addSize; i++)
                    {
                        FastCache(MakeClone(position, rotation, parentTransform));
                    }
                    clone = FastInstantiate(active, position, rotation, parent);

                    //if (WarmOnLoad)
                    //{
                    //    clone.SetActive(active);
                    //}
                }
            }

            return clone;
        }

        #endregion


        #region Destroy

        /// <summary>
        /// Disable GameObject of provided Component and put it into the pool for later use. If the pool size reached capacity limit - GameObject will be simply Destroyed.
        /// </summary>
        /// <typeparam name="T">Component of the GameObject that you want to put into the pool</typeparam>
        /// <param name="sceneObject"></param>
        public void FastDestroy<T>(T sceneObject) where T : Component
        {
            if (sceneObject != null)
                FastDestroy(sceneObject.gameObject);
            else
                Debug.LogWarning("Attempt to destroy a null object");
        }

        /// <summary>
        /// Disable provided GameObject and put it into the pool for later use. If the pool size reached capacity limit - GameObject will be simply Destroyed.
        /// </summary>
        /// <param name="sceneObject">GameObject to put into the pool</param>
        public void FastDestroy(GameObject sceneObject)
        {
            if (sceneObject != null)
            {
#if !AUTO_SIZE_POOL
                if (cache.Count < Capacity || Capacity <= 0)
#endif
                {
                    cache.Push(sceneObject);
                    cached_internal++;
                    if (ParentOnCache)
                    {
                        sceneObject.transform.SetParent(parentTransform, false);
                    }

                    switch (NotificationType)
                    {
                        case PoolItemNotificationType.Interface:
                            {
                                IFastPoolItem poolItem = sceneObject.GetComponent<IFastPoolItem>();
                                if (poolItem != null)
                                {
                                    poolItem.OnFastDestroy();
                                }
                                Debug.Assert(poolItem.RefCount == 0);
                            }
                            break;
                        case PoolItemNotificationType.SendMessage:
                            sceneObject.SendMessage("OnFastDestroy");
                            break;
                    }

                    sceneObject.SetActive(false);
                }
#if !AUTO_SIZE_POOL
                else
                {
                    GameObject.Destroy(sceneObject);
                }
#endif
            }
            else
            {
                Debug.LogWarning("Attempt to destroy a null object");
            }
        }

        #endregion

        private void FastCache(GameObject sceneObject)
        {
            if (sceneObject != null)
            {
                cache.Push(sceneObject);
                cached_internal++;
                if (ParentOnCache)
                {
                    sceneObject.transform.SetParent(parentTransform, false);
                }

                sceneObject.SetActive(false);
            }
        }

        private GameObject MakeClone(Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject clone = (GameObject)GameObject.Instantiate(sourcePrefab, position, rotation);

            if (clone == null)
                return null;

            clone.transform.SetParent(parent, false);
            clone.name = string.Format("{0}_{1}", clone.name, cached_internal);

            return clone;
        }


        private GameObject MakeClone<T>(Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            GameObject clone = (GameObject)GameObject.Instantiate(sourcePrefab, position, rotation);

            if (clone == null)
                return null;

            clone.transform.SetParent(parent, false);
            clone.name = string.Format("{0}_{1}", clone.name, cached_internal);

            T component = clone.GetComponent<T>();
            if (component == null)
            {
                clone.AddComponent<T>();
            }

            return clone;
        }

    }


}