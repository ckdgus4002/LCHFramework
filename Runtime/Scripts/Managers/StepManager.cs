using System;
using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Managers
{
    public delegate void OnCurrentStepIndexChangedDelegate(int prev, int current);
    
    public interface ICurrentStepIndexChanged
    {
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
    }
    
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1>, IStepManager<T2>, IPassCurrentStep, ICurrentStepIndexChanged where T1 : Component where T2 : Step
    {
        [SerializeField] private bool loop;
        [SerializeField] private T2 playOnStartOrNull;
        
        
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
        
        
        public T2 PrevStepOrNull { get; private set; }
        
        public T2 LeftStepOrNull { get; private set; }
        
        public T2 RightStepOrNull { get; private set; }


        public T2 CurrentStep
        {
            get
            {
                if (_currentStep == null) CurrentStep = Steps[0];

                return _currentStep;    
            }
            set
            {
                if (_currentStep == value) return;
                
                PrevStepOrNull = _currentStep;
                _currentStep = value;
                LeftStepOrNull = 0 < _currentStep.Index ? Steps[_currentStep.Index - 1] : loop ? Steps[^1] : null;
                RightStepOrNull = _currentStep.Index < Steps.Length - 1 ? Steps[_currentStep.Index + 1] : loop ? Steps[0] : null;
                
                foreach (var t in Steps.Where(t => t.IsShown)) t.Hide();
                _currentStep.Show();
                
                OnCurrentStepIndexChanged?.Invoke(PrevStepOrNull == null ? -1 : PrevStepOrNull.Index, _currentStep.Index);
            }
        }
        private T2 _currentStep;

        public T2 FirstStep => Steps[0];
        
        public T2 LastStep => Steps[^1];

        public T2[] Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T2>(true).OrderBy(t => t.Index).ToArray() : _steps;
        private T2[] _steps;
        
        
        
        protected override void Start()
        {
            base.Start();

            if (playOnStartOrNull != null)
            {
                CurrentStep = playOnStartOrNull;
            }
        }
        
        
        
        [ShowInInspector]
        public void PassCurrentStep()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
                throw new Exception("Can't execute when edit mode in editor. because cache variables is cached.");
#endif
            if (RightStepOrNull == null)
                Debug.LogError("Step is end.");
            else
                CurrentStep = RightStepOrNull;
        }
    }   
}