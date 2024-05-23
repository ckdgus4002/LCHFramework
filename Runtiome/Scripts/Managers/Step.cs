using LCHFramework.Attributes;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class Step : MonoBehaviour
    {
        public int Index => _index ?? (int)(_index = StepManager.Steps.IndexOf(this));
        private int? _index;

        public virtual bool IsShown => gameObject.activeSelf;

        protected StepManager StepManager => _stepManager == null ? _stepManager = GetComponentInParent<StepManager>() : _stepManager;
        private StepManager _stepManager;
        
        
        
        protected virtual void OnValidate() => name = $"{GetType().Name} ({Index})";
        
        
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        [ShowInInspector]
        public void SetCurrentStep()
        {
            StepManager.CurrentStep.Value = this;   
        }
    }
}

