using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UniRx;
using UnityEngine;

namespace LCHFramework
{
    public struct OnScreenSizeChangedMessage
    {
        public Vector2 prev;
        public Vector2 curret;
    }
    
    public struct OnMainCameraAspectChangedMessage
    {
        public float prev;
        public float curret;
    }
    
    public class LCHFramework : MonoSingleton<LCHFramework>
    {
#if UNITY_EDITOR
        public const string MenuItemRootPath = "Tools" + "/" + nameof(LCHFramework);
#endif
        
        
        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitializeOnLoadMethod()
        {
            var lchFrameworkOrNull = Resources.Load<LCHFramework>(nameof(LCHFramework));
            if (lchFrameworkOrNull == null) CreateGameObjectIfInstanceIsNull();
            else InstantiateIfInstanceIsNull(() => lchFrameworkOrNull);
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
        
        
        
        public Vector2 targetScreenResolution = new(1920, 1920);
        public bool isPreferredLandOrientation;
        
        
        
        public Vector2 PrevScreenSize { get; private set; }
        
        public float PrevMainCameraAspect { get; private set; }
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        
        
        private void Update()
        {
            var screenSize = Screen.Size;
            if (screenSize != PrevScreenSize) MessageBroker.Default.Publish(new OnScreenSizeChangedMessage { prev = PrevScreenSize, curret = screenSize });
            PrevScreenSize = screenSize;
            
            if (Camera.main == null) return;
            var mainCameraAspect = Camera.main.aspect;
            if (!Mathf.Approximately(mainCameraAspect, PrevMainCameraAspect)) MessageBroker.Default.Publish(new OnMainCameraAspectChangedMessage { prev = PrevMainCameraAspect, curret = mainCameraAspect });
            PrevMainCameraAspect = mainCameraAspect;
        }
    }
}