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
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1>, IStepManager<T2>, ICurrentStepIndexChanged, IPassCurrentStep where T1 : Component where T2 : Step
    {
        [SerializeField] private bool loop;
        public PlayOnStart playOnStart = new() { delayFrame = 1 };
        
        
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
        
        
        public T2 PrevStepOrNull { get; private set; }
        
        public T2 LeftStepOrNull { get; private set; }
        
        public T2 RightStepOrNull { get; private set; }
        
        
        public int CurrentStepIndex => CurrentStep.Index;
        
        public int PrevStepIndex => PrevStepOrNull == null ? -1 : PrevStepOrNull.Index;
        
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
        
        public T2[] Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T2>(true) : _steps;
        private T2[] _steps;
        
        
        
        protected override async void Start()
        {
            base.Start();
            
            if (playOnStart.stepOrNull != null)
            {
                for (var i = 0; i < playOnStart.delayFrame; i++) await Awaitable.NextFrameAsync();
                CurrentStep = playOnStart.stepOrNull;
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
        
        
        
        [Serializable]
        public struct PlayOnStart
        {
            public int delayFrame;
            public T2 stepOrNull;
        }
    }   
}