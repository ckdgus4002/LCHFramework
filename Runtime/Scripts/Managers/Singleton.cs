using System;

namespace LCHFramework.Managers
{
    public class Singleton<T> : IDisposable where T : class
    {
        protected Singleton()
        {
            Instance = (object)this as T;
        }
        
        
        
        public bool InstanceIsNull => Instance == null;
        
        public static T Instance
        {
            get => _instance;
            private set
            {
                var prevInstance = _instance;
                _instance = value;
                
                if (prevInstance != null && _instance != null && prevInstance != _instance) ((IDisposable)prevInstance).Dispose();
            }
        }
        private static T _instance;



        public virtual void Dispose()
        {
            if (Instance == this) Instance = null;   
        }
    }
}
