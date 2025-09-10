using System.Threading;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LCHFramework.Extensions
{
    public static class AsyncOperationHandleExtension
    {
        // public static Awaitable ToAwaitable<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken = default)
        // {
        //     var acs = new AwaitableCompletionSource();
        //     if (cancellationToken.IsCancellationRequested) 
        //     {
        //         acs.SetCanceled();
        //         return acs.Awaitable;
        //     }
        //     
        //     if (!handle.IsValid())
        //     {
        //         // autoReleaseHandle:true handle is invalid(immediately internal handle == null) so return completed.
        //         return acs.Awaitable;
        //     }
        //     
        //     if (handle.IsDone)
        //     {
        //         if (handle.Status == AsyncOperationStatus.Failed)
        //         {
        //             acs.SetException(handle.OperationException);
        //         }
        //         return acs.Awaitable;
        //     }
        //
        //     var result = new Awaitable(cancellationToken: cancellationToken);
        //     handle.Completed += op => result.SetResult(true);
        //     return result;
        // }
        
        public static async Awaitable ToAwaitable<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) 
                return;
            
            if (!handle.IsValid())
                return;
            
            if (handle.IsDone)
                return;
            
            await AwaitableUtility.WaitUntil(() => handle.IsDone);
        }
        
        // public static async Awaitable ToAwaitable(this AsyncOperationHandle handle, CancellationToken cancellationToken = default)
        // {
        //     if (cancellationToken.IsCancellationRequested) 
        //         return;
        //     
        //     if (!handle.IsValid())
        //         return;
        //     
        //     if (handle.IsDone)
        //         return;
        //     
        //     await AwaitableUtility.WaitUntil(() => handle.IsDone);
        // }
    }
}
