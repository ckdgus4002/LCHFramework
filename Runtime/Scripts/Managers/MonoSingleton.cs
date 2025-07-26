using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Components;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LCHFramework.Managers
{
    public abstract class AbstractMonoSingleton : LCHMonoBehaviour
    {
        protected static void EnsureInstance<T>(T value, T prevInstanceOrNull, Action<T> setInstance) where T: AbstractMonoSingleton
        {
            var valueIsNull = value is null;
            setInstance.Invoke(prevInstanceOrNull is not null && !valueIsNull && !value.IsDestroyPrevInstance
                ? prevInstanceOrNull
                : value);
            if (prevInstanceOrNull is not null && prevInstanceOrNull != value)
                Destroy(valueIsNull || value.IsDestroyPrevInstance ? prevInstanceOrNull.DestroyTarget : value.DestroyTarget);
        }
        
        
        
        protected virtual bool IsDontDestroyOnLoad => false;
        
        protected virtual bool IsDestroyPrevInstance => true;
        
        protected virtual Object DestroyTarget => gameObject;
    }
    
    [DefaultExecutionOrder(EventSystemExecutionOrder - 1)]
    public class MonoSingleton : AbstractMonoSingleton
    {
        private const int EventSystemExecutionOrder = -1000;
        
        
        
        private static readonly Dictionary<Type, MonoSingleton> instances = new();
        
        
        
        [SerializeField] private Component singletonType;
        [SerializeField] private bool destroyNotMonoSingletonTypeOnSceneLoaded = true;
        [SerializeField] private bool isDontDestroyOnLoad;
        [SerializeField] private bool isDestroyPrevInstance = true;
        
        
        
        protected override bool IsDontDestroyOnLoad => isDontDestroyOnLoad;

        protected override bool IsDestroyPrevInstance => isDestroyPrevInstance;
        
        private Type SingletonType => _singletonType ??= singletonType.GetType();
        private Type _singletonType;


        
#if UNITY_EDITOR
        private void OnValidate()
        {
            destroyNotMonoSingletonTypeOnSceneLoaded = true;
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            EnsureInstance(this, instances.GetValueOrDefault(SingletonType), t => instances[SingletonType] = t);
            
            if (instances.GetValueOrDefault(SingletonType) == singletonType && IsDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            if (instances.GetValueOrDefault(SingletonType) == singletonType) instances.Remove(SingletonType);
        }
        
        
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (destroyNotMonoSingletonTypeOnSceneLoaded) DestroyNotMonoSingletonOfType(SingletonType);
        }

        private void DestroyNotMonoSingletonOfType(Type type) => FindObjectsByType(type, FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
            .Cast<Component>()
            .Where(t => t.GetComponent<MonoSingleton>() ==null)
            .ForEach(t => 
            {
                Debug.Log($"Destroy: {t.transform.GetPath()}");
                Destroy(t.gameObject);
            });
    }
    
    public class MonoSingleton<T> : AbstractMonoSingleton where T : MonoSingleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = FindFirstObjectByType<T>());
            private set => EnsureInstance(value, _instance, instance => _instance = instance);
        }
        private static T _instance;
        
        
        
        protected static void CreateGameObjectIfInstanceIsNull() { if (InstanceIsNull) new GameObject(typeof(T).Name).AddComponent<T>(); }
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            Instance = (object)this as T;
            
            if (Instance == this && IsDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}