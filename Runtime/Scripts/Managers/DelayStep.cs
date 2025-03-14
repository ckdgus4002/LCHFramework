using System.Threading;
using LCHFramework.Packages.LCHFramework.Runtime.Scripts.Extensions;
using LCHFramework.Utilities;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class DelayStep : Step
    {
        [SerializeField] private float delay;
        
        
        private float _defaultDelay;
        private CancellationTokenSource _showCts;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            _defaultDelay = delay;
        }
        
        
        
        public override async void Show()
        {
            base.Show();
            
            CancellationTokenSourceUtility.ClearTokenSource(ref _showCts);
            delay = _defaultDelay;
            await Awaitable.WaitForSecondsAsync(delay, (_showCts = new CancellationTokenSource()).Token).Forget();
            if (_showCts == null || _showCts.IsCancellationRequested) return;

            MessageBroker.Default.Publish(new PassCurrentStepMessage());
        }
    }
}
