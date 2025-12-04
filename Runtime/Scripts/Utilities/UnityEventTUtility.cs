using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace LCHFramework.Utilities
{
    public static class UnityEventUtility<T>
    {
        public static void RemoveListener(UnityEvent<T> unityEvent, UnityAction<T> call)
        {
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#else
            unityEvent.RemoveListener(call);
#endif    
        }
        
        
        
        public static void AddListener(UnityEvent<T> unityEvent, UnityAction<T> call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(unityEvent, call);
#else
            unityEvent.AddListener(call);
#endif    
        }
        
        public static void AddBoolListener(UnityEvent<T> unityEvent, UnityAction<bool> call, bool argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddBoolPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(_ => call.Invoke(argument));
#endif    
        }
        
        public static void AddFloatListener(UnityEvent<T> unityEvent, UnityAction<float> call, float argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddFloatPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(_ => call.Invoke(argument));
#endif    
        }
        
        public static void AddIntListener(UnityEvent<T> unityEvent, UnityAction<int> call, int argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddIntPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(_ => call.Invoke(argument));
#endif    
        }
        
        public static void AddObjectListener<T2>(UnityEvent<T> unityEvent, UnityAction<T2> call, T2 argument) where T2 : Object
        {
#if UNITY_EDITOR
            UnityEventTools.AddObjectPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(_ => call.Invoke(argument));
#endif    
        }
        
        public static void AddStringListener(UnityEvent<T> unityEvent, UnityAction<string> call, string argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddStringPersistentListener(unityEvent, call, argument);
#else
            unityEvent.AddListener(_ => call.Invoke(argument));
#endif    
        }
    }
}