using System;
using LCHFramework.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Managers
{
    public interface ISingleton
    {
        public bool IsDestroyPrevInstance { get; }
    }
    
    public static class Singleton
    {
        public static void EnsureInstance<T>(T value, T prevInstanceOrNull, Action<T> setInstance, Action<T> disposeInstance) where T: class, ISingleton
        {
            var valueIsNull = value is null;
            setInstance.Invoke(prevInstanceOrNull is not null && !valueIsNull && !value.IsDestroyPrevInstance
                ? prevInstanceOrNull
                : value);
            if (prevInstanceOrNull is not null && prevInstanceOrNull != value)
                disposeInstance.Invoke(valueIsNull || value.IsDestroyPrevInstance ? prevInstanceOrNull : value);
        }
    }
    
    public abstract class AbstractMonoSingleton : LCHMonoBehaviour, ISingleton
    {
        protected virtual Object DestroyTarget => gameObject;
        
        protected virtual bool IsDontDestroyOnLoad => false;
        
        public virtual bool IsDestroyPrevInstance => true;
    }
    
    public class MonoSingleton<T> : AbstractMonoSingleton where T : MonoSingleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = FindFirstObjectByType<T>());
            private set => Singleton.EnsureInstance(value, _instance, t => _instance = t, t => Destroy(t.DestroyTarget));
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