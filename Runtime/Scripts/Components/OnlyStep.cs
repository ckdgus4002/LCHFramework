using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class OnlyStep : LCHMonoBehaviour
    {
        [SerializeField] private IIndex[] steps;
        [SerializeField] private UnityEvent<int, int> onShow;
        [SerializeField] private UnityEvent<int, int> onHide;
        
        
        private ICurrentStepIndexChanged CurrentStepIndexChanged
        {
            get
            {
                foreach (var t in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                    if (t.TryGetComponent<ICurrentStepIndexChanged>(out var result))
                    {
                        _stepManager = result;
                        break;
                    }

                return _stepManager;
            }
        }
        private ICurrentStepIndexChanged _stepManager;
        
        
        
        protected override void Start()
        {
            base.Start();
            
            CurrentStepIndexChanged.OnCurrentStepIndexChanged += OnCurrentStepIndexChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (CurrentStepIndexChanged != null) CurrentStepIndexChanged.OnCurrentStepIndexChanged -= OnCurrentStepIndexChanged;
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