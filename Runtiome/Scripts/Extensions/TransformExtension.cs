using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class TransformExtension
    {
        public static List<Transform> GetChildren(this Transform transform)
        {
            var result = new List<Transform>();
            foreach (Transform child in transform)
            {
                result.Add(child);
                result.AddRange(GetChildren(child));
            }
            
            return result;
        }
        
        public static string Path(this Transform transform)
        {
            var path = new StringBuilder(transform.name);
            while ((transform = transform.parent) != null) path = path.Insert(0, $"{transform.name}/");

            return path.ToString();
        }
        
        public static int Depth(this Transform transform, Transform parent = null)
        {
            var result = 1;
            do
            {
                if (transform.parent != null && transform.parent != parent)
                {
                    result++;
                    transform = transform.parent;
                }
                else if (transform.parent == parent)
                    return result;
                else
                    return -1;
                
            } while (true);
        }
    }
}