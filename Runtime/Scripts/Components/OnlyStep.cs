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
        [SerializeField] private  Step[] steps;
        [SerializeField] private  UnityEvent<int, int> onShow;
        [SerializeField] private  UnityEvent<int, int> onHide;
        
        
        private static IStepIndexManager StepIndexManager => _stepIndexManager ??= FindAnyComponentByType<IStepIndexManager>();
        private static IStepIndexManager _stepIndexManager;

        private  static ICurrentStepIndexChanged CurrentStepIndexChanged => _currentStepIndexChanged ??= FindAnyComponentByType<ICurrentStepIndexChanged>();
        private static ICurrentStepIndexChanged _currentStepIndexChanged;
        
        
        
        protected override void Start()
        {
            base.Start();
            
            CurrentStepIndexChanged.OnCurrentStepIndexChanged += OnCurrentStepIndexChanged;
        }
        
        
        
        private void OnCurrentStepIndexChanged(int prevStepIndex, int currentStepIndex)
            => OnCurrentStepIndexChanged(() => StepIndexManager.PrevStepIndex, () => StepIndexManager.CurrentStepIndex);
        
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