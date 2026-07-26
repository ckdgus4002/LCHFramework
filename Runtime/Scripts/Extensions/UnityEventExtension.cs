using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Extensions
{
    public static class UnityEventExtension
    {
        public static bool Contains(this UnityEvent unityEvent, Object target, string methodName)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (unityEvent.Equals(i, target, methodName))
                    return true;
            
            return false;
        }
        
        public static bool Equals(this UnityEvent unityEvent, int persistentIndex, Object target, string methodName)
            => persistentIndex < unityEvent.GetPersistentEventCount() && target == unityEvent.GetPersistentTarget(persistentIndex) && methodName == unityEvent.GetPersistentMethodName(persistentIndex);
    }
}