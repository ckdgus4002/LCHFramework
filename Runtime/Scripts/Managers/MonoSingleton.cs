using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton : MonoSingleton<MonoSingleton>
    {
        [SerializeField] private bool isDontDestroyOnLoad;
        [SerializeField] private bool isDestroyPrevInstance = true;
        
        protected override bool IsDontDestroyOnLoad => isDontDestroyOnLoad;
        protected override bool IsDestroyPrevInstance => isDestroyPrevInstance;
    }
    
    public class MonoSingleton<T> : LCHMonoBehaviour where T : MonoSingleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = FindFirstObjectByType<T>());
            private set
            {
                var prevInstanceOrNull = _instance;
                _instance = prevInstanceOrNull is not null && value is not null && !value.IsDestroyPrevInstance
                    ? prevInstanceOrNull
                    : value;
                if (prevInstanceOrNull is not null && prevInstanceOrNull != value)
                    Destroy((value is null || value.IsDestroyPrevInstance ? prevInstanceOrNull : value).DestroyTarget);
            }
        }
        private static T _instance;
        
        
        
        protected static void CreateGameObjectIfInstanceIsNull() { if (InstanceIsNull) new GameObject(typeof(T).Name).AddComponent<T>(); }
        
        
        
        protected virtual Object DestroyTarget => gameObject;
        
        protected virtual bool IsDontDestroyOnLoad => false;
        
        protected virtual bool IsDestroyPrevInstance => true;
        
        
        
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