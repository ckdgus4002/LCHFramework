using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class GameObjectExtension
    {
        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T result) 
            => gameObject.transform.TryGetComponentInParent(out result);

        public static T[] GetComponentsInParents<T>(this GameObject gameObject, bool includeInactive) where T : class
            => gameObject.transform.GetComponentsInParents<T>(includeInactive);

        public static T GetComponentInSibling<T>(this GameObject gameObject, bool includeMe = false)
            => gameObject.transform.GetComponentInSibling<T>(includeMe);

        public static List<T> GetComponentsInSibling<T>(this GameObject gameObject, bool includeMe = true)
            => gameObject.transform.GetComponentsInSibling<T>(includeMe);
        
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
            => !gameObject.TryGetComponent<T>(out var result) ? gameObject.AddComponent<T>() : result;
        
        public static void SetLayerInChildren(this GameObject gameObject, string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
            foreach (Transform child in gameObject.transform) child.gameObject.SetLayerInChildren(layerName);
        }
    }
}