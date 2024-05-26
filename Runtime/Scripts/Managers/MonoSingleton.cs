using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = FindAnyObjectByType<T>();
                
                if (_instance == null) _instance = new GameObject(nameof(T)).AddComponent<T>();
                
                return _instance;
            }
            private set => _instance = value;
        }
        private static T _instance;
        
        
        
        protected virtual bool IsDontDestroyOnLoad { get; }
        
        
        
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this) DestroyInstance();

            Instance = (T)(Component)this;
            
            if (IsDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }



        public virtual void DestroyInstance() => Destroy(Instance.gameObject);
    }
}