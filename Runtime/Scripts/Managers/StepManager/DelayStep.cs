using System.Threading;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    public class DelayStep : Step
    {
        public float delay;
        
        
        
        protected override async Awaitable StartShowAsync(CancellationToken cancellationToken)
        {
            await base.StartShowAsync(cancellationToken);
            
            await Awaitable.WaitForSecondsAsync(delay, cancellationToken).SuppressCancellationThrow();
        }
        
        protected override void EndShow()
        {
            base.EndShow();
            
            MessageBroker.Default.Publish(new PassCurrentStepMessage());
        }
    }
}
