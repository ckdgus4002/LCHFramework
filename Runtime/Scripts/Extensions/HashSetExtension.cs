using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class HashSetExtension
    {
        public static bool TryAdd<T>(this HashSet<T> hashSet, T item)
        {
            var result = hashSet.Contains(item); 
            if (!result) hashSet.Add(item);
            return !result;
        }
        
        public static T AddAndReturnItem<T>(this HashSet<T> hashSet, T item)
        {
            hashSet.Add(item);
            return item;
        }
        
        public static T RemoveAndReturnItem<T>(this HashSet<T> hashSet, T item)
        {
            hashSet.Remove(item);
            return item;
        }
    }
}