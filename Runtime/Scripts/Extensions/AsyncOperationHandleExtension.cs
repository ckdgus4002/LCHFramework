using System.Threading;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LCHFramework.Extensions
{
    public static class AsyncOperationHandleExtension
    {
        public static async Awaitable ToAwaitable(this AsyncOperationHandle handle, CancellationToken cancellationToken = default)
        {
            await AwaitableUtility.WaitUntil(() => cancellationToken.IsCancellationRequested || !handle.IsValid() || handle.IsDone);
        }
        
        public static async Awaitable ToAwaitable<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken = default)
        {
            await AwaitableUtility.WaitUntil(() => cancellationToken.IsCancellationRequested || !handle.IsValid() || handle.IsDone);
        }
    }
}