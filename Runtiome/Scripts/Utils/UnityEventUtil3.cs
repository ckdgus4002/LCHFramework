using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace LCHFramework.Utils
{
    public static partial class UnityEventUtil
    {
        public static void AddPersistentListener<T0, T1, T2>(UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(unityEvent, call);
#else
            unityEvent.AddListener(call);
#endif    
        }
        
        public static void RemovePersistentListener<T0, T1, T2>(UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(call);
#endif
        }
    }
}