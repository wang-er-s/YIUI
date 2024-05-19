using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ET
{
    public interface IMulProgress
    {
        float Current { get; }
        float Total { get; }
    }
    
    public class MulAsyncResult : ProgressResult<float>
    {
        private ListComponent<bool> progressFinishState;
        private ListComponent<IAsyncResult> _allProgress;
        private TimerComponent timerComponent;
        public override bool IsDone
        {
            get
            {
                if (isAllDone)
                {
                    RaiseOnProgressCallback(0).Coroutine();
                    return true;
                }
                return _allProgress.Count <= 0 || base.IsDone;
            }
        }

        private bool isAllDone = false;

        public static MulAsyncResult Create(TimerComponent timerComponent, [CallerMemberName]string debugName = "", bool cancelable = true, bool isFromPool = true)
        {
            var result = isFromPool ? ObjectPool.Instance.Fetch<MulAsyncResult>() : new MulAsyncResult();
            result.timerComponent = timerComponent;
            result._allProgress = ListComponent<IAsyncResult>.Create();
            result.progressFinishState = ListComponent<bool>.Create();
            result.OnCreate(debugName, cancelable);
            return result;
        }

        public void AddAsyncResult(IAsyncResult progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            progressFinishState.Add(progressResult.IsDone);
            SetSubProgressCb(progressResult);
            CheckAllFinish();
        }

        public void AddAsyncResult(IEnumerable<IAsyncResult> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IAsyncResult progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnCallback(f => RaiseOnProgressCallback(0).Coroutine());
        }

        private bool CheckAllFinish()
        {
            for (int i = 0; i < _allProgress.Count; i++)
            {
                var progressResult = _allProgress[i];
                if (!progressFinishState[i] &&  progressResult.IsDone)
                {
                    progressFinishState[i] = true;
                }
            }
            
            for (var index = 0; index < _allProgress.Count; index++)
            {
                var progressResult = _allProgress[index];
                if (!progressResult.IsDone)
                {
                    isAllDone = false;
                    return false;
                }
            }
            isAllDone = true;
            return true;
        }

        protected override async ETTask RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            //延迟一帧 否则会比子任务提前完成
            if (CheckAllFinish())
            {
                await this.timerComponent.WaitFrameAsync();
                SetResult();
            }
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            for (var index = 0; index < _allProgress.Count; index++)
            {
                var progressResult = _allProgress[index];
                if (progressResult.IsDone)
                {
                    totalProgress += 1;
                }
            }

            Progress = totalProgress / _allProgress.Count;
        }

        public override void Clear()
        {
            base.Clear();
            foreach (var asyncResult in _allProgress)
            {
                ObjectPool.Instance.Recycle(asyncResult);
            }

            timerComponent = null;
            _allProgress.Dispose();
            _allProgress = null;
            progressFinishState.Dispose();
            progressFinishState = null;
            isAllDone = false;
        }
    }
    
    public class MulProgressResult : ProgressResult<float>
    {
        private ListComponent<bool> progressFinishState;
        private ListComponent<IProgressResult<float>> _allProgress;
        private TimerComponent timerComponent;
        public override bool IsDone
        {
            get
            {
                if (isAllDone)
                {
                    RaiseFinish().Coroutine();
                    return true;
                }
                return _allProgress.Count <= 0 || base.IsDone;
            }
        }

        private bool isAllDone = false;

        public static MulProgressResult Create(TimerComponent timerComponent, [CallerMemberName]string debugName = "",bool cancelable = true,bool isFromPool = true)
        {
            var result = isFromPool ? ObjectPool.Instance.Fetch<MulProgressResult>() : new MulProgressResult();
            result.timerComponent = timerComponent;
            result._allProgress = ListComponent<IProgressResult<float>>.Create();
            result.progressFinishState = ListComponent<bool>.Create();
            result.OnCreate(debugName, cancelable);
            return result;
        }

        public void AddAsyncResult(IProgressResult<float> progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            progressFinishState.Add(progressResult.IsDone);
            SetSubProgressCb(progressResult);
            CheckAllFinish();
        }

        public void AddAsyncResult(IEnumerable<IProgressResult<float>> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IProgressResult<float> progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnProgressCallback(_ => this.RaiseOnProgressCallback(0).Coroutine());
            progressResult.Callbackable().OnCallback(_ =>
            {
                if (CheckAllFinish())
                {
                    RaiseFinish().Coroutine();
                }
            });
        }

        protected override async ETTask RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            await base.RaiseOnProgressCallback(Progress);
        }
        
        private bool CheckAllFinish()
        {
            for (int i = 0; i < _allProgress.Count; i++)
            {
                var progressResult = _allProgress[i];
                if (!progressFinishState[i] && progressResult.IsDone)
                {
                    progressFinishState[i] = true;
                }
            }

            for (int i = 0; i < progressFinishState.Count; i++)
            {
                if (!progressFinishState[i])
                {
                    isAllDone = false;
                    return false;
                }
            }

            isAllDone = true;
            return true;
        }

        private async ETTask RaiseFinish()
        {
            StringBuilder sb = null;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.Exception == null) continue;
                if (sb == null) sb = new StringBuilder();
                sb.AppendLine(progressResult.Exception.ToString());
            }

            //延迟一帧 否则会比子任务提前完成
            await this.timerComponent.WaitFrameAsync();
            if (sb != null)
            {
                SetException(sb.ToString());
            }
            else
            {
                SetResult();
            }
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.IsDone)
                {
                    totalProgress += 1;
                }
                else
                {
                    totalProgress += progressResult.Progress;
                }
            }
            Progress = totalProgress / _allProgress.Count;
        }

        public override void Clear()
        {
            base.Clear();
            foreach (var asyncResult in _allProgress)
            {
                ObjectPool.Instance.Recycle(asyncResult);
            }

            timerComponent = null;
            progressFinishState.Dispose();
            progressFinishState = null;
            _allProgress.Dispose();
            _allProgress = null;
            isAllDone = false;
        }
    }
}