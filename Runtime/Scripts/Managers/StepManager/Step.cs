using LCHFramework.Components;

namespace LCHFramework.Managers.StepManager
{
    public class Step : LCHMonoBehaviour
    {
        public override int Index => StepManager.Instance.Steps.IndexOf(this);  
        
        
        
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}