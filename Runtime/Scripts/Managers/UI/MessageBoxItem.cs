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
            if (MessageBox.Instance.Items.Where(t => t.IsShown).All(t => t == this)) MessageBox.Instance.Hide();
            gameObject.SetActive(false);
        }
    }
}