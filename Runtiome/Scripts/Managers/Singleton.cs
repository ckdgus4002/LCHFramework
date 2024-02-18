using UnityEngine;

namespace LCHFramework.Managers
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance
        {
            get => _instance == null ? _instance = FindAnyObjectByType<T>() : _instance;
            protected set => _instance = value;
        }
        private static T _instance;



        protected virtual void Awake()
        {
            if (Instance != this)
            {
                var older = Instance;
                Instance = (T)(Component)this;
                Destroy(older.gameObject);
            }
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}