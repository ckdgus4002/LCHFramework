using System;

namespace LCHFramework.Managers
{
    public abstract class Singleton<T> : IDisposable where T : class, new()
    {
        public static T Instance
        {
            get => _instance ??= new T();
            set
            {
                if (_instance != null && _instance != value) ((IDisposable)_instance).Dispose();
                
                _instance = value;
            }
        }
        private static T _instance;
        
        
        
        public virtual void Dispose() => Instance = null;
    }
}
