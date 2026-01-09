using System;
using LCHFramework.Components;

namespace LCHFramework.Managers.StepManager
{
    public class Step : LCHMonoBehaviour
    {
        public event Action onShow;
        public event Action onHide;
        
        
        public override int Index => StepManager.Instance.Steps.IndexOf(this);  
        
        
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            onShow?.Invoke();
        }

        public virtual void Hide()
        {
            onHide?.Invoke();
            gameObject.SetActive(false);
        }
    }
}