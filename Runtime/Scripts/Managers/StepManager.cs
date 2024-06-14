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
        
        
        public ReactiveProperty<T> CurrentStep => _currentSequence ??= new ReactiveProperty<T>(null, OnCurrentSequenceChanged);
        private ReactiveProperty<T> _currentSequence;
        
        public IReadOnlyList<T> Steps => _sequences.IsEmpty() ? _sequences = GetComponentsInChildren<T>(true).ToArray() : _sequences;
        private IReadOnlyList<T> _sequences;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (playOnAwake) CurrentStep.Value = firstStep;
        }
        
        
        
        protected virtual void OnCurrentSequenceChanged(T prevStep, T currentStep)
        {
            foreach (var t in Steps.Where(t => t.IsShown)) t.Hide();
            currentStep.Show();
        }

        [ShowInInspector]
        public void ReturnCurrentSequence() => CurrentStep.Value
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