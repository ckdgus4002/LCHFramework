using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class ListExtension
    {
        public static bool TryAdd<T>(this List<T> list, T item)
        {
            var result = list.Contains(item); 
            if (!result) list.Add(item);
            return !result;
        }
                
        public static T AddAndReturnItem<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }
        
        public static bool TryRemove<T>(this List<T> list, T item)
        {
            var result = list.Contains(item); 
            if (result) list.Remove(item);
            return result;
        }
        
        public static T RemoveAndReturnItem<T>(this List<T> list, T item)
        {
            list.Remove(item);
            return item;
        }
    }
}