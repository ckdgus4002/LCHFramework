using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton<T> : LCHMonoBehaviour where T : MonoSingleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = FindFirstObjectByType<T>());
            private set
            {
                var prevInstanceOrNull = _instance;
                _instance = prevInstanceOrNull is not null && value is not null && !value.DestroyPrevInstance
                    ? prevInstanceOrNull
                    : value;
                if (prevInstanceOrNull is not null && prevInstanceOrNull != value)
                    Destroy((value is null || value.DestroyPrevInstance ? prevInstanceOrNull : value).DestroyTarget);
            }
        }
        private static T _instance;
        
        
        
        protected static void CreateGameObjectIfInstanceIsNull() { if (InstanceIsNull) new GameObject(typeof(T).Name).AddComponent<T>(); }
        
        
        
        protected virtual Object DestroyTarget => gameObject;
        
        protected virtual bool IsDontDestroyOnLoad => false;
        
        protected virtual bool DestroyPrevInstance => true;
        
        
        
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