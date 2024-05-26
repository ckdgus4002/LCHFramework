using UnityEngine;

namespace LCHFramework.Managers
{
    public abstract class ScriptableSingleton<T> :
#if UNITY_EDITOR
        UnityEditor.ScriptableSingleton<T> where T : ScriptableObject 
#else
        ScriptableObject
#endif
    {
#if !UNITY_EDITOR
        public static T Instance { get; protected set; }
        
        
        
        protected abstract void InitializeForInstance();
#endif
    }
}
