using System;
using System.Threading;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class AwaitableUtility
    {
        public static async Awaitable WaitUntil(Func<bool> predicate, CancellationToken cancellationToken = default) { while (!predicate.Invoke()) await Awaitable.NextFrameAsync(cancellationToken); }
        
        public static async Awaitable WaitWhile(Func<bool> predicate, CancellationToken cancellationToken = default) { while (predicate.Invoke()) await Awaitable.NextFrameAsync(cancellationToken); }
        
        public static Awaitable CompletedTask => FromResult();
        
        public static Awaitable FromResult() => new AwaitableCompletionSource().Awaitable;
        
        public static Awaitable<T> FromResult<T>(T result)
        {
            var completionSource = new AwaitableCompletionSource<T>();
            completionSource.SetResult(result);
            return completionSource.Awaitable;
        }
    }
}