using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Extensions
{
    public static partial class UnityEventExtension
    {
        public static bool Contains(this UnityEvent unityEvent, Object target, UnityAction method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
    }
}