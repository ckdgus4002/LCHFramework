using LCHFramework.Attributes;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class Step : MonoBehaviour
    {
        public int Index => _index < 0 ? _index = StepManager.Steps.IndexOf(this) : _index;
        private int _index = -1;

        public virtual bool IsShown => gameObject.activeSelf;

        protected StepManager<Component, Step> StepManager => _stepManager == null ? _stepManager = GetComponentInParent<StepManager<Component, Step>>() : _stepManager;
        private StepManager<Component, Step> _stepManager;
        
        
        
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

