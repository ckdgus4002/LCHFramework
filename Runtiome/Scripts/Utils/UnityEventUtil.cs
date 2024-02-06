using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace LCHFramework.Utils
{
    public static partial class UnityEventUtil
    {
        public static void AddPersistentListener(UnityEvent unityEvent, UnityAction call)
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
    }
}