using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class OnlyStep : LCHMonoBehaviour
    {
        [SerializeField] private Object _currentStepIndexChangedOrNull;
        [SerializeField] private Step[] steps;
        [SerializeField] private UnityEvent<int, int> onShow;
        [SerializeField] private UnityEvent<int, int> onHide;
        
        
        private ICurrentStepIndexChanged CurrentStepIndexChanged => (ICurrentStepIndexChanged)(_currentStepIndexChangedOrNull == null ? _currentStepIndexChangedOrNull = (Object)FindAnyInterfaceByType<ICurrentStepIndexChanged>() : _currentStepIndexChangedOrNull);


        private void OnValidate()
        {
            _currentStepIndexChangedOrNull = (_currentStepIndexChangedOrNull is GameObject result && result.TryGetComponent<ICurrentStepIndexChanged>(out _)) || _currentStepIndexChangedOrNull is ICurrentStepIndexChanged ? _currentStepIndexChangedOrNull : null;
        }

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