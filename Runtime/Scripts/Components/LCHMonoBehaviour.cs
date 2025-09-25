using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class LCHMonoBehaviour : MonoBehaviour
    {
        public static bool TryFindObjectsByType<T>(out T[] result) where T : Object
            => TryFindObjectsByType(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID, out result);

        public static bool TryFindObjectsByType<T>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode, out T[] result) where T : Object
            => (result = FindObjectsByType<T>(findObjectsInactive, sortMode)).Any();
        
        public static bool TryFindObjectsByType(Type type, out Object[] result)
            => TryFindObjectsByType(type, FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID, out result);

        public static bool TryFindObjectsByType(Type type, FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode, out Object[] result)
            => (result = FindObjectsByType(type, findObjectsInactive, sortMode)).Any();
        
        public static bool TryFindAnyObjectByType<T>(out T result) where T : Object
            => TryFindAnyObjectByType(FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindAnyObjectByType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : Object
            => (result = FindAnyObjectByType<T>(findObjectsInactive)) != null;
        
        public static bool TryFindFirstObjectByType<T>(out T result) where T : Object
            => TryFindFirstObjectByType(FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindFirstObjectByType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : Object
            => (result = FindFirstObjectByType<T>(findObjectsInactive)) != null;

        public static bool TryFindFirstObjectByType(Type type, out Object result)
            => TryFindFirstObjectByType(type, FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindFirstObjectByType(Type type, FindObjectsInactive findObjectsInactive, out Object result)
            => (result = FindFirstObjectByType(type, findObjectsInactive)) != null;

        public static bool TryFindAnyInterfaceByType<T>(out T result) where T : class
            => TryFindAnyInterfaceByType(FindObjectsInactive.Exclude, out result);
        
        public static bool TryFindAnyInterfaceByType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : class
            => (result = FindAnyInterfaceByType<T>(findObjectsInactive)) != null;
        
        public static T FindAnyInterfaceByType<T>(FindObjectsInactive findObjectsInactive = FindObjectsInactive.Exclude) where T : class
        {
            foreach (var t in FindObjectsByType<Component>(findObjectsInactive, FindObjectsSortMode.InstanceID))
                if (t.TryGetComponent<T>(out var result))
                    return result;

            return null;
        }
                
        public static Coroutine RestartCoroutine(MonoBehaviour monoBehaviour, Coroutine stopCoroutine, IEnumerator startCoroutine)
        {
            if (stopCoroutine != null) monoBehaviour.StopCoroutine(stopCoroutine);

            return monoBehaviour.StartCoroutine(startCoroutine);
        }
        
        public static Coroutine RestartCoroutine(MonoBehaviour monoBehaviour, IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine)
        {
            foreach (var stopCoroutine in stopCoroutines) if (stopCoroutine != null) monoBehaviour.StopCoroutine(stopCoroutine);
            
            return monoBehaviour.StartCoroutine(startCoroutine);
        }
        
        
        
        [NonSerialized] public Matrix4x4 defaultTRS;
        [NonSerialized] public Matrix4x4 defaultLocalTRS;
        [NonSerialized] public string defaultName;
        
        
        
        [field: NonSerialized] public bool TRSIsInitialized { get; private set; }
        [field: NonSerialized] public int EnableCount { get; private set; }
        [field: NonSerialized] public int DisableCount { get; private set; }
        
        
        public bool IsDestroyed => this == null;
        
        public virtual int Index => transform.GetSiblingIndex();
        
        public virtual bool IsShown => gameObject.activeSelf;
        
        public float HalfWidth => Width * .5f;
        
        public float HalfHeight => Height * .5f;
        
        public virtual float Width => transform is RectTransform ? RectTransform.rect.size.x
                        : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.x
                        : TryGetComponent<Collider>(out var collider) ? collider.bounds.size.x
                        : throw new ArgumentOutOfRangeException(null, nameof(Width), null);
        
        public virtual float Height => transform is RectTransform ? RectTransform.rect.size.y
            : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.y
            : TryGetComponent<Collider>(out var collider) ? collider.bounds.size.y
            : throw new ArgumentOutOfRangeException(null, nameof(Height), null);
        
        public Canvas RootCanvasOrNull => !this.TryGetComponentInParent<Canvas>(out var result) ? null : result.rootCanvas;
        
        public RectTransform RectTransform => _rectTransform == null ? _rectTransform = (RectTransform)transform : _rectTransform;
        private RectTransform _rectTransform;
        
        
        
        protected virtual void Awake()
        {
            defaultName = name.Replace("(Clone)", "");
            
            if (transform is not UnityEngine.RectTransform) InitializeTRS();
        }
        
        protected virtual void OnEnable()
        {
            EnableCount++;
        }
        
        protected virtual IEnumerator Start()
        {
            if (transform is RectTransform) InitializeTRS();
            
            yield break;
        }
        
        protected virtual void OnDisable()
        {
            DisableCount++;
        }
        
        
        
        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            TRSIsInitialized = true;
        }
    }
}