using System;
using System.Collections.Generic;
using ET.Client;
using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class PrefabPool : Entity, IAwake<ResourcesLoaderComponent, string>, IAwake<ResourcesLoaderComponent>, IDestroy, IUpdate
    {
        public Dictionary<int, PrefabPoolSystem.OnePrefab> pathHash2Prefab;
        public Dictionary<int, int> goInstanceId2PathHash;
        public EntityRef<ResourcesLoaderComponent> res;
        public Transform root;
    }

    [EntitySystemOf(typeof(PrefabPool))]
    [FriendOf(typeof(ET.PrefabPool))]
    public static partial class PrefabPoolSystem
    {
        [EntitySystem]
        private static void Awake(this ET.PrefabPool self, ET.Client.ResourcesLoaderComponent res, string name)
        {
            self.root.name = name;
            self.Awake(res);
        }

        [EntitySystem]
        private static void Awake(this ET.PrefabPool self, ET.Client.ResourcesLoaderComponent res)
        {
            self.res = res;
            self.pathHash2Prefab = new Dictionary<int, OnePrefab>();
            self.goInstanceId2PathHash = new Dictionary<int, int>();
            self.root = new GameObject($"PrefabPool_{self.Scene().Name}").transform;
            UnityEngine.Object.DontDestroyOnLoad(self.root.gameObject);
        }

        public static IProgressResult<float, GameObject> AllocateProgress(this PrefabPool self, string path)
        {
            int pathHash = path.GetHashCode();
            var prefab = self.GetPrefab(pathHash);

            ProgressResult<float, GameObject> result = ProgressResult<float, GameObject>.Create();
            if (prefab.Caches.Count > 0)
            {
                var go = prefab.Caches[^1];
                prefab.Caches.RemoveAt(prefab.Caches.Count - 1);
                self.goInstanceId2PathHash.Add(go.Go.GetInstanceID(), pathHash);
                result.SetResult(go.Go);
                return result;
            }

            var result2 = self.res.UnWrap.LoadAssetProgress<GameObject>(path);
            result2.Callbackable().OnProgressCallback(r =>
            {
                result.UpdateProgress(r);
            });
            result2.Callbackable().OnCallback((r) =>
            {
                if (r.Exception != null)
                {
                    result.SetException(r.Exception);
                }
                else
                {
                    var go = UnityEngine.Object.Instantiate(r.Result);
                    self.goInstanceId2PathHash.Add(go.GetInstanceID(), pathHash);
                    if (result.HasBeenRecycled)
                    {
                        self.Free(go);
                    }
                    else
                    {
                        result.SetResult(go);
                    }
                }
                self.DelayFree(r).Coroutine();
            });

            return result;
        }
        
        private static async ETTask DelayFree(this PrefabPool self, IAsyncResult asyncResult)
        {
            await self.Root().GetComponent<TimerComponent>().WaitFrameAsync();
            asyncResult?.Dispose();
        }

        public static async ETTask<GameObject> Allocate(this PrefabPool self, string path)
        {
            int pathHash = path.GetHashCode();
            var prefab = self.GetPrefab(pathHash);

            if (prefab.Caches.Count > 0)
            {
                var go = prefab.Caches[^1];
                prefab.Caches.RemoveAt(prefab.Caches.Count - 1);
                self.goInstanceId2PathHash.Add(go.Go.GetInstanceID(), pathHash);
                return go.Go;
            }

            var go2 = await self.Instantiate(path);
            self.goInstanceId2PathHash.Add(go2.GetInstanceID(), pathHash);
            return go2;
        }

        public static GameObject AllocateSync(this PrefabPool self, string path)
        {
            int pathHash = path.GetHashCode();
            var prefab = self.GetPrefab(pathHash);

            if (prefab.Caches.Count > 0)
            {
                var go = prefab.Caches[^1];
                prefab.Caches.RemoveAt(prefab.Caches.Count - 1);
                self.goInstanceId2PathHash.Add(go.Go.GetInstanceID(), pathHash);
                return go.Go;
            }
            else
            {
                var go = self.InstantiateSync(path);
                self.goInstanceId2PathHash.Add(go.GetInstanceID(), pathHash);
                return go;
            }
        }

        public static void Free(this PrefabPool self, GameObject gameObject)
        {
            if (self.IsDisposed)
            {
                UnityEngine.Object.Destroy(gameObject);
            }

            if (gameObject == null) return;
            var insId = gameObject.GetInstanceID();
            if (!self.goInstanceId2PathHash.Remove(insId, out var pathHash))
            {
                Log.Warning($"对象不是通过对象池取出来的==={gameObject.name}");
                UnityEngine.Object.Destroy(gameObject);
                return;
            }

            if (self.pathHash2Prefab.TryGetValue(pathHash, out var prefab))
            {
                self.ResetGameObject(gameObject);
                prefab.AddCache(gameObject);
            }
            else
            {
                Log.Error("???取出来的怎么会么有呢");
            }
        }

        private static GameObject InstantiateSync(this PrefabPool self, string path)
        {
            // if (string.IsNullOrEmpty(path))
            // {
            //     return new GameObject(nameof(GameObject));
            // }
            //
            return UnityEngine.Object.Instantiate(self.res.UnWrap.LoadAssetSync<GameObject>(path));
        }
        
        private static async ETTask<GameObject> Instantiate(this PrefabPool self, string path)
        {
            // if (string.IsNullOrEmpty(path))
            // {
            //     return new GameObject(nameof(GameObject));
            // }
            //
            return UnityEngine.Object.Instantiate(await self.res.UnWrap.LoadAsset<GameObject>(path));
        }

        public static void SetCacheData(this PrefabPool self, string path, float delayDestroyTime, int maxCount)
        {
            int pathHash = path.GetHashCode();
            var prefab = self.GetPrefab(pathHash);
            prefab.DelayDestroyTime = delayDestroyTime;
            prefab.MaxCount = maxCount;
        }

        private static OnePrefab GetPrefab(this PrefabPool self, int pathHash)
        {
            if (!self.pathHash2Prefab.TryGetValue(pathHash, out var prefab))
            {
                prefab = ObjectPool.Instance.Fetch<OnePrefab>();
                prefab.PathHash = pathHash;
                prefab.DelayDestroyTime = 100000;
                prefab.MaxCount = 30;
                prefab.Caches = ListComponent<DelayDestroyGo>.Create();
                self.pathHash2Prefab.Add(pathHash, prefab);
            }

            return prefab;
        }

        private static void ResetGameObject(this PrefabPool self, GameObject gameObject)
        {
            gameObject.transform.SetParent(self.root);
            gameObject.SetActive(false);
        }

        [EnableClass]
        public class OnePrefab : IDisposable
        {
            public int PathHash;
            public float DelayDestroyTime;
            public int MaxCount;
            public ListComponent<DelayDestroyGo> Caches;

            public void AddCache(GameObject gameObject)
            {
                if (Caches.Count >= MaxCount)
                {
                    UnityEngine.Object.Destroy(gameObject);
                }
                else
                {
                    Caches.Add(new DelayDestroyGo(Time.time + DelayDestroyTime, gameObject));
                }
            }

            public void Dispose()
            {
                this.Caches?.Dispose();
                ObjectPool.Instance.Recycle(this);
            }
        }

        public struct DelayDestroyGo
        {
            public float DestroyTime;
            public GameObject Go;

            public DelayDestroyGo(float destroyTime, GameObject go)
            {
                DestroyTime = destroyTime;
                Go = go;
            }
        }

        [EntitySystem]
        private static void Destroy(this ET.PrefabPool self)
        {
            UnityEngine.Object.Destroy(self.root.gameObject);
            foreach (OnePrefab onePrefab in self.pathHash2Prefab.Values)
            {
                onePrefab.Dispose();
            }

            self.pathHash2Prefab.Clear();
            self.goInstanceId2PathHash.Clear();
        }
        
        [EntitySystem]
        private static void Update(this ET.PrefabPool self)
        {
            float curTime = Time.time;
            foreach (OnePrefab onePrefab in self.pathHash2Prefab.Values)
            {
                if (onePrefab.Caches.Count <= 0)
                {
                    continue;
                }

                for (int i = 0; i < onePrefab.Caches.Count; i++)
                {
                    DelayDestroyGo cache = onePrefab.Caches[i];
                    if (curTime > cache.DestroyTime)
                    {
                        UnityEngine.Object.Destroy(cache.Go);
                        onePrefab.Caches.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

}