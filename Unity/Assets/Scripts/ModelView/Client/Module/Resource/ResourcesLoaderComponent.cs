using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET.Client
{
    [EntitySystemOf(typeof(ResourcesLoaderComponent))]
    [FriendOf(typeof(ResourcesLoaderComponent))]
    public static partial class ResourcesLoaderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ResourcesLoaderComponent self)
        {
            self.package = YooAssets.GetPackage("DefaultPackage");
        }

        [EntitySystem]
        private static void Awake(this ResourcesLoaderComponent self, string packageName)
        {
            self.package = YooAssets.GetPackage(packageName);
        }

        [EntitySystem]
        private static void Destroy(this ResourcesLoaderComponent self)
        {
            foreach (var kv in self.handlers)
            {
                switch (kv.Value)
                {
                    case AssetHandle handle:
                        handle.Release();
                        break;
                    case AllAssetsHandle handle:
                        handle.Release();
                        break;
                    case SubAssetsHandle handle:
                        handle.Release();
                        break;
                    case RawFileHandle handle:
                        handle.Release();
                        break;
                    case SceneHandle handle:
                        if (!handle.IsMainScene())
                        {
                            handle.UnloadAsync();
                        }
                        break;
                }
            }
        }

        public static async ETTask<T> LoadAsset<T>(this ResourcesLoaderComponent self, string location) where T : UnityEngine.Object
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = self.package.LoadAssetAsync<T>(location);
                
                await handler.Task;

                self.handlers.Add(location, handler);
            }

            return (T)((AssetHandle)handler).AssetObject;
        }

        public static IProgressResult<float,T> LoadAssetProgress<T>(this ResourcesLoaderComponent self, string location) where T :UnityEngine.Object
        {
            if (self.loadingProgress.TryGetValue(location, out var result))
            {
                return (IProgressResult<float, T>)result;
            }

            ProgressResult<float,T> load = ProgressResult<float, T>.Create();
            if (self.handlers.TryGetValue(location, out var handler))
            {
                load.SetResult((T)((AssetHandle)handler).AssetObject);
            }
            else
            {
                self.loadAssetAsyncProgress(load, location).Coroutine();
            }

            return load;
        }

        private static async ETTask loadAssetAsyncProgress<T>(this ResourcesLoaderComponent self, ProgressResult<float, T> load, string location)
                where T : UnityEngine.Object
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>()
                    .Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            if (!self.handlers.TryGetValue(location, out var handler))
            {
                handler = self.package.LoadAssetAsync<T>(location);
                self.loadingProgress.Add(location, load);
                self.handlers.Add(location, handler);
                load.Callbackable().OnCallback(r => { self.loadingProgress.Remove(location); });
            }
            else
            {
                load.SetResult((T)((AssetHandle)handler).AssetObject);
                return;
            }


            var timer = self.Root().GetComponent<TimerComponent>(); 
            while (!handler.IsDone)
            {
                load.UpdateProgress(handler.Progress);
                await timer.WaitFrameAsync();
            }

            load.SetResult((T)((AssetHandle)handler).AssetObject);
        }

        public static async ETTask<Dictionary<string, T>> LoadAllAssets<T>(this ResourcesLoaderComponent self, string location) where T : UnityEngine.Object
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = self.package.LoadAllAssetsAsync<T>(location);
                await handler.Task;
                self.handlers.Add(location, handler);
            }

            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (UnityEngine.Object assetObj in ((AllAssetsHandle)handler).AllAssetObjects)
            {
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }

            return dictionary;
        }
        
        public static async ETTask LoadScene(this ResourcesLoaderComponent self, string location, LoadSceneMode loadSceneMode)
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            HandleBase handler;
            if (self.handlers.TryGetValue(location, out handler))
            {
                return;
            }

            handler = self.package.LoadSceneAsync(location);

            await handler.Task;
            self.handlers.Add(location, handler);
        }
        
        public static T LoadAssetSync<T>(this ResourcesLoaderComponent self, string location) where T : UnityEngine.Object
        {
            if (self.Root().GetComponent<CoroutineLockComponent>().Contains(CoroutineLockType.ResourcesLoader, location.GetHashCode()))
            {
                Log.Error($"不允许同时同步和异步加载 {location}");
                return null;
            }

            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = self.package.LoadAssetSync<T>(location);

                self.handlers.Add(location, handler);
            }

            return (T)((AssetHandle)handler).AssetObject;
        }
    }

    /// <summary>
    /// 用来管理资源，生命周期跟随Parent，比如CurrentScene用到的资源应该用CurrentScene的ResourcesLoaderComponent来加载
    /// 这样CurrentScene释放后，它用到的所有资源都释放了
    /// </summary>
    [ComponentOf]
    public class ResourcesLoaderComponent : Entity, IAwake, IAwake<string>, IDestroy
    {
        public ResourcePackage package;
        public Dictionary<string, HandleBase> handlers = new();
        public Dictionary<string, IAsyncResult> loadingProgress = new();
    }
}