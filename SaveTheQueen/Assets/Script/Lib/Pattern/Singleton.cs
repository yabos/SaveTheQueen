using UnityEngine;
using System.Collections;

namespace Lib.Pattern
{

    public interface SingletonDispose
    {
        void Init();
        void Release();
        void OnUpdate(float deltaTime);
    }

    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        static Singleton()
        {
            if (Singleton<T>.instance == null)
            {
                instance = new T();
            }
        }
        public static T Instance
        {
            get { return instance; }
        }
        protected Singleton() { }
    }


}