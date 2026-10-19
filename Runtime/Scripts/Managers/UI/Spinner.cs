using System.Linq;
using System.Threading;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class Spinner : Spinner<Spinner>
    {
    }
    
    public class Spinner<T>: MonoSingleton<T> where T : Spinner<T>
    {
        private GameObject wrapper;
        private CancellationTokenSource showCancellationTokenSource;
        
        
        public override bool IsShown => wrapper.activeInHierarchy;
        
        private GameObject SpinnerImage => _spinnerImage == null ? _spinnerImage = wrapper.transform.GetChildren().Last().gameObject : _spinnerImage;
        private GameObject _spinnerImage;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            wrapper = transform.GetChild(0).gameObject;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            CancellationTokenSourceUtility.ClearTokenSource(ref showCancellationTokenSource);
        }
        
        
        
        public virtual async Awaitable Show(float showImageDelay = 0)
        {
            CancellationTokenSourceUtility.RestartTokenSource(ref showCancellationTokenSource);
            
            wrapper.SetActive(true);
            SpinnerImage.SetActive(false);
            await Awaitable.WaitForSecondsAsync(showImageDelay, showCancellationTokenSource.Token).SuppressCancellationThrow();
            if (showCancellationTokenSource == null || showCancellationTokenSource.IsCancellationRequested) return;
            
            SpinnerImage.SetActive(true);
        }
        
        public virtual void Hide()
        {
            CancellationTokenSourceUtility.ClearTokenSource(ref showCancellationTokenSource);
            wrapper.SetActive(false);
        }
    }
}