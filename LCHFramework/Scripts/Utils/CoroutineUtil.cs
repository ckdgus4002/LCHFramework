using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Utils
{
    public static class CoroutineUtil
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