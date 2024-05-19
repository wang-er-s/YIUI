namespace ET
{
    public static class AsyncResultHelper
    {
        public static IAsyncResult<IAsyncResult> WaitAny(ListComponent<IAsyncResult> results, bool autoStopOthers = false)
        {
            AsyncResult<IAsyncResult> asyncResult = AsyncResult<IAsyncResult>.Create();
            foreach (IAsyncResult result in results)
            {
                result.Callbackable().OnCallback(r =>
                {
                    if (autoStopOthers)
                    {
                        foreach (IAsyncResult other in results)
                        {
                            if (other != r && !other.IsDone)
                            {
                                other.Cancel();
                            }
                        }
                    }

                    asyncResult.SetResult(r);
                });
            }
            return asyncResult;
        }
        
        public static IAsyncResult WaitAll(ListComponent<IAsyncResult> results, TimerComponent timerComponent)
        {
            MulAsyncResult mulProgressResult = MulAsyncResult.Create(timerComponent);
            mulProgressResult.AddAsyncResult(results);
            return mulProgressResult;
        } 
    }
}