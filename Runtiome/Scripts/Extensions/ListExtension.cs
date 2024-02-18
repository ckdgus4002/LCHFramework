using System.Collections.Generic;

namespace LCHFramework.Extensions
{
    public static class ListExtension
    {
        public static T AddAndReturn<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }
    }
}