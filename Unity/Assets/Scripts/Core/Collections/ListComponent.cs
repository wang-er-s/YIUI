using System;
using System.Collections.Generic;

namespace ET
{
    public class ListComponent<T>: List<T>, IDisposable, IPool
    {
        public ListComponent()
        {
        }
        
        public static ListComponent<T> Create()
        {
            return ObjectPool.Instance.Fetch<ListComponent<T>>();
        }
        
        public static ListComponent<T> Create(T val)
        {
            var list = Create();
            list.Add(val);
            return list;
        }

        public static ListComponent<T> Create(T val, T val2)
        {
            var list = Create();
            list.Add(val);
            list.Add(val2);
            return list;
        }

        public void Dispose()
        {
            this.Clear();
            if (this.Capacity > 64) // 超过64，让gc回收
            {
                return;
            }

            if (this.IsFromPool)
                ObjectPool.Instance.Recycle(this);
        }

        public bool IsFromPool { get; set; }
    }
}