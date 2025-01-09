using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class DictionaryExtension
    {
        public static bool IsEmpty<T1, T2>(Dictionary<T1, T2> dictionary)
            => dictionary == null || dictionary.Values.IsEmpty();
    }
}