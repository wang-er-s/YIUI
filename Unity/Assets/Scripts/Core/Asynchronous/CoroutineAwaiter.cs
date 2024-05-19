/*
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
using System.Runtime.ExceptionServices;

namespace ET
{
    
    public interface IAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }
    
    public interface IAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
    {
        bool IsCompleted { get; }

        T GetResult();
    }
    
    public class CoroutineAwaiter : IAwaiter
    {
        protected readonly object Lock = new object();
        protected bool Done = false;
        protected Exception Exception;
        protected Action Continuation;

        public bool IsCompleted => this.Done;

        public void GetResult()
        {
            lock (Lock)
            {
                if (!Done)
                    throw new Exception("The task is not finished yet");
            }

            if (Exception != null)
                ExceptionDispatchInfo.Capture(Exception).Throw();
        }

        public void SetResult(Exception exception)
        {
            lock (Lock)
            {
                if (Done)
                    return;

                this.Exception = exception;
                this.Done = true;
                try
                {
                    Continuation?.Invoke();
                }
                catch (Exception)
                {
                }
                finally
                {
                    this.Continuation = null;
                }
            }
        }

        public void OnCompleted(Action continuation)
        {
            ((ICriticalNotifyCompletion) this).UnsafeOnCompleted(continuation);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            lock (Lock)
            {
                if (this.Done)
                {
                    continuation();
                }
                else
                {
                    this.Continuation += continuation;
                }
            }
        }
    }

    public class CoroutineAwaiter<T> : CoroutineAwaiter, IAwaiter<T>
    {
        protected T Result;

        public CoroutineAwaiter()
        {
        }

        public new T GetResult()
        {
            lock (Lock)
            {
                if (!Done)
                    throw new Exception("The task is not finished yet");
            }

            if (Exception != null)
                ExceptionDispatchInfo.Capture(Exception).Throw();

            return Result;
        }

        public void SetResult(T result, Exception exception)
        {
            lock (Lock)
            {
                if (Done)
                    return;

                this.Result = result;
                this.Exception = exception;
                this.Done = true;
                try
                {
                    Continuation?.Invoke();
                }
                catch (Exception)
                {
                }
                finally
                {
                    this.Continuation = null;
                }
            }
        }
    }

    public struct AsyncResultAwaiter<T> : IAwaiter where T : IAsyncResult
    {
        private T _asyncResult;

        public AsyncResultAwaiter(T asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException(nameof(asyncResult));
            this._asyncResult = asyncResult;
        }

        public bool IsCompleted => _asyncResult.IsDone;

        public void GetResult()
        {
            if (!IsCompleted)
                throw new Exception("The task is not finished yet");

            if (_asyncResult.Exception != null)
                ExceptionDispatchInfo.Capture(_asyncResult.Exception).Throw();

            _ = _asyncResult.Result;
        }

        public void OnCompleted(Action continuation)
        {
            ((ICriticalNotifyCompletion) this).UnsafeOnCompleted(continuation);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            _asyncResult.Callbackable().OnCallback((ar) => { continuation(); });
        }
    }

    public struct AsyncResultAwaiter<T, TResult> : IAwaiter<TResult>
        where T : IAsyncResult<TResult>
    {
        private T _asyncResult;

        public AsyncResultAwaiter(T asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException(nameof(asyncResult));
            this._asyncResult = asyncResult;
        }

        public bool IsCompleted => _asyncResult.IsDone;

        public TResult GetResult()
        {
            if (!IsCompleted)
                throw new Exception("The task is not finished yet");

            if (_asyncResult.Exception != null)
                ExceptionDispatchInfo.Capture(_asyncResult.Exception).Throw();

            return this._asyncResult.Result;
        }

        public void OnCompleted(Action continuation)
        {
            ((ICriticalNotifyCompletion) this).UnsafeOnCompleted(continuation);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            _asyncResult.Callbackable().OnCallback((ar) => { continuation(); });
        }
    }
}