using System;
using System.Collections;
using System.Collections.Generic;
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
        
        public static LCHMonoBehaviour GetOrAddComponent(GameObject gameObject)
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
        
        
        public bool InitializedTRS { get; private set; }
        
        
        public float HalfWidth => Width * .5f;
        
        public float HalfHeight => Height * .5f;
        
        public virtual float Width => RectTransformOrNull != null ? RectTransformOrNull.rect.size.x
                        : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.x
                        : TryGetComponent<Collider>(out var colliderComponent) ? colliderComponent.bounds.size.x
                        : throw new ArgumentOutOfRangeException(null, "Width", null)
                        ;
        
        public virtual float Height => RectTransformOrNull != null ? RectTransformOrNull.rect.size.y
            : TryGetComponent<Renderer>(out var renderer) ? renderer.bounds.size.y
            : TryGetComponent<Collider>(out var collider) ? collider.bounds.size.y
            : throw new ArgumentOutOfRangeException(null, "Height", null)
            ;

        public Canvas RootCanvas => GetComponentInParent<Canvas>().rootCanvas;
        
        public RectTransform RectTransformOrNull => _rectTransform == null ? _rectTransform = transform as RectTransform : _rectTransform;
        private RectTransform _rectTransform;
        
        
        
        protected virtual void Awake()
        {
            if (RectTransformOrNull == null) InitializeTRS();
        }

        protected virtual void Start()
        {
            if (RectTransformOrNull != null) InitializeTRS();
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();   
        }



        public virtual void InitializeTRS()
        {
            defaultTRS = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            defaultLocalTRS = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            InitializedTRS = true;
        }
        
        public bool TryFindObjectOfType<T>(bool includeInactive, out T result) where T : Object => (result = FindObjectOfType<T>(includeInactive)) != null;
        
        public bool TryFindObjectOfType<T>(out T result) where T : Object => (result = FindObjectOfType<T>()) != null;
        
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

        public Coroutine RestartCoroutine(Coroutine stopCoroutine, IEnumerator startCoroutine) 
            => CoroutineUtil.RestartCoroutine(this, stopCoroutine, startCoroutine);

        public Coroutine RestartCoroutine(IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine)
            => CoroutineUtil.RestartCoroutine(this, stopCoroutines, startCoroutine);
    }
}