using UnityEngine;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Spinner: MonoSingleton<Spinner>
    {
        private CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;
        private CanvasGroup _canvasGroup;
        
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        private GameObject SpinnerImage => _spinnerImage == null ? _spinnerImage = Wrapper.transform.GetChild(0).gameObject : _spinnerImage;
        private GameObject _spinnerImage; 
        
        
        
        public virtual async Awaitable Show(float fadeInDelay = 0, float fadeInDuration = 0)
        {
            Wrapper.SetActive(true);
            SpinnerImage.SetActive(false);
            await Awaitable.WaitForSecondsAsync(fadeInDelay);
            
            SpinnerImage.SetActive(true);
            var startTime = Time.time;
            while (true)
            {
                CanvasGroup.alpha = (Time.time - startTime) / fadeInDuration;
                if (Time.time < startTime + fadeInDuration) await Awaitable.NextFrameAsync();
                else break;
            }
        }
        
        public virtual async Awaitable Hide(float fadeOutDuration = 0)
        {
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