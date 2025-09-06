using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Components
{
    [DefaultExecutionOrder(EventSystemExecutionOrder - 1)]
    public sealed class MonoSingleton : AbstractMonoSingleton
    {
        private const int EventSystemExecutionOrder = -1000;
        private static readonly Dictionary<Type, MonoSingleton> instances = new();
        
        
        
        [SerializeField] private Component singletonType;
        [SerializeField] private bool isDontDestroyOnLoad;
        [SerializeField] private bool isDestroyPrevInstance = true;
        
        
        
        protected override bool IsDontDestroyOnLoad => isDontDestroyOnLoad;
        
        public override bool IsDestroyPrevInstance => isDestroyPrevInstance;
        
        public Type SingletonType => _singletonType ??= singletonType.GetType();
        private Type _singletonType;
        
        
        
#if UNITY_EDITOR
        private void OnValidate() => isDontDestroyOnLoad = isDontDestroyOnLoad && transform.parent == null;
#endif
        
        protected override void Awake()
        {
            base.Awake();
            
            Singleton.EnsureInstance(this, instances.GetValueOrDefault(SingletonType), t => instances[SingletonType] = t, t => Destroy(t.DestroyTarget));
            
            if (instances.GetValueOrDefault(SingletonType) == this)
            {
                DestroyNotMonoSingletonByType();
                SceneManager.sceneLoaded += OnSceneLoaded;
                if (IsDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }
        
        private void OnDestroy()
        {
            if (instances.GetValueOrDefault(SingletonType) == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                instances.Remove(SingletonType);
            }
        }
        
        
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => DestroyNotMonoSingletonByType();

        public void DestroyNotMonoSingletonByType()
            => FindObjectsByType(SingletonType, FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
                .Cast<Component>()
                .Where(t => t.GetComponent<MonoSingleton>() == null)
                .ForEach(t =>
                {
                    Debug.Log($"Destroy Not Mono Singleton Type: {t.transform.GetPath()}");
                    Destroy(t.gameObject);
                });
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonoSingleton), true)]
    public class MonoSingletonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Toggle("Is Destroy Not Mono Singleton Of Type", true);
            
            base.OnInspectorGUI();
        }
    }
#endif
}