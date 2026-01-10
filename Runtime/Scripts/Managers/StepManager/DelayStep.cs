using LCHFramework.Extensions;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    public class DelayStep : Step
    {
        public float delay;
        
        
        
        protected override async Awaitable StartShowAsync()
        {
            await base.StartShowAsync();
            
            await Awaitable.WaitForSecondsAsync(delay, showCts.Token).SuppressCancellationThrow();
        }
        
        protected override async Awaitable EndShowAsync()
        {
            await base.EndShowAsync();
            
            MessageBroker.Default.Publish(new PassCurrentStepMessage());
        }
    }
}
