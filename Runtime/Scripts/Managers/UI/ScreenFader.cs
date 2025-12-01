using System;
using System.Threading;
using LCHFramework.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenFader : MonoSingleton<ScreenFader>, ILoadSceneUI
    {
        public const float DefaultFadeInDuration = 0.5f;
        public const float DefaultFadeOutDuration = 0.5f;
        public const string DefaultFadeMessage = "";
        
        
        
        public TMP_Text messageText;
        
        
        protected CancellationTokenSource cts;
        
        
        public override bool IsShown => Wrapper.activeInHierarchy;
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public virtual string[] LoadingMessages => _loadingMessages ??= new[] { DefaultFadeMessage };
        private string[] _loadingMessages;
        
        protected CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;
        private CanvasGroup _canvasGroup;
        
        protected GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        protected Image Image => _image == null ? _image = GetComponentInChildren<Image>(true) : _image;
        private Image _image;
        
        
        
        public Awaitable LoadAsync(Func<string> getMessage, float fadeInDuration, float fadeOutDuration, Func<float> getPercentOrNull, Func<bool> getIsDone)
            => FadeAsync(Color.black, getMessage, fadeInDuration, 0, fadeOutDuration, 0, getIsDone);
        
        public Awaitable FadeAsync(Color color, float fadeInDuration, float fadeOutDuration)
            => FadeAsync(color, fadeInDuration, 0, fadeOutDuration, 0);
        
        public Awaitable FadeAsync(Color color, float fadeInDuration, int fadeInEase, float fadeOutDuration, int fadeOutEase)
        {
            var startTime = Time.time;
            return FadeAsync(color, () => LoadingMessages.Pick(), fadeInDuration, fadeInEase, fadeOutDuration, fadeOutEase, () => startTime + fadeInDuration + fadeOutDuration <= Time.time);
        }
        
        public abstract Awaitable FadeAsync(Color color, Func<string> getMessage, float fadeInDuration, int fadeInEase, float fadeOutDuration, int fadeOutEase, Func<bool> getIsDone);
    }
}