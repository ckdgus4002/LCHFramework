using System.Linq;
using LCHFramework.Extensions;

namespace LCHFramework.Managers.UI
{
    public class MessageBox : MonoSingleton<MessageBox>
    {
        private MessageBoxItem[] Items => _items ??= GetComponentsInChildren<MessageBoxItem>(true);
        private MessageBoxItem[] _items;
        
        
        
        public void Show(string key)
        {
            var item = Items.FirstOrDefault(t => t.Key == key);
            Items.ForEach(t =>
            {
                if (t == item) t.Show();
                else t.Hide();
            });
        }
    }
}
