using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LCHFramework.Extensions
{
    public static class IEnumerableExtension
    {
        private static readonly Random Random = new();
        public static T Pick<T>(this IEnumerable<T> enumerable)
            => enumerable.ElementAt(Random.Next(enumerable.Count()));

        public static T[] Copy<T>(this T[] array, int length) => Copy(array, 0, length);
        
        public static T[] Copy<T>(this T[] array, int startIndex, int length)
        {
            var clampedStartIndex = Mathf.Clamp(startIndex, 0, array.Length - 1);
            var clampedLength = Mathf.Clamp(length, 0, array.Length);
            var result = new T[clampedStartIndex + clampedLength <= array.Length ? clampedLength
                : array.Length <= clampedLength ? 1
                : array.Length - clampedStartIndex];
            for (var i = 0; i < result.Length; i++) result[i] = array[startIndex + i];
            return result;
        }

        public static bool TryFirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out T result)
        {
            result = enumerable.FirstOrDefault(predicate);
            return !EqualityComparer<T>.Default.Equals(result, default);
        }
        
        public static bool TryLastOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out T result)
        {
            result = enumerable.LastOrDefault(predicate);
            return !EqualityComparer<T>.Default.Equals(result, default);
        }

        public static bool IsExists<T>(this IEnumerable<T> enumerable) => !IsEmpty(enumerable);
        
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable == null || enumerable.All(item => EqualityComparer<T>.Default.Equals(item, default));

        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value) 
            => IndexOf(enumerable, t => EqualityComparer<T>.Default.Equals(t, value));
        
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> func)
        {
            var result = 0;
            foreach (var item in enumerable)
            {
                if (!func.Invoke(item)) result++;
                else return result;
            }
            return -1;
        }

        public static T ClampedElementAt<T>(this IEnumerable<T> enumerable, int index) 
            => enumerable.ElementAt(Mathf.Clamp(index, 0, enumerable.Count() - 1));

        public static void RadioActive<T>(this IEnumerable<T> components, int index, bool value) where T : Component
            => components.Select(t => t.gameObject).RadioActive(index, value);
        
        public static void RadioActive(this IEnumerable<GameObject> gameObjects, int index, bool value) 
        {
            var i = 0;
            foreach (var item in gameObjects)
            {
                item.SetActive(i == index ? value : !value);
                ++i;
            }
        }

        public static void RadioActive<T>(this IEnumerable<T> components, T item, bool value) where T : Component
            => components.Select(t => t.gameObject).RadioActive(item.gameObject, value);
        
        public static void RadioActive(this IEnumerable<GameObject> gameObjects, GameObject item, bool value) 
        {
            foreach (var t in gameObjects) t.SetActive(t == item ? value : !value);
        }
        
        public static void SetActive<T>(this IEnumerable<T> components, bool value) where T : Component
        {
            foreach (var component in components) component.SetActive(value);
        }
        
        public static void SetActive(this IEnumerable<GameObject> gameObjects, bool value)
        {
            foreach (var gameObject in gameObjects) gameObject.SetActive(value);
        }
        
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var t in enumerable) action?.Invoke(t);
        }
        
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var i = 0;
            foreach (var t in enumerable)
            {
                action?.Invoke(t, i);
                i++;
            }
        }
    }
}