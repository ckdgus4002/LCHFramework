using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LCHFramework.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || enumerable.Count() < 1 || enumerable.All(a => a == null);
        
        public static T[] Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var rnd = new Random();
            return enumerable.OrderBy(x => rnd.Next()).ToArray();
        }

        public static void SetActive(this IEnumerable<Component> components, bool value)
        {
            foreach (var component in components) component.SetActive(value);
        }
        
        public static void SetActive(this IEnumerable<GameObject> gameObjects, bool value)
        {
            foreach (var gameObject in gameObjects) gameObject.SetActive(value);
        }

        public static bool TryFirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out T result) where T : class
        {
            result = enumerable.FirstOrDefault(predicate);
            return result != null;
        }
    }
}