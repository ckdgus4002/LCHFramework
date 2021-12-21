using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class GameObjectsExtension
    {
        public static T[] ToArray<T>(this List<GameObject> list) where T : Component
        {
            var array = new T[list.Count];
            for (var i = 0; i < list.Count; i++) array[i] = list[i].GetComponent<T>();

            return array;
        }
    }
}