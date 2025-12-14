using System.Linq;
using LCHFramework.Components;

namespace LCHFramework.Managers.UI.MessageBoxItems
{
    public class MessageBoxItem : LCHMonoBehaviour
    {
        internal string mKey => name;
        
        
        
        internal virtual void Show(params object[] objects) => gameObject.SetActive(true);
        
        public virtual void Hide()
        {
            if (MessageBox.Instance.Items.Where(t => t.IsShown).All(t => t == this)) MessageBox.Instance.Hide();
            gameObject.SetActive(false);
        }
    }
}