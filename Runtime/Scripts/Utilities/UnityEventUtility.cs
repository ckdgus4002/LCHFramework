using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace LCHFramework.Utilities
{
    public static class UnityEventUtility
    {
        public static void RemovePersistentListener(UnityEvent unityEvent, UnityAction call)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(call);
#endif    
        }
        
        public static void RemovePersistentListener<T>(UnityEvent unityEvent, UnityAction<T> call, T argument)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(() => call(argument));
#endif
        }
        
        
        public static void AddPersistentListener(UnityEvent unityEvent, UnityAction call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(unityEvent, call);
#else
            unityEvent.AddListener(call);
#endif    
        }
        
        public static void AddBoolPersistentListener(UnityEvent unityEvent, UnityAction<bool> call, bool argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddBoolPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(() => call(argument));
#endif    
        }
        
        public static void AddFloatPersistentListener(UnityEvent unityEvent, UnityAction<float> call, float argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddFloatPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(() => call(argument));
#endif    
        }
        
        public static void AddIntPersistentListener(UnityEvent unityEvent, UnityAction<int> call, int argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddIntPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(() => call(argument));
#endif    
        }
        
        public static void AddObjectPersistentListener<T>(UnityEvent unityEvent, UnityAction<T> call, T argument) where T : Object
        {
#if UNITY_EDITOR
            UnityEventTools.AddObjectPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(() => call(argument));
#endif    
        }
        
        public static void AddStringPersistentListener(UnityEvent unityEvent, UnityAction<string> call, string argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddStringPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(() => call(argument));
#endif    
        }
    }
}