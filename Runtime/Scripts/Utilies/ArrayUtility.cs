using System;

namespace LCHFramework.Utility
{
    public static class ArrayUtils
    {
        public static void ForEach<T>(this T[] array, Action<T> action) => Array.ForEach(array, action);
    }
}