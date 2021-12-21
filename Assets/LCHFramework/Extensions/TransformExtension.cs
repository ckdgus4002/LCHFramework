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
                foreach (var child in transform.GetChildren())
                {
                    foundChild = child.FindInChildren(name);
                    if (foundChild != null) break;
                }
            }
            
            return foundChild;
        }

        public static List<T> FindObjectsOfName<T>(this Transform transform, string name) where T : Component
        {
            var children = transform.GetChildren();
            var foundChildren = new List<T>();
            foreach (var child in children) foundChildren.AddRange(child._FindObjectsOfName<T>(name));

            return foundChildren;
        }
        
        private static List<T> _FindObjectsOfName<T>(this Transform transform, string name) where T : Component
        {
            var foundChildren = new List<T>();
            if (transform.name.Contains(name)) foundChildren.Add(transform.GetComponent<T>());
            
            var children = transform.GetChildren();
            foreach (var child in children) foundChildren.AddRange(child._FindObjectsOfName<T>(name));

            return foundChildren;
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            var children = new Transform[transform.childCount];

            for (var i = 0; i < transform.childCount; i++) children[i] = transform.GetChild(i);

            return children;
        }

        public static Transform GetRandomChild(this Transform transform)
            => transform.GetChild(Random.Range(0, transform.childCount));

        public static string Path(this Transform transform)
        {
            var path = new StringBuilder(transform.name);
            while ((transform = transform.parent) != null) path = path.Insert(0, $"{transform.name}/");

            return path.ToString();
        }
    }
}