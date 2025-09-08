using System;
using System.Linq;
using LCHFramework.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Managers.UI
{
    public class Loading : MonoSingleton<Loading>
    {
        public const float DefaultFadeInTime = 0.5f;
        public const float DefaultFadeOutTime = 0.5f;
        public const string DefaultLoadingMessage = "Loading...";
        
        
        
        public TMP_Text messageText;
        public Slider slider;
        
        
        private CanvasGroup canvasGroup;
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public override bool IsShown => Wrapper.activeSelf;
        
        public virtual string[] LoadingMessages => _loadingMessages ??= new[] { DefaultLoadingMessage };
        private string[] _loadingMessages;
        
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.Find(nameof(Wrapper)).gameObject : _wrapper;
        private GameObject _wrapper;
        
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            messageText = GetComponentsInChildren<TMP_Text>().FirstOrDefault(t => t.name.Contains("Message", StringComparison.OrdinalIgnoreCase) && t.name.Contains("Text", StringComparison.OrdinalIgnoreCase));
            slider = GetComponentInChildren<Slider>();
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            
            if (Instance != this) return;
            
            canvasGroup = GetComponentInChildren<CanvasGroup>();
            Hide();
        }
        
        
        
        public async Awaitable LoadAsync(Func<float> getPercentOrNull, Func<bool> getIsDone)
            => await LoadAsync(() => LoadingMessages.Pick(), DefaultFadeInTime, DefaultFadeOutTime, getPercentOrNull, getIsDone);
        
        public async Awaitable LoadAsync(Func<string> getMessage, float fadeInTime, float fadeOutTime, Func<float> getPercentOrNull, Func<bool> getIsDone)
        {
            Wrapper.SetActive(true);
            
            var startTime = Time.time;
            while (true)
            {
                canvasGroup.alpha = (Time.time - startTime) / fadeInTime;
                messageText.text = getMessage.Invoke();
                slider.gameObject.SetActive(getPercentOrNull != null);
                var isDone = getIsDone.Invoke();
                slider.value = isDone ? 1 
                    : getPercentOrNull != null && startTime + fadeInTime <= Time.time ? getPercentOrNull.Invoke() 
                    : 0;
                if (!isDone) await Awaitable.NextFrameAsync();
                else break;
            }

            var endTime = Time.time;
            while (true)
            {
                canvasGroup.alpha = 1 - (Time.time - endTime) / fadeOutTime;
                if (Time.time - endTime <= fadeOutTime) await Awaitable.NextFrameAsync();
                else break;
            }
            
            Hide();
        }
        
        private void Hide() => Wrapper.SetActive(false);
    }
}