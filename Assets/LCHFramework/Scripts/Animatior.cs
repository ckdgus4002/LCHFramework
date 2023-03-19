using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework
{
    public static class Animatior
    {
        public static Coroutine Play<T>(MonoBehaviour monoBehaviour, IEnumerable<(float, T)> animations, Action<int, T> action, bool loop = false)
            => monoBehaviour.StartCoroutine(PlayCor(animations, action, loop));
        
        public static IEnumerator PlayCor<T>(IEnumerable<(float, T)> animations, Action<int, T> action, bool loop = false)
        {
            do
            {
                var copiedAnimations = animations.ToList();
                var elapsed = 0f;
                while (!copiedAnimations.IsEmpty())
                {
                    foreach (var elapsedAnimation in copiedAnimations.Where(item => item.Item1 <= elapsed).ToList())
                    {
                        action?.Invoke(animations.IndexOf(elapsedAnimation), elapsedAnimation.Item2);
                        copiedAnimations.Remove(elapsedAnimation);
                    }
                
                    yield return null;
                    elapsed += Time.deltaTime;
                }
                
            } while (loop);
        }
    }
}