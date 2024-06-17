using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class ComponentExtension
    {
        public static bool TryGetComponentInParent<T>(this Component component, out T result) => (result = component.GetComponentInParent<T>()) != null;
        
        public static T[] GetComponentsInParents<T>(this Component component, bool includeInactive) where T : class
        {
            LinkedList<T> results = new();
            var parent = component.transform.parent;
            while (true)
            {
                if ((includeInactive || parent.gameObject.activeSelf) && parent.TryGetComponent<T>(out var result)) results.AddLast(result);

                if (parent.parent != null) parent = parent.parent;
                else break;
            }
            
            return results.ToArray();
        }
        
        public static T GetComponentInSibling<T>(this Component component, bool includeMe = false)
        {
            T result = default;
            var parent = component.transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var sibling = parent.GetChild(i);
                if ((includeMe || sibling != component.transform) && sibling.TryGetComponent(out result)) break;
            }
            
            return result;
        }
        
        public static List<T> GetComponentsInSibling<T>(this Component component, bool includeMe = true)
        {
            var result = new List<T>(16);
            var parent = component.transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var sibling = parent.GetChild(i);
                if ((includeMe || sibling != component.transform) && sibling.TryGetComponent(out T @try))
                    result.Add(@try);
            }
            
            return result;
        }
        
        public static void SetActive(this Component component, bool value) 
            => component.gameObject.SetActive(value);
        
        public static void RadioActiveInSiblings(this Component component, bool value)
            => component.ActionInSiblings<Transform>(sibling => sibling.SetActive(sibling == component.transform ? value : !value));

        public static void ActionInSiblings<T>(this Component component, Action<T> action)
        {
            var parent = component.transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var type = parent.GetChild(i).GetComponent<T>();
                if (type != null) action?.Invoke(type);
            }
        }
    }
}