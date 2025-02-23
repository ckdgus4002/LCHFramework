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
            
            CancellationTokenSourceUtility.ClearTokenSources(ctses);
            
            delay = _defaultDelay;
            
            await Awaitable.WaitForSecondsAsync(delay);

            PassCurrentStep.PassCurrentStep();
        }
    }
}
