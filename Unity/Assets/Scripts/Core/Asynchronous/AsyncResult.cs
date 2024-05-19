﻿/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ET
{
    public class AsyncResult : IAsyncResult, IPromise
    {

        private bool _done;
        private object _result;
        private Exception _exception;

        private bool _cancelled;
        protected bool Cancelable;
        protected bool CancellationRequested;

        protected readonly object Lock = new object();

        private Callbackable _callbackable;
        
        public string debugName;
        [StaticField]
        public static int useCount = 0;

        public static AsyncResult Create([CallerMemberName] string debugName = "", bool isFromPool = true,
            bool cancelable = true)
        {
             var result = isFromPool ? ObjectPool.Instance.Fetch<AsyncResult>() : new AsyncResult();
             result.OnCreate(debugName, cancelable);
             return result;
        }

        protected virtual void OnCreate(string debugName, bool cancelable)
        {
            this.debugName = debugName + useCount++;
            this.Cancelable = cancelable;
            this.HasBeenRecycled = false;
        }

        /// <summary>
        /// Exception
        /// </summary>
        public virtual Exception Exception => this._exception;

        /// <summary>
        /// Returns  "true" if this task finished.
        /// </summary>
        public virtual bool IsDone => this._done;

        /// <summary>
        /// The execution result
        /// </summary>
        public virtual object Result
        {
            get
            {
                if (!IsDone)
                {
                    throw new NotSupportedException(
                        $"name:{debugName} does not allow call GetResult directly when task not completed. Please use 'await'.");
                }

                object ret = _result;
                if (_exception != null)
                {
                    throw _exception;
                }

                return ret;
            }
        }

        public virtual bool IsCancellationRequested => this.CancellationRequested;

        /// <summary>
        /// Returns "true" if this task was cancelled before it completed normally.
        /// </summary>
        public virtual bool IsCancelled => this._cancelled;

        public virtual void SetException(string error)
        {
            if (this._done)
                return;

            var exception = new Exception(string.IsNullOrEmpty(error) ? "unknown error!" : error);
            SetException(exception);
        }

        public virtual void SetException(Exception exception)
        {
            lock (Lock)
            {
                if (this._done)
                    return;

                this._exception = exception;
                this._done = true;
                Monitor.PulseAll(Lock);
            }

            this.RaiseOnCallback();
        }

        public virtual void SetResult(object result = null)
        {
            lock (Lock)
            {
                if (this._done)
                    return;

                this._result = result;
                this._done = true;
                Monitor.PulseAll(Lock);
            }
            
            this.RaiseOnCallback();
        }

        public virtual void SetCancelled()
        {
            lock (Lock)
            {
                if (!this.Cancelable || this._done)
                    return;

                this._cancelled = true;
                this._exception = new OperationCanceledException();
                this._done = true;
                Monitor.PulseAll(Lock);
            }

            this.RaiseOnCallback();
        }

        /// <summary>
        /// Attempts to cancel execution of this task.  This attempt will 
        /// fail if the task has already completed, has already been cancelled,
        /// or could not be cancelled for some other reason.If successful,
        /// and this task has not started when "Cancel" is called,
        /// this task should never run. 
        /// </summary>
        /// <exception cref="NotSupportedException">If not supported, throw an exception.</exception>
        /// <returns></returns>
        public virtual bool Cancel()
        {
            if (!this.Cancelable)
                return false;

            if (this.IsDone)
                return false;

            this.CancellationRequested = true;
            this.SetCancelled();
            return true;
        }

        protected virtual void RaiseOnCallback()
        {
            _callbackable?.RaiseOnCallback();
        }

        public virtual ICallbackable Callbackable()
        {
            lock (Lock)
            {
                return this._callbackable ??= ET.Callbackable.Create(this);
            }
        }

        public void FreeFromPool()
        {
            ObjectPool.Instance.Recycle(this);
        }

        [StaticField]
        private static IAsyncResult voidResult;
        
        public static IAsyncResult Void()
        {
            if (voidResult == null)
            {
                var result = Create(isFromPool: false);
                result.SetResult();
                voidResult = result;
            }
            return voidResult;
        }

        public virtual void Clear()
        {
            _done = false;
            _result = null;
            _exception = null;
            _cancelled = false;
            Cancelable = false;
            CancellationRequested = false;
            ObjectPool.Instance.Recycle(_callbackable);
            _callbackable = null;
            this.IsFromPool = false;
        }

        public void Dispose()
        {
            if (this.IsFromPool)
            {
                this.Clear();
                ObjectPool.Instance.Recycle(this);
                this.HasBeenRecycled = true;
            }
        }

        public bool IsFromPool { get; set; }
        public bool HasBeenRecycled { get; set; }
    }

    public class AsyncResult<TResult> : AsyncResult, IAsyncResult<TResult>, IPromise<TResult>
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(AsyncResult<TResult>));

        private Callbackable<TResult> _callbackable;

        public new static AsyncResult<TResult> Create([CallerMemberName] string debugName = "", bool isFromPool = true,
            bool cancelable = true)
        {
            var result = isFromPool ? ObjectPool.Instance.Fetch<AsyncResult<TResult>>() : new AsyncResult<TResult>();
            result.OnCreate(debugName, cancelable);
            return result;
        } 

        /// <summary>
        /// The execution result
        /// </summary>
        public new virtual TResult Result
        {
            get
            {
                if (!IsDone)
                {
                    throw new NotSupportedException(
                        "does not allow call GetResult directly when task not completed. Please use 'await'.");
                }

                TResult result = base.Result != null ? (TResult)base.Result : default;
                if (Exception != null)
                {
                    throw Exception;
                }

                return result;
            }
        }

        public virtual void SetResult(TResult result)
        {
            base.SetResult(result);
        }

        protected override void RaiseOnCallback()
        {
            base.RaiseOnCallback();
            _callbackable?.RaiseOnCallback();
        }

        public new virtual ICallbackable<TResult> Callbackable()
        {
            lock (Lock)
            {
                return this._callbackable ??= Callbackable<TResult>.Create(this);
            }
        }
        
        [StaticField]
        private static IAsyncResult<TResult> voidResult;
        
        /// <summary>
        /// 返回一个完成的IProgressResult<float>
        /// </summary>
        /// <returns></returns>
        public new static IAsyncResult<TResult> Void()
        {
            if (voidResult == null)
            {
                var result = Create(isFromPool: false);
                result.SetResult();
                voidResult = result;
            }
            return voidResult;
        }

        public override void Clear()
        {
            base.Clear();
            ObjectPool.Instance.Recycle(_callbackable);
            _callbackable = null;
        }
    }
}
