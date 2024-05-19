using System;
using System.Collections.Generic;

namespace ET
{
    public class HashSetComponent<T>: HashSet<T>, IDisposable, IPool
    {
        public HashSetComponent()
        {
        }
        
        public static HashSetComponent<T> Create()
        {
            return ObjectPool.Instance.Fetch(typeof (HashSetComponent<T>)) as HashSetComponent<T>;
        }

        public void Dispose()
        {
            this.Clear();
            if (this.Count > 64) // 超过64，让gc回收
            {
                return;
            }
            if(this.IsFromPool)
                ObjectPool.Instance.Recycle(this);
        }

        public bool IsFromPool { get; set; }
    }
}