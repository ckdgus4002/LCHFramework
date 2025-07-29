using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Loading : MonoSingleton<Loading>
    {
        public const float DefaultFadeInTime = 0.5f;
        public const float DefaultFadeOutTime = 0.5f;
        
        
        
        public TMP_Text messageText;
        public Slider slider;
        
        
        private CanvasGroup canvasGroup;
        
        
        public virtual string DefaultLoadingMessage => LoadingMessages[Random.Range(0, LoadingMessages.Length)];

        public virtual string[] LoadingMessages => _loadingMessages ??= new[] { "Loading..." };
        private string[] _loadingMessages;
        
        
        
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
            
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        
        
        public async Awaitable Load(Func<float> getPercentOrNull, Func<bool> getIsDone)
            => await Load(() => DefaultLoadingMessage, DefaultFadeInTime, DefaultFadeOutTime, getPercentOrNull, getIsDone);
        
        public async Awaitable Load(Func<string> getMessage, float fadeInTime, float fadeOutTime, Func<float> getPercentOrNull, Func<bool> getIsDone)
        {
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
                canvasGroup.alpha = (Time.time - endTime) / fadeOutTime;
                if (Time.time - endTime <= fadeOutTime) await Awaitable.NextFrameAsync();
                else break;
            }
        }
    }
}