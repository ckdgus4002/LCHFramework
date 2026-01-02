using System;
using System.Linq;
using System.Threading;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Loading : MonoSingleton<Loading>, ILoadSceneUI
    {
        public const float DefaultFadeInDuration = 0.5f;
        public const float DefaultFadeOutDuration = 0.5f;
        public const string DefaultLoadingMessage = "";
        
        
        
        public TMP_Text messageText;
        public Slider slider;
        
        
        private CancellationTokenSource cts;
        
        
        public override bool IsShown => Wrapper.activeInHierarchy;
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public virtual string[] LoadingMessages => _loadingMessages ??= new[] { DefaultLoadingMessage };
        private string[] _loadingMessages;
        
        private CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;
        private CanvasGroup _canvasGroup;
        
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            messageText = GetComponentsInChildren<TMP_Text>().FirstOrDefault(t => t.name.Contains("Message", StringComparison.OrdinalIgnoreCase) && t.name.Contains("Text", StringComparison.OrdinalIgnoreCase));
            slider = GetComponentInChildren<Slider>();
        }
#endif
        
        
        
        public Awaitable LoadAsync(Func<float> getPercentOrNull, Func<bool> getIsDone)
            => LoadAsync(() => LoadingMessages.Pick(), DefaultFadeInDuration, DefaultFadeOutDuration, getPercentOrNull, getIsDone);
        
        public Awaitable LoadAsync(Func<string> getMessage, float fadeInDuration, float fadeOutDuration, Func<float> getPercentOrNull, Func<bool> getIsDone)
        {
            CancellationTokenSourceUtility.RestartTokenSource(ref cts);
            return Task();
            async Awaitable Task()
            {
                Wrapper.SetActive(true);
                var startTime = Time.time;
                while (true)
                {
                    CanvasGroup.alpha = (Time.time - startTime) / fadeInDuration;
                    var message = getMessage.Invoke();
                    messageText.SetActive(!string.IsNullOrEmpty(message));
                    messageText.text = message;
                    var isDone = getIsDone.Invoke();
                    slider.value = isDone ? 1 
                        : getPercentOrNull != null && startTime + fadeInDuration <= Time.time ? getPercentOrNull.Invoke() 
                        : 0;
                    if (!isDone)
                    {
                        await Awaitable.NextFrameAsync(cts.Token).SuppressCancellationThrow();
                        if (cts.IsCancellationRequested) return;
                    }
                    else
                        break;
                }
            
                var endTime = Time.time;
                while (true)
                {
                    CanvasGroup.alpha = 1 - (Time.time - endTime) / fadeOutDuration;
                    var message = getMessage.Invoke();
                    messageText.SetActive(!string.IsNullOrEmpty(message));
                    messageText.text = message;
                    slider.value = 1;
                    if (Time.time - endTime <= fadeOutDuration)
                    {
                        await Awaitable.NextFrameAsync(cts.Token).SuppressCancellationThrow();
                        if (cts.IsCancellationRequested) return;
                    }
                    else
                        break;
                }
            
                Wrapper.SetActive(false);
            }
        }
    }
}