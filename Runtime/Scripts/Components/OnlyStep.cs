using System;
using System.Linq;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class OnlyStep : OnlyStep<Step>
    {
    }
    
    public class OnlyStep<T> : MonoBehaviour where T : Step
    {
        [SerializeField] private T[] steps = Array.Empty<T>();
        [SerializeField] private UnityEvent<T, T> onShow;
        [SerializeField] private UnityEvent<T, T> onHide;
        
        
        
        private void Start()
        {
            StepManager<T>.Instance.CurrentStep.OnValueChanged += OnCurrentStepChanged;
        }

        private void OnDestroy()
        {
            if (StepManager<T>.Instance != null) StepManager<T>.Instance.CurrentStep.OnValueChanged -= OnCurrentStepChanged;   
        }
        
        
        
        private void OnCurrentStepChanged(T prevOrNull, T current)
        {
            if (steps.Contains(current))
            {
                Show();
                onShow?.Invoke(prevOrNull, current);
            }
            else
            {
                Hide();
                onHide?.Invoke(prevOrNull, current);
            }
        }

        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);
    }
}