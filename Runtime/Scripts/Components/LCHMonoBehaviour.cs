using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class LCHMonoBehaviour : MonoBehaviour, IDestroyGameObject
    {
        public static float GetWidth(Transform transform) => transform is RectTransform rectTransform ? Math.Abs(rectTransform.rect.size.x * rectTransform.lossyScale.x)
            : transform.TryGetComponent<Renderer>(out var result1) ? Math.Abs(result1.bounds.size.x * transform.lossyScale.x)
            : transform.TryGetComponent<Collider>(out var result2) ? Math.Abs(result2.bounds.size.x * transform.lossyScale.x)
            : -1;
        
        public static float GetHeight(Transform transform) => transform is RectTransform rectTransform ? Math.Abs(rectTransform.rect.size.y * rectTransform.lossyScale.y)
            : transform.TryGetComponent<Renderer>(out var result1) ? Math.Abs(result1.bounds.size.y * transform.lossyScale.y)
            : transform.TryGetComponent<Collider>(out var result2) ? Math.Abs(result2.bounds.size.y * transform.lossyScale.y)
            : -1;
        
        public static bool TryFindObjectsByType<T>(out T[] result) where T : Object
#if UNITY_6000_6_OR_NEWER
            => TryFindObjectsByType(FindObjectsInactive.Exclude, out result);
#else
            => TryFindObjectsByType(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID, out result);
#endif
        
#if UNITY_6000_6_OR_NEWER
        public static bool TryFindObjectsByType<T>(FindObjectsInactive findObjectsInactive, out T[] result) where T : Object
            => (result = FindObjectsByType<T>(findObjectsInactive)).Exists();
#else
        public static bool TryFindObjectsByType<T>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode, out T[] result) where T : Object
            => (result = FindObjectsByType<T>(findObjectsInactive, sortMode)).Exists();
#endif
        
        public static bool TryFindAnyObjectByType<T>(out T result) where T : Object
            => TryFindAnyObjectByType(FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindAnyObjectByType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : Object
            => (result = FindAnyObjectByType<T>(findObjectsInactive)) != null;
        
        public static bool TryFindAnyInterfaceByType<T>(out T result) where T : class
            => TryFindAnyInterfaceByType(FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindAnyInterfaceByType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : class
            => (result = FindAnyInterfaceByType<T>(findObjectsInactive)) != null;
        
        public static T FindAnyInterfaceByType<T>(FindObjectsInactive findObjectsInactive = FindObjectsInactive.Exclude) where T : class
        {
#if UNITY_6000_6_OR_NEWER
            foreach (var t in FindObjectsByType<Component>(findObjectsInactive))
#else
            foreach (var t in FindObjectsByType<Component>(findObjectsInactive, FindObjectsSortMode.InstanceID))
#endif
                if (t is T result)
                    return result;
            
            return null;
        }
        
        public static bool TryFindAnyInterfacesByType<T>(out T[] result) where T : class
            => TryFindAnyInterfacesByType(FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindAnyInterfacesByType<T>(FindObjectsInactive findObjectsInactive, out T[] result) where T : class
            => (result = FindAnyInterfacesByType<T>(findObjectsInactive)).Exists();
        
        public static T[] FindAnyInterfacesByType<T>(FindObjectsInactive findObjectsInactive = FindObjectsInactive.Exclude) where T : class
        {
#if UNITY_6000_6_OR_NEWER
            return FindObjectsByType<Component>(findObjectsInactive).OfType<T>().ToArray();
#else
            return FindObjectsByType<Component>(findObjectsInactive, FindObjectsSortMode.InstanceID).OfType<T>().ToArray();
#endif
        }
        
        public static Coroutine RestartCoroutine(MonoBehaviour monoBehaviour, Coroutine stopCoroutine, IEnumerator startCoroutine)
        {
            StopCoroutine(monoBehaviour, stopCoroutine);
            
            return monoBehaviour.StartCoroutine(startCoroutine);
        }
        
        public static Coroutine RestartCoroutine(MonoBehaviour monoBehaviour, IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine)
        {
            StopCoroutines(monoBehaviour, stopCoroutines);
            
            return monoBehaviour.StartCoroutine(startCoroutine);
        }
        
        public static void StopCoroutines(MonoBehaviour monoBehaviour, IEnumerable<Coroutine> stopCoroutines) { foreach (var stopCoroutine in stopCoroutines) StopCoroutine(monoBehaviour, stopCoroutine); }
        
        public static void StopCoroutine(MonoBehaviour monoBehaviour, Coroutine stopCoroutine) { if (stopCoroutine != null) monoBehaviour.StopCoroutine(stopCoroutine); }
        
        
        
        [NonSerialized] public Matrix4x4 defaultTRS;
        [NonSerialized] public Matrix4x4 defaultLocalTRS;
        [NonSerialized] public string defaultName;
        
        
        [field: NonSerialized] public bool TRSIsInitialized { get; private set; }
        [field: NonSerialized] public int EnableCount { get; private set; }
        [field: NonSerialized] public int DisableCount { get; private set; }
        
        
        public bool IsDestroyed => this == null;
        
        public virtual int Index => transform.GetSiblingIndex();
        
        public virtual bool IsShown => gameObject.activeInHierarchy;
        
        public float HalfWidth => Width * .5f;
        
        public float HalfHeight => Height * .5f;
        
        public virtual float Width => GetWidth(transform);
        
        public virtual float Height => GetHeight(transform);
        
        public Canvas RootCanvasOrNull => !this.TryGetComponentInParent<Canvas>(out var result) ? null : result.rootCanvas;
        
        public RectTransform RectTransform => _rectTransform == null ? _rectTransform = (RectTransform)transform : _rectTransform;
        [NonSerialized] private RectTransform _rectTransform;
        
        
        
        protected virtual void Awake()
        {
            defaultName = name.Replace("(Clone)", "");
            
            if (transform is not UnityEngine.RectTransform) InitializeTRS();
        }
        
        protected virtual void OnEnable() => EnableCount++;
        
        protected virtual IEnumerator Start()
        {
            if (transform is RectTransform) InitializeTRS();
            
            yield break;
        }
        
        protected virtual void OnDisable() => DisableCount++;
        
        
        
        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            TRSIsInitialized = true;
        }
        
        public virtual void DestroyGameObject(bool allowDestroyingAssets = false) => ObjectUtility.Destroy(gameObject, allowDestroyingAssets);
    }
}