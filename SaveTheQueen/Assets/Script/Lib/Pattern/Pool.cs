using System.Collections.Generic;

namespace Lib.Pattern
{
    public class Pool<T> where T : class, new()
    {
        private Stack<T> _items = new Stack<T>();
        private object _sync = new object();

        public void Release()
        {
            _items.Clear();
        }

        public T Get()
        {
            lock (_sync)
            {
                if (_items.Count == 0)
                {
                    return new T();
                }
                else
                {
                    return _items.Pop();
                }
            }
        }

        public T GetNull()
        {
            lock (_sync)
            {
                if (_items.Count == 0)
                {
                    return null;
                }
                else
                {
                    return _items.Pop();
                }
            }
        }

        public void Free(T item)
        {
            lock (_sync)
            {
                _items.Push(item);
            }
        }
    }
}