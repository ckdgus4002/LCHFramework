using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Extensions;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Managers
{
    public interface IPassCurrentStep
    {
        public void PassCurrentStep();
    }
    
    public delegate void OnCurrentStepIndexChangedDelegate(int prev, int current);
    
    public interface ICurrentStepIndexChanged
    {
        public int PrevStepIndex { get; }

        public int CurrentStepIndex { get; }
        
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
    }
    
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1>, ICurrentStepIndexChanged, IPassCurrentStep
        where T1 : MonoSingleton<T1>
        where T2 : Step
    {
        public T2[] steps;
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnStart;
        [SerializeField] [HideIf(nameof(playOnStart), false)] private int playOnStartDelayFrame;
        [SerializeField] [HideIf(nameof(playOnStart), false)] private T2 playOnStartStep;
        
        
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
        
        
        public T2 PrevStepOrNull { get; private set; }
        
        public T2 LeftStepOrNull { get; private set; }
        
        public T2 RightStepOrNull { get; private set; }
        
        
        public T2 CurrentStep
        {
            get
            {
                if (_currentStep == null) CurrentStep = steps[0];
                
                return _currentStep;    
            }
            set
            {
                if (_currentStep == value) return;
                
                PrevStepOrNull = _currentStep;
                _currentStep = value;
                LeftStepOrNull = 0 < _currentStep.Index ? steps[_currentStep.Index - 1] : loop ? steps[^1] : null;
                RightStepOrNull = _currentStep.Index < steps.Length - 1 ? steps[_currentStep.Index + 1] : loop ? steps[0] : null;
                
                foreach (var t in steps.Where(t => t.IsShown)) t.Hide();
                _currentStep.Show();
                
                Debug.Log($"CurrentStep is Changed. {PrevStepOrNull.Index} -> {_currentStep.Index}");
                OnCurrentStepIndexChanged?.Invoke(PrevStepOrNull == null ? -1 : PrevStepOrNull.Index, _currentStep.Index);
            }
        }
        private T2 _currentStep;
        
        public T2 StartStep => steps[0];
        
        public T2 LastStep => steps[^1];
        
        public int PrevStepIndex => PrevStepOrNull == null ? -1 : PrevStepOrNull.Index;

        public int CurrentStepIndex => CurrentStep.Index;
        
        
        
        protected virtual void Reset()
        {
            CollectSteps();
            InitializePlayOnStart();
        }

        protected override void Awake()
        {
            base.Awake();
            
            if (steps.IsEmpty()) CollectSteps();
        }
        
        protected override async void Start()
        {
            base.Start();

            if (playOnStart)
            {
                for (var i = 0; i < playOnStartDelayFrame; i++) await Awaitable.NextFrameAsync();
                Play();
            }
        }
        
        
        
        [Button]
        private void CollectSteps()
        {
            if (steps.IsEmpty()) steps = GetComponentsInChildren<T2>(true);
        }

        [Button]
        private void InitializePlayOnStart()
        {
            playOnStart = true;
            playOnStartDelayFrame = 1;
            playOnStartStep = !steps.IsExists() ? null : StartStep;
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
        
        public void Play() => CurrentStep = playOnStartStep;
    }   
}