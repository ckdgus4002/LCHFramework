using System;
using LCHFramework.Utilities;
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

                if (_instance == null) throw new NullReferenceException($"Can't find any objects by type<{nameof(T)} in {string.Concat(SceneManagerUtility.GetAllScenes(), ", ")} scene.");
                
                return _instance;
            }
            private set => _instance = value;
        }
        private static T _instance;
        
        
        
        [SerializeField] private bool isDontDestroyOnLoad;
        
        
        
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this) Destroy(Instance);

            Instance = (T)(Component)this;
            
            if (isDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}