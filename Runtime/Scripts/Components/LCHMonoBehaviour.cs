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
        
        public static Object FindAnyComponentByTypes(params Type[] types)
        {
            foreach (var result in FindObjectsByType<LCHMonoBehaviour>(FindObjectsSortMode.None))
                if (types.All(type => result.GetComponent(type)))
                    return result;

            return null;
        }
        
        public static T FindAnyComponentByType<T>()
        {
            foreach (var t in FindObjectsByType<LCHMonoBehaviour>(FindObjectsSortMode.None))
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
        
        
        protected readonly List<CancellationTokenSource> _ctses = new();
        
        
        public bool TRSIsInitialized { get; private set; }
        
        public int EnableCount { get; private set; }
        
        public int DisableCount { get; private set; }


        public bool IsDestroyed => this == null;

        public virtual int Index => transform.GetSiblingIndex();
        
        public virtual bool IsShown => gameObject.activeSelf;
        
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

        public Canvas RootCanvasOrNull => !TryGetComponentInParent<Canvas>(out var result) ? null : result.rootCanvas;
        
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
            StopAllCoroutines();
            CancellationTokenSourceUtility.ClearTokenSources(_ctses);
        }
        
        
        
        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            TRSIsInitialized = true;
        }
        
        protected Coroutine RestartCoroutine(Coroutine stopCoroutine, IEnumerator startCoroutine) => RestartCoroutine(this, stopCoroutine, startCoroutine);
        
        protected Coroutine RestartCoroutine(IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine) => RestartCoroutine(this, stopCoroutines, startCoroutine);
        
        protected bool TryGetComponentInParent<T>(out T result) => ComponentExtension.TryGetComponentInParent(this, out result);
        
        protected T[] GetComponentsInParents<T>(bool includeInactive) where T : class => ComponentExtension.GetComponentsInParents<T>(this, includeInactive);
        
        protected T GetComponentInSibling<T>(bool includeMe = false) => ComponentExtension.GetComponentInSibling<T>(this, includeMe);
        
        protected List<T> GetComponentsInSibling<T>(bool includeMe = true) => ComponentExtension.GetComponentsInSibling<T>(this, includeMe);
    }
}