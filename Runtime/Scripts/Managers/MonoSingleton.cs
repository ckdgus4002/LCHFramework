using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton<T> : LCHMonoBehaviour where T : Component
    {
        public static T Instance
        {
            get => _instance == null ? _instance = FindAnyObjectByType<T>() : _instance; 
            private set
            {
                if (_instance != null && _instance != value) Destroy(_instance);
                
                _instance = value;
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
    }
}