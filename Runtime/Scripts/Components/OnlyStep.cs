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
            StepManager<T>.Instance.CurrentStep.OnValueChanged += OnCurrentSequenceChanged;
        }

        private void OnDestroy()
        {
            if (StepManager<T>.Instance != null) StepManager<T>.Instance.CurrentStep.OnValueChanged -= OnCurrentSequenceChanged;   
        }
        
        
        
        private void OnCurrentSequenceChanged(T prev, T current)
        {
            if (steps.Contains(current))
            {
                Show();
                onShow?.Invoke(prev, current);
            }
            else
            {
                Hide();
                onHide?.Invoke(prev, current);
            }
        }

        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);
    }
}