using System;
using LCHFramework.Components;
using UnityEngine;
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
        
        
        
        protected virtual Object DestroyTarget => gameObject;
        
        protected virtual bool IsDontDestroyOnLoad => false;
        
        protected virtual bool IsDestroyPrevInstance => true;
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

            if (Instance == this && IsDontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}