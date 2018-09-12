using UnityEngine;

namespace Aniz.Graph
{
    //************************************************************************
    // BhvMono
    //------------------------------------------------------------------------
    public abstract class GraphMono : MonoBehaviour
    {
        void Awake() { BhvOnAwake(); }

        void Start() { BhvOnStart(); }

        void OnDestroy() { BhvOnDestroy(); }

        protected virtual void BhvOnAwake() { }

        protected virtual void BhvOnStart() { }

        protected virtual void BhvOnDestroy() { }
    }
}
