using System;
using LCHFramework.Extensions;
using TMPro;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class Toast : Toast<Toast>
    {
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    public class Toast<T> : MonoSingleton<T> where T : Toast<T>
    {
        public const float DefaultFadeInDuration = 0.5f;
        public const float DefaultFadeOutDuration = 0.5f;
        
        
        
        private GameObject wrapper;
        
        
        public override bool IsShown => wrapper.activeInHierarchy;
        
        private CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;
        private CanvasGroup _canvasGroup;
        
        private TMP_Text[] Texts => _texts ??= GetComponentsInChildren<TMP_Text>(true);
        [NonSerialized] private TMP_Text[] _texts;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            wrapper = transform.GetChild(0).gameObject;
        }
        
        
        
        public virtual async Awaitable Show(string message, float fadeInDuration = DefaultFadeInDuration, float fadeOutDuration = DefaultFadeOutDuration)
        {
            wrapper.SetActive(true);
            Texts.ForEach(text => text.text = message);
            var startTime = Time.time;
            fadeInDuration = Mathf.Max(fadeInDuration, float.Epsilon);
            while (true)
            {
                CanvasGroup.alpha = (Time.time - startTime) / fadeInDuration;
                if (Time.time < startTime + fadeInDuration)
                {
                    await Awaitable.NextFrameAsync(destroyCancellationToken).SuppressCancellationThrow();
                    if (destroyCancellationToken.IsCancellationRequested) return;
                }
                else
                    break;
            }
            
            var endTime = Time.time;
            fadeOutDuration = Mathf.Max(fadeOutDuration, float.Epsilon);
            while (true)
            {
                CanvasGroup.alpha = 1 - (Time.time - endTime) / fadeOutDuration;
                if (Time.time - endTime <= fadeOutDuration)
                {
                    await Awaitable.NextFrameAsync(destroyCancellationToken).SuppressCancellationThrow();
                    if (destroyCancellationToken.IsCancellationRequested) return;
                }
                else 
                    break;
            }
            
            wrapper.SetActive(false);
        }
    }
}