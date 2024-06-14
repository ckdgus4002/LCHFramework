using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public interface IReadOnlyStepManager<T> where T : Step
    {
        public ReactiveProperty<T> CurrentStep { get; }
        
        public T[] Steps { get; }

        public void ShowStep(T step);
        
        public void PassCurrentStep();
    }
    
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1>, IReadOnlyStepManager<T2> where T1 : Component where T2 : Step
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool loop;

        public ReactiveProperty<T2> CurrentStep => _currentStep;
        [SerializeField] private ReactiveProperty<T2> _currentStep;
        
        public T2[] Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T2>(true).ToArray() : _steps;
        private T2[] _steps;
        
        
        
        protected override void Awake()
        {
            base.Awake();

            CurrentStep.OnValueChanged += OnCurrentStepChanged;
            
            if (playOnAwake) ShowStep(CurrentStep.Value);
        }



        protected virtual void OnCurrentStepChanged(T2 prevStepOrNull, T2 currentStep) => ShowStep(currentStep);

        public void ShowStep(T2 step)
        {
            foreach (var t in Steps.Where(t => t.IsShown)) t.Hide();
            step.Show();
        }

        [ShowInInspector]
        public void PassCurrentStep() => CurrentStep.Value
            = CurrentStep.Value.Index < Steps.Length - 1 ? Steps[CurrentStep.Value.Index + 1] 
            : !loop ? Steps[^1] 
            : Steps[0];
    }   
}