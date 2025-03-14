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
    public class SetCurrentStepMessage
    {
        public SetCurrentStepMessage(Func<Step, bool> func) => Func = func;
        
        public SetCurrentStepMessage(Type type) => Func = step => step.GetType() == type;
        
        public SetCurrentStepMessage(int index) => Func = step => step.Index == index;
        
        public Func<Step, bool> Func;
    }
    
    public struct PassCurrentStepMessage
    {
    }
    
    public struct OnCurrentStepChangedMessage
    {
        public Step prevStepOrNull;
        public Step currentStep;
    }
    
    public class StepManager : StepManager<StepManager, Step>
    {
    }
    
    [DefaultExecutionOrder(1)]
    public class StepManager<T1, T2> : MonoSingleton<T1>
        where T1 : MonoSingleton<T1>
        where T2 : Step
    {
        public T2 startStep;
        public bool loop;
        [SerializeField] private bool playOnStart = true;
        
        
        private readonly CompositeDisposable disposables = new();
        
        
        public bool IsPlayed { get; private set; }
        public T2 PrevStepOrNull { get; private set; }
        public T2 LeftStepOrNull { get; private set; }
        public T2 RightStepOrNull { get; private set; }
        
        
        public T2 CurrentStep
        {
            get
            {
                if (_currentStep == null) CurrentStep = startStep;
                
                return _currentStep;
            }
            set
            {
                if (_currentStep == (value ??= startStep)) return;
                
                IsPlayed = true;
                PrevStepOrNull = _currentStep;
                _currentStep = value;
                LeftStepOrNull = 0 < _currentStep.Index ? Steps[_currentStep.Index - 1] : loop ? Steps[^1] : null;
                RightStepOrNull = _currentStep.Index < Steps.Count - 1 ? Steps[_currentStep.Index + 1] : loop ? FirstStep : null;
                
                Steps.Where(t => t.IsShown).ForEach(t => t.Hide());
                _currentStep.Show();
                
                Debug.Log($"CurrentStep is changed. {(PrevStepOrNull == null ? "" : $"{PrevStepOrNull.name} -> ")}{_currentStep.name}");
                MessageBroker.Default.Publish(new OnCurrentStepChangedMessage { prevStepOrNull = PrevStepOrNull, currentStep = _currentStep });
            }
        }
        private T2 _currentStep;
        
        public T2 FirstStep => Steps[0];
        
        public T2 LastStep => Steps[^1];
        
        private List<T2> Steps => _steps.IsEmpty() ? _steps = GetComponentsInChildren<T2>(true).ToList() : _steps;
        private List<T2> _steps;
        
        
        
        protected override void Start()
        {
            base.Start();
            
            disposables.Add(MessageBroker.Default.Receive<SetCurrentStepMessage>().Subscribe(message =>
            {
                CurrentStep = Steps.FirstOrDefault(step => message.Func.Invoke(step));
            }));
            
            disposables.Add(MessageBroker.Default.Receive<PassCurrentStepMessage>().Subscribe(_ =>
            {
                PassCurrentStep();
            }));
            
            if (playOnStart) CurrentStep = startStep;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            disposables.Clear();
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
                CurrentStep = startStep;
        }
    }
}