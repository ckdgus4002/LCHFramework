using System;
using System.Threading;
using System.Threading.Tasks;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class DelayStep : Step
    {
        [SerializeField] private float delay;
        
        
        private float _defaultDelay;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            _defaultDelay = delay;
        }
        
        
        
        public override async void Show()
        {
            base.Show();
            
            CancellationTokenSourceUtility.ClearTokenSources(_ctses);
            
            delay = _defaultDelay;
            
            await Task.Delay(TimeSpan.FromSeconds(delay), _ctses.AddAndReturn(new CancellationTokenSource()).Token).SuppressCancellationThrow();

            PassCurrentStep.PassCurrentStep();
        }
    }
}
