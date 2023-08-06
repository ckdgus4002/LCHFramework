#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine.Events;

namespace LCHFramework.Utils
{
    public static partial class UnityEventUtil
    {
        public static void AddPersistentListener<T0>(UnityEvent<T0> unityEvent, UnityAction<T0> call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(unityEvent, call);
#else
            unityEvent.AddListener(call);
#endif    
        }
        
        public static void RemovePersistentListener<T0>(UnityEvent<T0> unityEvent, UnityAction<T0> call)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(call);
#endif
        }
    }
}