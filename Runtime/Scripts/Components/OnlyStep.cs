using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class OnlyStep : LCHMonoBehaviour
    {
        private static ICurrentStepIndexChanged CurrentStepIndexChanged
        {
            get
            {
                if (_stepManager == null)
                    foreach (var t in FindObjectsByType<LCHMonoBehaviour>(FindObjectsSortMode.None))
                        if (t.TryGetComponent<ICurrentStepIndexChanged>(out var result))
                        {
                            _stepManager = result;
                            break;
                        }    

                return _stepManager;
            }
        }
        private static ICurrentStepIndexChanged _stepManager;
        
        
        
        [SerializeField] private Step[] steps;
        [SerializeField] private UnityEvent<int, int> onShow;
        [SerializeField] private UnityEvent<int, int> onHide;
        
        
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (!CurrentStepIndexChanged.OnCurrentStepIndexChanged.Contains((OnCurrentStepIndexChangedDelegate)OnCurrentStepIndexChanged))
                CurrentStepIndexChanged.OnCurrentStepIndexChanged += OnCurrentStepIndexChanged;
        }
        
        
        
        private void OnCurrentStepIndexChanged(int prevStepIndex, int currentStepIndex)
        {
            if (steps.Any(t => t.Index == currentStepIndex))
            {
                Show();
                onShow?.Invoke(prevStepIndex, currentStepIndex);
            }
            else
            {
                Hide();
                onHide?.Invoke(prevStepIndex, currentStepIndex);
            }
        }

        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);
    }
}