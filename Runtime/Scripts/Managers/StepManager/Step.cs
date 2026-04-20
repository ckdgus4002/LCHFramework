using System.Threading;
using LCHFramework.Components;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    public class Step : LCHMonoBehaviour
    {
        public CancellationTokenSource showCancellationTokenSource;
        
        
        
        protected virtual void OnDestroy() => CancellationTokenSourceUtility.ClearTokenSource(ref showCancellationTokenSource);
        
        
        
        public async Awaitable ShowAsync()
        {
            CancellationTokenSourceUtility.RestartTokenSource(ref showCancellationTokenSource);
            
            gameObject.SetActive(true);
            await StartShowAsync(showCancellationTokenSource.Token).SuppressCancellationThrow();
            if (showCancellationTokenSource == null || showCancellationTokenSource.IsCancellationRequested) return;
            
            EndShow();
        }
        
        protected virtual Awaitable StartShowAsync(CancellationToken cancellationToken) => AwaitableUtility.CompletedTask;
        
        protected virtual void EndShow() { }
        
        public virtual void Hide() => gameObject.SetActive(false);
    }
}