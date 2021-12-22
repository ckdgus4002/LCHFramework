using System;
using System.Collections.Generic;
using System.Linq;

namespace LCHFramework
{
    public static class IEnumerable
    {
        private static void Shuffle<T>(ref IEnumerable<T> enumerable)
        {
            var random = new Random();
            enumerable = enumerable.OrderBy(x => random.Next());
        }
    }
}