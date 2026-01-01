using System.Threading;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    public class DelayStep : Step
    {
        public float delay;
        
        
        private CancellationTokenSource _showCts;
        
        
        
        public override async void Show()
        {
            base.Show();
            
            CancellationTokenSourceUtility.RestartTokenSource(ref _showCts);
            await Awaitable.WaitForSecondsAsync(delay, _showCts.Token).SuppressCancellationThrow();
            if (_showCts.IsCancellationRequested) return;

            MessageBroker.Default.Publish(new PassCurrentStepMessage());
        }
    }
}
