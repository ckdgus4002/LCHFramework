using System.Threading;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class Spinner: MonoSingleton<Spinner>
    {
        private CancellationTokenSource showCancellationTokenSource;
        
        
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        private GameObject SpinnerImage => _spinnerImage == null ? _spinnerImage = Wrapper.transform.GetChild(0).gameObject : _spinnerImage;
        private GameObject _spinnerImage;
        
        
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            CancellationTokenSourceUtility.ClearTokenSource(ref showCancellationTokenSource);
        }
        
        
        
        public virtual async Awaitable Show(float showImageDelay = 0)
        {
            CancellationTokenSourceUtility.RestartTokenSource(ref showCancellationTokenSource);
            
            Wrapper.SetActive(true);
            SpinnerImage.SetActive(false);
            await Awaitable.WaitForSecondsAsync(showImageDelay, showCancellationTokenSource.Token).SuppressCancellationThrow();
            if (showCancellationTokenSource == null || showCancellationTokenSource.IsCancellationRequested) return;
            
            SpinnerImage.SetActive(true);
        }
        
        public virtual void Hide()
        {
            CancellationTokenSourceUtility.ClearTokenSource(ref showCancellationTokenSource);
            Wrapper.SetActive(false);
        }
    }
}