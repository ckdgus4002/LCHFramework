using System.Threading;
using LCHFramework.Components;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    public class Step : LCHMonoBehaviour
    {
        protected CancellationTokenSource showCts;
        
        
        public override int Index => StepManager.Instance.Steps.IndexOf(this);
        
        
        
        public async Awaitable ShowAsync()
        {
            gameObject.SetActive(true);
            CancellationTokenSourceUtility.RestartTokenSource(ref showCts);
            
            await StartShowAsync();
            if (showCts.IsCancellationRequested) return;
            
            await EndShowAsync();
        }
        
        protected virtual Awaitable StartShowAsync() => AwaitableUtility.CompletedTask;

        protected virtual Awaitable EndShowAsync() => AwaitableUtility.CompletedTask;
        
        public virtual void Hide() => gameObject.SetActive(false);
    }
}