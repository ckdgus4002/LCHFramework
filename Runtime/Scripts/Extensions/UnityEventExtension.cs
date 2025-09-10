using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Extensions
{
    public static class UnityEventExtension
    {
        public static bool Contains(this UnityEvent unityEvent, Object target, UnityAction method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
        
        public static bool Contains(this UnityEvent unityEvent, Object target, UnityAction<bool> method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
        
        public static bool Contains(this UnityEvent unityEvent, Object target, UnityAction<float> method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
        
        public static bool Contains(this UnityEvent unityEvent, Object target, UnityAction<int> method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
        
        public static bool Contains<T>(this UnityEvent unityEvent, Object target, UnityAction<T> method) where T : Object
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
        
        public static bool Contains(this UnityEvent unityEvent, Object target, UnityAction<string> method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
    }
}