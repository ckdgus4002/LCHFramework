using System;

namespace LCHFramework.Managers
{
    public class Singleton<T> : ISingleton, IDisposable where T : Singleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = Activator.CreateInstance<T>());
            private set => Singleton.EnsureInstance(value, _instance, t => _instance = t, t => t.Dispose());
        }
        private static T _instance;
        
        
        
        public virtual bool IsDestroyPrevInstance => true;
        
        public bool IsDestroyed { get; private set; }
        
        
        
        public virtual void Dispose()
        {
            IsDestroyed = true;
            if (Instance == this)
            {
                Instance = null;
            }   
        }
    }
}
