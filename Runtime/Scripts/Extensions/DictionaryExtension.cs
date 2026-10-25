using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class DictionaryExtension
    {
        public static TKey AddAndReturnKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.Add(key, value);
            return key;
        }
        
        public static TValue AddAndReturnValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.Add(key, value);
            return value;
        }
        
        public static bool IsEmpty<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
            => dictionary == null || dictionary.Values.IsEmpty();
    }
}