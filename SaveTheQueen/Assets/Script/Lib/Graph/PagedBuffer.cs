using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aniz.Graph
{
    public interface IPagedBuffer<TItem>
    {
        int Capacity { get; }
        int Count { get; }
        void Add(TItem item);
        void Clear();
        void Shrink();
        TItem this[int index] { get; set; }
        int IndexOf(TItem item);
    }

    public class PagedBuffer<TItem> : IPagedBuffer<TItem>, IEnumerable<TItem>
    {
        List<List<TItem>> pages = new List<List<TItem>>();

        int capacityPerPage;

        void AddPage()
        {
            Debug.Assert(this.Count == this.capacityPerPage * this.pages.Count);
            this.pages.Add(new List<TItem>(this.capacityPerPage));
            CondDebug.LogVerbose("PagedBuffer.AddPage() called. Total page count : " + this.pages.Count);
        }

        public int Capacity { get { return this.capacityPerPage * this.pages.Count; } }

        public int Count { get; private set; } // 모든 페이지의 원소 개수 합

        public PagedBuffer(int capacityPerPage)
        {
            this.capacityPerPage = capacityPerPage;
            AddPage();
        }

        public void Add(TItem item)
        {
            if (this.Count == this.capacityPerPage * this.pages.Count)
                AddPage();

            this.pages[this.Count / this.capacityPerPage].Add(item);
            ++this.Count;
        }

        public void Clear()
        {
            for (int idx = 0; idx < this.pages.Count; ++idx)
                this.pages[idx].Clear();

            this.Count = 0;
        }

        public void Shrink()
        {
            for (int idx = 1; idx < this.pages.Count; ++idx)
                this.pages.RemoveAt(idx);

            this.Count = 0;
        }

        public TItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");

                return this.pages[index / this.capacityPerPage][index % this.capacityPerPage];
            }
            set
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");

                this.pages[index / this.capacityPerPage][index % this.capacityPerPage] = value;
            }
        }

        public int IndexOf(TItem item)
        {
            for (var idx = 0; idx < this.Count; ++idx)
                if (Equals(item, this[idx]))
                    return idx;

            return -1;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            if (this.Count == 0)
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
