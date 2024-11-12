using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static bool InstanceIsNull => Instance == null;
        
        public static T Instance
        {
            get => _instance == null ? Instance = FindFirstObjectByType<T>() : _instance;
            private set
            {
                var prevInstanceOrNull = _instance;
                _instance = prevInstanceOrNull != null && value != null && !value.DestroyPrevInstance ? prevInstanceOrNull : value;
                if (prevInstanceOrNull != null && prevInstanceOrNull != value)
                    Destroy((value == null || value.DestroyPrevInstance ? prevInstanceOrNull : value).DestroyTarget);
            }
        }
        private static T _instance;
        
        
        
        protected virtual Object DestroyTarget => gameObject;
        
        protected virtual bool IsDontDestroyOnLoad => false;
        
        protected virtual bool DestroyPrevInstance => true;
        
        
        
        protected virtual void Awake()
        {
            Instance = (object)this as T;
            
            if (Instance == this && IsDontDestroyOnLoad) DontDestroyOnLoad(DestroyTarget);
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}