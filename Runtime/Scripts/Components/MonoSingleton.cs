using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LCHFramework.Components
{
    [DefaultExecutionOrder(EventSystemExecutionOrder - 1)]
    public class MonoSingleton : AbstractMonoSingleton
    {
        private const int EventSystemExecutionOrder = -1000;
        public const bool DestroyNotMonoSingletonType = true;
        
        
        
        private static readonly Dictionary<Type, MonoSingleton> instances = new();
        
        
        
        [SerializeField] private Component singletonType;
        [SerializeField] private bool isDontDestroyOnLoad;
        [SerializeField] private bool isDestroyPrevInstance = true;
        
        
        
        protected override bool IsDontDestroyOnLoad => isDontDestroyOnLoad;
        
        protected override bool IsDestroyPrevInstance => isDestroyPrevInstance;
        
        public Type SingletonType => _singletonType ??= singletonType.GetType();
        private Type _singletonType;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            EnsureInstance(this, instances.GetValueOrDefault(SingletonType), t => instances[SingletonType] = t);

            if (instances.GetValueOrDefault(SingletonType) == this && IsDontDestroyOnLoad)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                DontDestroyOnLoad(gameObject);
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
        
        
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (DestroyNotMonoSingletonType) DestroyNotMonoSingletonOfType(SingletonType);
        }

        private void DestroyNotMonoSingletonOfType(Type type) => FindObjectsByType(type, FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
            .Cast<Component>()
            .Where(t => t.GetComponent<MonoSingleton>() ==null)
            .ForEach(t => 
            {
                Debug.Log($"Destroy Not Mono Singleton Type: {t.transform.GetPath()}");
                Destroy(t.gameObject);
            });
    }
}
