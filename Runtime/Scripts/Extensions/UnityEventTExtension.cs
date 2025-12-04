using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Extensions
{
    public static class UnityEventTExtension
    {
        public static bool Contains<T>(this UnityEvent<T> unityEvent, Object target, string methodName)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (Equals(unityEvent, i, target, methodName))
                    return true;
            
            return false;
        }
        
        public static bool Equals<T>(this UnityEvent<T> unityEvent, int persistentIndex, Object target, string methodName)
            => target == unityEvent.GetPersistentTarget(persistentIndex) && methodName == unityEvent.GetPersistentMethodName(persistentIndex);
    }
}