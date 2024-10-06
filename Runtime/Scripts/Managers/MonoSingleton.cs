using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton<T> : LCHMonoBehaviour where T : Component
    {
        public static bool InstanceIsNull => Instance == null;
        
        public static T Instance
        {
            get => _instance == null ? _instance = FindAnyObjectByType<T>() : _instance; 
            private set
            {
                var prevInstance = _instance;
                _instance = value;
                
                if (prevInstance != null && _instance != null && prevInstance != _instance) Destroy(prevInstance);
            }
        }
        private static T _instance;
        
        
        
        [SerializeField] private bool isDontDestroyOnLoad;
        
        
        
        protected override void Awake()
        {
            base.Awake();

            Instance = (object)this as T;
            
            if (isDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}