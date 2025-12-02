using System;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class AwaitableUtility
    {
        public static async Awaitable WaitUntil(Func<bool> predicate) => await WaitWhile(() => !predicate.Invoke());
        
        public static async Awaitable WaitWhile(Func<bool> predicate) { while (predicate.Invoke()) await Awaitable.NextFrameAsync(); }

        public static Awaitable<T> FromResult<T>(T result)
        {
            var completionSource = new AwaitableCompletionSource<T>();
            completionSource.SetResult(result);
            return completionSource.Awaitable;
        }
    }
}