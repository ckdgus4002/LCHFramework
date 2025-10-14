using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UniRx;
using UnityEngine;

namespace LCHFramework
{
    public class LCHFramework : MonoSingleton<LCHFramework>
    {
#if UNITY_EDITOR
        public const string MenuItemRootPath = "Tools" + "/" + nameof(LCHFramework);
#endif
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod() => CreateGameObjectIfInstanceIsNull();
        
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
        
        
        
        public Vector2 targetScreenResolution = new(1920, 1920);
        
        
        
        public Vector2 PrevScreenSize { get; private set; }
        
        public float PrevMainCameraAspect { get; private set; }
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        
        
        private void Update()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            if (screenSize != PrevScreenSize) MessageBroker.Default.Publish(new ScreenSizeChangedMessage());
            PrevScreenSize = screenSize;

            if (Camera.main == null) return;
            var mainCameraAspect = Camera.main.aspect;
            if (!Mathf.Approximately(mainCameraAspect, PrevMainCameraAspect)) MessageBroker.Default.Publish(new MainCameraAspectChangedMessage());
            PrevMainCameraAspect = mainCameraAspect;
        }
    }
}