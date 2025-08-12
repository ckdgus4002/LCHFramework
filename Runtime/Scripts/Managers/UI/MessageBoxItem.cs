using System.Linq;
using LCHFramework.Components;

namespace LCHFramework.Managers.UI
{
    public class MessageBoxItem : LCHMonoBehaviour
    {
        public string Key => name;
        
        
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            if (MessageBox.Instance.Items.All(t => !t.IsShown)) MessageBox.Instance.Hide();
        }
    }
}
