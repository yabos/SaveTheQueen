using Aniz.Graph;
using UnityEngine;

namespace Lib.Pool
{
    public abstract class MonoFastPool : GraphMono, IFastPoolItem
    {

        #region "IFastPoolItem"

        protected int m_parentInstanceID;

        public int ParentInstanceID
        {
            get { return m_parentInstanceID; }
        }

        public GameObject PoolGameObject
        {
            get { return this.gameObject; }
        }

        public virtual void OnFastInstantiate(int instanceID)
        {
            Lock();
            m_parentInstanceID = instanceID;
        }

        public virtual void OnFastDestroy()
        {
            Unlock();
        }

        #endregion // "IFastPoolItem"

        #region "IRef"

        private int m_lock;
        private int m_refFrameCount;
        private int m_refReleaseTime = 0;

        public int RefCount
        {
            get { return m_lock; }
        }

        public void Lock()
        {
            ++m_lock;
        }

        public int Unlock()
        {
            if (m_lock > 0)
            {
                --m_lock;
            }
            else
            {
                m_refFrameCount = Time.frameCount;
            }
            return m_lock;
        }

        public bool CanFree(int frameCount)
        {
            if (m_lock > 0)
            {
                return false;
            }

            if (m_refFrameCount == 0)
            {
                return false;
            }

            if (frameCount - m_refFrameCount > m_refReleaseTime)
            {
                return true;
            }
            return true;
        }

        #endregion // "IRef"
    }
}