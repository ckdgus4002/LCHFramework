using LCHFramework.Components;

namespace LCHFramework.Managers
{
    public class Step : LCHMonoBehaviour
    {
        protected virtual void OnValidate() => name = $"{GetType().Name} ({Index})";
        
        
        
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}

