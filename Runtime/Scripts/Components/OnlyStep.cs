using System;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class OnlyStep : LCHMonoBehaviour
    {
        private ICurrentStepIndexChanged CurrentStepIndexChanged => (ICurrentStepIndexChanged)(_currentStepIndexChanged == null ? _currentStepIndexChanged = (LCHMonoBehaviour)FindAnyComponentByType<ICurrentStepIndexChanged>() : _currentStepIndexChanged);
        [SerializeField] private LCHMonoBehaviour _currentStepIndexChanged;
        
        
        [SerializeField] private Step[] steps;
        [SerializeField] private UnityEvent<int, int> onShow;
        [SerializeField] private UnityEvent<int, int> onHide;
            
            
        
        protected override void Start()
        {
            base.Start();
            
            CurrentStepIndexChanged.OnCurrentStepIndexChanged += OnCurrentStepIndexChanged;
        }
        
        
        
        private void OnCurrentStepIndexChanged(int prevStepIndex, int currentStepIndex)
            => OnCurrentStepIndexChanged(() => CurrentStepIndexChanged.PrevStepIndex, () => CurrentStepIndexChanged.CurrentStepIndex);
        
        private void OnCurrentStepIndexChanged(Func<int> prevStepIndex, Func<int> currentStepIndex)
        {
            var isShow = steps.Any(t => t.Index == currentStepIndex.Invoke());
            
            if (isShow) Show(); else Hide();
            
            (isShow ? onShow : onHide)?.Invoke(prevStepIndex.Invoke(), currentStepIndex.Invoke());
        }

        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);
    }
}