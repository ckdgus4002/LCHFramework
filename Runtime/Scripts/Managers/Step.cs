using LCHFramework.Components;
using LCHFramework.Data;

namespace LCHFramework.Managers
{
    public class Step : LCHMonoBehaviour
    {
        protected IPassCurrentStep PassCurrentStep => _passCurrentStep ??= GetComponentInParent<IPassCurrentStep>();
        private IPassCurrentStep _passCurrentStep;
        
        
        
        protected virtual void OnValidate() => name = $"{GetType().Name} ({Index})";
        
        
        
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}

