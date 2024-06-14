using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class OnlyStep : OnlyStep<StepManager, Step>
    {
    }
    
    public class OnlyStep<T1, T2> : MonoBehaviour where T1 : StepManager<T1, T2> where T2 : Step
    {
        [SerializeField] private T2[] steps;
        [SerializeField] private UnityEvent<T2, T2> onShow;
        [SerializeField] private UnityEvent<T2, T2> onHide;
        
        
        
        private void Start()
        {
            StepManager<T1, T2>.Instance.CurrentStep.OnValueChanged += OnCurrentStepChanged;
        }

        private void OnDestroy()
        {
            if (StepManager<T1, T2>.Instance.TryIsNotNull(out var stepManager)) stepManager.CurrentStep.OnValueChanged -= OnCurrentStepChanged;
        }
        
        
        
        private void OnCurrentStepChanged(T2 prevOrNull, T2 current)
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