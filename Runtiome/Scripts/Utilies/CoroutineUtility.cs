using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Utility
{
    public static class CoroutineUtility
    {
        public static Coroutine RestartCoroutine(MonoBehaviour monoBehaviour, Coroutine stopCoroutine, IEnumerator startCoroutine)
        {
            if (stopCoroutine != null) monoBehaviour.StopCoroutine(stopCoroutine);

            return monoBehaviour.StartCoroutine(startCoroutine);
        }
        
        public static Coroutine RestartCoroutine(MonoBehaviour monoBehaviour, IEnumerable<Coroutine> stopCoroutines, IEnumerator startCoroutine)
        {
            foreach (var stopCoroutine in stopCoroutines) if (stopCoroutine != null) monoBehaviour.StopCoroutine(stopCoroutine);
            
            return monoBehaviour.StartCoroutine(startCoroutine);
        }
    }
}