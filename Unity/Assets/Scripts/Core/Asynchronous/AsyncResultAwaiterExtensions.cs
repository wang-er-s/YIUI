namespace ET
{
    public static class AsyncResultAwaiterExtensions
    {
        public static async ETTask GetAwaiter(this IAsyncResult target)
        {
            ETTask task = ETTask.Create(true);
            target.Callbackable().OnCallback(r =>
            {
                if (r.Exception != null)
                {
                    task.SetException(r.Exception);
                }
                else
                {
                    task.SetResult();
                }
            });
            await task;
        }

        public static async ETTask<TResult> GetAwaiter<TResult>(this IAsyncResult<TResult> target)
        {
            ETTask<TResult> task = ETTask<TResult>.Create(true);
            target.Callbackable().OnCallback(r =>
            {
                if (r.Exception != null)
                {
                    task.SetException(r.Exception);
                }
                else
                {
                    task.SetResult(r.Result);
                }
            });
            return await task;
        }
    }
}