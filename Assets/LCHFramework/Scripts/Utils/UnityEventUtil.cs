using UnityEditor.Events;
using UnityEngine.Events;

namespace LCHFramework.Utils
{
    public class UnityEventUtil
    {
        public static void AddPersistentListener(UnityEvent unityEvent, UnityAction call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(unityEvent, call);
#else
            unityEvent.AddListener(call);
#endif    
        }
        
        public static void AddPersistentListener<T>(UnityEvent<T> unityEvent, UnityAction<T> call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(unityEvent, call);
#else
            unityEvent.AddListener(call);
#endif    
        }
        
        public static void RemovePersistentListener(UnityEvent unityEvent, UnityAction call)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(call);
#endif
        }
        
        public static void RemovePersistentListener<T>(UnityEvent<T> unityEvent, UnityAction<T> call)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(call);
#endif
        }
    }
}