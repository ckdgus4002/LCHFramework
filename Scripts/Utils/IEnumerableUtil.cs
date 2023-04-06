using System;
using System.Collections.Generic;
using LCHFramework.Extensions;

namespace LCHFramework.Utils
{
    public static class IEnumerableUtil
    {
        private static readonly Random Random = new();
        private static void Shuffle<T>(ref IEnumerable<T> enumerable)
            => enumerable = enumerable.Shuffle();
    }
}