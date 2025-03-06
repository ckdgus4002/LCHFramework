using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Attributes;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Managers
{
    public class SetCurrentStepIndexMessage
    {
        public int index;
    }
    
    public class SetCurrentStepMessage
    {
        public Type type;
    }
    
    public class PassCurrentStepMessage
    {
    }
    
    public class StartStepMessage
    {
        public Step endStepOrNull;
        public Step startStep;
    }
    
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    public class StepManager<T1, T2> : MonoSingleton<T1>
        where T1 : MonoSingleton<T1>
        where T2 : Step
    {
        public T2 startStep;
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] [HideIf(nameof(playOnStart), false)] private int playOnStartDelayFrame = 1;
        
        
        public bool IsPlayed { get; private set; }
        public T2 PrevStepOrNull { get; private set; }
        public T2 LeftStepOrNull { get; private set; }
        public T2 RightStepOrNull { get; private set; }
        
        
        public int PrevStepIndex => PrevStepOrNull == null ? -1 : PrevStepOrNull.Index;
        
        public T2 CurrentStep
        {
            get
            {
                if (_currentStep == null) Play();
                
                return _currentStep;
            }
            set
            {
                if (value == null) 
                    Debug.LogError("CurrentStep dont set. Because value is null");
                else if (_currentStep == value)
                    Debug.LogError("CurrentStep dont set. Because value is same value.");
                else
                {
                    IsPlayed = true;
                    PrevStepOrNull = _currentStep;
                    _currentStep = value;
                    LeftStepOrNull = 0 < _currentStep.Index ? Steps[_currentStep.Index - 1] : loop ? Steps[^1] : null;
                    RightStepOrNull = _currentStep.Index < Steps.Count - 1 ? Steps[_currentStep.Index + 1] : loop ? startStep : null;
                
                    Steps.Where(t => t.IsShown).ForEach(t => t.Hide());
                    _currentStep.Show();
                
                    Debug.Log($"CurrentStep is changed. {PrevStepIndex} -> {_currentStep.Index}");
                    MessageBroker.Default.Publish(new StartStepMessage { endStepOrNull = PrevStepOrNull, startStep = _currentStep });    
                }
            }
        }
        private T2 _currentStep;
        
        public T2 LastStep => Steps[^1];

        private List<T2> Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T2>(true).ToList() : _steps;
        private List<T2> _steps;
        
        
        
        protected override async void Start()
        {
            base.Start();
            
            MessageBroker.Default.Receive<PassCurrentStepMessage>().Subscribe(t => PassCurrentStep()).AddTo(gameObject);
            
            MessageBroker.Default.Receive<SetCurrentStepMessage>().Subscribe(message => CurrentStep = Steps.FirstOrDefault(step => step.GetType() == message.type)).AddTo(gameObject);
            
            MessageBroker.Default.Receive<SetCurrentStepIndexMessage>().Subscribe(message => CurrentStep = Steps[message.index]).AddTo(gameObject);
            
            if (playOnStart)
            {
                for (var i = 0; i < playOnStartDelayFrame; i++) await Awaitable.NextFrameAsync();
                Play();
            }
        }
        
        
        
        [Button(nameof(PassCurrentStep))]
        public void PassCurrentStep()
        {
            if (Application.isEditor && !UnityEngine.Application.isPlaying) 
                Debug.LogError("Can't execute when edit mode in editor. because cache variables is cached.");
            else if (IsPlayed && CurrentStep == LastStep && RightStepOrNull == null)
                Debug.LogError("Step is end.");
            else if (IsPlayed)
                CurrentStep = RightStepOrNull;
            else
                Play();
        }
        
        protected void Play()
        {
            if (IsPlayed) return;
            
            CurrentStep = startStep;
        }
    }

}