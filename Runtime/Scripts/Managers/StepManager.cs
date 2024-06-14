using System.Collections.Generic;
using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class StepManager : StepManager<Step>
    {
    }
    
    public class StepManager<T> : MonoSingleton<StepManager<T>> where T : Step
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool loop;
        [SerializeField] private T firstStep;
        
        
        public ReactiveProperty<T> CurrentStep => _currentStep ??= new ReactiveProperty<T>(null, OnCurrentStepChanged);
        private ReactiveProperty<T> _currentStep;
        
        public IReadOnlyList<T> Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T>(true).ToArray() : _steps;
        private IReadOnlyList<T> _steps;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (playOnAwake) CurrentStep.Value = firstStep;
        }
        
        
        
        protected virtual void OnCurrentStepChanged(T prevStep, T currentStep)
        {
            foreach (var t in Steps.Where(t => t.IsShown)) t.Hide();
            currentStep.Show();
        }

        [ShowInInspector]
        public void ReturnCurrentStep() => CurrentStep.Value
            = !loop && CurrentStep.Value.Index == 0 ? Steps[0]
            : loop && CurrentStep.Value.Index == 0 ? Steps[^1]
            : Steps[CurrentStep.Value.Index - 1];
        
        [ShowInInspector]
        public void PassCurrentStep() => CurrentStep.Value
            = CurrentStep.Value.Index < Steps.Count - 1 ? Steps[CurrentStep.Value.Index + 1] 
            : !loop ? Steps[^1] 
            : Steps[0];
    }   
}