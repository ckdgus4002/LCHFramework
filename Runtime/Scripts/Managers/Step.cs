using LCHFramework.Components;

namespace LCHFramework.Managers
{
    public class Step : LCHMonoBehaviour
    {
        // ReSharper disable once MergeConditionalExpression
        protected IPassCurrentStep PassCurrentStep => _passCurrentStep == null ? _passCurrentStep = GetComponentInParent<IPassCurrentStep>() : _passCurrentStep;
        private IPassCurrentStep _passCurrentStep;
        
        
        
        protected virtual void OnValidate() => name = $"{GetType().Name} ({Index})";
        
        
        
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}

