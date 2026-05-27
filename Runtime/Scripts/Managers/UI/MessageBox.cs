using LCHFramework.Extensions;
using LCHFramework.Managers.UI.MessageBoxItems;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class MessageBox : MonoSingleton<MessageBox>
    {
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        private GameObject _wrapper;
        
        internal MessageBoxItem[] Items => _items ??= GetComponentsInChildren<MessageBoxItem>(true);
        private MessageBoxItem[] _items;
        
        
        
        public bool IsShownFromKey(string key) => Items.TryFirstOrDefault(t => t.mKey == key, out var result) && result.IsShown;
        
        public MessageBoxItem Show(string key, params object[] objects)
        {
            Wrapper.SetActive(true);

            MessageBoxItem result = null;
            Items.ForEach(t =>
            {
                if (t.mKey == key)
                {
                    t.Show(objects);
                    result = t;
                }
                else if (t.IsShown)
                    t.Hide();
            });
            return result;
        }

        public MessageBoxItem Hide(string key)
        {
            if (Items.TryFirstOrDefault(t => t.mKey == key, out var result)) result.Hide();
            return result;
        }
        
        public void Hide() => Wrapper.SetActive(false);
    }
}
