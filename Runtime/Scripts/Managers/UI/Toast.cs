using TMPro;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Toast : MonoSingleton<Toast>
    {
        public const float DefaultFadeInDuration = 0.5f;
        public const float DefaultFadeOutDuration = 0.5f;
        
        
        
        private CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;
        private CanvasGroup _canvasGroup;
        
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        private TMP_Text Text => _text == null ? _text = GetComponentInChildren<TMP_Text>(true) : _text;
        private TMP_Text _text;
        
        
        
        public virtual async Awaitable Show(string message, float fadeInDuration = DefaultFadeInDuration, float fadeOutDuration = DefaultFadeOutDuration)
        {
            Wrapper.SetActive(true);
            Text.text = message;
            var startTime = Time.time;
            while (true)
            {
                CanvasGroup.alpha = (Time.time - startTime) / fadeInDuration;
                if (Time.time < startTime + fadeInDuration) await Awaitable.NextFrameAsync();
                else break;
            }
            
            var endTime = Time.time;
            while (true)
            {
                CanvasGroup.alpha = 1 - (Time.time - endTime) / fadeOutDuration;
                if (Time.time - endTime <= fadeOutDuration) await Awaitable.NextFrameAsync();
                else break;
            }
            
            Wrapper.SetActive(false);
        }
    }
}