using System.Collections.Generic;
using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class StepManager : MonoSingleton<StepManager>
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool loop;
        [SerializeField] private Step firstStep;
        
        
        public ReactiveProperty<Step> CurrentStep => _currentSequence ??= new ReactiveProperty<Step>(null, OnCurrentSequenceChanged);
        private ReactiveProperty<Step> _currentSequence;
        
        public IReadOnlyList<Step> Steps => _sequences.IsEmpty() ? _sequences = GetComponentsInChildren<Step>(true).ToArray() : _sequences;
        private IReadOnlyList<Step> _sequences;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (playOnAwake) CurrentStep.Value = firstStep;
        }
        
        
        
        protected virtual void OnCurrentSequenceChanged(Step prevStep, Step currentStep)
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