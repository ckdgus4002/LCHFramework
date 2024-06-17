using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class MonoSingleton<T> : LCHMonoBehaviour where T : Component
    {
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = FindAnyObjectByType<T>();

                return _instance;
            }
            private set => _instance = value;
        }
        private static T _instance;
        
        
        
        [SerializeField] private bool isDontDestroyOnLoad;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (Instance != null && Instance != this) Destroy(Instance);

            Instance = (object)this as T;
            
            if (isDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}