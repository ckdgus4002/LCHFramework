using System.Linq;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class MessageBox : MonoSingleton<MessageBox>
    {
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.Find(nameof(Wrapper)).gameObject : _wrapper;
        private GameObject _wrapper;
        
        public MessageBoxItem[] Items => _items ??= GetComponentsInChildren<MessageBoxItem>(true);
        private MessageBoxItem[] _items;
        
        
        
        public void Show(string key, params object[] objects)
        {
            Wrapper.SetActive(true);
            var item = Items.FirstOrDefault(t => t.mKey == key);
            Items.ForEach(t =>
            {
                if (t == item) t.Show(objects);
                else t.Hide();
            });
        }
        
        public void Hide() => Wrapper.SetActive(false);
    }
}
