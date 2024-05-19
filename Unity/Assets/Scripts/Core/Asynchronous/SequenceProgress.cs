using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ET
{
    public class SequenceProgress : ProgressResult<float>
    {
        private ListComponent<Func<IProgressResult<float>>> progressQueue;
        private int index = 0;
        private IProgressResult<float> currentProgress;
        private TimerComponent timerComponent;

        public override bool IsDone => currentProgress == null || base.IsDone;

        public static SequenceProgress Create(TimerComponent timerComponent, [CallerMemberName] string debugName = "", bool cancelable = true,
            bool isFromPool = true)
        {
            var result = isFromPool ? ObjectPool.Instance.Fetch<SequenceProgress>() : new SequenceProgress();
            result.timerComponent = timerComponent;
            result.OnCreate(debugName, cancelable);
            return result;
        }

        public void AddAsyncResult(Func<IProgressResult<float>> progressResult)
        {
            if(progressResult == null) return;
            progressQueue.Add(progressResult);
            if (currentProgress == null)
            {
                SetNextProgress();
            }
        }

        private void SetNextProgress()
        {
            if (index < progressQueue.Count)
            {
                currentProgress = progressQueue[index].Invoke();
                index++;
                SetSubProgressCb(currentProgress);
            }
            else
            {
                currentProgress = null;
                RaiseFinish().Coroutine();
            }
        }

        public void AddAsyncResult(IEnumerable<Func<IProgressResult<float>>> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IProgressResult<float> progressResult)
        {
            progressResult.Callbackable().OnProgressCallback(_ => this.RaiseOnProgressCallback(0).Coroutine());
            progressResult.Callbackable().OnCallback(_ =>
            {
                    SetNextProgress();
            });
        }

        protected override async ETTask RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            await base.RaiseOnProgressCallback(Progress);
        }

        private async ETTask RaiseFinish()
        {
            await this.timerComponent.WaitFrameAsync();
            SetResult();
        }

        private void UpdateProgress()
        {
            float totalProgress = index + currentProgress.Progress;
            // 1 是当前正在执行的progress
            Progress = totalProgress / progressQueue.Count;
        }

        public override void Clear()
        {
            base.Clear();
            this.timerComponent = null;
            progressQueue.Dispose();
            index = 0;
        }
    }
}