using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Components;
using LCHFramework.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Managers
{
    public abstract class AbstractMonoSingleton : LCHMonoBehaviour
    {
        protected static void EnsureInstance<T1, T2>(T1 value, T2 prevInstanceOrNull, Action<T2> setInstance, Func<T2, Object> getPrevInstanceDestroyTarget) where T1: AbstractMonoSingleton where T2: Component
        {
            var valueIsNull = value is null;
            setInstance.Invoke(prevInstanceOrNull is not null && !valueIsNull && !value.IsDestroyPrevInstance
                ? prevInstanceOrNull
                : value as T2);
            if (prevInstanceOrNull is not null && prevInstanceOrNull != value)
            {
                var prevInstanceDestroyTarget = getPrevInstanceDestroyTarget.Invoke(prevInstanceOrNull);
                Destroy(valueIsNull || value.IsDestroyPrevInstance ? prevInstanceDestroyTarget : value.DestroyTarget);
            }
        }
        
        
        
        protected virtual Object DestroyTarget => gameObject;
        protected virtual bool IsDontDestroyOnLoad => false;
        protected virtual bool IsDestroyPrevInstance => true;
    }
    
    /// <summary>
    /// DestroyTarget is gameObject.
    /// </summary>
    [DefaultExecutionOrder(EventSystemExecutionOrder - 1)]
    public class MonoSingleton : AbstractMonoSingleton
    {
        private const int EventSystemExecutionOrder = -1000;
        
        public static IReadOnlyDictionary<Type, Component> Instances => instances;
        private static readonly Dictionary<Type, Component> instances = new();
        
        
        
        public static void DestroyNotMonoSingletonOfType(Type singletonType) => FindObjectsByType(singletonType)?.Cast<Component>().Where(t => t.GetComponent<MonoSingleton>() ==null).ForEach(t => Destroy(t.gameObject));
        
        
        
        [SerializeField] private Component singletonComponent;
        [SerializeField] private bool isDontDestroyOnLoad;
        [SerializeField] private bool isDestroyPrevInstance = true;
        
        
        private Type SingletonType => singletonComponent.GetType();
        protected override bool IsDontDestroyOnLoad => isDontDestroyOnLoad;
        protected override bool IsDestroyPrevInstance => isDestroyPrevInstance;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            DestroyNotMonoSingletonOfType(SingletonType);
            
            var prevInstanceOrNull = instances.GetValueOrDefault(SingletonType);
            EnsureInstance(this, prevInstanceOrNull, instance => instances[SingletonType] = instance, prevInstance => prevInstance.gameObject);
            
            if (instances.GetValueOrDefault(SingletonType) == singletonComponent && IsDontDestroyOnLoad) DontDestroyOnLoad(DestroyTarget);
        }

        protected virtual void OnDestroy()
        {
            if (instances.GetValueOrDefault(SingletonType) == singletonComponent) instances.Remove(SingletonType);
        }
    }
    
    public class MonoSingleton<T> : AbstractMonoSingleton where T : MonoSingleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = FindFirstObjectByType<T>());
            private set
            {
                var prevInstanceOrNull = _instance;
                EnsureInstance(value, prevInstanceOrNull, instance => _instance = instance, prevInstance => prevInstance.DestroyTarget);
            }
        }
        private static T _instance;
        
        
        
        protected static void CreateGameObjectIfInstanceIsNull() { if (InstanceIsNull) new GameObject(typeof(T).Name).AddComponent<T>(); }
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            Instance = (object)this as T;
            
            if (Instance == this && IsDontDestroyOnLoad) DontDestroyOnLoad(DestroyTarget);
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}