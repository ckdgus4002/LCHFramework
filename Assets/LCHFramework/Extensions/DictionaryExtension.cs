using System.Collections.Generic;
using System.Linq;

namespace LCHFramework.Extensions
{
    public static class DictionaryExtension
    {
        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) 
            => dictionary == null || dictionary.Count == 0 || dictionary.All(item => item.Value.Equals(null));
    }
}