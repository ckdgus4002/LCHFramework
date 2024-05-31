using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using LCHFramework.Utilies;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace LCHFramework
{
    public class LCHFramework : MonoSingleton<LCHFramework>
    {
#if UNITY_EDITOR
        public const string MenuItemRootPath = "DevTool/LCHFramework";
#endif
        public const bool IsSupported = true;
        
        
        
        public static Coroutine Delay(float seconds, Action callback) => Delay(Instance, seconds, callback);
        
        public static Coroutine Delay(MonoBehaviour monoBehaviour, float seconds, Action callback)
        {
            return monoBehaviour.StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                var now = DateTime.Now;
                yield return new WaitWhile(() => now.AddSeconds(seconds) < DateTime.Now);
                
                callback?.Invoke();
            }
        }

        public static Coroutine DelayEndOfFrame(Action callback) => DelayEndOfFrame(Instance, callback);
        
        public static Coroutine DelayEndOfFrame(MonoBehaviour monoBehaviour, Action callback)
        {
            return monoBehaviour.StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                yield return new WaitForEndOfFrame();
                
                callback?.Invoke();
            }
        }

        public static Coroutine DelayFrame(int frameCount, Action callback) => DelayFrame(Instance, frameCount, callback);
        
        public static Coroutine DelayFrame(MonoBehaviour monoBehaviour, int frameCount, Action callback)
        {
            return monoBehaviour.StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                var start = Time.frameCount;
                yield return new WaitWhile(() => start + frameCount < Time.frameCount);
                
                callback?.Invoke();
            }
        }

        public static Coroutine PlayAnimations<T>(IEnumerable<(float, T)> animations, Action<int, T> action, bool loop = false) => PlayAnimations(Instance, animations, action, loop);
        
        public static Coroutine PlayAnimations<T>(MonoBehaviour monoBehaviour, IEnumerable<(float, T)> animations, Action<int, T> action, bool loop = false)
        {
            return monoBehaviour.StartCoroutine(Coroutine());
            IEnumerator Coroutine()
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
        
        
        
        public Vector2 targetScreenResolution = new(2160, 2160);
        
        
        public Vector2 PrevScreenSize { get; private set; }
        
        public float PrevMainCameraAspect { get; private set; }
        
        
        
        private void Update()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            if (ScreenUtility.IsSizeChanged(PrevScreenSize))
                foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                    foreach (var item in rootGameObject.GetComponentsInChildren<IScreenSizeChanged>())
                        item.OnScreenSizeChanged(PrevScreenSize, screenSize);
            PrevScreenSize = screenSize;


            var mainCameraAspect = Camera.main.aspect;
            if (CameraUtility.IsAspectChanged(Camera.main, PrevMainCameraAspect))
                foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                    foreach (var item in rootGameObject.GetComponentsInChildren<IMainCameraAspectChanged>())
                        item.OnMainCameraAspectChanged(PrevMainCameraAspect, mainCameraAspect);
            PrevMainCameraAspect = mainCameraAspect;
        }
    }
}