using System;
using System.Linq;
using LCHFramework.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenFader : MonoSingleton<ScreenFader>, ILoadSceneUI
    {
        public const float DefaultFadeInTime = 0.5f;
        public const float DefaultFadeOutTime = 0.5f;
        public const string DefaultFadeMessage = "";
        
        
        
        public TMP_Text messageText;
        
        
        public override bool IsShown => Wrapper.activeSelf;
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public virtual string[] LoadingMessages => _loadingMessages ??= new[] { DefaultFadeMessage };
        private string[] _loadingMessages;
        
        protected CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;
        private CanvasGroup _canvasGroup;
        
        protected GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        protected Image Image => _image == null ? _image = GetComponentInChildren<Image>(true) : _image;
        private Image _image;
        
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            messageText = GetComponentsInChildren<TMP_Text>().FirstOrDefault(t => t.name.Contains("Message", StringComparison.OrdinalIgnoreCase) && t.name.Contains("Text", StringComparison.OrdinalIgnoreCase));
        }
#endif  
        
        
        public Awaitable OnLoadSceneAsync(Func<string> getMessage, float fadeInDuration, float fadeOutDuration, Func<float> getPercentOrNull, Func<bool> getIsDone)
            => FadeAsync(Color.black, getMessage, fadeInDuration, 0, fadeOutDuration, 0, getIsDone);

        public async Awaitable FadeAsync(Color color, float fadeInDuration, float fadeOutDuration)
        {
            var startTime = Time.time;
            await FadeAsync(color, () => LoadingMessages.Pick(), fadeInDuration, 0, fadeOutDuration, 0, () => startTime + fadeInDuration + fadeOutDuration <= Time.time);
        }
        
        public abstract Awaitable FadeAsync(Color color, Func<string> getMessage, float fadeInDuration, int fadeInEase, float fadeOutDuration, int fadeOutEase, Func<bool> getIsDone);
    }
}