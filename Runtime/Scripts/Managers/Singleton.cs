namespace LCHFramework.Managers
{
    public abstract class Singleton<T> where T : new()
    {
        public static T Instance { get; protected set; }
        
        
        
        /// <remarks>
        /// ex. Instance = new T();
        /// </remarks>>
        protected abstract void InitializeInstance();
    }
}
