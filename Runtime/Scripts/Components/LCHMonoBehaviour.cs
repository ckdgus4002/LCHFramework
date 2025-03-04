using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class LCHMonoBehaviour : MonoBehaviour
    {
        public static bool TryFindAnyObjectOfType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : Object => (result = FindAnyObjectByType<T>(findObjectsInactive)) != null;
        
        public static bool TryFindAnyObjectOfType<T>(out T result) where T : Object => (result = FindAnyObjectByType<T>()) != null;
        
        public static Object FindAnyObjectByTypes(params Type[] types)
        {
            var otherTypes = types.Length < 2 ? Array.Empty<Type>() : types[1..];
            foreach (var t in FindObjectsByType(types[0], FindObjectsSortMode.None))
            {
                var component = (Component)t;
                if (otherTypes.All(type => component.GetComponent(type)))
                    return t;
            }
            
            return null;
        }
        
        public static T FindAnyInterfaceByType<T>() where T : class
        {
            foreach (var t in FindObjectsByType<Component>(FindObjectsSortMode.None))
                if (t.TryGetComponent<T>(out var result))
                    return result;

            return default;
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
        
        
        protected readonly List<CancellationTokenSource> ctses = new();
        protected readonly List<IDisposable> disposables = new();
        
        
        public bool TRSIsInitialized { get; private set; }
        public int EnableCount { get; private set; }
        public int DisableCount { get; private set; }
        
        
        public bool IsDestroyed => this == null;
        
        public virtual int Index => transform.GetSiblingIndex();
        
        public virtual bool IsShown => gameObject.activeSelf;
        
        public virtual bool DoStopAllCoroutinesWhenDisable => true;
        
        public virtual bool DoClearTokenSourcesWhenDisable => true;
        
        public virtual bool DoDisposeDisposablesWhenDisable => true;
        
        public float HalfWidth => Width * .5f;
        
        public float HalfHeight => Height * .5f;
        
        public virtual float Width => transform is RectTransform ? RectTransformOrNull.rect.size.x
                        : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.x
                        : TryGetComponent<Collider>(out var colliderComponent) ? colliderComponent.bounds.size.x
                        : throw new ArgumentOutOfRangeException(null, "Width", null)
                        ;
        
        public virtual float Height => transform is RectTransform ? RectTransformOrNull.rect.size.y
            : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.y
            : TryGetComponent<Collider>(out var collider) ? collider.bounds.size.y
            : throw new ArgumentOutOfRangeException(null, "Height", null)
            ;

        public Canvas RootCanvasOrNull => !this.TryGetComponentInParent<Canvas>(out var result) ? null : result.rootCanvas;
        
        public RectTransform RectTransformOrNull => _rectTransform == null ? _rectTransform = (RectTransform)transform : _rectTransform;
        private RectTransform _rectTransform;
        
        
        
        protected virtual void Awake()
        {
            if (transform is not RectTransform) InitializeTRS();
        }

        protected virtual void OnEnable()
        {
            EnableCount++;
        }

        protected virtual void Start()
        {
            if (transform is RectTransform) InitializeTRS();
        }

        protected virtual void OnDisable()
        {
            DisableCount++;
            if (DoStopAllCoroutinesWhenDisable) StopAllCoroutines();
            if (DoClearTokenSourcesWhenDisable) CancellationTokenSourceUtility.ClearTokenSources(ctses);
            if (DoDisposeDisposablesWhenDisable) { disposables.ForEach(t => t.Dispose()); disposables.Clear(); }
        }
        
        
        
        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            TRSIsInitialized = true;
        }
    }
}