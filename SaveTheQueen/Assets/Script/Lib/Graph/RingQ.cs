using System.Collections.Generic;
using System.Collections;
using System;

namespace Aniz.Graph
{
    public interface IRingQ<TItem>
    {
        int Count { get; }
        int Capacity { get; set; }
        TItem Enqueue(TItem item);
        TItem Dequeue();
        TItem Peek(); // Returns the object at the beginning of the IRingQ without removing it.
        TItem PeekLast();
        void Clear();
        TItem this[int index] { get; set; }
        int IndexOf(TItem item);
        void Insert(int index, TItem item);
        void RemoveAt(int index);
        void CopyTo(List<TItem> list);
    }

    [System.Serializable]
    public class RingQ<TItem> : IRingQ<TItem>, IEnumerable<TItem>
    {
        private TItem[] buffer;
        private int head; // index of last item
        private int tail; // index of first item

        public RingQ(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "must be positive");

            this.buffer = new TItem[capacity];
            this.head = capacity - 1;
        }

        public int Count { get; private set; }

        public int Capacity
        {
            get { return this.buffer.Length; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "must be positive");

                if (value == this.buffer.Length)
                    return;

                var newBuffer = new TItem[value];
                var count = 0;
                while (this.Count > 0 && count < value)
                    buffer[count++] = Dequeue();

                this.buffer = newBuffer;
                this.Count = count;
                this.head = count - 1;
                this.tail = 0;
            }
        }

        public TItem Enqueue(TItem item)
        {
            this.head = (this.head + 1) % this.Capacity;
            var overwritten = this.buffer[this.head];
            this.buffer[this.head] = item;

            if (this.Count == this.Capacity)
                this.tail = (this.tail + 1) % this.Capacity;
            else
                ++this.Count;

            return overwritten;
        }

        public TItem Dequeue()
        {
            if (this.Count == 0)
                throw new InvalidOperationException("queue exhausted");

            var dequeued = this.buffer[this.tail];
            this.buffer[this.tail] = default(TItem);
            this.tail = (this.tail + 1) % this.Capacity;
            --this.Count;

            return dequeued;
        }

        public TItem Peek()
        {
            return this.buffer[this.tail];
        }

        public TItem PeekLast()
        {
            return this.buffer[this.head];
        }

        public void Clear()
        {
            this.head = this.Capacity - 1;
            this.tail = 0;
            this.Count = 0;
        }

        public TItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");

                return this.buffer[(this.tail + index) % this.Capacity];
            }
            set
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");

                this.buffer[(this.tail + index) % this.Capacity] = value;
            }
        }

        public int IndexOf(TItem item)
        {
            for (var i = 0; i < this.Count; ++i)
                if (Equals(item, this[i]))
                    return i;

            return -1;
        }

        public void Insert(int index, TItem item)
        {
            if (index < 0 || index > this.Count)
                throw new ArgumentOutOfRangeException("index");

            if (this.Count == index)
                Enqueue(item);
            else
            {
                var last = this[this.Count - 1];
                for (var i = index; i < this.Count - 2; ++i)
                    this[i + 1] = this[i];
                this[index] = item;
                Enqueue(last);
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
                throw new ArgumentOutOfRangeException("index");

            for (var i = index; i > 0; --i)
                this[i] = this[i - 1];

            Dequeue();
        }

        public void CopyTo(List<TItem> list)
        {
            if (this.Count == 0)
                return;

            list.Clear();
            list.Capacity = this.Count; // @@@
            for (int itr = 0; itr < this.Count; ++itr)
            {
                list.Add(this.buffer[(this.tail + itr) % this.Capacity]);
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            if (this.Count == 0 || this.Capacity == 0)
                yield break;

            for (var i = 0; i < this.Count; ++i)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
