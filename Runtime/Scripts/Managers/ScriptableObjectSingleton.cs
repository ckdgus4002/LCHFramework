using UnityEngine;

namespace LCHFramework.Managers
{
#if UNITY_EDITOR
    public class ScriptableSingleton<T> : UnityEditor.ScriptableSingleton<T>
#else
    public class ScriptableSingleton<T> : ScriptableObject 
#endif
        where T : ScriptableObject
    {
#if !UNITY_EDITOR
        private static T s_Instance;
        
        public ScriptableSingleton()
        {
            if (s_Instance != null && s_Instance != this) Destroy(s_Instance);

            s_Instance = (object)this as T;
        }

        public static T instance
        {
            get => s_Instance;
            set => s_Instance = value;
        }
#endif
    }
}
