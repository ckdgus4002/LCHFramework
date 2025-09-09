using System;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class AwaitableUtility
    {
        public static async Awaitable WaitUntil(Func<bool> predicate) => await WaitWhile(() => !predicate.Invoke());
        
        public static async Awaitable WaitWhile(Func<bool> predicate) { while (predicate()) await Awaitable.NextFrameAsync(); }
    }
}