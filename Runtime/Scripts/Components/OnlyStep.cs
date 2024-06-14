using System.Linq;
using LCHFramework.Extensions;
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
        [SerializeField] private T[] steps;
        [SerializeField] private UnityEvent<T, T> onShow;
        [SerializeField] private UnityEvent<T, T> onHide;
        
        
        private IReadOnlyStepManager<T> StepManager
        {
            get
            {
                foreach (var t in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (t.TryGetComponent<IReadOnlyStepManager<T>>(out var result))
                    {
                        _stepManager = result;
                        break;
                    }

                return _stepManager;
            }
        }
        private IReadOnlyStepManager<T> _stepManager;
        
        
        
        private void Start()
        {
            StepManager.CurrentStep.OnValueChanged += OnCurrentStepChanged;
        }

        private void OnDestroy()
        {
            if (StepManager != null) StepManager.CurrentStep.OnValueChanged -= OnCurrentStepChanged;
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