using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class TransformExtension
    {
        public static Transform FindInChildren(this Transform transform, string name)
        {
            var foundChild = transform.Find(name);
            if (foundChild == null)
            {
                foreach (Transform child in transform)
                {
                    foundChild = child.FindInChildren(name);
                    if (foundChild != null) break;
                }
            }
            
            return foundChild;
        }
        
        public static List<T> FindObjectsOfName<T>(this Transform transform, string name) where T : Component
            => transform.FindObjectsOfName<T>(name, string.Equals);

        public static List<T> FindObjectsOfName<T>(this Transform transform, string name, Func<string, string, bool> namePredicate) where T : Component
        {
            var foundChildren = new List<T>();
            foreach (Transform child in transform) foundChildren.AddRange(child._FindObjectsOfName<T>(name, namePredicate));

            return foundChildren;
        }
        
        private static List<T> _FindObjectsOfName<T>(this Transform transform, string name, Func<string, string, bool> namePredicate) where T : Component
        {
            var foundChildren = new List<T>();
            if (namePredicate(transform.name, name)) foundChildren.Add(transform.GetComponent<T>());
            
            foreach (Transform child in transform) foundChildren.AddRange(child._FindObjectsOfName<T>(name, namePredicate));

            return foundChildren;
        }

        public static bool ContainsWord(string name, string word)
            => name.Contains(word);

        public static Transform[] GetChildren(this Transform transform)
        {
            var children = new Transform[transform.childCount];

            for (var i = 0; i < transform.childCount; i++) children[i] = transform.GetChild(i);

            return children;
        }

        public static Transform GetRandomChild(this Transform transform)
            => transform.GetChildren().ExPick();

        public static string Path(this Transform transform)
        {
            var path = new StringBuilder(transform.name);
            while ((transform = transform.parent) != null) path = path.Insert(0, $"{transform.name}/");

            return path.ToString();
        }
    }
}