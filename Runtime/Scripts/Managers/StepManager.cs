using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Managers
{
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1>, IStepManager<T2>, ICurrentStepIndexChanged, IPassCurrentStep where T1 : MonoSingleton<T1> where T2 : Step
    {
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnStart;
        [SerializeField] [HideIf(nameof(playOnStart), false)] private int playOnStartDelayFrame = 1;
        [SerializeField] [HideIf(nameof(playOnStart), false)] private T2 playOnStartStep;
        
        
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
        
        
        public T2 PrevStepOrNull { get; private set; }
        
        public T2 LeftStepOrNull { get; private set; }
        
        public T2 RightStepOrNull { get; private set; }
        
        
        protected override bool IsDontDestroyOnLoad => true;
        
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
                
                Debug.Log($"CurrentStep is Changed. {PrevStepOrNull.Index} -> {_currentStep.Index}");
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

            if (playOnStart)
            {
                for (var i = 0; i < playOnStartDelayFrame; i++) await Awaitable.NextFrameAsync();
                CurrentStep = playOnStartStep;
            }
        }
        
        
        
        [Button]
        public void PassCurrentStep()
        {
            if (Application.IsEditor && !UnityEngine.Application.isPlaying) 
                Debug.LogError("Can't execute when edit mode in editor. because cache variables is cached.");
            else if (RightStepOrNull == null)
                Debug.LogError("Step is end.");
            else
                CurrentStep = RightStepOrNull;
        }
    }   
}