using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Extensions
{
    public static partial class UnityEventExtension
    {
        public static bool Contains<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent, Object target, UnityAction<T0, T1, T2> method)
        {
            for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                if (target == unityEvent.GetPersistentTarget(i) && method.Method.Name == unityEvent.GetPersistentMethodName(i))
                    return true;
                
            return false;
        }
    }
}