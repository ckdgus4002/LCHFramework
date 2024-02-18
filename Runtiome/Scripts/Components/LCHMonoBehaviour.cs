using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class LCHMonoBehaviour : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        public static void RuntimeInitializeOnLoadMethod()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            foreach (var item in _getOrAddComponent)
                if (scene == item.Value.gameObject.scene)
                    _getOrAddComponent.Remove(item.Key);
        }
        
        public static LCHMonoBehaviour GetOrAddMonoBehaviour(GameObject gameObject)
        {
            if (_getOrAddComponent == null)
            {
                _getOrAddComponent = new Dictionary<GameObject, LCHMonoBehaviour>();
                if (!_getOrAddComponent.ContainsKey(gameObject)) _getOrAddComponent.Add(gameObject, gameObject.GetOrAddComponent<LCHMonoBehaviour>());
            }

            return _getOrAddComponent[gameObject];
        }
        private static Dictionary<GameObject, LCHMonoBehaviour> _getOrAddComponent;
        
        
        
        [NonSerialized] public Matrix4x4 defaultTRS;
        [NonSerialized] public Matrix4x4 defaultLocalTRS;
        [NonSerialized] public string defaultName;
        
        
        public bool TRSIsInitialized { get; private set; }
        public bool NameIsInitialized { get; private set; }
        public bool IsDestroyed { get; private set; }
        
        
        public float HalfWidth => Width * .5f;
        
        public float HalfHeight => Height * .5f;
        
        public virtual float Width => transform is RectTransform ? RectTransform.rect.size.x
                        : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.x
                        : TryGetComponent<Collider>(out var colliderComponent) ? colliderComponent.bounds.size.x
                        : throw new ArgumentOutOfRangeException(null, "Width", null)
                        ;
        
        public virtual float Height => transform is RectTransform ? RectTransform.rect.size.y
            : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.y
            : TryGetComponent<Collider>(out var collider) ? collider.bounds.size.y
            : throw new ArgumentOutOfRangeException(null, "Height", null)
            ;

        public Canvas RootCanvas => GetComponentInParent<Canvas>().rootCanvas;
        
        public RectTransform RectTransform => _rectTransform == null ? _rectTransform = (RectTransform)transform : _rectTransform;
        private RectTransform _rectTransform;
        
        
        
        protected virtual void Awake()
        {
            if (transform is not UnityEngine.RectTransform) InitializeTRS();
            InitializeName();
        }

        protected virtual void Start()
        {
            if (transform is RectTransform) InitializeTRS();
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();   
        }

        protected virtual void OnDestroy()
        {
            IsDestroyed = true;
        }


        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            TRSIsInitialized = true;
        }

        private void InitializeName()
        {
            defaultName = name;
            NameIsInitialized = true;
        }
        
        public bool TryFindAnyObjectOfType<T>(FindObjectsInactive findObjectsInactive, out T result) where T : Object => (result = FindAnyObjectByType<T>(findObjectsInactive)) != null;
        
        public bool TryFindAnyObjectOfType<T>(out T result) where T : Object => (result = FindAnyObjectByType<T>()) != null;

        public T GetComponentInParents<T>(bool includeInactive)
        {
            T result = default;
            var parent = transform.parent;
            while (true)
            {
                if ((includeInactive || parent.gameObject.activeSelf) && parent.TryGetComponent(out result)) break;

                if (parent.parent != null) parent = parent.parent;
                else break;
            }

            return result;
        }
        
        public T[] GetComponentsInParents<T>(bool includeInactive) where T : class
        {
            LinkedList<T> results = new();
            var parent = transform.parent;
            while (true)
            {
                if ((includeInactive || parent.gameObject.activeSelf) && parent.TryGetComponent<T>(out var result)) results.AddLast(result);

                if (parent.parent != null) parent = parent.parent;
                else break;
            }

            return results.ToArray();
        }
        
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
            var result = new List<T>(16);
            var parent = transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var sibling = parent.GetChild(i);
                if ((includeMe || sibling != transform) && sibling.TryGetComponent(out T component))
                    result.Add(component);
            }
        
            return result;
        }

        public Coroutine RestartCoroutine(Coroutine stopCoroutine, IEnumerator startCoroutine) 
            => CoroutineUtil.RestartCoroutine(this, stopCoroutine, startCoroutine);

        public Coroutine RestartCoroutine(IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine)
            => CoroutineUtil.RestartCoroutine(this, stopCoroutines, startCoroutine);
    }
}