using System;
using LCHFramework.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Managers
{
    public abstract class AbstractMonoSingleton : LCHMonoBehaviour
    {
        protected static void EnsureInstance<T1, T2>(T1 value, T2 prevInstanceOrNull, ref T2 instance, Func<T2, Object> getPrevInstanceDestroyTarget) where T1: AbstractMonoSingleton where T2: Component
        {
            var valueIsNull = value is null;
            instance = prevInstanceOrNull is not null && !valueIsNull && !value.IsDestroyPrevInstance
                ? prevInstanceOrNull
                : value as T2;
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
    
    public class MonoSingleton : AbstractMonoSingleton
    {
        [SerializeField] private Component singletonType;
        [SerializeField] private bool isDontDestroyOnLoad;
        [SerializeField] private bool isDestroyPrevInstance = true;
        
        
        
        protected override bool IsDontDestroyOnLoad => isDontDestroyOnLoad;
        protected override bool IsDestroyPrevInstance => isDestroyPrevInstance;
        
        
        
        protected override void Awake()
        {
            base.Awake();

            var prevInstanceOrNull = !TryFindFirstObjectByType(singletonType.GetType(), out var result) ? null : (Component)result;
            Component instance = null;
            EnsureInstance(this, prevInstanceOrNull, ref instance, prevInstance => prevInstance.gameObject);
            
            if (instance == this && IsDontDestroyOnLoad) DontDestroyOnLoad(DestroyTarget);
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
                EnsureInstance(value, prevInstanceOrNull, ref _instance, prevInstance => prevInstance.DestroyTarget);
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