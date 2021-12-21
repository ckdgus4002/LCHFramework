using System;
using UnityEngine;

namespace LCHFramework
{
    public static class ComponentExtension
    {
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