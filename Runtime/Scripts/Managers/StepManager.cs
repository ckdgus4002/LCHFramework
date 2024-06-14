using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1> where T1 : Component where T2 : Step 
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool loop;
        [SerializeField] protected T2 firstStep;
        
        
        public ReactiveProperty<T2> CurrentStep => _currentStep ??= new ReactiveProperty<T2>(onValueChanged: OnCurrentStepChanged);
        private ReactiveProperty<T2> _currentStep;
        
        public T2[] Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T2>(true).ToArray() : _steps;
        private T2[] _steps;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (playOnAwake) CurrentStep.Value = firstStep;
        }
        
        
        
        protected virtual void OnCurrentStepChanged(T2 prevStep, T2 currentStep)
        {
            foreach (var t in Steps.Where(t => t.IsShown)) t.Hide();
            currentStep.Show();
        }

        [ShowInInspector]
        public void PassCurrentStep() => CurrentStep.Value
            = CurrentStep.Value.Index < Steps.Length - 1 ? Steps[CurrentStep.Value.Index + 1] 
            : !loop ? Steps[^1] 
            : Steps[0];
    }   
}