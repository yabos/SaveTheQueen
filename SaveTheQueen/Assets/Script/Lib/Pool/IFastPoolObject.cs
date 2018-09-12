using UnityEngine;

namespace Lib.Pool
{
    public interface IRef
    {
        int RefCount { get; }
        void Lock();
        int Unlock();
        bool CanFree(int frameCount);
    }

    public interface IFastPoolItem : IRef
    {
        int ParentInstanceID { get; }
        GameObject PoolGameObject { get; }
        void OnFastInstantiate(int instanceID);
        void OnFastDestroy();
    }
}