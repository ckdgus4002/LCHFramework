namespace LCHFramework.Managers
{
    public class Singleton<T> where T : new()
    {
        public static T Instance => _instance == null ? _instance = new T() : _instance;
        private static T _instance;
    }
}
