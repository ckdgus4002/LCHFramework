using System;
using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class IListExtension
    {
        private static readonly Random Random = new();
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(0, n + 1);
                if (k != n) (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static int LastIndexOf<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (var i = list.Count - 1; 0 <= i; i--)
                if (predicate.Invoke(list[i]))
                    return i;

            return -1;
        }
    }
}
