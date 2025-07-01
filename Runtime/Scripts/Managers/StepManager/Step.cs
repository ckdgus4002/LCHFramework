using LCHFramework.Components;

namespace LCHFramework.Managers
{
    public class Step : LCHMonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}

