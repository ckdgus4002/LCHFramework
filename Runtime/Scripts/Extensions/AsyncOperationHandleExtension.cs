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
            await AwaitableUtility.WaitUntil(() => handle.IsDone, cancellationToken);
        }
        
        public static async Awaitable<T> ToAwaitable<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken = default)
        {
            await AwaitableUtility.WaitUntil(() => handle.IsDone, cancellationToken);

            return !handle.IsValid() ? default : handle.Result;
        }
    }
}