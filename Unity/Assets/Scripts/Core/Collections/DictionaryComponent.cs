using System;
using System.Collections.Generic;

namespace ET
{
    public class DictionaryComponent<TK,TV> : Dictionary<TK,TV>, IDisposable, IPool
    {
        public DictionaryComponent()
        {
        }
        
        public static DictionaryComponent<TK,TV> Create()
        {
            return ObjectPool.Instance.Fetch<DictionaryComponent<TK,TV>>();
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