using System.Collections.Generic;
using Lib.Pattern;
using UnityEngine;

namespace Aniz.Factory
{
    public interface IPoolFactory
    {
        void DestroyPools();
    }

    public abstract class PoolFactory : IPoolFactory
    {
        private readonly Transform m_rootTransform;
        private Dictionary<int, GameObject> m_poolFactories = new Dictionary<int, GameObject>();

        public PoolFactory(Transform root)
        {
            m_rootTransform = root;
        }

        protected Transform CreatePoolFactory(GameObject prefab)
        {
            int instanceID = prefab.GetInstanceID();

            if (m_poolFactories.ContainsKey(instanceID))
            {
                return m_poolFactories[instanceID].transform;
            }
            GameObject poolGameObject = new GameObject(prefab.name);
            poolGameObject.transform.SetParent(m_rootTransform, true);

            m_poolFactories.Add(instanceID, poolGameObject);
            return poolGameObject.transform;
        }

        public virtual void DestroyPools()
        {
            foreach (KeyValuePair<int, GameObject> keyValuePair in m_poolFactories)
            {
                GameObjectFactory.Destroy(keyValuePair.Value.gameObject);
            }
            m_poolFactories.Clear();
        }

        public virtual void DestroyPoolFactory(int instanceID)
        {
            if (m_poolFactories.ContainsKey(instanceID))
            {
                GameObject gameObject = m_poolFactories[instanceID].gameObject;
                GameObjectFactory.Destroy(gameObject);
                gameObject = null;
            }
            m_poolFactories.Remove(instanceID);
        }
    }
}