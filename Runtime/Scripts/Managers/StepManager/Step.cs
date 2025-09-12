using LCHFramework.Components;

namespace LCHFramework.Managers.StepManager
{
    public class Step : LCHMonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}

