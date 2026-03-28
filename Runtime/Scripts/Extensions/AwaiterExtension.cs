using System.Collections;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class AwaiterExtension
    {
        public static IEnumerator ToYieldInstruction<T>(this Awaitable<T>.Awaiter awaiter)
        {
            yield return new WaitUntil(() => awaiter.IsCompleted);
        }
    }
}