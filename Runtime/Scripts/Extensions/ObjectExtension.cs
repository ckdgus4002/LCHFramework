using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class ObjectExtension
    {
        public static bool TryIsNotNull<T>(this T @object, out T result) where T : Object
            => (result = @object == null ? null : @object) != null;
    }
}