using System;
using System.Collections;
using System.Collections.Generic;
using LCHFramework.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Modules
{
    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {
        [NonSerialized] public Matrix4x4 defaultTRS;
        [NonSerialized] public Matrix4x4 defaultLocalTRS;


        public bool InitializedTRS { get; private set; }
        
        
        public float HalfWidth => Width * .5f;
        
        public float HalfHeight => Height * .5f;
        
        public virtual float Width => RectTransform != null ? RectTransform.rect.size.x
                        : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.x
                        : TryGetComponent<Collider>(out var colliderComponent) ? colliderComponent.bounds.size.x
                        : throw new ArgumentOutOfRangeException(null, "Width", null)
                        ;
        
        public virtual float Height => RectTransform != null ? RectTransform.rect.size.y
            : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.y
            : TryGetComponent<Collider>(out var collider) ? collider.bounds.size.y
            : throw new ArgumentOutOfRangeException(null, "Height", null)
            ;

        public RectTransform RectTransform => _rectTransform == null ? _rectTransform = transform as RectTransform : _rectTransform;
        [NonSerialized] private RectTransform _rectTransform;


        protected virtual void Awake()
        {
            if (RectTransform == null) InitializeTRS();
        }

        protected virtual void Start()
        {
            if (RectTransform != null) InitializeTRS();
        }

        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            InitializedTRS = true;
        }
        
        protected virtual void OnDisable() => StopAllCoroutines();



        protected bool TryFindObjectOfType<T>(bool includeInactive, out T result) where T : Object => (result = FindObjectOfType<T>(includeInactive)) != null;
        
        protected bool TryFindObjectOfType<T>(out T result) where T : Object => (result = FindObjectOfType<T>()) != null;
        
        public T GetComponentInSibling<T>(bool includeMe = false)
        {
            T result = default;
            var parent = transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var sibling = parent.GetChild(i);
                if ((includeMe || sibling != transform) && sibling.TryGetComponent(out result)) break;
            }
        
            return result;
        }
 
        public List<T> GetComponentsInSibling<T>(bool includeMe = true)
        {
            var result = new List<T>();
            var parent = transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var sibling = parent.GetChild(i);
                if ((includeMe || sibling != transform) && sibling.TryGetComponent(out T component))
                    result.Add(component);
            }
        
            return result;
        }

        protected Coroutine RestartCoroutine(Coroutine stopCoroutine, IEnumerator startCoroutine) 
            => CoroutineUtil.RestartCoroutine(this, stopCoroutine, startCoroutine);

        protected Coroutine RestartCoroutine(IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine)
            => CoroutineUtil.RestartCoroutine(this, stopCoroutines, startCoroutine);
    }
}