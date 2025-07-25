using System;
using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class IListExtension
    {
        private static readonly Random Random = new();
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null) return;
            
            var n = list.Count;
            while (1 < n)
            {
                n--;
                var k = Random.Next(0, n + 1);
                if (k != n) (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
