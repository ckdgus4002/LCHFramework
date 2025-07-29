using System;

namespace LCHFramework.Managers
{
    public interface ISingleton
    {
        public bool IsDestroyPrevInstance { get; }
    }
    
    public static class Singleton
    {
        public static void EnsureInstance<T>(T value, T prevInstanceOrNull, Action<T> setInstance, Action<T> disposeInstance) where T: class, ISingleton
        {
            var valueIsNull = value is null;
            setInstance.Invoke(prevInstanceOrNull is not null && !valueIsNull && !value.IsDestroyPrevInstance
                ? prevInstanceOrNull
                : value);
            if (prevInstanceOrNull is not null && prevInstanceOrNull != value)
                disposeInstance.Invoke(valueIsNull || value.IsDestroyPrevInstance ? prevInstanceOrNull : value);
        }
    }
    
    public class Singleton<T> : IDisposable, ISingleton where T : Singleton<T>
    {
        public static bool InstanceIsNull => Instance is null;
        
        public static T Instance
        {
            get => _instance ?? (Instance = Activator.CreateInstance<T>());
            private set => Singleton.EnsureInstance(value, _instance, t => _instance = t, t => t.Dispose());
        }
        private static T _instance;
        
        
        
        public virtual bool IsDestroyPrevInstance => true;
        
        
        
        public virtual void Dispose()
        {
            if (Instance == this)
            {
                Instance = null;
            }   
        }
    }
}
